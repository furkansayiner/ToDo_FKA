using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext context;

        // HomeController sınıfının yapıcı metodu
        public HomeController(ToDoContext _context) => context = _context;

        // Ana sayfa (Index) yönlendirici
        public IActionResult Index(string id)
        {
            // Filtreleri oluştur ve ViewBag'e ekle
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            // Kategorileri, Durumları ve Zaman filtrelerini ViewBag'e ekle
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            // ToDos sorgusunu başlat
            IQueryable<ToDo> query = context.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status);

            // Kategoriye göre filtreleme
            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }

            // Duruma göre filtreleme
            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }

            // Zaman filtrelemesi
            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }

            // Sorguyu tamamla ve sıralama yaparak listeyi al
            var tasks = query.OrderBy(t => t.DueDate).ToList();

            return View(tasks);
        }

        // Yeni görev ekranına gitmek için HTTP GET işlemi
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            var task = new ToDo { StatusId = "Open" };
            return View(task);
        }

        // Yeni görev eklemek için HTTP POST işlemi
        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Statuses = context.Statuses.ToList();
                return View(task);
            }
        }

        // Filtreleme işlemi için HTTP POST işlemi
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }

        // Görevi tamamlandı olarak işaretleme işlemi için HTTP POST işlemi
        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, ToDo selected)
        {
            // Seçilen görevi veritabanından bul
            selected = context.ToDos.Find(selected.Id)!;

            if (selected != null)
            {
                selected.StatusId = "Close";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { ID = id });
        }

        // Tamamlanan görevleri silme işlemi için HTTP POST işlemi
        [HttpPost]
        public IActionResult DeleteComplete(string id)
        {
            // Kapatılan görevleri bul
            var toDelete = context.ToDos.Where(t => t.StatusId == "Close").ToList();

            // Bulunan görevleri sil
            foreach (var task in toDelete)
            {
                context.ToDos.Remove(task);
            }
            context.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }
    }
}

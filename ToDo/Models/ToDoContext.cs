using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    // Veritabanı bağlantısı için kullanılan DbContext sınıfı.
    public class ToDoContext : DbContext
    {
        // DbContextOptions kullanarak ToDoContext sınıfını yapılandırır.
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        // Veritabanındaki ToDo nesnelerini temsil eden DbSet.
        public DbSet<ToDo> ToDos { get; set; }

        // Veritabanındaki Kategori nesnelerini temsil eden DbSet.
        public DbSet<Category> Categories { get; set; }

        // Veritabanındaki Durum nesnelerini temsil eden DbSet.
        public DbSet<Status> Statuses { get; set; }
        public object Statuss { get; internal set; }

        // Veritabanı modeli oluşturulurken kullanılan yöntem. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kategori verilerini başlangıçta dolduran kod bloğu.
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", Name = "Çalışma" },
                new Category { CategoryId = "home", Name = "Ev işi" },
                new Category { CategoryId = "ex", Name = "Egzersiz" },
                new Category { CategoryId = "shop", Name = "Alışveriş" },
                new Category { CategoryId = "call", Name = "Bağlantı" }
            );

            // Durum verilerini başlangıçta dolduran kod bloğu.
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "closed", Name = "Completed" }
            );
        }
    }
}

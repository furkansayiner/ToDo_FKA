using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDo
    {
        // Görevin benzersiz kimliği.
        public int Id { get; set; }

        // Görev açıklaması; boş bırakılamaz ve zorunludur. Eğer boş bırakılırsa "Description is required." hatası alınır.
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        // Görevin kategori kimliği.
        [Required(ErrorMessage = "Category is required.")]
        public string CategoryId { get; set; }

        // Görevin ait olduğu kategori nesnesi.
        public Category Category { get; set; }

        // Görevin son teslim tarihi (isteğe bağlı).
        public DateTime? DueDate { get; set; }

        // Görevin durum kimliği.
        [Required(ErrorMessage = "Status is required.")]
        public string StatusId { get; set; }

        // Görevin ait olduğu durum nesnesi.
        public Status Status { get; set; }

        // Görevin süresi geçmişse (StatusId "Open" ve DueDate bugünden önceyse) true döner, aksi takdirde false döner.
        public bool Overdue => StatusId == "Open" && DueDate < DateTime.Today;
    }
}

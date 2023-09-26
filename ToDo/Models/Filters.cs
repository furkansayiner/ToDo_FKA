namespace ToDoApp.Models
{
    public class Filters
    {
        // Filters sınıfının yapılandırıcısı, filtreleri bir dize olarak alır ve dizeyi analiz ederek özellikleri doldurur.
        public Filters(string filterstring)
        {
            // Varsayılan olarak "all-all-all" olarak ayarlanmış olan Filterstring özelliği,
            // filtrelerin dize temsili olarak kabul eder.
            Filterstring = filterstring ?? "all-all-all";

            // "-" karakterine göre ayrılmış olan filtreleri parçalar ve ilgili özelliklere atar.
            string[] filters = Filterstring.Split('-');

            if (filters.Length >= 1)
            {
                CategoryId = filters[0];
            }
            if (filters.Length >= 2)
            {
                Due = filters[1];
            }
            if (filters.Length >= 3)
            {
                StatusId = filters[2];
            }
        }

        // Dize temsili olarak filtrelerin toplandığı özellik.
        public string Filterstring { get; set; } = "all-all-all";

        // Kategori filtresinin değeri.
        public string CategoryId { get; }

        // Zaman (due) filtresinin değeri.
        public string Due { get; set; } = "all";

        // Durum filtresinin değeri.
        public string StatusId { get; }

        // Kategori filtresinin "all" olup olmadığını kontrol eder.
        public bool HasCategory => CategoryId.ToLower() != "all";

        // Zaman filtresinin "all" olup olmadığını kontrol eder.
        public bool HasDue => Due.ToLower() != "all";

        // Durum filtresinin boş olup olmadığını ve "all" olup olmadığını kontrol eder.
        public bool HasStatus => !string.IsNullOrEmpty(StatusId) && StatusId.ToLower() != "all";

        // Zaman filtresi değerlerini ve açıklamalarını içeren bir sözlük.
        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string> {
                {"future","Gelecek" },
                {"past","Geçmiş" },
                {"today","Bugün" },
            };

        // Zaman filtresinin "past" değerini temsil eder.
        public bool IsPast => Due.ToLower() == "past";

        // Zaman filtresinin "future" değerini temsil eder.
        public bool IsFuture => Due.ToLower() == "future";

        // Zaman filtresinin "today" değerini temsil eder.
        public bool IsToday => Due.ToLower() == "today";

        // Bu özellik, Statuses özelliğinin dinamik olarak ayarlanmasını sağlar.
        public dynamic Statuses { get; internal set; }
        public static dynamic DueFiltresValues { get; internal set; }
    }
}

using System.Collections.Generic;

namespace Library.Entities
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public List<Loan> Loans { get; set; }
    }
}

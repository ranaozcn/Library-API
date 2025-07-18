using System;

namespace Library.Entities
{
    public class Loan
    {
        public int LoanID { get; set; }
        public DateTime Borrowed { get; set; }
        public DateTime Returned{ get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}

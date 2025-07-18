using System.Collections.Generic;

namespace Library.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; } 
        public List<Loan> Loans { get; set; }
    }
}

using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.LoanEndpoint
{
    public class BorrowBookRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
    public class BorrowBookResponse
    {
        public int LoanId { get; set; }
        public DateTime Returned { get; set; }
    }
    public class BorrowBookEndpoint : Endpoint<BorrowBookRequest, BorrowBookResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public BorrowBookEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Post("/loans/borrow");
            AllowAnonymous();
        }
        public override async Task HandleAsync(BorrowBookRequest request,CancellationToken ct)
        {
            var user = await _libraryDbContext.Users.FindAsync(request.UserId);
            var book = await _libraryDbContext.Books.FindAsync(request.BookId);
            if (user == null)
            {
                AddError(r => r.UserId, "Kullanıcı bulunamadı");
                return;
            }
            if (book == null) 
            {
                AddError(r => r.BookId, "Kitap bulunamadı");
                return;
            }
            if (!book.IsAvailable)
            {
                AddError(r => r.BookId, "Kitap ödünç verilmiş");
                return;
            }
            book.IsAvailable = false;
            var loan = new Loan();
            loan.UserId = request.UserId;
            loan.BookId = request.BookId;
            loan.Borrowed = DateTime.UtcNow;
            loan.Returned = DateTime.UtcNow.AddDays(14);

            _libraryDbContext.Loans.Add(loan);
            await _libraryDbContext.SaveChangesAsync(ct);
            await SendAsync(new BorrowBookResponse { Returned = DateTime.UtcNow });
        }
    }
}

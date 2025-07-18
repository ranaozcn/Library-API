using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.LoanEndpoint
{
    public class ReturnBookRequest
    {
        public int LoanId { get; set; }
    }
    public class ReturnBookResponse
    {
        public string Message { get; set; } 
    }
    public class ReturnBookEndpoint : Endpoint<ReturnBookRequest, ReturnBookResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public ReturnBookEndpoint (LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Post("/loans/return");
            AllowAnonymous();
        }
        public override async Task HandleAsync(ReturnBookRequest request, CancellationToken ct)
        {
            var loan = await _libraryDbContext.Loans.FindAsync(request.LoanId);
            if (loan == null)
            {
                AddError(r => r.LoanId, "İlgili ödünç kaydı bulunamadı");
                return;
            }
            var book = await _libraryDbContext.Books.FindAsync(loan.BookId);
            if (book == null)
            {
                AddError(r => r.LoanId, "İade edilecek kitap bulunamadı");
                return;
            }
            book.IsAvailable = true;
            loan.Returned = DateTime.UtcNow;
            await _libraryDbContext.SaveChangesAsync(ct);
            await SendAsync(new ReturnBookResponse { Message = $"{book.Title} kitabı başarıyla iade edildi" });
        }
    }
}

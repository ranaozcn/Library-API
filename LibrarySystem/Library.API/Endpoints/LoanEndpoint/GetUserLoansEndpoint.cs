using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.LoanEndpoint
{
    public class GetUserLoansRequest
    {
        public int UserId { get; set; }
    }
    public class GetUserLoansResponse
    {
        public int LoanId { get; set; }
        public string BookTitle { get; set; }
        public DateTime Borrowed { get; set; }
        public DateTime Returned { get; set; }
    }
    public class GetUserLoansEndpoint : Endpoint<GetUserLoansRequest,List<GetUserLoansResponse>>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public GetUserLoansEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Get("/loans/user/{userId}");
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetUserLoansRequest request, CancellationToken ct)
        {
            var user = await _libraryDbContext.Users.FirstOrDefaultAsync(u => u.UserID == request.UserId, ct);
            if (user == null)
            {
                AddError(r => r.UserId, "Kullanıcı bulunamadı");
                return;
            }
            
            var allLoans = await _libraryDbContext.Loans
                .Include(l => l.Book)
                .ToListAsync(ct);

            List<GetUserLoansResponse> response = new List<GetUserLoansResponse>();

            foreach (var loan in allLoans)
            {
                if (loan.UserId == request.UserId)
                {
                    GetUserLoansResponse loanResponse = new GetUserLoansResponse();
                    loanResponse.LoanId = loan.LoanID;
                    loanResponse.BookTitle = loan.Book != null ? loan.Book.Title : "Bilinmeyen Kitap";
                    loanResponse.Borrowed = loan.Borrowed;
                    loanResponse.Returned = loan.Returned;

                    response.Add(loanResponse);
                }
            }

            await SendAsync(response);
        }

    }
}

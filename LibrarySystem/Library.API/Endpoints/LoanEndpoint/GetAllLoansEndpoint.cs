using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.LoanEndpoint
{
    public class GetAllLoansResponse
    {
        public List<Loan> Loans { get; set; }
    }
    public class GetAllLoansEndpoint : EndpointWithoutRequest<GetAllLoansResponse>
    {
        private readonly LibraryDbContext _libraryDbcontext;
        public GetAllLoansEndpoint(LibraryDbContext libraryDbcontext)
        {
            _libraryDbcontext = libraryDbcontext;
        }
        public override void Configure()
        {
            Get("/loans");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var loans = await _libraryDbcontext.Loans.ToListAsync();
            await SendAsync(new() { Loans = loans});
        }
    }
}

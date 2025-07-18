using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.BookEndpoint
{
    public class GetAllBooksResponse
    {
        public List<Book> Books { get; set; }
    }
    public class GetBooksEndpoint : EndpointWithoutRequest<GetAllBooksResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public GetBooksEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Get("/books");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var book = await _libraryDbContext.Books.ToListAsync();
            await SendAsync(new() { Books = book });
        }
    }
}

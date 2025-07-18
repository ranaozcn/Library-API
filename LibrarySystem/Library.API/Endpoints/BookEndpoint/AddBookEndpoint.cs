using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.BookEndpoint
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public bool IsAvailable { get; set; }
    }

    public class CreateBookResponse
    {
        public int BookId { get; set; }
    }
    public class AddBookEndpoint : Endpoint<CreateBookRequest, CreateBookResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public AddBookEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Post("/books");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateBookRequest request, CancellationToken ct)
        {
            var book = new Book();
            book.Title = request.Title;
            book.Author = request.Author;
            book.IsAvailable = request.IsAvailable;

            _libraryDbContext.Books.Add(book);
            await _libraryDbContext.SaveChangesAsync(ct);
            await SendAsync(new CreateBookResponse { BookId = book.BookID });
        }
    }
}

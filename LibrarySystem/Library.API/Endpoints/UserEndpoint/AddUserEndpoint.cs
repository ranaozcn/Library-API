using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.UserEndoint
{
    public class CreateUserRequest
    {
        public string UserName { get; set; } = null!;
    }
    public class CreateUserResponse
    {
        public int UserId { get; set; }
    }
    public class AddUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public AddUserEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public override void Configure()
        {
            Post("/users");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
        {
            var user = new User();
            user.UserName = request.UserName;

            _libraryDbContext.Users.Add(user);
            await _libraryDbContext.SaveChangesAsync(ct);
            await SendAsync(new CreateUserResponse { UserId = user.UserID });
        }
    }
}
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Library.Entities;
using Library.DataAccess;

namespace Library.API.Endpoints.UserEndpoint
{
    public class GetAllUsersResponse
    {
        public List<User> Users { get; set; }
    }
    public class GetAllUsersEndpoint : EndpointWithoutRequest<GetAllUsersResponse>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public GetAllUsersEndpoint(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public override void Configure()
        {
            Get("/users");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await _libraryDbContext.Users.ToListAsync();
            await SendAsync(new() { Users = users });
        }
    }
}

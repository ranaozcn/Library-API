using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Library.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LibraryConnStr")));
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();


var app = builder.Build();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi();

app.Run();

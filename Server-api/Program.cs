using Microsoft.EntityFrameworkCore;
using MediatR.RequestHandling;
using Scalar.AspNetCore;
using Server_api.Data;
using Server_api.Features.Projects.CreateProject;
using Server_api.Infrastructure.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddMediator<CreateProjectCommand>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGroup("/api/auth").MapIdentityApi<User>();
app.MapEndpoints();

app.Run();



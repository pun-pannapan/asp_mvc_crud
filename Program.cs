using asp_mvc_crud.Data;
using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories;
using asp_mvc_crud.Repositories.Implementations;
using asp_mvc_crud.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository Pattern
builder.Services.AddScoped<ILibraryRepository<Member>, LibraryRepository<Member>>();
builder.Services.AddScoped<ILibraryRepository<Author>, LibraryRepository<Author>>();
builder.Services.AddScoped<ILibraryRepository<Category>, LibraryRepository<Category>>();
builder.Services.AddScoped<ILibraryRepository<Book>, LibraryRepository<Book>>();
builder.Services.AddScoped<ILibraryRepository<Borrowing>, LibraryRepository<Borrowing>>();

// Register Specific Repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Migrate and Seed DB on app start
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

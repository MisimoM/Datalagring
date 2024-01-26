using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Business.Services;
using Presentation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Marko\Desktop\Projects\Education\DataBase\Uppgift-Databaser\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));
    
    services.AddScoped<BookRepository>();
    services.AddScoped<AuthorRepository>();
    services.AddScoped<GenreRepository>();
    services.AddScoped<BookService>();

    services.AddScoped<ArtistRepository>();
    services.AddScoped<AlbumRepository>();
    services.AddScoped<TrackRepository>();
    services.AddScoped<AlbumService>();

    services.AddScoped<MenuService>();

}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();
await menuService.ShowMainMenu();




//@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Marko\Desktop\Projects\Education\DataBase\Uppgift-Databaser\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"
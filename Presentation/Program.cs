using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Marko\Desktop\Projects\Education\DataBase\Uppgift-Databaser\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"));
}).Build();

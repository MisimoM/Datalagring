using Presentation.Managers;
using System.Diagnostics;

namespace Presentation.Services
{
    public class MenuService(BookManager bookManager, AlbumManager albumManager, ProductManager productManager)
    {
        private readonly BookManager _bookManager = bookManager;
        private readonly AlbumManager _albumManager = albumManager;
        private readonly ProductManager _productManager = productManager;
        public async Task MainMenu()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Books, Albums and Random Products");
                Console.WriteLine("");
                Console.WriteLine("1. Show Books");
                Console.WriteLine("2. Show Albums");
                Console.WriteLine("3. Show Products");
                Console.WriteLine("4. Exit the shop");
                Console.WriteLine("");

                Console.Write("Enter your choice: ");
                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        await BookMenu();
                        break;
                    case "2":
                        await AlbumsMenu();
                        break;
                    case "3":
                        await ProductsMenu();
                        break;
                    case "4":
                        Console.WriteLine("Exiting the shop...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task BookMenu()
        {
            while (true)
            {
                try
                {
                    await _bookManager.ShowAllBooks();

                    Console.WriteLine("");
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Book");
                    Console.WriteLine("2. Edit Book");
                    Console.WriteLine("3. Remove Book");
                    Console.WriteLine("4. Go back to Main Menu");
                    Console.WriteLine("");

                    Console.Write("Enter your option: ");
                    var userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            await _bookManager.AddBook();
                            break;
                        case "2":
                            await _bookManager.UpdateBook();
                            break;
                        case "3":
                            await _bookManager.RemoveBook();
                            break;
                        case "4":
                            await MainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }
        }

        private async Task AlbumsMenu()
        {
            while (true)
            {
                try
                {
                    await _albumManager.ShowAllAlbums();

                    Console.WriteLine("");
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Album");
                    Console.WriteLine("2. Edit Album");
                    Console.WriteLine("3. Remove Album");
                    Console.WriteLine("4. Go back to Main Menu");
                    Console.WriteLine("");

                    Console.Write("Enter your option: ");
                    var userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            await _albumManager.AddAlbum();
                            break;
                        case "2":
                            await _albumManager.UpdateAlbum();
                            break;
                        case "3":
                            await _albumManager.RemoveAlbum();
                            break;
                        case "4":
                            await MainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }
        }
        public async Task ProductsMenu()
        {
            while (true)
            {
                try
                {
                    await _productManager.ShowAllProducts();

                    Console.WriteLine("");
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Product");
                    Console.WriteLine("2. Edit Product");
                    Console.WriteLine("3. Remove Product");
                    Console.WriteLine("4. Go back to Main Menu");
                    Console.WriteLine("");

                    Console.Write("Enter your option: ");
                    var userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            await _productManager.AddProduct();
                            break;
                        case "2":
                            await _productManager.UpdateProduct();
                            break;
                        case "3":
                            await _productManager.RemoveProduct();
                            break;
                        case "4":
                            await MainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }
        }
    }
}

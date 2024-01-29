using Presentation.Views;
using Business.Services;
using Shared.Models.Album;
using Shared.Models.Book;
using Shared.Models.Product;

namespace Presentation.Services
{
    public class MenuService(BookService bookService, AlbumService albumService, ProductService productService)
    {
        private readonly BookService _bookService = bookService;
        private readonly AlbumService _albumService = albumService;
        private readonly ProductService _productService = productService;

        public async Task ShowMainMenu()
        {
            Menus.MainMenu();

            Console.Write("Enter your choice: ");
            string? userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    await ShowBooksMenu();
                    break;
                case "2":
                    await ShowAlbumsMenu();
                    break;
                case "3":
                    await ShowProductsMenu();
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

        private async Task ShowBooksMenu()
        {
            while(true)
            {
                Console.WriteLine("Showing Books");
                try
                {
                    var books = await _bookService.GetBooksAsync();

                    if (!books.Any())
                    {
                        Console.Clear();
                        Console.WriteLine("No books available.");
                    }

                    foreach (var book in books)
                    {
                        Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, Price: ${book.Price:F0}, Author: {book.Author}, Genre: {book.Genre}");
                    }

                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Book");
                    Console.WriteLine("2. Edit Book");
                    Console.WriteLine("3. Remove Book");
                    Console.WriteLine("4. Go back to Main Menu");

                    var option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            await AddBook();
                            break;
                        case "2":
                            await UpdateBook();
                            break;
                        case "3":
                            await RemoveBook();
                            break;
                        case "4":
                            await ShowMainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task AddBook()
        {
            try
            {
                Console.WriteLine("Enter details for the new book:");

                Console.Write("Title: ");
                var title = Console.ReadLine();

                Console.Write("Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price format.");
                    return;
                }

                Console.Write("Author: ");
                var author = Console.ReadLine();

                Console.Write("Genre: ");
                var genre = Console.ReadLine();

                var newBookModel = new BookModel
                {
                    Title = title!,
                    Price = price,
                    Author = author!,
                    Genre = genre!
                };

                var newBookEntity = await _bookService.CreateBookAsync(newBookModel);

                if (newBookEntity != null)
                {
                    Console.WriteLine($"New book added successfully. Book ID: {newBookEntity.Id}");
                }
                else
                {
                    Console.WriteLine("Failed to add the new book.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task RemoveBook()
        {
            Console.WriteLine("Enter the ID of the book you want to remove");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                bool removed = await _bookService.RemoveBookAsync(input);

                if (removed)
                {
                    Console.WriteLine($"Book with ID {input} successfully removed.");
                }
                else
                {
                    Console.WriteLine($"Unable to remove book with ID {input}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }


        }
        private async Task UpdateBook()
        {
            Console.WriteLine("Enter the ID of the book you want to update");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Enter updated details for the book:");

                Console.Write("New Title: ");
                var newTitle = Console.ReadLine();

                Console.Write("New Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var newPrice) || newPrice < 0)
                {
                    Console.WriteLine("Invalid price format. Please enter a valid positive number.");
                    return;
                }

                Console.Write("New Author: ");
                var newAuthor = Console.ReadLine();

                Console.Write("New Genre: ");
                var newGenre = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newTitle) || string.IsNullOrWhiteSpace(newAuthor) || string.IsNullOrWhiteSpace(newGenre))
                {
                    Console.WriteLine("Invalid input. Title, author, and genre cannot be empty.");
                    return;
                }

                var updatedBookModel = new BookModel
                {
                    Title = newTitle!,
                    Price = newPrice,
                    Author = newAuthor!,
                    Genre = newGenre!
                };

                bool updated = await _bookService.UpdateBookAsync(bookId, updatedBookModel);

                if (updated)
                {
                    Console.WriteLine($"Book with ID {bookId} successfully updated.");
                }
                else
                {
                    Console.WriteLine($"Unable to update book with ID {bookId}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }

        private async Task ShowAlbumsMenu()
        {
            while (true)
            {
                Console.WriteLine("Showing Albums");
                try
                {
                    var albums = await _albumService.GetAlbumsAsync();

                    Console.Clear();
                    if (!albums.Any())
                    {
                        Console.WriteLine("No albums available.");
                    }

                    foreach (var album in albums)
                    {
                        Console.WriteLine($"Id: {album.Id}, Title: {album.Title}, Artist: {album.Artist} Price: ${album.Price:F0}");
                        Console.WriteLine("Tracks:");
                        foreach (var track in album.Tracks)
                        {
                            Console.WriteLine($"Title: {track.Title}");
                        }
                    }

                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Album");
                    Console.WriteLine("2. Edit Album");
                    Console.WriteLine("3. Remove Album");
                    Console.WriteLine("3. Go back to Main Menu");

                    var option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            await AddAlbum();
                            break;
                        case "2":
                            await UpdateAlbum();
                            break;
                        case "3":
                            await RemoveAlbum();
                            break;
                        case "4":
                            await ShowMainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            
        }

        private async Task AddAlbum()
        {
            try
            {
                Console.WriteLine("Enter details for the new album:");

                Console.Write("Title: ");
                var title = Console.ReadLine();

                Console.Write("Artist: ");
                var artist = Console.ReadLine();

                Console.Write("Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price format.");
                    return;
                }

                Console.WriteLine("Enter details for the tracks (press Enter to finish):");
                var trackModels = new List<TrackModel>();

                while (true)
                {
                    Console.Write("Track Title (press Enter to finish): ");
                    var trackTitle = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(trackTitle))
                        break;

                    trackModels.Add(new TrackModel { Title = trackTitle! });
                }

                var newAlbumModel = new AlbumModel
                {
                    Title = title!,
                    Artist = artist!,
                    Price = price,
                };

                var newAlbumEntity = await _albumService.CreateAlbumAsync(newAlbumModel, trackModels);

                if (newAlbumEntity != null)
                {
                    Console.WriteLine($"New album added successfully. Album ID: {newAlbumEntity.Id}");
                }
                else
                {
                    Console.WriteLine("Failed to add the new album.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task RemoveAlbum()
        {
            Console.WriteLine("Enter the ID of the album you want to remove");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                bool removed = await _albumService.RemoveAlbumAsync(input);

                if (removed)
                {
                    Console.WriteLine($"Album with ID {input} successfully removed.");
                }
                else
                {
                    Console.WriteLine($"Unable to remove album with ID {input}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }


        }

        private async Task UpdateAlbum()
        {
            Console.WriteLine("Enter the ID of the album you want to update");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int albumId))
            {
                Console.WriteLine("Enter updated details for the album:");

                Console.Write("New Title: ");
                var newTitle = Console.ReadLine();

                Console.Write("New Artist: ");
                var newArtist = Console.ReadLine();

                Console.Write("New Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var newPrice) || newPrice < 0)
                {
                    Console.WriteLine("Invalid price format. Please enter a valid positive number.");
                    return;
                }

                Console.WriteLine("Enter details for the new tracks (press Enter to finish):");
                var updatedTrackModels = new List<TrackModel>();

                while (true)
                {
                    Console.Write("Track Title (press Enter to finish): ");
                    var trackTitle = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(trackTitle))
                        break;

                    updatedTrackModels.Add(new TrackModel { Title = trackTitle! });
                }

                var updatedAlbumModel = new AlbumModel
                {
                    Title = newTitle!,
                    Artist = newArtist!,
                    Price = newPrice,
                };

                var updatedAlbumEntity = await _albumService.UpdateAlbumAsync(albumId, updatedAlbumModel, updatedTrackModels);

                if (updatedAlbumEntity != null)
                {
                    Console.WriteLine($"Album with ID {albumId} successfully updated.");
                }
                else
                {
                    Console.WriteLine($"Unable to update album with ID {albumId}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }

        private async Task ShowProductsMenu()
        {
            while (true)
            {
                Console.WriteLine("Showing Products");
                try
                {
                    var products = await _productService.GetProductsAsync();

                    if (!products.Any())
                    {
                        Console.Clear();
                        Console.WriteLine("No products available.");
                    }

                    foreach (var product in products)
                    {
                        Console.WriteLine($"Id: {product.Id}, Category: {product.Category} Name: {product.Name}, Price: ${product.Price:F0}, Manufacturer: {product.Manufacturer}");
                    }

                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Add Product");
                    Console.WriteLine("2. Edit Product");
                    Console.WriteLine("3. Remove Product");
                    Console.WriteLine("4. Go back to Main Menu");

                    var option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            await AddProduct();
                            break;
                        case "2":
                            await UpdateProduct();
                            break;
                        case "3":
                            await RemoveProduct();
                            break;
                        case "4":
                            await ShowMainMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        private async Task AddProduct()
        {
            try
            {
                Console.WriteLine("Enter details for the new product:");

                Console.Write("Name: ");
                var name = Console.ReadLine();

                Console.Write("Manufacturer: ");
                var manufacturer = Console.ReadLine();

                Console.Write("Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price format.");
                    return;
                }

                Console.Write("Category: ");
                var category = Console.ReadLine();

                var newProductModel = new ProductModel
                {
                    Name = name!,
                    Manufacturer = manufacturer!,
                    Price = price,
                    Category = category!
                };

                await _productService.CreateProductAsync(newProductModel);

                Console.WriteLine("New product added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task RemoveProduct()
        {
            Console.WriteLine("Enter the ID of the product you want to remove");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                bool removed = await _productService.RemoveProductAsync(input);

                if (removed)
                {
                    Console.WriteLine($"Product with ID {input} successfully removed.");
                }
                else
                {
                    Console.WriteLine($"Unable to remove product with ID {input}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }
        private async Task UpdateProduct()
        {
            Console.WriteLine("Enter the ID of the product you want to update");
            Console.Write("ID number: ");

            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Enter updated details for the product:");

                Console.Write("New Name: ");
                var newName = Console.ReadLine();

                Console.Write("New Manufacturer: ");
                var newManufacturer = Console.ReadLine();

                Console.Write("New Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var newPrice) || newPrice < 0)
                {
                    Console.WriteLine("Invalid price format. Please enter a valid positive number.");
                    return;
                }

                Console.Write("New Category: ");
                var newCategory = Console.ReadLine();

                var updatedProductModel = new ProductModel
                {
                    Name = newName!,
                    Manufacturer = newManufacturer!,
                    Price = newPrice,
                    Category = newCategory!
                };

                bool updated = await _productService.UpdateProductAsync(productId, updatedProductModel);

                if (updated)
                {
                    Console.WriteLine($"Product with ID {productId} successfully updated.");
                }
                else
                {
                    Console.WriteLine($"Unable to update product with ID {productId}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }
    }
}

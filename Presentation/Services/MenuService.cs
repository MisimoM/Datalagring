using Presentation.Views;
using Business.Services;
using Shared.Models;

namespace Presentation.Services
{
    public class MenuService(BookService bookService, AlbumService albumService)
    {
        private readonly BookService _bookService = bookService;
        private readonly AlbumService _albumService = albumService;

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

                    if (!albums.Any())
                    {
                        Console.Clear();
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
                            //await UpdateAlbum();
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
    }
}

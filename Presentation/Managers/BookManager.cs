using Business.Services;
using Shared.Models.Book;
using System.Diagnostics;

namespace Presentation.Managers
{
    public class BookManager(BookService bookService)
    {
        private readonly BookService _bookService = bookService;

        public async Task ShowAllBooks()
        {
            try
            {
                var books = await _bookService.GetBooksAsync();

                Console.Clear();
                Console.WriteLine("Showing Books");
                Console.WriteLine("");

                if (!books.Any())
                {
                    Console.WriteLine("No books available.");
                }

                foreach (var book in books)
                {
                    Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, Price: ${book.Price:F0}, Author: {book.Author}, Genre: {book.Genre}");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        public async Task AddBook()
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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task RemoveBook()
        {
            try
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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        public async Task UpdateBook()
        {
            try
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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}

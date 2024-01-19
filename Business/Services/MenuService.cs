
using Infrastructure.Entities;

namespace Infrastructure.Services
{
    public class MenuService
    {
        public MenuService()
        { 

        }

        public void ShowMainMenu()
        {
            Console.WriteLine("Books and Albums");
            Console.WriteLine("");
            Console.WriteLine("1. Show Books");
            Console.WriteLine("2. Show Albums");

            Console.Write("Enter your choice: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    ShowProductMenu<BookEntity>();
                    break;

                case "2":
                    ShowProductMenu<AlbumEntity>();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    break;
            }
        }

        public void ShowProductMenu<TEntity>()
        {

        }

    }
}

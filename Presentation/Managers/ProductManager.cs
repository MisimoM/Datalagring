using Business.Services;
using Shared.Models.Product;
using System.Diagnostics;

namespace Presentation.Managers
{
    public class ProductManager(ProductService productService)
    {
        private readonly ProductService _productService = productService;

        public async Task ShowAllProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();

                Console.Clear();
                Console.WriteLine("Showing Products");
                Console.WriteLine("");

                if (!products.Any())
                {
                    Console.WriteLine("No products available.");
                }

                foreach (var product in products)
                {
                    Console.WriteLine($"Id: {product.Id}, Category: {product.Category}, Name: {product.Name}, Price: ${product.Price:F0}, Manufacturer: {product.Manufacturer}, Quantity: {product.InventoryQuantity}");
                    Console.WriteLine("");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task AddProduct()
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

                Console.Write("Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
                {
                    Console.WriteLine("Invalid quantity format.");
                    return;
                }

                var newProductModel = new ProductModel
                {
                    Name = name!,
                    Manufacturer = manufacturer!,
                    Price = price,
                    Category = category!,
                    InventoryQuantity = quantity
                };

                await _productService.CreateProductAsync(newProductModel);

                Console.WriteLine("New product added successfully!");
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task RemoveProduct()
        {
            try
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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        public async Task UpdateProduct()
        {
            try
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

                    Console.Write("New Inventory Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out var newInventoryQuantity) || newInventoryQuantity < 0)
                    {
                        Console.WriteLine("Invalid inventory quantity format. Please enter a valid positive number.");
                        return;
                    }

                    var updatedProductModel = new ProductModel
                    {
                        Name = newName!,
                        Manufacturer = newManufacturer!,
                        Price = newPrice,
                        Category = newCategory!,
                        InventoryQuantity = newInventoryQuantity
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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}

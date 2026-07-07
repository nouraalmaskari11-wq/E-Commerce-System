using E_Commerce_System.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_System
{
    public class Program
    {
        public static ECommerceContext context = new ECommerceContext();

        //1
        public static void RegisterUser()
        {
            Console.WriteLine("  Register New User ");
            Console.WriteLine("----------------------------------------");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string passwordHash = Console.ReadLine();

            Console.Write("Enter full name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter address: ");
            string address = Console.ReadLine();


            User user = new User
            {
                username = username,
                email = email,
                passwordHash = passwordHash,
                fullName = fullName,
                phoneNumber = phone,
                address = address,
                registrationDate = DateTime.Now,
                isActive = true
            };

            context.Users.Add(user);

            context.SaveChanges();

            Console.WriteLine($"user registered successfully! user Id: {user.userId}");
        }

        //2
        public static void AddProduct()
        {
            Console.WriteLine("  Add New Product ");
            Console.WriteLine("----------------------------------------");

            List<Category> categories = context.Categories.ToList();
            if (!categories.Any())
            {
                Console.WriteLine("no categories found. add a category first.");
                return;
            }

            Console.WriteLine("Available categories:");

            foreach (Category cat in categories)
            {
                Console.WriteLine($"category Id:{cat.categoryId}, category name:{cat.categoryName}");
            }

            Console.Write("Select category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Category category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine("category not found.");
                return;
            }

            Console.Write("Enter product name: ");
            string productName = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            Console.Write("Enter price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Enter stock quantity: ");
            int stockQuantity = int.Parse(Console.ReadLine());

            Console.Write("Enter image URL: ");
            string imageUrl = Console.ReadLine();

            Product product = new Product
            {
                productName = productName,
                description = description,
                price = price,
                stockQuantity = stockQuantity,
                imageUrl = imageUrl,
                categoryId = categoryId,
                createdAt = DateTime.Now,
                isAvailable = true
            };

            context.Products.Add(product);

            context.SaveChanges();

            Console.WriteLine($"Product {product.productName} added successfully! product Id: {product.productId}");
        }

        //3
        public static void PlaceOrder()
        {
            Console.WriteLine("  Place an Order ");
            Console.WriteLine("----------------------------------------");

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            User user = context.Users.Find(userId);
            if (user == null)
            {
                Console.WriteLine("user not found.");
                return;
            }

            Console.Write("Enter shipping address: ");
            string shippingAddress = Console.ReadLine();

            Console.Write("Enter payment method: ");
            string paymentMethod = Console.ReadLine();

            Order order = new Order
            {
                userId = userId,
                orderDate = DateTime.Now,
                status = "Pending",
                totalAmount = 0,
                shippingAddress = shippingAddress,
                paymentMethod = paymentMethod
            };

            context.Orders.Add(order);

            context.SaveChanges();

            decimal total = 0;

            bool work = true;

            while (work)
            {
                Console.WriteLine("--- Add product to order ---");
                Console.WriteLine("");

                List<Product> products = context.Products.Where(p => p.isAvailable && p.stockQuantity > 0).ToList();

                if (!products.Any())
                {
                    Console.WriteLine("no products available.");
                    break;
                }

                Console.WriteLine("available products:");
                foreach (Product p in products)
                {
                    Console.WriteLine($"product Id:{p.productId}, product name:{p.productName}, price:{p.price:F2}, stock: {p.stockQuantity}");
                }

                Console.Write("Enter Product ID: or enter 0 to stop adding product");
                int productId = int.Parse(Console.ReadLine());
                if (productId == 0)
                {
                    work = false;
                    break;
                }


                Product product = context.Products.Find(productId);
                if (product == null || !product.isAvailable || product.stockQuantity <= 0)
                {
                    Console.WriteLine("Product not available.");
                    return;
                }

                Console.Write("Enter quantity: ");
                int quantity = int.Parse(Console.ReadLine());
                if (quantity <= 0 || quantity > product.stockQuantity)
                {
                    Console.WriteLine("Invalid quantity.");
                    return;
                }

                OrderProduct orderItem = new OrderProduct
                {
                    orderId = order.orderId,
                    productId = productId,
                    quantity = quantity
                };

                context.OrderProducts.Add(orderItem);

                decimal itemTotal = product.price * quantity;
                total += itemTotal;
                product.stockQuantity -= quantity;

                Console.WriteLine($"Added {quantity} * {product.productName}, price: {itemTotal:F2}");

            }

            if (total > 0)
            {
                order.totalAmount = total;

                context.SaveChanges();

                Console.WriteLine($"Order placed successfully! Order ID: {order.orderId}, Total: {total:F2}");
            }
            else
            {
                context.Orders.Remove(order);

                context.SaveChanges();

                Console.WriteLine("order cancelled no items added.");
            }
        }


        //4
        public static void WriteReview()
        {
            Console.WriteLine("  Write a Product Review ");
            Console.WriteLine("----------------------------------------");

            List<User> users = context.Users.ToList();
            if (!users.Any())
            {
                Console.WriteLine("no users found.");
                return;
            }

            Console.WriteLine("Available Users:");
            foreach (User u in users)
            {
                Console.WriteLine($"User Id: {u.userId}, User name: {u.username}");
            }

            Console.Write("Select User ID: ");
            int userId = int.Parse(Console.ReadLine());

            List<Product> products = context.Products.ToList();
            if (!products.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("Available Products:");
            foreach (Product p in products)
            {
                Console.WriteLine($" product Id: {p.productId}, product name: {p.productName}");
            }

            Console.Write("Select Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            Console.Write("Enter rating (1-5): ");
            int rating = int.Parse(Console.ReadLine());

            Console.Write("Enter comment ,optional: ");
            string comment = Console.ReadLine();

            Review review = new Review
            {
                userId = userId,
                productId = productId,
                rating = rating,
                comment = comment,
                reviewDate = DateTime.Now
            };

            context.Reviews.Add(review);
            context.SaveChanges();

            Console.WriteLine($"Review added successfully! review Id: {review.reviewId}");
        }

        //5
        public static void UpdateProduct()
        {
            Console.WriteLine("  Update Product Price and Availability ");
            Console.WriteLine("----------------------------------------");

            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = context.Products.FirstOrDefault(p => p.productId == productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.WriteLine($" details:");

            Console.WriteLine($"Product: {product.productName}");
            Console.WriteLine($"Price: {product.price:F2}");
            Console.WriteLine($"Available: {product.isAvailable}");

            Console.Write("Enter new price: ");
            decimal priceInput = decimal.Parse(Console.ReadLine());
            if (priceInput != null)
            {
                product.price = priceInput;
            }

            Console.Write("Is available : true or false ");
            bool availInput = bool.Parse(Console.ReadLine());

            if (availInput != null)
            {
                product.isAvailable = availInput;
            }

            context.SaveChanges();

            Console.WriteLine($"Product updated successfully!");

            Console.WriteLine($"New Price:{product.price:F2}");
            Console.WriteLine($"Available: {product.isAvailable}");
        }

        //6
        public static void CancelOrder()
        {
            Console.WriteLine("  Cancel an Order ");
            Console.WriteLine("----------------------------------------");

            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            Order order = context.Orders.Include(o => o.OrderProducts).FirstOrDefault(o => o.orderId == orderId);

            if (order == null)
            {
                Console.WriteLine("order not found.");
                return;
            }

            if (order.status == "Cancelled")
            {
                Console.WriteLine("order is already cancelled.");
                return;
            }

            
            foreach (var Item in order.OrderProducts)
            {
                var product = context.Products.Find(Item.productId);
                if (product != null)
                {

                    product.stockQuantity += Item.quantity;

                    Console.WriteLine($"restored {Item.quantity} of {product.productName}");
                }
            }

            order.status = "Cancelled";

            context.SaveChanges();

            Console.WriteLine($"order {orderId} cancelled successfully. stock restored .");
        }

        //7
        public static void DeleteReview()
        {
            Console.WriteLine(" Delete a Review ");
            Console.WriteLine("--------------------------------");

            Console.WriteLine("Enter review ID: ");
            int reviewId = int.Parse(Console.ReadLine());

            Review review = context.Reviews.FirstOrDefault(r => r.reviewId == reviewId);

            if (review == null)
            {
                Console.WriteLine("review not found.");
                return;
            }

            context.Reviews.Remove(review);

            context.SaveChanges();

            Console.WriteLine($"review {reviewId} deleted successfully.");
        }

        //8
        public static void ViewAllProducts()
        {
            Console.WriteLine("  View All Products ");
            Console.WriteLine("----------------------------------------");

            List<Product> products = context.Products.ToList();

            if (!products.Any())
            {
                Console.WriteLine("no products found.");
                return;
            }

            foreach (var p in products)
            {
                Console.WriteLine($"ID:{p.productId} | Name:{p.productName} | Price: {p.price:F2} | Stock:{p.stockQuantity} | Available:{p.isAvailable}");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        //9
        public static void FilterProducts()
        {
            Console.WriteLine("  Filter Products by Category and Price Range ");
            Console.WriteLine("----------------------------------------");

            List<Category> categories = context.Categories.ToList();

            if (!categories.Any())
            {
                Console.WriteLine("categories not found.");
                return;
            }

            Console.WriteLine("Available Categories:");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (Category cat in categories)
            {
                Console.WriteLine($"category Id: {cat.categoryId}. category name: {cat.categoryName}");
            }

            Console.Write("Enter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Console.Write("Enter minimum price: ");
            decimal minPrice = decimal.Parse(Console.ReadLine());

            Console.Write("Enter maximum price: ");
            decimal maxPrice = decimal.Parse(Console.ReadLine());

            List<Product> products = context.Products.Where(p => p.categoryId == categoryId && p.price >= minPrice && p.price <= maxPrice).OrderBy(p => p.price).ToList();

            if (!products.Any())
            {
                Console.WriteLine("no product found matching your chosen category.");
            }
            else
            {

                foreach (Product p in products)
                {
                    Console.WriteLine($"ID:{p.productId} | Name: {p.productName} | Price:{p.price:F2} | Stock: {p.stockQuantity}");
                    Console.WriteLine("-----------------------------------------------------------------------------------");
                }
            }
        }

        //10
        public static void GetCategoryWithProducts()
        {
            Console.WriteLine("  Get Category with All Its Products ");
            Console.WriteLine("----------------------------------------");

            List<Category> categories = context.Categories.ToList();

            if (!categories.Any())
            {
                Console.WriteLine("categories not found.");
                return;
            }

            Console.WriteLine("Available categories:");

            foreach (var cat in categories)
            {
                Console.WriteLine($"category Id:{cat.categoryId}. category Name:{cat.categoryName}");
            }

            Console.Write("Enter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            var category = context.Categories.Include(c => c.Products).FirstOrDefault(c => c.categoryId == categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            Console.WriteLine($"Category:{category.categoryName}");

            Console.WriteLine($"Description:{category.description}");

            Console.WriteLine($"Products in this category {category.Products}:");

            if (category.Products != null)
            {


                foreach (var p in category.Products)
                {
                    Console.WriteLine($"ID:{p.productId} | Name:{p.productName} | Price:{p.price:F2} | Stock:{p.stockQuantity}");
                    Console.WriteLine("--------------------------------------------------------------------------------------------");

                }
            }
            else
            {
                Console.WriteLine("no products in this category.");
            }
        }

        //11
        public static void ViewOrderHistory()
        {
            Console.WriteLine("  View Order History ");
            Console.WriteLine("----------------------------------------");

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            var user = context.Users.Include(u => u.Orders).ThenInclude(o => o.OrderProducts).ThenInclude(p => p.Product).FirstOrDefault(u => u.userId == userId);

            if (user == null)
            {
                Console.WriteLine("user not found");
                return;
            }

            Console.WriteLine($"order history for {user.username}");

            if (user.Orders == null)
            {
                Console.WriteLine("orders not found.");
            }
            else
            {
                foreach (var order in user.Orders)
                {
                    Console.WriteLine($"Order ID {order.orderId}");
                    Console.WriteLine($"Order Date: {order.orderDate}");
                    Console.WriteLine($"Status: {order.status}");
                    Console.WriteLine($"Total:{order.totalAmount:F2}");
                    Console.WriteLine($"Products:");

                    if (order.OrderProducts != null)
                    {
                        foreach (var item in order.OrderProducts)
                        {
                            Console.WriteLine($"product Nam: {item.Product.productName}, Unit Price: {item.Product.price:F2} * {item.quantity}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("items not found.");
                    }
                }
            }
        }

        //12
        public static void ProductSummaryReport()
        {
            Console.WriteLine("Product Summary Report ");
            Console.WriteLine("-----------------------------------------");

            var products = context.Products.Select(
                p => new
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    CategoryName = p.Category.categoryName,
                    ReviewCount = p.Reviews.Count(),
                    AvgRating = p.Reviews.Average(r => r.rating)
                }).ToList();

            foreach (var p in products)
            {
                Console.WriteLine($"Product Name:{p.ProductName}, Product ID:{p.ProductId}, Category Name:{p.CategoryName}, Review Count: {p.ReviewCount}, Avg Rating = {p.AvgRating} ");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------");
            }

            Console.WriteLine("Enter product ID:");
            int productId = int.Parse(Console.ReadLine());

            Product product = context.Products.FirstOrDefault(p => p.productId == productId);
            if (product != null)
            {

            }
            else
            {
                Console.WriteLine("product not found ");
            }


        }

        static void Main(string[] args)
        {


            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("  E-Commerce System");
                Console.WriteLine("=======================================");
                Console.WriteLine(" 1- Register a New User ");
                Console.WriteLine(" 2- Add a New Product to a Category ");
                Console.WriteLine(" 3- Place an Order ");
                Console.WriteLine(" 4- Write a Product Review ");
                Console.WriteLine(" 5- Update Product Price and Availability ");
                Console.WriteLine(" 6- Cancel an Order ");
                Console.WriteLine(" 7- Delete a Review ");
                Console.WriteLine(" 8- View All Products ");
                Console.WriteLine(" 9- Filter Products by Category and Price Range ");
                Console.WriteLine("10- Get Category with All Its Products ");
                Console.WriteLine("11- View Order History ");
                Console.WriteLine("12- Product Summary Report");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("=======================================");
                Console.WriteLine("");
                Console.WriteLine("Select an option: ");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        RegisterUser();
                        break;
                    case 2:
                        AddProduct();
                        break;
                    case 3:
                        PlaceOrder();
                        break;
                    case 4:
                        WriteReview();
                        break;
                    case 5:
                        UpdateProduct();
                        break;
                    case 6:
                        CancelOrder();
                        break;
                    case 7:
                        DeleteReview();
                        break;
                    case 8:
                        ViewAllProducts();
                        break;
                    case 9:
                        FilterProducts();
                        break;
                    case 10:
                        GetCategoryWithProducts();
                        break;
                    case 11:
                        ViewOrderHistory();
                        break;
                    case 12:
                        ProductSummaryReport();
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. try again.");
                        break;
                }
                if (!exit)
                {
                    Console.WriteLine("Press any key to continue....");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.WriteLine("Goodbye!");
        }
    }
}

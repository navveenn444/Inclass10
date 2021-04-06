using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EntityFramework_Assignment
{
    class Order
    {
        public int OrderId { get; set; }
        public string CustName { get; set; }
        //DateTime OrderDate { get; set; }
        public List<OrderDetail> ProductList { get; set; }
    }
    class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int SupplierId { get; set; }
        public List<OrderDetail> OrderList { get; set; }
    }
    class OrderDetail
    {
        public int id { get; set; }

        public Order SaleOrderDetail { get; set; }
        public Product SaleProductDetail { get; set; }
        public int quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

    class OrderDetailsContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ClassAssign;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (OrderDetailsContext context = new OrderDetailsContext())
            {
                context.Database.EnsureCreated();
                Order order1 = new Order { CustName = "Naveen" };
                Order order2 = new Order { CustName = "Tharun" };
                Order order3 = new Order { CustName = "Mahi" };
                Order order4 = new Order { CustName = "Virat" };
                Product product1 = new Product
                {
                    ProductName = "Mobile",
                    ProductPrice = 200.00,
                    SupplierId = 2001
                };
                Product product2 = new Product
                {
                    ProductName = "Beers",
                    ProductPrice = 20.00,
                    SupplierId = 2002
                };
                Product product3 = new Product
                {
                    ProductName = "Nutella",
                    ProductPrice = 5.00,
                    SupplierId = 2003
                };
                Product product4 = new Product
                {
                    ProductName = "Icecream",
                    ProductPrice = 15.00,
                    SupplierId = 2004
                };

                OrderDetail orderDetail = new OrderDetail
                {
                    SaleOrderDetail = order1,
                    SaleProductDetail = product1,
                    quantity = 2,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail1 = new OrderDetail
                {
                    SaleOrderDetail = order1,
                    SaleProductDetail = product2,
                    quantity = 1,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail2 = new OrderDetail
                {
                    SaleOrderDetail = order2,
                    SaleProductDetail = product3,
                    quantity = 5,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail3 = new OrderDetail
                {
                    SaleOrderDetail = order3,
                    SaleProductDetail = product4,
                    quantity = 3,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail4 = new OrderDetail
                {
                    SaleOrderDetail = order3,
                    SaleProductDetail = product3,
                    quantity = 1,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail5 = new OrderDetail
                {
                    SaleOrderDetail = order4,
                    SaleProductDetail = product4,
                    quantity = 10,
                    OrderDate = DateTime.Now
                };
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);
                context.Orders.Add(order4);
                context.Products.Add(product1);
                context.Products.Add(product2);
                context.Products.Add(product3);
                context.Products.Add(product4);
                context.OrderDetails.Add(orderDetail);
                context.OrderDetails.Add(orderDetail1);
                context.OrderDetails.Add(orderDetail2);
                context.OrderDetails.Add(orderDetail3);
                context.OrderDetails.Add(orderDetail4);
                context.OrderDetails.Add(orderDetail5);
                context.SaveChanges();

                // Display all orders where a product is sold
                var a = context.Orders
                    .Include(c => c.ProductList)
                    .Where(c => c.ProductList.Count != 0);
                Console.WriteLine("................Order where a product is sold................");
                foreach (var i in a)
                {
                    Console.WriteLine("OrderID={0},CustomerName={1}", i.OrderId, i.CustName);
                }

                // For a given product, find the order where it is sold the maximum.
                Order output = context.OrderDetails
                    .Where(c => c.SaleProductDetail.ProductName == "Mobile")
                    .OrderByDescending(c => c.quantity)
                    .Select(c => c.SaleOrderDetail)
                    .First();
                Console.WriteLine("................Order where maximum amount of Mobiles have been sold............");
                Console.WriteLine("OrderID={0},CustomerName={1}", output.OrderId,  output.CustName);

            }


        }
    }
}
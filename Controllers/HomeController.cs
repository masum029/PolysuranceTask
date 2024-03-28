using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PolysuranceTest.Models;
using System.Diagnostics;

namespace PolysuranceTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            string orderJsonFile = @"data/orders.json";
            string productJsonFile = @"data/products.json";
            string discountJsonFile = @"data/discounts.json";

            string orderJson = System.IO.File.ReadAllText(orderJsonFile);
            string productJson = System.IO.File.ReadAllText(productJsonFile);
            string discountJson = System.IO.File.ReadAllText(discountJsonFile);

            var orders = JsonConvert.DeserializeObject<List<Order>>(orderJson);
            var products = JsonConvert.DeserializeObject<List<Product>>(productJson);
            var discounts = JsonConvert.DeserializeObject<List<Discount>>(discountJson);

            decimal totalSalesBeforeDiscount = 0;
            decimal totalSalesAfterDiscount = 0;
            decimal totalDiscountLost = 0;
            int totalCustomers = 0;
            decimal totalDiscountPercentage = 0;

            foreach (var order in orders)
            {
                totalCustomers++;

                decimal subtotal = order.Items.Sum(item =>
                {
                    var product = products.FirstOrDefault(p => p.Sku == item.Sku);
                    return product != null ? product.Price * item.Quantity : 0;
                });

                totalSalesBeforeDiscount += subtotal;

                decimal discount = 0;
                if (!string.IsNullOrEmpty(order.Discount))
                {
                    var discountCodes = order.Discount.Split(',');
                    foreach (var code in discountCodes)
                    {
                        var discountItem = discounts.FirstOrDefault(d => d.Key == code);
                        if (discountItem != null)
                            discount += discountItem.Value;
                    }
                }

                decimal discountedSubtotal = subtotal * (1 - discount);
                totalSalesAfterDiscount += discountedSubtotal;
                totalDiscountLost += subtotal * discount;
            }

            if (totalCustomers > 0)
            {
                totalDiscountPercentage = (totalDiscountLost / totalSalesBeforeDiscount) * 100;
            }

            ViewData["totalSalesBeforeDiscount"] = totalSalesBeforeDiscount;
            ViewData["totalSalesAfterDiscount"] = totalSalesAfterDiscount;
            ViewData["totalDiscountLost"] = totalDiscountLost;
            ViewData["totalDiscountPercentage"] = totalDiscountPercentage;

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

    }
}

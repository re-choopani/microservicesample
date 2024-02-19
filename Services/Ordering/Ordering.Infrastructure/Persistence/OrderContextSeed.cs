using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!await orderContext.Orders.AnyAsync())
            {
                await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("seed orders");
            }
        }
        public static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order
                {
                    FirstName = "sdfgs",
                    LastName = "sdfgs",
                    UserName ="ali",
                    EmailAddress = "sdfgs",
                    City = "sdfgs",
                    Country = "sdfgs",
                    TotalPrice = 0,
                    BankName="sdf",
                    PaymentMethod = 1,
                    RefCode = "Sdf",
                    LastModifiedBy = "ali",
                    ModifiedDate = DateTime.Now,
                }
            };
        }
    }
}

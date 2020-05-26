using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Repos;
using Xunit;

namespace Telegram.Bot.Sales.Tests.Repos
{
    public class NatificationRepoTests:SqlServerTest
    {
        public NatificationRepoTests()
        {
            Seed();
        }
        private void Seed()
        {
            using var context = new ApplicationContext(ContextOptions, null);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public void CanAdd()
        {
            //
            using var context = new ApplicationContext(ContextOptions, null);
            var notif = new Notification()
            {
                OrderCurrent = new OrderCurrent()
                {
                    Customer = new Customer() { CodeTelegram = 123456, Email = "test@test.com", Name = "Name1" },
                    Product = new Product() { Name = "Product", ProductCod = "cod", ShopId = 1 }, 
                    ExpectedPercentDiscount=5,
                    TimeStartNotif=DateTime.Now
                },
                OrderCurrentId=1,
                TypeNotificationId=1
            };

            var repo  = new NotificationRepo(context);
            //
            var result = repo.Add(notif).Result;
            var list = context.Notifications.Include(n=>n.OrderCurrent).ThenInclude(n=>n.Product).ToList();
            //
            Assert.Equal(4, result);
            Assert.Single(list);         
            Assert.Equal("Product", list[0].OrderCurrent.Product.Name);
          
        }
    }
}

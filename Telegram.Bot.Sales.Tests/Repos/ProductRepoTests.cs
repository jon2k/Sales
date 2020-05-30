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
    public class ProductRepoTests : SqlServerTest
    {
        public ProductRepoTests()
        {
            Seed();
        }
        private void Seed()
        {
            using var context = new ApplicationContext(ContextOptions, null);
          //  context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var pr= context.Products.Where(n => n.Url == "www.test.com");
            context.Products.RemoveRange(pr);
            context.SaveChanges();
        }
        [Fact]
        public void CanAdd()
        {
            //
            using var context = new ApplicationContext(ContextOptions, null);
            var shop = context.Shops.FirstOrDefault(n => n.Id == 1);
            var product = new Product()
            {
              Name="Name", Url="www.test.com", Shop=shop
            };

            var repo = new ProductRepo(context);
            //
            var result = repo.Add(product).Result;
            var list = context.Products.ToList();
            //
            
            Assert.Equal("www.test.com", list[0].Url);

        }
    }
}

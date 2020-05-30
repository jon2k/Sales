using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Repos;
using Xunit;

namespace Telegram.Bot.Sales.Tests.Repos
{
    public class CustomerRepoTests:SqlServerTest
    {
        public CustomerRepoTests()
        {       
            Seed();
        }     

        private void Seed()
        {
            using var context = new ApplicationContext(ContextOptions, null);
            // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var cus = context.Customers.Where(n => n.Name == "Name1" || n.Name == "Name2");
            context.Customers.RemoveRange(cus);
            context.SaveChanges();
            var one = new Customer() { CodeTelegram = 123456, Email = "test@test.com", Name = "Name1" };
            var two = new Customer() { CodeTelegram = 123456789, Email = "test2@test.com", Name = "Name2", IsDeleted = true };

            context.Customers.AddRange(one, two);

            context.SaveChanges();
        }

        [Fact]
        public void CanGetAll()
        {
            using var context = new ApplicationContext(ContextOptions, null);
            var customerRepos = new CustomerRepo(context);

            var customers = customerRepos.GetAll();

            Assert.Equal(2, customers.Count);
            Assert.Equal("Name1", customers[0].Name);
            Assert.Equal("Name2", customers[1].Name);
            Assert.Equal("test@test.com", customers[0].Email);
            Assert.Equal(123456, customers[0].CodeTelegram);
            Assert.Equal(123456789, customers[1].CodeTelegram);
        }
        [Fact]
        public void CanGetOneByCodTelegram()
        {
            using var context = new ApplicationContext(ContextOptions, null);
            var customerRepos = new CustomerRepo(context);

            var customers = customerRepos.GetOneByCodTelegram(123456);
            var customers1 = customerRepos.GetOneByCodTelegram(12345);

            Assert.Equal("Name1", customers.Name);
            Assert.Null(customers1);
        }
        [Fact]
        public void CanGetDeletedCustomers()
        {
            using var context = new ApplicationContext(ContextOptions, null);
            var customerRepos = new CustomerRepo(context);

            var customers = customerRepos.GetDeletedCustomers(123456);
            var customers1 = customerRepos.GetOneByCodTelegram(123456789);

            Assert.Null(customers);
            Assert.Equal("Name2",customers1.Name);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Tests
{
    public abstract class SqlServerTest
    {
        protected DbContextOptions<ApplicationContext> ContextOptions { get; }

        public SqlServerTest()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlServer(@"Data Source=EVGENIY-PC\SQLEXPRESS;Initial Catalog=discount; Integrated Security=true")
                    .Options;
        }
       
    }
}

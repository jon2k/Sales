using System;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Ozon : IParser
    {
        private readonly ApplicationContext _context;
        public Ozon(ApplicationContext context)
        {
            _context = context;
        }
        public Shop Shop => throw new NotImplementedException();

        public Task<(Product, string)> Parsing(string urlProduct)
        {
            throw new NotImplementedException();
        }

        public Task<(Product, string)> ParsingProductAsync(string urlProduct)
        {
            throw new NotImplementedException();
        }

        public Task<(ProductPriceHistory, string)> ParsingPriceAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

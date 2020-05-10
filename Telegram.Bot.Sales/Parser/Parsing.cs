using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Parser
{
    public class Parsing : IParser
    {
        private readonly ApplicationContext _context;
        public Parsing(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<(Product product, string msgAlarm)> ParsingProductAsync(string urlProduct)
        {
            if (urlProduct != null)
            {
                var definitionParser = new DedinitionParser(_context);
                IParser typeParser = definitionParser.DefinitionTypeParser(urlProduct, out string message);
                if (message == null)
                {
                    return await typeParser.ParsingProductAsync(urlProduct);
                }
                else
                {
                    return (product: null, msgAlarm: message);
                }
            }
            else
            {
                return (product: null, msgAlarm: "Argument must not be null.");
            }
        }


        public async Task<(ProductPriceHistory productPriceHistory, string msgAlarm)> ParsingPriceAsync(Product product)
        {
            IParser parserPrice;
            if (product != null)
            {
                if (product.Shop.Parser!=null)
                {
                    parserPrice = product.Shop.Parser;
                }
                else
                {
                    var definitionParser = new DedinitionParser(_context);
                    parserPrice = definitionParser.DefinitionTypeParser(product.Url, out string message);
                }
                //var parserPrice = product.Shop.Parser;
                return await parserPrice.ParsingPriceAsync(product);
            }
            else
            {
                return (productPriceHistory: null, msgAlarm: "Argument must not be null.");
            }
        }
    }
}

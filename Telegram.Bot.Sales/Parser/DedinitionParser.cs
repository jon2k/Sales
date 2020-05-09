using System;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Parser.ShopsParser;

namespace Telegram.Bot.Sales.Parser
{
    public class DedinitionParser
    {
        private readonly ApplicationContext _context;
        public DedinitionParser(ApplicationContext context)
        {
            _context = context;
        }
        public IParser DefinitionTypeParser(string urlProduct, out string msg)
        {
            if (urlProduct != null)
            {

                try
                {
                    var uri = new Uri(urlProduct);
                    string host = uri.Host;
                    msg = null;
                    switch (host)
                    {
                        case "www.ozon.ru":
                            return new Ozon(_context);
                        case "ozon.ru":
                            return new Ozon(_context);
                        //  break;
                        case "www.wildberries.ru":
                            return new WB(_context);
                        //  break;
                        case "wildberries.ru":
                            return new WB(_context);
                        case "www.mvideo.ru":
                            return new Mvideo(_context);
                        case "mvideo.ru":
                            return new Mvideo(_context);

                        default:
                            msg = $"Sorry. We don't work with {host}.";
                            return null;
                            //   break;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    return null;
                }
            }
            else
            {
                msg = "Argument must not be null.";
                return null;
            }
        }
    }
}

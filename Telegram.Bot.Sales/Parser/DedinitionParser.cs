using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Parser.ShopsParser;

namespace Telegram.Bot.Sales.Parser
{
    public class DedinitionParser
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;
        public DedinitionParser(ApplicationContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
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
                        case "www.wildberries.ru":
                            return new WB(_context, _logger);
                        case "wildberries.ru":
                            return new WB(_context, _logger);
                        case "www.mvideo.ru":
                            return new Mvideo(_context, _logger);
                        case "mvideo.ru":
                            return new Mvideo(_context, _logger);
                        case "www.bask.ru":
                            return new Bask(_context, _logger);
                        case "bask.ru":
                            return new Bask(_context, _logger);
                        case "www.planeta-sport.ru":
                            return new PlanetaSport(_context, _logger);
                        case "planeta-sport.ru":
                            return new PlanetaSport(_context, _logger);
                        default:
                            msg = $"Sorry. We don't work with {host}.";
                            return null;
                            //   break;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    _logger.LogError($"{DateTime.Now} -- {nameof(DefinitionTypeParser)} --  {ex.Message}");
                    return null;
                }
            }
            else
            {
                msg = "Argument must not be null.";
                _logger.LogError($"{DateTime.Now} -- {nameof(DefinitionTypeParser)} --  {msg}");
                return null;
            }
        }
    }
}

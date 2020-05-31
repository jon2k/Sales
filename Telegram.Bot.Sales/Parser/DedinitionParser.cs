using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Parser.ShopsParser;
using Telegram.Bot.Sales.Parser.ShopsParser.Shops;

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
                    if (!host.Contains("www."))
                    {
                        host = "www." + host;
                    }
                    msg = null;
                    switch (host)
                    {                   
                        case "www.wildberries.ru":
                            return new WB(_context, _logger);                     
                        case "www.mvideo.ru":
                            return new Mvideo(_context, _logger);                       
                        case "www.bask.ru":
                            return new Bask(_context, _logger);                  
                        case "www.planeta-sport.ru":
                            return new PlanetaSport(_context, _logger);
                        case "www.lamoda.ru":
                            return new Lamoda(_context, _logger);
                        case "www.trial-sport.ru":
                            return new TrialSport(_context, _logger);
                        case "www.velostrana.ru":
                            return new Velostrana(_context, _logger);
                        case "www.velosklad.ru":
                            return new Velosklad(_context, _logger);
                        case "www.velodrive.ru":
                            return new Velodrive(_context, _logger);

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

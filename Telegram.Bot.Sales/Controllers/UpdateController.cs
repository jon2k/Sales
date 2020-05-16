using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Sales.SelectService;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly ISelectService _selectService;

        public UpdateController(ISelectService selectService)
        {
            _selectService = selectService;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await _selectService.SelectTypeUpdateAndExecute(update);
            return Ok();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Successfull" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

    }
}

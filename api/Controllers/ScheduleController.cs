using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly IBackgroundMessage _bgMessage;
        private readonly IConfiguration _configuration;

        public ScheduleController(IBackgroundMessage bgMessage, IConfiguration configuration)
        {
            _bgMessage = bgMessage;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ScheduleModel model)
        {
            string signature = Request.Headers[_configuration["Scheduler:Header"]!]!;
            await _bgMessage.CreateScheduleJob(model, signature);
            return Ok();
        }
    }
}
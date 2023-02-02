using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace api.Controllers
{
    [Route("api/v1/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly IBackgroundMessage _bgMessage;

        public ScheduleController(IBackgroundMessage bgMessage)
        {
            _bgMessage = bgMessage;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ScheduleModel model)
        {
            await _bgMessage.CreateScheduleJob(model);
            return Ok();
        }
    }
}
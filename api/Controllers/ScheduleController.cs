using api.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace api.Controllers
{
    [Route("api/v1/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly ISchedulerFactory _schedulerFactory;

        public ScheduleController(ILogger<ScheduleController> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            IScheduler _scheduler = await _schedulerFactory.GetScheduler();
            await _scheduler.ScheduleJob(job, trigger);
            return Ok();
        }
    }
}
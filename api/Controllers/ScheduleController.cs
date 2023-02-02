using api.Models;
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ScheduleModel model)
        {
            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity(model.JobName!, model.GroupName!)
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            var trigger = TriggerBuilder.Create()
                .WithIdentity(model.TriggerName!, model.GroupName!)
                .UsingJobData("group_name", model.GroupName!)
                .UsingJobData("trigger_name", model.TriggerName!)
                .UsingJobData("job_name", model.JobName!)
                .WithDescription(model.Description!)
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
using api.Interfaces;
using api.Models;
using Quartz;

namespace api.Services
{
    public class BackgroundMessageService : IBackgroundMessage
    {
        private readonly ILogger<BackgroundMessageService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;


        public BackgroundMessageService(ILogger<BackgroundMessageService> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }
        public async Task CreateScheduleJob(ScheduleModel model)
        {
            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<BackgroundMessageJob>()
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
        }
    }
}
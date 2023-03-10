using api.Exceptions;
using api.Interfaces;
using api.Models;
using api.Stores;
using Quartz;

namespace api.Services
{
    public class BackgroundMessageService : IBackgroundMessage
    {
        private readonly ILogger<BackgroundMessageService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IRequestValidator _reqVal;

        private readonly ITriggerHandler _triggerHandler;


        public BackgroundMessageService(ILogger<BackgroundMessageService> logger, ISchedulerFactory schedulerFactory, IRequestValidator reqVal, ITriggerHandler triggerHandler)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _reqVal = reqVal;
            _triggerHandler = triggerHandler;
        }
        public async Task CreateScheduleJob(ScheduleModel model, string signature)
        {
            // check the signature
            // try
            // {
            var isValidate = _reqVal.Validate(model, signature);

            if (!isValidate.Item1)
                throw new ErrorResponseException(
                    StatusCodes.Status401Unauthorized,
                    "Signature validation failed!",
                    new List<Error>()
                );
            // _logger.LogInformation($"{isValidate.Item1} | sig | {signature}");
            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<BackgroundMessageJob>()
                .WithIdentity(model.JobName!, model.GroupName!)
                .Build();

            // Trigger the job to run now, and then every 40 seconds

            if (model.TriggerType!.ToUpper() == TriggerTypes.Cron)
            {
                IScheduler _scheduler = await _schedulerFactory.GetScheduler();
                ITrigger trigger = _triggerHandler.CreateConTrigger(model);
                await _scheduler.ScheduleJob(job, trigger);
                return;
            }

            if (model.TriggerType!.ToUpper() == TriggerTypes.Interval)
            {
                IScheduler _scheduler = await _schedulerFactory.GetScheduler();
                ITrigger trigger = _triggerHandler.CreateIntervalTrigger(model);
                await _scheduler.ScheduleJob(job, trigger);
                return;
            }

            throw new ErrorResponseException(
                    StatusCodes.Status400BadRequest,
                    "Invalid trigger type!",
                    new List<Error>()
                );
        }
    }
}
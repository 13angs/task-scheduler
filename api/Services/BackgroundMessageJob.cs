using api.Models;
using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using Simple.RabbitMQ;

namespace api.Services
{
    public class BackgroundMessageJob : IJob
    {
        private readonly IMessagePublisher _publisher;
        private readonly ILogger<BackgroundMessageJob> _logger;
        private readonly IConfiguration _configuration;

        public BackgroundMessageJob(IMessagePublisher publisher, ILogger<BackgroundMessageJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _publisher = publisher;
            _publisher.Connect(
                _configuration!["RabbitMQ:HostName"]!,
                _configuration!["RabbitMQ:ExchangeName"]!,
                ExchangeType.Fanout
            );
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobDataMap = context.MergedJobDataMap;
            string jobName = jobDataMap.GetString("job_name")!;
            string groupName = jobDataMap.GetString("group_name")!;
            string triggerName = jobDataMap.GetString("trigger_name")!;
            string description = jobDataMap.GetString("description")!;
            string triggerType = jobDataMap.GetString("trigger_type")!;
            string cronStr = jobDataMap.GetString("cron_str")!;

            ScheduleModel model = new ScheduleModel
            {
                JobName = jobName,
                GroupName = groupName,
                TriggerName = triggerName,
                Description = description,
                TriggerType = triggerType,
                CronStr = cronStr
            };

            string message = JsonConvert.SerializeObject(model);

            // publish the message
            _publisher.Publish(message, _configuration["RabbitMQ:RouteKey"], null);
            await Console.Out.WriteLineAsync();
        }
    }
}
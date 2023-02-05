using api.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Simple.RabbitMQ;

namespace api.BgServices
{
    public class BackgroundMessageCollector : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundMessageCollector> _logger;
        private readonly IConfiguration _configuration;

        public BackgroundMessageCollector(IServiceProvider serviceProvider, ILogger<BackgroundMessageCollector> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IMessageSubscriber subscriber = scope.ServiceProvider.GetRequiredService<IMessageSubscriber>();
                subscriber.Connect(
                    _configuration!["RabbitMQ:HostName"]!,
                    _configuration!["RabbitMQ:ExchangeName"]!,
                    _configuration!["RabbitMQ:QueueName"]!,
                    _configuration!["RabbitMQ:RouteKey"]!,
                    null,
                    ExchangeType.Fanout
                );

                subscriber.Subscribe(processMessage);
                return Task.CompletedTask;
            }
        }

        public bool processMessage(string message, IDictionary<string, object> headers)
        {
            // _logger.LogInformation(message);
            ScheduleModel model = JsonConvert.DeserializeObject<ScheduleModel>(message)!;

            if(model != null)
            {
                _logger.LogInformation($"group: {model.GroupName} | job: {model.JobName} | trigger_type: {model.TriggerType}");
                return true;
            }
            return false;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
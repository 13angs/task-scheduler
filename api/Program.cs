using api.BgServices;
using api.Exceptions;
using api.Interfaces;
using api.Services;
using Newtonsoft.Json.Serialization;
using Quartz;
using Simple.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add quartz
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UsePersistentStore(opt => {
        string cs = configuration["ConnectionStrings:Mysql"]!;
        // opt.UseClustering();
        opt.UseMySql(mysql => {
            mysql.ConnectionString=cs;
            mysql.TablePrefix="QRTZ_";
        });
        opt.UseJsonSerializer();
    });
});
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});


// configure controller to use Newtonsoft as a default serializer
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            .Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                    = new DefaultContractResolver()
);

builder.Services.AddScoped<IBackgroundMessage, BackgroundMessageService>();
builder.Services.AddScoped<IRequestValidator, RequestValidator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
builder.Services.AddScoped<IMessageSubscriber, MessageSubscriber>();
builder.Services.AddHostedService<BackgroundMessageCollector>();
builder.Services.AddScoped<ITriggerHandler, TriggerHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.UseResponseExceptionHandler();

app.MapControllers();


// await SampleTask.Run();

// using (var scope = builder.Services.BuildServiceProvider().CreateScope())
// {

//     var schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
//     var scheduler = await schedulerFactory.GetScheduler();
//     // define the job and tie it to our HelloJob class
//     var job = JobBuilder.Create<HelloJob>()
//         .WithIdentity("myJob", "group1")
//         .Build();

//     // Trigger the job to run now, and then every 40 seconds
//     var trigger = TriggerBuilder.Create()
//         .WithIdentity("myTrigger", "group1")
//         .StartNow()
//         .WithSimpleSchedule(x => x
//             .WithIntervalInSeconds(10)
//             .WithRepeatCount(3))
//         .Build();
//     var job2 = JobBuilder.Create<HelloJob>()
//         .WithIdentity("myJob2", "group2")
//         .Build();

//     // Trigger the job to run now, and then every 40 seconds
//     var trigger2 = TriggerBuilder.Create()
//         .WithIdentity("myTrigger2", "group2")
//         .StartNow()
//         .WithSimpleSchedule(x => x
//             .WithIntervalInSeconds(10)
//             .WithRepeatCount(3))
//         .Build();

//     await scheduler.ScheduleJob(job, trigger);
//     await scheduler.ScheduleJob(job2, trigger2);
// }


await app.RunAsync();

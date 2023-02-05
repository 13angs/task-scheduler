using api.Interfaces;
using api.Models;
using Quartz;

namespace api.Services
{
    public class TriggerHandler : ITriggerHandler
    {
        public ITrigger CreateConTrigger(ScheduleModel model)
        {
            try{
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(model.TriggerName!, model.GroupName!)
                    .UsingJobData("group_name", model.GroupName!)
                    .UsingJobData("job_name", model.JobName!)
                    .UsingJobData("trigger_name", model.TriggerName!)
                    .UsingJobData("description", model.Description!)
                    .UsingJobData("cron_str", model.CronStr!)
                    .UsingJobData("trigger_type", model.TriggerType!)
                    .WithDescription(model.Description!)
                    .WithCronSchedule(model.CronStr!)
                    .ForJob(model.JobName!, model.GroupName!)
                    .Build();
                return trigger;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
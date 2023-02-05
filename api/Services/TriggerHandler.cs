using api.Interfaces;
using api.Models;
using api.Stores;
using Quartz;

namespace api.Services
{
    public class TriggerHandler : ITriggerHandler
    {
        public ITrigger CreateConTrigger(ScheduleModel model)
        {
            try
            {
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
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public ITrigger CreateIntervalTrigger(ScheduleModel model)
        {
            try
            {
                var config = TriggerBuilder.Create()
                    .WithIdentity(model.TriggerName!, model.GroupName!)
                    .UsingJobData("group_name", model.GroupName!)
                    .UsingJobData("job_name", model.JobName!)
                    .UsingJobData("trigger_name", model.TriggerName!)
                    .UsingJobData("description", model.Description!)
                    .UsingJobData("cron_str", model.CronStr!)
                    .UsingJobData("trigger_type", model.TriggerType!)
                    .WithDescription(model.Description!)
                    .WithDailyTimeIntervalSchedule(x => x
                    .OnDaysOfTheWeek(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday)
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(9, 0))
                    .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 0)))
                    .StartNow();

                ITrigger trigger = TriggerBuilder.Create().Build();

                // interval in seconds
                if (model.IntervalIn!.ToUpper() == IntervalInTypes.Second)
                    trigger = config
                        .WithCalendarIntervalSchedule(x => x
                            .WithIntervalInSeconds(model.IntervalValue))
                        .ForJob(model.JobName!, model.GroupName!)
                        .Build();
                // interval in minutes
                if (model.IntervalIn!.ToUpper() == IntervalInTypes.Minute)
                    trigger = config
                        .WithCalendarIntervalSchedule(x => x
                            .WithIntervalInMinutes(model.IntervalValue))
                        .ForJob(model.JobName!, model.GroupName!)
                        .Build();
                // interval in hours
                if (model.IntervalIn!.ToUpper() == IntervalInTypes.Hour)
                    trigger = config
                        .WithCalendarIntervalSchedule(x => x
                            .WithIntervalInHours(model.IntervalValue))
                        .ForJob(model.JobName!, model.GroupName!)
                        .Build();

                // interval in days
                if (model.IntervalIn!.ToUpper() == IntervalInTypes.Day)
                    trigger = config
                        .WithCalendarIntervalSchedule(x => x
                            .WithIntervalInDays(model.IntervalValue))
                        .ForJob(model.JobName!, model.GroupName!)
                        .Build();

                return trigger;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
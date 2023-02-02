using Quartz;

namespace api.Services
{
    public class BackgroundMessageJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var jobDataMap = context.MergedJobDataMap;
            string jobName = jobDataMap.GetString("job_name")!;
            string groupName = jobDataMap.GetString("group_name")!;
            string triggerName = jobDataMap.GetString("trigger_name")!;
            await Console.Out.WriteLineAsync($"group: {groupName} | job: {jobName} | trigger: {triggerName}");
        }
    }
}
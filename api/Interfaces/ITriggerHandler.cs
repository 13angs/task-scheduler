using api.Models;
using Quartz;

namespace api.Interfaces
{
    public interface ITriggerHandler
    {
        public ITrigger CreateConTrigger(ScheduleModel model);
        public ITrigger CreateIntervalTrigger(ScheduleModel model);
    }
}
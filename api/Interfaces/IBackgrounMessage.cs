using api.Models;

namespace api.Interfaces
{
    public interface IBackgroundMessage
    {
        public Task CreateScheduleJob(ScheduleModel model);
    }
}
using api.Models;

namespace api.Interfaces
{
    public interface IRequestValidator
    {
        public Tuple<bool, string> Validate(ScheduleModel content, string signature);
    }
}
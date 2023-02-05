using System.Security.Cryptography;
using System.Text;
using api.Interfaces;
using api.Models;
using Newtonsoft.Json;

namespace api.Services
{
    public class RequestValidator : IRequestValidator
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor contextAccessor;

        public RequestValidator(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this.configuration = configuration;
            this.contextAccessor = contextAccessor;
        }
        public Tuple<bool, string> Validate(ScheduleModel content, string signature)
        {
            // get the x-static-signature header for validation
            // string schedulerSig = contextAccessor.HttpContext!.Request.Headers[configuration["Scheduler:Header"]!]!;

            // get static secret id from appsetting.json
            string schedulerSecret = configuration["Scheduler:Secret"]!;
            // convert secret key to byte array
            var signKeyBytes = Convert.FromBase64String(schedulerSecret);

            using (var hmacsha256 = new HMACSHA256(signKeyBytes))
            {
                string strContent = JsonConvert.SerializeObject(new
                {
                    group_name=content.GroupName,
                    trigger_name=content.TriggerName, 
                    job_name=content.JobName, 
                    description=content.Description,
                    trigger_type=content.TriggerType,
                    cron_str=content.CronStr,
                    interval_in=content.IntervalIn,
                    interval_value=content.IntervalValue,
                });
            var bytes = Encoding.UTF8.GetBytes(strContent);

            var hashResult = hmacsha256.ComputeHash(bytes);
            var contentSignature = Convert.ToBase64String(hashResult);

            if (signature == contentSignature)
            {
                return new(true, contentSignature);
            }
        }

            return new (false, "");
        }

}
}
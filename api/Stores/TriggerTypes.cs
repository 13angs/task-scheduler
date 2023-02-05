namespace api.Stores
{
    public static class TriggerTypes
    {
        public static string? Cron = "CRON";
        public static string? Interval = "INTERVAL";
        public static List<string> Types = new List<string>{
            Cron,
            Interval
        };
    }
}
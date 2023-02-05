namespace api.Stores
{
    public static class IntervalInTypes
    {
        public static string? Second = "SECOND";
        public static string? Minute = "MINUTE";
        public static string? Hour = "HOUR";
        public static string? Day = "DAY";

        public static List<string> Types = new List<string>{
            Second,
            Minute,
            Hour,
            Day
        };
    }
}
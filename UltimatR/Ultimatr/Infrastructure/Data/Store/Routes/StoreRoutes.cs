namespace UltimatR
{
    public static class StoreRoutes
    {
        public static string EntryStore { get; set; }  = "ds/entry";
        public static string EventStore { get; set; }  = "ds";
        public static string ReportStore { get; set; } = "ds/report";
        public static string StateStore { get; set; }  = "ds/state";
        public static string ConfigStore { get; set; } = "ds/config";

        public static class Constant
        {
            public const string EntryStore =  "ds/entry";
            public const string EventStore =  "ds";
            public const string ReportStore = "ds/report";
            public const string StateStore =  "ds/state";
            public const string ConfigStore = "ds/config";
        }
    }
}


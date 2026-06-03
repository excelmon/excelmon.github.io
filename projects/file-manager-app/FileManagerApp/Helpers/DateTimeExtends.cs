namespace AppLibrary.Helpers
{
    public static class DateTimeExtends
    {
        public static DateOnly ToDateOnly(this DateTime date)
        {
            return DateOnly.FromDateTime(date);
        }

        public static string NowAsString() 
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return timestamp;
        }

        public static string TodayAsString() 
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd");
            return timestamp;
        }
    }
}

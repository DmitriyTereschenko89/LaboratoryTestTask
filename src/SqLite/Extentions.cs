namespace SqLite;
    public static class Extentions
    {
        public static string GetConnectionString(this SqLiteOptions sqLiteOptions)
        {
            return $"Data Source={sqLiteOptions.DataSource};";
        }
    }

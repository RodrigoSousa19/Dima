namespace Dima.Core
{
    public static class Configurations
    {
        public const int DefaultPageSize = 25;
        public const int DefaultPageNumber = 1;
        public const int DefaultStatusCode = 200;
        public const decimal PremiumPrice = 799.90M;
        public static string ConnectionString { get; set; } = string.Empty;
        public static string BackEndUrl { get; set; } = string.Empty;
        public static string FrontEndUrl { get; set; } = string.Empty;
    }
}

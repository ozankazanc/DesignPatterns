namespace WebApp.Strategy.Models
{
    public class Settings
    {
        public static string claimDatabaseType = "databasetype";
        public EDatabaseType databaseType;
        public EDatabaseType defaultDatabaseType => EDatabaseType.SqlServer;
    }
}

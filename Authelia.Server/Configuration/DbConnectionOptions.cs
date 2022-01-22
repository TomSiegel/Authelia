namespace Authelia.Server.Configuration
{
    public class DbConnectionOptions
    {
        public string MySql { get; set; }
        public string MsSql { get; set; }
        public string Oracle { get; set; }

        public string GetConnectionString()
        {
            if (!String.IsNullOrEmpty(MySql)) return MySql;
            if (!String.IsNullOrEmpty(MsSql)) return MsSql;
            if (!String.IsNullOrEmpty(Oracle)) return Oracle;

            return null;
        }
    }
}

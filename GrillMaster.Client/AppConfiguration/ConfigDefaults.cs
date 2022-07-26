namespace GrillMaster.Client.AppConfiguration
{
    internal static class ConfigDefaults
    {
        public static class Placeholders
        {
            public const string Host = "__HOST__";
            public const string Port = "__PORT__";
            public const string UserName = "__USERNAME__";
            public const string Password = "__PASSWORD__";
        }

        public static class ServerOptions
        {
            public const string Server = "http://localhost";
            public const int Port = 5008;
        }

        public static class UserOptions
        {
            public const string UserName = "user";
            public const string Password = "password";
        }
    }
}

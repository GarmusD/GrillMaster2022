using Microsoft.Extensions.Configuration;

namespace GrillMaster.Client.AppConfiguration
{
    public class AppConfig
    {
        public ServerOptions Server { get; set; } = new ServerOptions();
        public AuthOptions Auth { get; set; } = new AuthOptions();
        public OutputOptions Output { get; set; } = new OutputOptions();

        public bool CmdLineHandled { get; private set; } = true;
        public bool Verbose { get; internal set; } = false;

        public static AppConfig? Load(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);

            var devEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (devEnv != null && devEnv.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
            { 
                builder.AddJsonFile($"appsettings.{devEnv}.json", true, true);
            }

            try
            {
                var config = builder.Build();
                AppConfig? appCfg = config.Get<AppConfig>();
                if (appCfg is null) return null;
                appCfg.CmdLineHandled = CmdLineConfig.Merge(args, appCfg);
                return appCfg;
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }
    }
}

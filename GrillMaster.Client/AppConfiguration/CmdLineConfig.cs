using System.CommandLine;
using System.CommandLine.Invocation;

namespace GrillMaster.Client.AppConfiguration
{
    internal class CmdLineConfig
    {
        /// <summary>
        /// Merge command line options into AppConfig
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="appConfig">Instance of AppConfig</param>
        /// <returns>Return True if command line is handled (-h, --help)</returns>
        internal static bool Merge(string[] args, AppConfig appConfig)
        {
            bool handled = true;

            var outputOption = new Option<Output>(new[] { "--output", "-o" }, "Specify how the app should render a grilled items.") { IsRequired = true };
            var jsonFileNameOption = new Option<FileInfo?>(new[] { "--filename", "-f" }, () => { return new FileInfo("grilled_order.json"); }, "Filename for json output.");

            var serverHostOption = new Option<string?>(new[] { "--server", "-s" }, "Host where GrillMaster.API app is running.");
            var serverPortOption = new Option<int?>(new[] { "--port", "-t" }, "Port where GrillMaster.API app is running.");

            var authUsernameOption = new Option<string?>(new string[] { "--user", "-u" }, "Username to authenticate.");
            var authPasswordOption = new Option<string?>(new string[] { "--password", "-p" }, "Password to authenticate.");

            var verboseOption = new Option<bool>(new string[] { "--verbose", "-v" }, () => { return false; }, "Display additional details.");

            RootCommand rootCommand = new("Client app for GrillMaster2022 Optimizer API.")
            {
                outputOption,
                jsonFileNameOption,
                serverHostOption,
                serverPortOption,
                authUsernameOption,
                authPasswordOption,
                verboseOption
            };

            rootCommand.SetHandler((context) => 
                {
                    new ServerBinder(appConfig.Server, serverHostOption, serverPortOption).Bind(context);
                    new AuthBinder(appConfig.Auth, authUsernameOption, authPasswordOption).Bind(context);
                    new OutputBinder(appConfig.Output, outputOption, jsonFileNameOption).Bind(context);
                    new VerboseBinder(appConfig, verboseOption).Bind(context);
                    handled = false;
                });

            rootCommand.Invoke(args);
            return handled;
        }

        private class ServerBinder
        {
            private readonly Option<string?> _serverHostOption;
            private readonly Option<int?> _serverPortOption;
            private readonly ServerOptions _serverOptions;

            public ServerBinder(ServerOptions serverOptions, Option<string?> serverHostOption, Option<int?> serverPortOption)
            {
                _serverHostOption = serverHostOption;
                _serverPortOption = serverPortOption;
                _serverOptions = serverOptions;
            }

            public void Bind(InvocationContext invocationContext)
            {
                var host = invocationContext.ParseResult.GetValueForOption(_serverHostOption);
                var port = invocationContext.ParseResult.GetValueForOption(_serverPortOption);
                if (host is not null) _serverOptions.Host = host;
                if (port is not null) _serverOptions.Port = port.Value;
            }
        }

        private class AuthBinder
        {
            private readonly Option<string?> _authUsernameOption;
            private readonly Option<string?> _authPasswordOption;
            private readonly AuthOptions _authOptions;

            public AuthBinder(AuthOptions authOptions, Option<string?> authUsernameOption, Option<string?> authPasswordOption)
            {
                _authUsernameOption = authUsernameOption;
                _authPasswordOption = authPasswordOption;
                _authOptions = authOptions;
            }

            public void Bind(InvocationContext invocationContext)
            {
                var username = invocationContext.ParseResult.GetValueForOption(_authUsernameOption);
                var password = invocationContext.ParseResult.GetValueForOption(_authPasswordOption);
                if (username is not null) _authOptions.UserName = username;
                if (password is not null) _authOptions.Password = password;
            }
        }

        private class OutputBinder
        {
            private readonly Option<Output> _outputOption;
            private readonly Option<FileInfo?> _jsonFileNameOption;
            private readonly OutputOptions _outputOptions;

            public OutputBinder(OutputOptions outputOptions, Option<Output> outputOption, Option<FileInfo?> jsonFileNameOption)
            {
                _outputOption = outputOption;
                _jsonFileNameOption = jsonFileNameOption;
                _outputOptions = outputOptions;
            }

            public void Bind(InvocationContext invocationContext)
            {
                var output = invocationContext.ParseResult.GetValueForOption(_outputOption);
                var jsonFileName = invocationContext.ParseResult.GetValueForOption(_jsonFileNameOption);
                _outputOptions.Output = output;
                _outputOptions.JsonOutputPath = output switch
                {
                    Output.Console => null,
                    Output.Json => jsonFileName,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private class VerboseBinder
        {
            private readonly AppConfig _appConfig;
            private readonly Option<bool> _verboseOption;

            public VerboseBinder(AppConfig appConfig, Option<bool> verboseOption)
            {
                _appConfig = appConfig;
                _verboseOption = verboseOption;
            }

            public void Bind(InvocationContext invocationContext)
            {
                _appConfig.Verbose = invocationContext.ParseResult.GetValueForOption(_verboseOption);
            }
        }
    }
}

using GrillMaster.Client;
using GrillMaster.Client.AppConfiguration;
using GrillMaster.Client.Exceptions;
using GrillMaster.Client.Renderer;
using RestSharp;

const string MenuGeneratorAPI = "https://grill-master-svc.herokuapp.com/api/menus";

AppConfig? appCfg = AppConfig.Load(args);

CheckAppConfig();

if (appCfg!.CmdLineHandled) Environment.Exit(0);

VerboseLine("Client for GrillOptimizer2022 API.");

string grillMenu = GetMenuFromGenerator();

string optimizedOrder = GetOptimizedOrder(appCfg, grillMenu);

RenderOptimizedOrder(optimizedOrder);
// Program Done!



// *************************************************
// Methods
// *************************************************
void Verbose(string message = "")
{
    if (appCfg.Verbose) Console.Write(message);
}

void VerboseLine(string message = "")
{
    if (appCfg.Verbose) Console.WriteLine(message);
}

void CheckAppConfig()
{
    if (appCfg == null)
    {
        Console.WriteLine("Please check the configuration file 'appsettings.json'. It may be malformed.");
        Environment.Exit(ExitCodes.JsonConfigMalformed);
    }
}

string GetMenuFromGenerator()
{
    Verbose("Requesting menu from the menu generator... ");

    Uri menuApiUri = new (MenuGeneratorAPI);
    UriBuilder builder = new () { Scheme = menuApiUri.Scheme, Host = menuApiUri.Host };

    RestClient menuApiClient = new (builder.Uri);
    RestRequest request = new (menuApiUri.PathAndQuery);
    RestResponse response = menuApiClient.Execute(request);
    if (response.StatusCode == 0 || response.Content is null)
    {
        if (appCfg.Verbose) Console.WriteLine();
        Console.WriteLine("Error while requesting menu generator API. Try again later.");
        Environment.Exit(ExitCodes.MenuGeneratorOffline);
    }

    VerboseLine("Done.");
    return response.Content;
}

string GetOptimizedOrder(AppConfig appCfg, string grillMenu)
{
    GrillMasterClient gmClient = new(appCfg);
    try
    {
        Verbose("Optimizing grill order... ");
        string result = gmClient.DoGrillOrder(grillMenu);
        VerboseLine("Done.");
        return result;
    }
    catch (GrillMasterBaseException ex)
    {
        if (appCfg.Verbose) Console.WriteLine();
        Console.WriteLine(ex.Message);
        Environment.Exit(ex.ExitCode);
    }
    //Silence the compiler
    return string.Empty;
}

void RenderOptimizedOrder(string optOrder)
{
    try
    {
        Renderer.RenderOutput(appCfg!.Output, optOrder);
    }
    catch (GrillMasterBaseException gmEx)
    {
        Console.WriteLine(gmEx.Message);
        Environment.Exit(gmEx.ExitCode);
    }
}
using GrillMaster.Client;
using GrillMaster.CLI.AppConfiguration;
using GrillMaster.CLI.Exceptions;
using GrillMaster.CLI.Renderer;
using System.Text.Json;
using GrillMaster.CLI;
using GrillMaster.Data.DTO;

AppConfig? appCfg = AppConfig.Load(args);

CheckAppConfig();

if (appCfg!.CmdLineHandled) Environment.Exit(0);

VerboseLine("Client for GrillOptimizer2022 API.");

var grillMenu = await GetMenuFromGeneratorAsync();

var optimizedOrder = await GetOptimizedOrderAsync(appCfg, grillMenu);

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

async Task<GrillOrder> GetMenuFromGeneratorAsync()
{
    const string MenuGeneratorAPI = "https://grill-master-svc.herokuapp.com/api/menus";

    Verbose("Requesting menu from the menu generator... ");
    HttpClient client = new ();
    var response = await client.GetAsync(MenuGeneratorAPI);
    if (response.StatusCode == 0)
    {
        if (appCfg.Verbose) Console.WriteLine();
        Console.WriteLine("Error while requesting menu generator API. Try again later.");
        Environment.Exit(ExitCodes.MenuGeneratorOffline);
    }
    VerboseLine("Done.");

    try
    {
        string strResult = await response.Content.ReadAsStringAsync();
        GrillOrder? result = JsonSerializer.Deserialize<GrillOrder>(strResult, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (result is null) throw new GrillMasterInvalidJsonException();
        return result;
    }
    catch(Exception ex) when (ex is JsonException || ex is NotSupportedException)
    {
        Console.WriteLine("Error while requesting menu generator API. Try again later.");
        Environment.Exit(ExitCodes.MenuGeneratorOffline);
    }
    //Should not reach here.
    return null;
}

async Task<OptimizedOrder> GetOptimizedOrderAsync(AppConfig appCfg, GrillOrder grillOrder)
{
    using GrillMasterClient gmClient = new(appCfg.Server.Uri);
    await GMClientAuthenticateAsync(gmClient, new(appCfg.Auth.UserName, appCfg.Auth.Password));
    return await GMClientOptimizeOrderAsync(gmClient, grillOrder);
}

async Task GMClientAuthenticateAsync(GrillMasterClient client, AuthUserRequest authUser)
{
    try
    {
        Verbose("Authenticating to API server... ");
        var res = await client.AuthenticateAsync(authUser);
        VerboseLine("Done.");
        VerboseLine($"Auth token: {res}");
    }
    catch(GrillMasterOfflineException offEx)
    {
        Console.WriteLine(offEx.Message);
        Environment.Exit(ExitCodes.ApiServerOffline);
    }
    catch (GrillMasterNotAuthenticatedException authEx)
    {
        Console.WriteLine(authEx.Message);
        Environment.Exit(ExitCodes.AuthError);
    }
}

async Task<OptimizedOrder> GMClientOptimizeOrderAsync(GrillMasterClient client, GrillOrder grillOrder)
{
    try
    {
        VerboseLine("Optimizing grill order... ");
        return await client.OptimizeGrillOrderAsync(grillOrder);
    }
    catch (GrillMasterOfflineException offEx)
    {
        Console.WriteLine(offEx.Message);
        Environment.Exit(ExitCodes.ApiServerOffline);
    }
    catch(GrillMasterApiErrorException apiEx)
    { 
        Console.WriteLine(apiEx.Message);
        Environment.Exit(ExitCodes.ApiError);
    }
    catch(GrillMasterInvalidJsonException jsonEx)
    {
        Console.WriteLine(jsonEx.Message);
        Environment.Exit(ExitCodes.InvalidOptimizedJson);
    }
    //Should not reach here.
    return null;
}

void RenderOptimizedOrder(OptimizedOrder optOrder)
{
    try
    {
        Renderer.RenderOutput(appCfg!.Output, optOrder);
    }
    catch (GMBaseException gmEx)
    {
        Console.WriteLine(gmEx.Message);
        Environment.Exit(gmEx.ExitCode);
    }
}

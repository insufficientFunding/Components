﻿using Components.Interfaces.TypeDescription;
using Components.Logging;
using Components.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Components.IntegrationTest;

class Program
{
    public static IDictionary<string, IComponentDescription> ComponentDescriptions { get; private set; } = null!;

    #region Services
    public static IServiceProvider Services { get; private set; } = null!;

    private static void ConfigureServices ()
    {
        IServiceCollection services = new ServiceCollection ();

        services.AddLogging (x => LoggingSetup.SetupLogging (x, true, false));

        services.AddSingleton<IComponentService, ComponentService> ();

        Services = services.BuildServiceProvider ();
    }
    #endregion

    static void Main (string [] args)
    {
        ConfigureServices ();

        string solutionPath = TestHelper.GetSolutionPath ();

        string dataPath = $"{solutionPath}/Components.ComponentLibrary";

        IComponentService componentService = Services.GetRequiredService<IComponentService> ();

        componentService.ReadDescriptions (dataPath);
        
        string targetPath = $"{solutionPath}/Components.IntegrationTest/Svg";

        TestHelper.RenderAllToSvg (componentService.GetDescriptions(), targetPath);

    }
}

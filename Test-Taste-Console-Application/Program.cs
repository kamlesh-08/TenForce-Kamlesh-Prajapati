using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            // Configure the services.
            ConfigureServices(serviceCollection);

            // Execute the code that creates the outputs.
            RunServiceOperations(serviceCollection);
        }

        private static void RunServiceOperations(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the services from the service provider.
            var screenOutputService = serviceProvider.GetService<IOutputService>();

            try
            {
                Console.WriteLine("Starting output of planets and their average moon gravity...");
                screenOutputService.OutputAllPlanetsAndTheirAverageMoonGravityToConsole();
                Console.WriteLine("Completed output of planets and their average moon gravity.");

                Console.WriteLine("Starting output of all moons and their mass...");
                screenOutputService.OutputAllMoonsAndTheirMassToConsole();
                Console.WriteLine("Completed output of all moons and their mass.");

                Console.WriteLine("Starting output of all planets and their moons...");
                screenOutputService.OutputAllPlanetsAndTheirMoonsToConsole();
                Console.WriteLine("Completed output of all planets and their moons.");
            }
            catch (Exception exception)
            {
                // Display the thrown exceptions to the users and developers.
                Logger.Instance.Error($"{LoggerMessage.ScreenOutputOperationFailed} {exception.Message}");
                Console.WriteLine($"{ExceptionMessage.ScreenOutputOperationFailed} {exception.Message}");
            }
            finally
            {
                // Dispose of the service provider.
                serviceProvider.Dispose();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Configure all the services.
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()),
                new FileInfo(ConfigurationFileName.Logger));
            serviceCollection.AddHttpClient<HttpClientService>();
            serviceCollection.AddSingleton<IPlanetService, PlanetService>();
            serviceCollection.AddSingleton<IOutputService, ScreenOutputService>();
            serviceCollection.AddSingleton<IMoonService, MoonService>();
        }
    }

}

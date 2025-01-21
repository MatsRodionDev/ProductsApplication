using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace OrderService.API.Configuration;

public static class HostBuilderExtension
{
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            var application = context.HostingEnvironment.ApplicationName.FormatName();
            var environment = context.HostingEnvironment.EnvironmentName.FormatName();

            configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("Application", application)
                .Enrich.WithProperty("Environment", environment)
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["Elasticsearch:Uri"]!))
                {
                    IndexFormat = $"{application}-logs-{environment}-{DateTime.UtcNow:dd-MM-yyyy}",
                    AutoRegisterTemplate = true,
                })
                .ReadFrom.Configuration(context.Configuration);
        });

        return hostBuilder;
    }

    private static string FormatName(this string name)
    {
        return name.ToLower().Replace(".", "-");
    }
}
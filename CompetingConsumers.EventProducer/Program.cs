using CompetingConsumers.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompetingConsumers.EventProducer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                        services.AddTransient<IDomainEventProducer, DomainEventProducer>();
                        services.AddTransient<IMapper<PostCreatedEvent, DomainEvent>, PostCreatedEventMapper>();
                        services.AddEventStoreConnection(
                            hostContext.Configuration.GetConnectionString("EventStore"),
                            "CompetingConsumers.Producer");
                    });
        }
    }
}
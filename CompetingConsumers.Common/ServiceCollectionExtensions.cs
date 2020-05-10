using System;
using EventStore.ClientAPI;
using Microsoft.Extensions.DependencyInjection;

namespace CompetingConsumers.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStoreConnection(
            this IServiceCollection services,
            string connectionString,
            string connectionName)
        {
            var connectionSettings = ConnectionSettings.Create()
                .KeepReconnecting()
                .KeepRetrying()
                .Build();

            return services.AddSingleton(
                provider =>
                {
                    var conn = EventStoreConnection.Create(
                        connectionSettings,
                        new Uri(connectionString),
                        connectionName);
                    conn.ConnectAsync().Wait();
                    return conn;
                });
        }
    }
}
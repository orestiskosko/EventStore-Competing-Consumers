using System.Threading;
using CompetingConsumers.Common;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompetingConsumers.EventConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHangfire(
                (provider, opts) =>
                {
                    opts.UseMemoryStorage();
                    opts.UseActivator(new HangfireJobActivator(provider));
                });

            services.AddTransient<PostEventHandler>();
            services.AddTransient<PersistentSubscription<PostEventHandler>>();
            services.AddTransient<PersistentSubscriptionConsumerExecutor<PostEventHandler>>();
            services.AddEventStoreConnection(Configuration.GetConnectionString("EventStore"), "CompetingConsumers.Consumer");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            var subWorker = app.ApplicationServices.GetRequiredService<PersistentSubscriptionConsumerExecutor<PostEventHandler>>();
            var hangfireClient = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

            for (var i = 1; i <= 2; i++)
                hangfireClient.AddOrUpdate(
                    $"consumer_{i}",
                    () => subWorker.ExecuteAsync(CancellationToken.None),
                    Cron.Minutely);

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
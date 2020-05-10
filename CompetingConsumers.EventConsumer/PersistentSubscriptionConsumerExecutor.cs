using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompetingConsumers.EventConsumer
{
    public class PersistentSubscriptionConsumerExecutor<THandler> : IExecutor
        where THandler : IPersistentSubscriptionHandler
    {
        private readonly PersistentSubscription<THandler> _subscription;

        public PersistentSubscriptionConsumerExecutor(PersistentSubscription<THandler> subscription)
        {
            _subscription = subscription;
        }

        public ExecutorOptions Options { get; }

        public async Task ExecuteAsync(CancellationToken token = default)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            cts.CancelAfter(TimeSpan.FromSeconds(55));

            await _subscription.StartAsync(
                new PersistentSubscriptionOptions
                {
                    StreamName = "$ce-post",
                    GroupName = "post-consumers"
                },
                cts.Token);

            // Wait until token is cancelled
            while (!cts.Token.IsCancellationRequested) await Task.Delay(1000, CancellationToken.None);

            _subscription.Stop(10000);
        }
    }
}
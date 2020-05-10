using System.Threading;
using System.Threading.Tasks;

namespace CompetingConsumers.EventConsumer
{
    public interface IExecutor
    {
        ExecutorOptions Options { get; }
        Task ExecuteAsync(CancellationToken token = default);
    }
}
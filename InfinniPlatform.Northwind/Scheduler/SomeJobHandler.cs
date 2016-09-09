using System;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Northwind.Scheduler
{
    /// <summary>
    /// Пример обработчика заданий.
    /// </summary>
    public class SomeJobHandler : IJobHandler
    {
        public async Task Handle(IJobInfo jobInfo, IJobHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Greetings from {nameof(SomeJobHandler)}!");
        }
    }
}
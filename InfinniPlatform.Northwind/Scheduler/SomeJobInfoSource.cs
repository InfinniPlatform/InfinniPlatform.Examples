using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Northwind.Scheduler
{
    /// <summary>
    /// Пример источника заданий.
    /// </summary>
    public class SomeJobInfoSource : IJobInfoSource
    {
        public Task<IEnumerable<IJobInfo>> GetJobs(IJobInfoFactory factory)
        {
            var jobs = new[]
                       {
                           // Задание будет выполняться каждую минуту
                           // с помощью обработчика SomeJobHandler
                           factory.CreateJobInfo<SomeJobHandler>("SomeJob",
                               b => b.CronExpression(e => e.Seconds(m => m.Each(0))))
                       };

            return Task.FromResult<IEnumerable<IJobInfo>>(jobs);
        }
    }
}
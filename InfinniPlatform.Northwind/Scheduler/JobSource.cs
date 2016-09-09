using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Northwind.Scheduler
{
    /// <summary>
    /// Пример источника заданий.
    /// </summary>
    public class JobSource : IJobInfoSource
    {
        public Task<IEnumerable<IJobInfo>> GetJobs(IJobInfoFactory factory)
        {
            var jobs = new[]
                       {
                           // Задание будет выполняться ежедневно в 10:35 с помощью обработчика SomeJobHandler
                           factory.CreateJobInfo<SomeJobHandler>("SomeJob", b => b.CronExpression(e => e.AtHourAndMinuteDaily(10, 35)))
                       };

            return Task.FromResult<IEnumerable<IJobInfo>>(jobs);
        }
    }
}
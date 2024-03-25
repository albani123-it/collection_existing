using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace Collectium.Job
{
    public class Every5MinuteJob : IJob
    {

        public Every5MinuteJob()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("wohoooooo");

            return Task.CompletedTask;
        }
    }
}

namespace AspNetCoreTestProject
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public class WaitingForeverHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("doing work...");
                await Task.Delay(1000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class ImmediatelyCrashingBackgroundService: BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => throw new Exception("Crash right away");
    }

    public class YieldingAndThenCrashingBackgroundService: BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield(); // Hand over control explicitly, because then the base class doesn't wait for the result, and ignores the error
            throw new Exception("Crash after yielding");
        }
    }}

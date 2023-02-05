using EasyNetQ;
using Shared.RabbitMQ.Manager;

namespace RabbitMQ.Consumer
{
    public static class AppBuilderExtension
    {
        public static IApplicationBuilder MessageQueueSubscribe(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IHostApplicationLifetime>();
            var bus = services.GetService<IBus>();
            var manager = services.GetService<RabbitMQManager>();
            var logger = services.GetService<ILogger<MessageQueueService>>();

            if (lifeTime != null)
            {
                lifeTime.ApplicationStarted.Register(() => { new MessageQueueService(manager, logger).Subscribe(); });
                lifeTime.ApplicationStopped.Register(() =>
                {
                    if (bus != null)
                    {
                        bus.Dispose();
                    }
                });
            }

            return appBuilder;
        }
    }
}

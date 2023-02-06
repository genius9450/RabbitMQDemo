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
            //var manager = services.GetService<RabbitMQManager>();
            //var logger = services.GetService<ILogger<MessageQueueService>>();
            var service = services.GetService<SubscribeService>();

            if (lifeTime != null && service != null)
            {
                lifeTime.ApplicationStarted.Register(() => { service.Subscribe(); });
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

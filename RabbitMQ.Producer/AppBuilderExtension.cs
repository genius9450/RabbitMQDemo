using Autofac;
using EasyNetQ;

namespace RabbitMQ.Producer
{
    public static class AppBuilderExtension
    {
        public static void RegisterRabbitMqBus(this ContainerBuilder builder, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("RabbitMqConfig:ConnectionString");
            builder.RegisterInstance(RabbitHutch.CreateBus(connectionString)).As<IBus>().SingleInstance();
        }

    }
}

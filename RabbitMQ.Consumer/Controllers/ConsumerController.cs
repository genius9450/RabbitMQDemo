using Autofac;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Consumer.Consume;

namespace RabbitMQ.Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly SubscribeService _subscribeService;
        private readonly ILifetimeScope _lifetimeScope;

        public ConsumerController(ILogger<ConsumerController> logger, SubscribeService subscribeService, ILifetimeScope lifetimeScope)
        {
            _logger = logger;
            _subscribeService = subscribeService;
            _lifetimeScope = lifetimeScope;
        }


        [HttpPost("Init")]
        public async Task Init()
        {
            _logger.LogInformation("init");
            //_subscribeService.SubscribeWithLock();

            var consumer = _lifetimeScope.Resolve<FanoutMessageConsumer>();
            await consumer.ConsumeAsync("Resolve Consumer", null, null);
            _lifetimeScope.Dispose();
        }

    }
}
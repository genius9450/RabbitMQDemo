using Microsoft.AspNetCore.Mvc;

namespace RabbitMQ.Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly SubscribeService _subscribeService;

        public ConsumerController(ILogger<ConsumerController> logger, SubscribeService subscribeService)
        {
            _logger = logger;
            _subscribeService = subscribeService;
        }


        [HttpPost("Init")]
        public void Init()
        {
            _logger.LogInformation("init");
            //_subscribeService.Subscribe();
        }

    }
}
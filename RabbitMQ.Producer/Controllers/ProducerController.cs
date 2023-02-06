using Microsoft.AspNetCore.Mvc;
using Shared.RabbitMQ.Manager;
using Shared.RabbitMQ.Manager.Model;

namespace RabbitMQ.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly RabbitMQProducerService _mqService;

        public ProducerController(ILogger<ProducerController> logger, RabbitMQProducerService mqService)
        {
            _logger = logger;
            _mqService = mqService;
        }

        [HttpPost("SendDirect")]
        public async Task SendDirectAsync (string message)
        {
            await _mqService.PushMessageAsync<string>(new PushMessageArgs<string>()
            {
                SendType = SendType.Direct,
                ExchangeName = "demo.test.direct",
                RouteKey = "demo.1",
                SendData = $"Direct({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendFanout")]
        public async Task SendFanoutAsync(string message)
        {
            await _mqService.PushMessageAsync<string>(new PushMessageArgs<string>
            {
                SendType = SendType.Fanout,
                ExchangeName = "demo.test.fanout",
                SendData = $"Fanout({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendTopic")]
        public async Task SendTopicAsync(string message)
        {
            await _mqService.PushMessageAsync<string>(new PushMessageArgs<string>
            {
                SendType = SendType.Topic,
                ExchangeName = "demo.test.topic",
                SendData = $"Topic({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}",
                RouteKey = "demo.topic.1"
            });
        }

    }
}
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
                SendType = SendType.direct,
                ExchangeName = "demo.test.direct",
                RouteKey = "demo.1",
                SendData = $"direct({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendFanout")]
        public async Task SendFanoutAsync(string message)
        {
            await _mqService.PushMessageAsync<string>(new PushMessageArgs<string>
            {
                SendType = SendType.fanout,
                ExchangeName = "demo.test.fanout",
                SendData = $"fanout({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendTopic")]
        public async Task SendTopicAsync(string message)
        {
            await _mqService.PushMessageAsync<string>(new PushMessageArgs<string>
            {
                SendType = SendType.topic,
                ExchangeName = "demo.test.topic",
                SendData = $"topic({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}",
                RouteKey = "demo.topic.1"
            });
        }

    }
}
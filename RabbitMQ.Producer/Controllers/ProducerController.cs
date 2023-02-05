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
        private readonly RabbitMQManager _mqManager;

        public ProducerController(ILogger<ProducerController> logger, RabbitMQManager mqManager)
        {
            _logger = logger;
            _mqManager = mqManager;
        }

        [HttpPost("SendDirect")]
        public async Task SendDirectAsync (string message)
        {
            await _mqManager.SendDirectAsync<string>(new SendArgs<string>
            {
                ExchangeName = "demo.test.direct",
                RouteKey = "demo.1",
                SendData = $"Direct({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendFanout")]
        public async Task SendFanoutAsync(string message)
        {
            await _mqManager.SendFanoutAsync<string>(new SendArgs<string>
            {
                ExchangeName = "demo.test.fanout",
                SendData = $"Fanout({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}"
            });
        }

        [HttpPost("SendTopic")]
        public async Task SendTopicAsync(string message)
        {
            await _mqManager.SendTopicAsync<string>(new SendArgs<string>
            {
                ExchangeName = "demo.test.topic",
                SendData = $"Topic({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {message}",
                RouteKey = "demo.topic.1"
            });
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using HiperMicroservice.Model;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using HiperShared.Model;
using RabbitMQ.Client.Events;

namespace HiperMicroservice.Controllers
{
    [Route("api/v1/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "messages";
        public MessagesController(IConfiguration configuration)
        {
            var rabbitMQConfigurations = new RabbitMQConfigurations();
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
                configuration.GetSection("RabbitMQConfigurations"))
                    .Configure(rabbitMQConfigurations);

            _factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfigurations.HostName,
                Port = rabbitMQConfigurations.Port,
                UserName = rabbitMQConfigurations.UserName,
                Password = rabbitMQConfigurations.Password
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageInputModel message)
        {
            using(var connection = _factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue:QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var bytesMessage = Services.MessageServices.BytefyMessage(message);

                    channel.BasicPublish(
                        exchange: "", 
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return Accepted();
        }

        [HttpGet]
        public IActionResult GetMessage()
        {
            var message = new MessageInputModel();
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QUEUE_NAME,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, eventArgs)=>
                    {
                        Services.MessageServices.StringfyMessage(eventArgs.Body.ToArray());
                        Services.MessageServices.NotifyUser(message);

                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue: QUEUE_NAME,
                         autoAck: true,
                         consumer: consumer);
                }
            }

            return Ok(message);                        
        }
    }
}

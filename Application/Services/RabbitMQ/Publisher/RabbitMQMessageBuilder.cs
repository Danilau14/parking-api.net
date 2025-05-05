using ParkingApi.Core.Enums;
using ParkingApi.Core.Interfaces;

namespace ParkingApi.Application.Services.RabbitMQ.Publisher
{
    public class RabbitMQMessageBuilder : IRabbitMQMessageBuilder
    {
        private readonly IRabbitMQService _rabbitmqService;

        public RabbitMQMessageBuilder(IRabbitMQService rabbitmqService)
        {
            _rabbitmqService = rabbitmqService;
        }

        public async Task PublishAuditMessageAsync(
            string entity,
            Actions action, 
            bool state, 
            int? userId = null,
            string? response = null,
            string? queueName = null
        ) 
        {
            var message = new MessageDto
            {
                Entity = entity,
                Action = action,
                State = state,
                UserId = userId
            };

            if (response != null)
            {
                message.Response = response;    
            }

            await _rabbitmqService.PublishMessage(message, queueName);
        }
    }
}

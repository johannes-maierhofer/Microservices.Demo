using BuildingBlocks.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailSender.MessageConsumers
{
    public class SendEmailConsumer : IConsumer<EmailSenderContracts.SendEmail>
    {
        private readonly ILogger<SendEmailConsumer> _logger;

        public SendEmailConsumer(ILogger<SendEmailConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<EmailSenderContracts.SendEmail> context)
        {
            _logger.LogInformation($"Handle SendEmail for message with subject '{context.Message.Subject}'.");
            // throw new ApplicationException("Cannot send email test.");
            
            return Task.CompletedTask;
        }
    }

    public class SendEmailConsumerDefinition : ConsumerDefinition<SendEmailConsumer>
    {
        public SendEmailConsumerDefinition()
        {
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendEmailConsumer> consumerConfigurator)
        {
            consumerConfigurator.UseMessageRetry(r => r.Exponential(
                3,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(1)));
        }
    }

    // consuming faults
    // https://masstransit-project.com/usage/exceptions.html#consuming-faults
    public class SendEmailConsumerFault : IConsumer<Fault<EmailSenderContracts.SendEmail>>
    {
        private readonly ILogger<SendEmailConsumerFault> _logger;

        public SendEmailConsumerFault(ILogger<SendEmailConsumerFault> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<EmailSenderContracts.SendEmail>> context)
        {
            _logger.LogInformation($"Handle faulted SendEmail command for message with subject '{context.Message.Message.Subject}'.");
            return Task.CompletedTask;
        }
    }
}
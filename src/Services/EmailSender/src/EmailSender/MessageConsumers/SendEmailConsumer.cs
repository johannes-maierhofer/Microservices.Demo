using Argo.MD.EmailSender.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Argo.MD.EmailSender.MessageConsumers
{
    public class SendEmailConsumer : IConsumer<SendEmail>
    {
        private readonly ILogger<SendEmailConsumer> _logger;

        public SendEmailConsumer(ILogger<SendEmailConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SendEmail> context)
        {
            _logger.LogInformation($"Handle SendEmail for message with subject '{context.Message.Subject}'.");
            // throw new ApplicationException("Cannot send email test.");
            
            return Task.CompletedTask;
        }
    }

    public class SendEmailConsumerDefinition : ConsumerDefinition<SendEmailConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendEmailConsumer> consumerConfigurator,
            IRegistrationContext context)
        {
            consumerConfigurator.UseMessageRetry(r => r.Exponential(
                3,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(1)));

            consumerConfigurator.UseConcurrentMessageLimit(8);

            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
        }
    }

    // consuming faults
    // https://masstransit-project.com/usage/exceptions.html#consuming-faults
    public class SendEmailConsumerFault : IConsumer<Fault<SendEmail>>
    {
        private readonly ILogger<SendEmailConsumerFault> _logger;

        public SendEmailConsumerFault(ILogger<SendEmailConsumerFault> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<SendEmail>> context)
        {
            _logger.LogInformation($"Handle faulted SendEmail command for message with subject '{context.Message.Message.Subject}'.");
            return Task.CompletedTask;
        }
    }
}
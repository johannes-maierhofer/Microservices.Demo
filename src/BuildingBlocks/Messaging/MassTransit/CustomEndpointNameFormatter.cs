using MassTransit;

namespace BuildingBlocks.Messaging.MassTransit
{
    public class CustomEndpointNameFormatter : IEndpointNameFormatter
    {
        public string TemporaryEndpoint(string tag)
        {
            throw new NotImplementedException();
        }

        public string Consumer<T>() where T : class, IConsumer
        {
            throw new NotImplementedException();
        }

        public string Message<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public string Saga<T>() where T : class, ISaga
        {
            throw new NotImplementedException();
        }

        public string ExecuteActivity<T, TArguments>() where T : class, IExecuteActivity<TArguments> where TArguments : class
        {
            throw new NotImplementedException();
        }

        public string CompensateActivity<T, TLog>() where T : class, ICompensateActivity<TLog> where TLog : class
        {
            throw new NotImplementedException();
        }

        public string SanitizeName(string name)
        {
            throw new NotImplementedException();
        }

        public string Separator => "_";
    }
}

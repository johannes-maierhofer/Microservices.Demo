﻿namespace Argo.MD.Customers.Api.IntegrationTests
{
    [CollectionDefinition("App")]
    public class AppTestCollection : ICollectionFixture<AppTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
        // see https://xunit.net/docs/shared-context
    }
}

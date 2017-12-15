using Xunit;

namespace WebApiCore.IntegrationTests.Helpers
{

    [CollectionDefinition("Database collection")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

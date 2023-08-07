using Xunit;

namespace StoreAdmin.Data.Test
{
    [CollectionDefinition("SharedFileCollection")]
    public class SharedFileCollection : ICollectionFixture<TestDataFixture>
    {
        // This class has no code because we use it only to define the shared fixture.
        // The TestDataFixture will be created and disposed for all test classes in this collection.
    }
}

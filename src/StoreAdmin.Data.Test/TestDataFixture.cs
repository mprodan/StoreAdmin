namespace StoreAdmin.Data.Test
{
    public class TestDataFixture 
    {
        public static string TestPath = "stores.test.sqlite";

        public TestDataFixture()
        {
            // Run once before any tests in the class.
            if (File.Exists(TestPath))
            {
                File.Delete(TestPath);
            }
        }

    }
}
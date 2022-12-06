namespace JasperFx.Core.Tests
{
    public class TestDirectory : IDisposable
    {
        private string _previousDirectory;

        public void ChangeDirectory()
        {
            //This approach has the disadvantage to not run in parallel, but keeps existing tests in place without changes.
            _previousDirectory = Directory.GetCurrentDirectory();
            var srcDir = Directory.GetParent(_previousDirectory).Parent;
            var testDataPath = Path.Combine(srcDir.FullName, "TestData", Guid.NewGuid().ToString());
            if (!Directory.Exists(testDataPath))
            {
                Directory.CreateDirectory(testDataPath);
            }
            Directory.SetCurrentDirectory(testDataPath);
            
            FileSystem.CleanDirectory(testDataPath);
        }

        public void Dispose()
        {
            Directory.SetCurrentDirectory(_previousDirectory);
        }
    }
}
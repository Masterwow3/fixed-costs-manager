using BankingFetcher.Standard.Test.Config;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Text;

namespace BankingFetcher.Standard.Test
{
    public abstract class TestBase
    {
        public TestBase()
        {
            TestDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            var sensitiveConfigFilePath = Path.Combine(TestDirectory, "Config","SensitiveData.json");
            SensitiveDataConfig = JsonConvert.DeserializeObject<SensitiveData>(File.ReadAllText(sensitiveConfigFilePath, Encoding.UTF8));
        }

        public string TestDirectory { get; }
        public SensitiveData SensitiveDataConfig { get; }
    }
}
using System.IO;
using System.Reflection;
using System.Text;
using BankingFetcher.Net.Test.Config;
using Newtonsoft.Json;

namespace BankingFetcher.Net.Test
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
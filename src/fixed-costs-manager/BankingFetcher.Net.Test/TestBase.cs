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
            var sensitiveConfigFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Config","SensitiveData.json");
            SensitiveDataConfig = JsonConvert.DeserializeObject<SensitiveData>(File.ReadAllText(sensitiveConfigFilePath, Encoding.UTF8));
        }

        public SensitiveData SensitiveDataConfig { get; }
    }
}
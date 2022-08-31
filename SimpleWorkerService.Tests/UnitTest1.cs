using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
//using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using URLValidator;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using URLValidation;

namespace URLValidator.UnitTests.Services
{

    public class TestURL
    {

        private readonly IConfiguration _config;

        public TestURL()
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrEmpty(environmentName))
            {
                configurationBuilder.AddJsonFile($"appsettings.{environmentName.ToLower()}.json", optional: true, reloadOnChange: true);
            }

            configurationBuilder.AddEnvironmentVariables();

            _config = configurationBuilder.Build();
        }

        [Fact]
        public void IsURLValid()
        {
            string websiteURL = _config.GetValue<string>("WebsiteURL");
            Console.WriteLine("This is output from {0}", websiteURL);
            Console.WriteLine("Hosting Environment: {0}", Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"));
            bool result = ValidateURL.IsValid(websiteURL);

            Assert.True(result, "1 should not be prime");
            var temp = "my class!";
            Console.WriteLine("This is output from {0}", temp);
        }
    }

}
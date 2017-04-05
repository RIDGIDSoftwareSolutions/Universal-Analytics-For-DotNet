using NUnit.Framework;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture, Category("Integration")]
    public class ConfigurationManagerWrapperTests
    {
        [Test]
        public void It_Returns_The_App_Setting_If_It_Exists()
        {
            //--arrange

            //--act
            var value = new ConfigurationManagerWrapper().GetAppSetting("testAppSetting");

            //--assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.Not.Empty);
        }

        [Test]
        public void It_Throws_A_ConfigEntryMissingException_If_The_App_Settings_Doesnt_Exist()
        {
            //--arrange
            var appSettingThatDoesntExist = "abc123";

            //--act / assert
            Assert.Throws<ConfigEntryMissingException>(() => new ConfigurationManagerWrapper()
                .GetAppSetting(appSettingThatDoesntExist));
        }
    }
}

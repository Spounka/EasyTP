using System.Globalization;
using NUnit.Framework;
using Student.ValidationRules;

namespace Student.Tests.ValidationRules
{
    [TestFixture]
    public class IPRuleTest
    {
        private IPRule validationRule;
        private CultureInfo _cultureInfo;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            validationRule = new IPRule();
            _cultureInfo = new CultureInfo("ar-DZ", false);
        }

        [Test]
        public void IPRule_Empty_Null_Whitespace_FalseTest([Values("", " ", "\t", "\n", null)] string values)
        {
            // Arrange

            // Act
            var validationResult = validationRule.Validate(values, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }


        [Test]
        public void IPRule_OneByte_FalseTest()
        {
            // Arrange
            const string ip = "102";

            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_TwoBytes_FalseTest()
        {
            // Arrange
            const string ip = "102.105";

            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_ThreeBytes_FalseTest()
        {
            // Arrange
            const string ip = "102.105.100";

            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_LettersAndNumbers_FalseTest([Values("192.2ss.012", "qqq.qqq.qqq", "023.1b.f3.1c")] string ip)
        {
            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_OverThreeNumbers_FalseTest()
        {
            // Arrange
            const string ip = "102.1050.100.080";

            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_FourBytesWithWhiteSpace_FalseTest()
        {
            // Arrange
            const string ip = "102 1050/100.080";

            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void IPRule_FourBytesNoLetters_TrueTest(
            [Values("0.0.0.0", "127.0.0.1", "192.168.1.1", "10.14.18.1", "localhost")]
            string ip)
        {
            // Act
            var validationResult = validationRule.Validate(ip, _cultureInfo);
            // Assert
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}
using System;
using System.Globalization;
using System.Net;
using NUnit.Framework;
using Student.ValidationRules;

namespace Student.Tests.ValidationRules
{
    [TestFixture]
    public class PortRuleTest
    {
        private PortRule validationRule;
        private CultureInfo _cultureInfo;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            validationRule = new PortRule();
            _cultureInfo = new CultureInfo("ar-DZ", false);
        }

        [Test]
        public void PortRule_Characters_FalseTest([Values("a", "28b", "31cb", "_28zx.")] string port)
        {
            // ACT
            var validationResult = validationRule.Validate(port, _cultureInfo);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void PortRule_EmptyWhiteSpaceNull_FalseTest([Values("", " ", "\t", "\n", null)] string port)
        {
            // ACT
            var validationResult = validationRule.Validate(port, _cultureInfo);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void PortRule_PortUnderRange_FalseTest()
        {
            var port = (IPEndPoint.MinPort - 1).ToString();
            // ACT
            var validationResult = validationRule.Validate(port, _cultureInfo);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void PortRule_PortOverRange_FalseTest()
        {
            var port = (IPEndPoint.MaxPort + 1).ToString();
            // ACT
            var validationResult = validationRule.Validate(port, _cultureInfo);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void PortRule_PortValid_TrueTest()
        {
            var port = new Random().Next(IPEndPoint.MinPort, IPEndPoint.MaxPort + 1).ToString();
            // ACT
            var validationResult = validationRule.Validate(port, _cultureInfo);

            // ASSERT
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}
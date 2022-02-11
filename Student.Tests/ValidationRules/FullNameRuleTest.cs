using System.Globalization;
using NUnit.Framework;
using Student.ValidationRules;

namespace Student.Tests.ValidationRules
{
    [TestFixture]
    public class FullNameRuleTest
    {
        [Test]
        public void FullName_EmptyString_FalseTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("", culture);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void FullName_WhiteSpacesString_FalseTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("\t", culture);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void FullName_NoSpaces_FalseTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("NazihBoudaakkar", culture);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void FullName_ValidNameString_TrueTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("Nazih Boudaakkar", culture);

            // ASSERT
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void FullName_ValidNameWithNumbers_FalseTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("Nazih 22Boudaakkar", culture);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void FullName_ThreeNames_TrueTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("Nazih Von Boudaakkar", culture);

            // ASSERT
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void FullName_OneName_FalseTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("Nazih", culture);

            // ASSERT
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void FullName_FourNames_TrueTest()
        {
            // ARRANGE
            var fullNameTestRule = new FullNameRule();
            var culture = new CultureInfo("ar-DZ", false);

            // ACT
            var validationResult = fullNameTestRule.Validate("Nazih Spounka Von Schpeiner", culture);

            // ASSERT
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}
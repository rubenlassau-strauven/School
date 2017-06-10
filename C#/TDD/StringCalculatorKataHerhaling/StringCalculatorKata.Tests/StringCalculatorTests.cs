using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StringCalculatorKata.Tests
{
    [TestFixture]
    class StringCalculatorTests
    {
        Random random = new Random();

        [Test]
        public void Add_EmptyString_ReturnsZero()
        {
            //Arrange

            //Act
            var addResult = StringCalculator.Add("");

            //Assert
            Assert.That(addResult,Is.EqualTo(0));
        }

        [Test]
        public void Add_OneNumberInString_ReturnsNumber()
        {
            //Arrange
            var number = random.Next(1, 10);

            //Act
            var addResult = StringCalculator.Add(number.ToString());

            //Assert
            Assert.That(addResult, Is.EqualTo(number));
        }

        [Test]
        public void Add_TwoNumbersSplitByCommaInString_ReturnsSumOfNumbers()
        {
            //Arrange
            var numberOne = random.Next(1, 10);
            var numberTwo = random.Next(1, 10);

            //Act
            var addResult = StringCalculator.Add(numberOne+","+numberTwo);

            //Assert
            Assert.That(addResult, Is.EqualTo(numberOne+numberTwo));
        }

       
        [Test]
        public void Add_RandomAmountOfNumbersSplitWithComma_ReturnsSumOfAllNumbers()
        {
            //Arrange
            int totaal = 0;
            List<int> numbers = new List<int>();
            for (int i = 0; i < random.Next(2, 10); i++)
            {
                numbers.Add(random.Next(1, 10));
            }
            numbers.ForEach(n => totaal += n);

            StringBuilder numbersString = new StringBuilder();
            numbers.ForEach(n => numbersString.Append(n + ","));


            //Act
            var addResult = StringCalculator.Add(numbersString.ToString().TrimEnd(','));

            //Assert
            Assert.That(addResult, Is.EqualTo(totaal));
        }

        [TestCase("5,6,8,o","o")]
        [TestCase("5/6,8,o", "5/6")]
        [TestCase("5,6,8+/", "8+/")]
        public void Add_StringWithNonNumberOrComma_ThrowsException(string numbers, string expected)
        {
            //Arrange

            //Act

            //Assert
            Assert.That(
                () => StringCalculator.Add(numbers),
                Throws.ArgumentException.With.Message.EqualTo($"Invalid number {expected}"));
        }

        [TestCase("1,5\n7",13)]
        [TestCase("1\n2\n9", 12)]
        public void Add_StringWith3NumbersSplitByCommaAndNewline_ReturnsSum(string numbers, int expected)
        {
            //Arrrange

            //Act

            //Assert
            Assert.That(StringCalculator.Add(numbers), Is.EqualTo(expected));
        }

        [Test]
        public void Add_StringWithCommaAndNewlineNextToEachOther_ThrowsException()
        {
            //Arrrange

            //Act

            //Assert
            Assert.That(
                () => StringCalculator.Add("5,8,\n12"),
                Throws.ArgumentException.With.Message.EqualTo($"Invalid number "));
        }

        [Test]
        public void Add_StringWithColonAsDelimiter_ReturnsSum()
        {
            //Arrrange

            //Act

            //Assert
            Assert.That(StringCalculator.Add("//:\n5:8:12"), Is.EqualTo(25));
        }
    }
}

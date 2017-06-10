using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorKata.Tests
{
    [TestFixture]
    public class StringCalculatorTests
    {
        //Testnaam: methodenaam_context_expected
        StringCalculator sut = new StringCalculator();

        [Test]
        public void ShouldReturn0WhenStringIsEmpty()
        {
            //ARRANGE

            //ACT

            //ASSERT
            Assert.That(sut.Add(""), Is.EqualTo(0));
        }

        [Test]
        [TestCase("1", 1)]
        [TestCase("10", 10)]
        [TestCase("100", 100)]
        public void ShouldReturnNumberWhenOneNumberIsGiven(string input, int expected)
        {
            //ARRANGE

            //ACT
            var result = sut.Add(input);

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1,2", 3)]
        [TestCase("0,10", 10)]
        [TestCase("100,0", 100)]
        public void ShouldReturnSumWhenTwoNumbersAreGiven(string input, int expected)
        {
            //ARRANGE

            //ACT
            var result = sut.Add(input);

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1,2,0,4,8", 15)]
        [TestCase("0,10,0", 10)]
        [TestCase("100,1", 101)]
        public void ShouldReturnSumWhenUnkownAmountOfNumbersAreGiven(string input, int expected)
        {
            //ARRANGE

            //ACT
            var result = sut.Add(input);

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1,aaa", "aaa")]
        [TestCase("aaa", "aaa")]
        [TestCase("1\n,2", "")]
        public void ShouldThrowArgumentExceptionIfContainsInvalidNumber(string input, string invalidPart)
        {
            //ASSERT
            Assert.That(
                () => sut.Add(input),
                Throws.ArgumentException.With.Message.EqualTo($"Invalid number {invalidPart}"));
        }

        [Test]
        [TestCase("1,2\n0,4,8", 15)]
        [TestCase("0\n10\n0", 10)]
        [TestCase("100\n1", 101)]
        public void ShouldAcceptNewlineAsDelimiter(string input, int expected)
        {
            //ARRANGE

            //ACT
            var result = sut.Add(input);

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("//;\n1;2", 3)]
        [TestCase("//$\n5$0", 5)]
        [TestCase("//x\n10x1", 11)]
        public void ShouldSupportDifferentDelimiters(string input, int expected)
        {
            //ARRANGE

            //ACT
            var result = sut.Add(input);

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("-1","-1")]
        [TestCase("1,-5", "-5")]
        [TestCase("//x\n10x-1", "-1")]
        [TestCase("1,-2\n0,4,-8", "-2 -8")]
        public void ShouldThrowExceptionWhenNegativeNumbersAreGiven(string input, string invalidPart)
        {
            //ASSERT
            Assert.That(
                () => sut.Add(input),
                Throws.ArgumentException.With.Message.EqualTo($"Negatives not allowed: {invalidPart}"));
        }
    }
}

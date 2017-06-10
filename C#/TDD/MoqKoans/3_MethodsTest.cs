using System;
using NUnit.Framework;
using MoqKoans.KoansHelpers;
using Moq;

namespace MoqKoans
{
    [TestFixture]
    public class Moq3_MethodsTest : Koan
    {
        // This is an interface that we will be mocking.
        public interface IVolume
        {
            int Louder(int amount);
            int Quieter(int amount);
            string CurrentVolume();
        }

        [Test]
        public void UsingWhatYouHaveLearned_CreateACompletelyFunctionalIVolumeMock()
        {
            // Use Moq to create a Mock<IVolume> instance.
            // Have the internal volume start at 50.
            // The Louder and Quieter methods should incrament/decrament the internal volume by the passed in amount.
            // The Louder and Quieter methods should return the internal volume level after the change.
            // For example if the internal volume was 10, and .Quieter(2) is called, then it should return 8.
            // The volume should stay in the range 0 to 100. It should not go below 0 or over 100.
            // If .Louder(999) is called, then the volume should be at 100.
            // The CurrentVolume() methoud should always return the internal volume level, as a string.
            // If a negative number is passed to either Louder or Quieter, then it should throw an ArgumentOutOfRangeException.

            var mock = new Mock<IVolume>();
            var volumeLevel = 50;
            // ...setup your mock here...
            
            mock.Setup(m => m.CurrentVolume()).Returns(() => volumeLevel.ToString());
            mock.Setup(m => m.Louder(It.IsAny<int>())).Returns<int>(p =>
            {
                if (p < 0)
                    throw new ArgumentOutOfRangeException();
                if (volumeLevel + p <= 100)
                {
                    volumeLevel += p;
                } else
                {
                    volumeLevel = 100;
                }
                return volumeLevel;
            });
            mock.Setup(m => m.Quieter(It.IsAny<int>())).Returns<int>(p =>
            {
                if (p < 0)
                    throw new ArgumentOutOfRangeException();
                if (volumeLevel - p >= 0)
                {
                    volumeLevel -= p;
                } else
                {
                    volumeLevel = 0;
                }
                return volumeLevel;
            });

            // Do not change these Asserts. Your setup mock should make all of these pass the way they are.
            var volume = mock.Object;
            Assert.AreEqual("50", volume.CurrentVolume());
            Assert.AreEqual(40, volume.Quieter(10));
            Assert.AreEqual("40", volume.CurrentVolume());
            Assert.AreEqual(60, volume.Louder(20));
            Assert.AreEqual("60", volume.CurrentVolume());
            Assert.AreEqual(0, volume.Quieter(1000));
            Assert.AreEqual("0", volume.CurrentVolume());
            Assert.AreEqual(100, volume.Louder(1000));
            Assert.AreEqual("100", volume.CurrentVolume());
            try
            {
                volume.Louder(-1);
                Assert.Fail("Louder did not throw an exception on a negative input.");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
            }
            try
            {
                volume.Quieter(-1);
                Assert.Fail("Quieter did not throw an exception on a negative input.");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
            }
        }
    }
}

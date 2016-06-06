using InfinniPlatform.Northwind.IoC;
using InfinniPlatform.Sdk.IoC;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Northwind.Tests.IoC
{
    [TestFixture(Category = "UnitTest")]
    public class NorthwindContainerModuleTest
    {
        [Test]
        public void LoadDoesNotThrowException()
        {
            // Given
            var target = new NorthwindContainerModule();
            var builder = new Mock<IContainerBuilder>();

            // When
            TestDelegate action = () => target.Load(builder.Object);

            // Then
            Assert.DoesNotThrow(action);
        }
    }
}
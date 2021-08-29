using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sandbox.ModAPI.Ingame;
using static IngameScript.Program;

namespace Auto_Mining_Platform_Tests
{
    [TestClass]
    public class ScreenManagerTests
    {
        private readonly ScriptConfig dummyConfig = new ScriptConfig(new Mock<IMessageQueueAppender>().Object);
        private readonly Mock<IScreenMessage> mockMessage = new Mock<IScreenMessage>();
        private readonly Mock<IStateProvider> mockStateProvider = new Mock<IStateProvider>();

        private ScreenManager screenManager;

        [TestCleanup]
        public void CleanUp()
        {
            dummyConfig.AllToDefault();
            mockMessage.Reset();
        }

        private ScreenManager NewScreenManager(IMyTextSurface coreDisplay)
        {
            return new ScreenManager(dummyConfig, mockStateProvider.Object, mockMessage.Object, coreDisplay);
        }

        [TestMethod]
        public void Constructor_WhenNullCoreDisplayGiven_ThenNotAddedToDisplays()
        {
            // Act
            screenManager = new ScreenManager(dummyConfig, mockStateProvider.Object, mockMessage.Object, null);

            // Assert
            Assert.AreEqual(0, screenManager.GetDisplayCount());
        }

        [TestMethod]
        public void ConsumeBlock_WhenTextSurceGiven_ThenShouldBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();
            mockTerminalBlock.As<IMyTextPanel>();

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(1, screenManager.GetDisplayCount());

        }

        public void ConsumeBlock_WhenNotSurfaceOrProviderGiven_ThenShouldNotBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(0, screenManager.GetDisplayCount());
        }

        [TestMethod]
        public void ConsumeBlock_WhenProviderGivenAndLineIsValidAndIsInSurfaceCount_ThenShouldBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();
            var mockSurfaceProvider = mockTerminalBlock.As<IMyTextSurfaceProvider>();
            
            var mockTextSurface = new Mock<IMyTextSurface>();

            mockSurfaceProvider.Setup(p => p.SurfaceCount).Returns(1);
            mockSurfaceProvider.Setup(p => p.GetSurface(0)).Returns(mockTextSurface.Object);

            dummyConfig.MainTag = "/Dummy/";
            mockTerminalBlock.SetupGet(p =>p.CustomData).Returns("@0 /Dummy/");

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(1, screenManager.GetDisplayCount());
        }

        [TestMethod]
        public void ConsumeBlock_WhenProviderGivenAndLineIsValidAndIsNotInSurfaceCount_ThenShouldNotBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();
            var mockSurfaceProvider = mockTerminalBlock.As<IMyTextSurfaceProvider>();

            var mockTextSurface = new Mock<IMyTextSurface>();

            mockSurfaceProvider.Setup(p => p.SurfaceCount).Returns(0);
            mockSurfaceProvider.Setup(p => p.GetSurface(0)).Returns(mockTextSurface.Object);

            dummyConfig.MainTag = "/Dummy/";
            mockTerminalBlock.SetupGet(p => p.CustomData).Returns("@0 /Dummy/");

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(0, screenManager.GetDisplayCount());
        }

        [TestMethod]
        public void ConsumeBlock_WhenProviderGivenAndLineIsNotValid_ThenShouldNotBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();
            var mockSurfaceProvider = mockTerminalBlock.As<IMyTextSurfaceProvider>();

            var mockTextSurface = new Mock<IMyTextSurface>();

            mockSurfaceProvider.Setup(p => p.SurfaceCount).Returns(0);
            mockSurfaceProvider.Setup(p => p.GetSurface(It.IsAny<int>())).Returns(mockTextSurface.Object);

            dummyConfig.MainTag = "/Dummy/";
            mockTerminalBlock.SetupGet(p => p.CustomData).Returns("Not Correct Custom Data");

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(0, screenManager.GetDisplayCount());
        }

        [TestMethod]
        public void ConsumeBlock_WhenProviderGivenAndMultipleLineIsValidAndIsInSurfaceCount_ThenShouldNotBeAddedToDisplays()
        {
            // Arrange
            var mockTerminalBlock = new Mock<IMyTerminalBlock>();
            var mockSurfaceProvider = mockTerminalBlock.As<IMyTextSurfaceProvider>();

            var mockTextSurface1 = new Mock<IMyTextSurface>();
            var mockTextSurface2 = new Mock<IMyTextSurface>();

            mockSurfaceProvider.Setup(p => p.SurfaceCount).Returns(2);
            mockSurfaceProvider.Setup(p => p.GetSurface(0)).Returns(mockTextSurface1.Object);
            mockSurfaceProvider.Setup(p => p.GetSurface(1)).Returns(mockTextSurface2.Object);

            dummyConfig.MainTag = "/Dummy/";
            mockTerminalBlock.SetupGet(p => p.CustomData).Returns("@0 /Dummy/\n" +
                                                                  "@1 /Dummy/\n" +
                                                                  "Not Correct");

            screenManager = NewScreenManager(null);

            // Act
            screenManager.ConsumeBlock(mockTerminalBlock.Object);

            // Assert
            Assert.AreEqual(2, screenManager.GetDisplayCount());
        }
    }
}

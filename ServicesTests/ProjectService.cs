using System.Threading.Tasks;
using Consumables;
using Models;
using Moq;
using NUnit.Framework;
using Sartain_Studios_Common.Logging;
using Services;

namespace ServicesTests
{
    public class ProjectServiceTests
    {
        private const string SampleId1 = "5eba08740bdc1e00945702411";
        private const string SampleId2 = "8aty08740bdc2e00945706666";

        private const string SampleUserId1 = "5eba08740b4c1e4494570222";
        private const string SampleUserId2 = "5eba08740b4c1e4494570999";

        private static readonly ProjectModel ProjectModel1 = new ProjectModel
        {
            Id = SampleId1
        };

        private static readonly ProjectModel ProjectModel2 = new ProjectModel
        {
            Id = SampleId2
        };

        private Mock<ILoggerWrapper> _loggerWrapperMock;
        private Mock<IProjectConsumable> _userConsumableMock;
        private ProjectService _userService;

        [SetUp]
        public void Setup()
        {
            _loggerWrapperMock = new Mock<ILoggerWrapper>();
            _userConsumableMock = new Mock<IProjectConsumable>();

            _userService = new ProjectService(_loggerWrapperMock.Object, _userConsumableMock.Object);
        }

        [Test]
        public async Task GetAllAsync_CallsGetAllAsyncOnceAsync()
        {
            await _userService.GetAllAsync(SampleUserId1);

            _userConsumableMock.Verify(x => x.GetAllAsync(SampleUserId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_CallsGetByIdAsyncOnce()
        {
            await _userService.GetByIdAsync(SampleUserId1, SampleId1);

            _userConsumableMock.Verify(x => x.GetByIdAsync(SampleUserId1, SampleId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsModelWithSpecifiedId()
        {
            _userConsumableMock.Setup(x => x.GetByIdAsync(SampleUserId1, SampleId1))
                .Returns(Task.FromResult(ProjectModel1));

            var result = await _userService.GetByIdAsync(SampleUserId1, SampleId1);

            Assert.AreEqual(SampleId1, result.Id);
        }

        [Test]
        public async Task UpdateAsync()
        {
            await _userService.UpdateAsync(SampleUserId1, SampleId1, ProjectModel1);

            _userConsumableMock.Verify(x => x.UpdateAsync(SampleUserId1, SampleId1, ProjectModel1), Times.Once());
        }

        [Test]
        public async Task CreateAsync()
        {
            await _userService.CreateAsync(SampleUserId1, ProjectModel1);

            _userConsumableMock.Verify(x => x.CreateAsync(SampleUserId1, ProjectModel1), Times.Once());
        }

        [Test]
        public async Task DeleteAsync()
        {
            await _userService.DeleteAsync(SampleUserId1, SampleId1);

            _userConsumableMock.Verify(x => x.DeleteAsync(SampleUserId1, SampleId1), Times.Once());
        }
    }
}
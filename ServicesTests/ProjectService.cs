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
        private Mock<IProjectConsumable> _eventConsumable;
        private ProjectService _projectService;

        [SetUp]
        public void Setup()
        {
            _loggerWrapperMock = new Mock<ILoggerWrapper>();
            _eventConsumable = new Mock<IProjectConsumable>();

            _projectService = new ProjectService(_loggerWrapperMock.Object, _eventConsumable.Object);
        }

        [Test]
        public async Task GetAllAsync_CallsGetAllAsyncOnceAsync()
        {
            await _projectService.GetAllAsync(SampleUserId1);

            _eventConsumable.Verify(x => x.GetAllAsync(SampleUserId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_CallsGetByIdAsyncOnce()
        {
            await _projectService.GetByIdAsync(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.GetByIdAsync(SampleUserId1, SampleId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsModelWithSpecifiedId()
        {
            _eventConsumable.Setup(x => x.GetByIdAsync(SampleUserId1, SampleId1))
                .Returns(Task.FromResult(ProjectModel1));

            var result = await _projectService.GetByIdAsync(SampleUserId1, SampleId1);

            Assert.AreEqual(SampleId1, result.Id);
        }

        [Test]
        public async Task UpdateAsync()
        {
            await _projectService.UpdateAsync(SampleUserId1, SampleId1, ProjectModel1);

            _eventConsumable.Verify(x => x.UpdateAsync(SampleUserId1, SampleId1, ProjectModel1), Times.Once());
        }

        [Test]
        public async Task CreateAsync()
        {
            await _projectService.CreateAsync(SampleUserId1, ProjectModel1);

            _eventConsumable.Verify(x => x.CreateAsync(SampleUserId1, ProjectModel1), Times.Once());
        }

        [Test]
        public async Task DeleteAsync()
        {
            await _projectService.DeleteAsync(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.DeleteAsync(SampleUserId1, SampleId1), Times.Once());
        }
    }
}
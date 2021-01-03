using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables;
using Models;
using Moq;
using NUnit.Framework;
using Sartain_Studios_Common.Logging;
using Services;

namespace ServicesTests
{
    public class EventServiceTests
    {
        private const string SampleId1 = "5eba08740bdc1e00945702411";
        private const string SampleId2 = "8aty08740bdc2e00945706666";

        private const string SampleUserId1 = "5eba08740b4c1e4494570222";
        private const string SampleUserId2 = "5eba08740b4c1e4494570999";

        private static readonly EventModel EventModel1 = new EventModel
        {
            Id = SampleId1,
            ProjectId = SampleId1
        };

        private static readonly EventModel EventModel2 = new EventModel
        {
            Id = SampleId2
        };

        private Mock<ILoggerWrapper> _loggerWrapperMock;
        private Mock<IEventConsumable> _eventConsumable;
        private EventService _eventService;

        [SetUp]
        public void Setup()
        {
            _loggerWrapperMock = new Mock<ILoggerWrapper>();
            _eventConsumable = new Mock<IEventConsumable>();

            _eventService = new EventService(_loggerWrapperMock.Object, _eventConsumable.Object);
        }

        [Test]
        public async Task GetAllByIdAsync_CallsGetAllAsyncOnce()
        {
            await _eventService.GetAllByIdAsync(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.GetAllAsync(SampleUserId1), Times.Once());
        }

        [Test]
        public async Task GetAllByIdAsync_ReturnsOnlyModelsWithSpecifiedId()
        {
            var eventModels = new List<EventModel>
            {
                EventModel1,
                new EventModel
                {
                    Id = SampleId2,
                    ProjectId = SampleId2
                }
            };

            _eventConsumable.Setup(x => x.GetAllAsync(SampleUserId1)).Returns(Task.FromResult(eventModels.AsEnumerable()));

            var result = await _eventService.GetAllByIdAsync(SampleUserId1, SampleId1);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(SampleId1, result.First().Id);
        }

        [Test]
        public async Task GetAllByIdAsync_ReturnsEntitiesByDateTimeInDescendingOrder()
        {
            var eventModels = new List<EventModel>
            {
                new EventModel
                {
                    Id = "1",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "2",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T12:00:00.000+00:00")
                },
                new EventModel
                {
                    Id = "3",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T13:00:00.000+00:00")
                },
                new EventModel
                {
                    Id = "4",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T18:13:00.000+00:00")
                }
            };

            _eventConsumable.Setup(x => x.GetAllAsync(SampleUserId1))
                .Returns(Task.FromResult(eventModels.AsEnumerable()));

            var result = await _eventService.GetAllByIdAsync(SampleUserId1, SampleId1);

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual("3", result.ToList()[0].Id);
            Assert.AreEqual("1", result.ToList()[1].Id);
            Assert.AreEqual("4", result.ToList()[2].Id);
            Assert.AreEqual("2", result.ToList()[3].Id);
        }

        [Test]
        public async Task CanBeStopped_CallsGetAllAsyncOnceAsync()
        {
            var eventModels = new List<EventModel>
            {
                new EventModel
                {
                    Id = "1",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "2",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T12:00:00.000+00:00")
                },
                new EventModel
                {
                    Id = "3",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T13:00:00.000+00:00")
                },
                new EventModel
                {
                    Id = "4",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T18:13:00.000+00:00")
                }
            };

            _eventConsumable.Setup(x => x.GetAllAsync(SampleUserId1)).Returns(Task.FromResult(eventModels.AsEnumerable()));

            await _eventService.CanBeStopped(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.GetAllAsync(SampleUserId1), Times.Once());
        }

        [Test]
        public async Task CanBeStopped_ReturnsNotFoundExceptionIfNoRecordsExistsWithNullEndDateTime()
        {
            var eventModels = new List<EventModel>
            {
                new EventModel
                {
                    Id = "1",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "2",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T12:00:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "3",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T13:00:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "4",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T18:13:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                }
            };

            _eventConsumable.Setup(x => x.GetAllAsync(SampleUserId1)).Returns(Task.FromResult(eventModels.AsEnumerable()));

            var result = await _eventService.CanBeStopped(SampleUserId1, SampleId1);

            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task CanBeStopped_ReturnsIdOfEventModelWithNullEndDateTime()
        {
            var eventModels = new List<EventModel>
            {
                new EventModel
                {
                    Id = "1",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "2",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T12:00:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "3",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-22T13:00:00.000+00:00"),
                    EndDateTime = DateTime.Parse("2020-06-22T12:30:00.000+00:00")
                },
                new EventModel
                {
                    Id = "4",
                    ProjectId = SampleId1,
                    StartDateTime = DateTime.Parse("2020-06-21T18:13:00.000+00:00")
                }
            };

            _eventConsumable.Setup(x => x.GetAllAsync(SampleUserId1)).Returns(Task.FromResult(eventModels.AsEnumerable()));

            var result = await _eventService.CanBeStopped(SampleUserId1, SampleId1);

            Assert.AreEqual("4", result);
        }

        [Test]
        public async Task GetAllAsync_CallsGetAllAsyncOnceAsync()
        {
            await _eventService.GetAllAsync(SampleUserId1);

            _eventConsumable.Verify(x => x.GetAllAsync(SampleUserId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_CallsGetByIdAsyncOnce()
        {
            await _eventService.GetByIdAsync(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.GetByIdAsync(SampleUserId1, SampleId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsModelWithSpecifiedId()
        {
            _eventConsumable.Setup(x => x.GetByIdAsync(SampleUserId1, SampleId1))
                .Returns(Task.FromResult(EventModel1));

            var result = await _eventService.GetByIdAsync(SampleUserId1, SampleId1);

            Assert.AreEqual(SampleId1, result.Id);
        }

        [Test]
        public async Task UpdateAsync()
        {
            await _eventService.UpdateAsync(SampleUserId1, SampleId1, EventModel1);

            _eventConsumable.Verify(x => x.UpdateAsync(SampleUserId1, SampleId1, EventModel1), Times.Once());
        }

        [Test]
        public async Task CreateAsync()
        {
            await _eventService.CreateAsync(SampleUserId1, EventModel1);

            _eventConsumable.Verify(x => x.CreateAsync(SampleUserId1, EventModel1), Times.Once());
        }

        [Test]
        public async Task DeleteAsync()
        {
            await _eventService.DeleteAsync(SampleUserId1, SampleId1);

            _eventConsumable.Verify(x => x.DeleteAsync(SampleUserId1, SampleId1), Times.Once());
        }
    }
}
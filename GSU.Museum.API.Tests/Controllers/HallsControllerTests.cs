using AutoMapper;
using GSU.Museum.API.Controllers;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using GSU.Museum.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GSU.Museum.API.Tests.Controllers
{
    public class HallsControllerTests
    {
        private readonly ExhibitsService _exhibitsService;
        private readonly ExhibitsService _exhibitsServiceEmptyRepo;
        private readonly StandsService _standsService;
        private readonly StandsService _standsServiceEmptyRepo;
        private readonly HallsService _service;
        private readonly HallsService _serviceEmptyRepo;
        private readonly HallsController _hallsController;
        private readonly HallsController _hallsController2;
        private readonly List<Hall> _halls;
        private const string HallId = "123456789012345678901234";

        public HallsControllerTests()
        {
            var mockRepo = new Mock<IHallsRepository>();
            var mockRepoEmpty = new Mock<IHallsRepository>();
            List<Hall> _hallsEmpty = new List<Hall>();

            _halls = new List<Hall>()
            {
                new Hall()
                {
                    Id = "123456789012345678901234",
                    State = true,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = "123456789012345678901111",
                            State = true,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            TextBe = new List<string>(){ "Be1", "Be2" },
                            TextEn = new List<string>(){ "En", "En2" },
                            TextRu = new List<string>(){ "Ru1", "Ru2" },
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901212",
                                    State = true,
                                    TitleRu = "TitleRu",
                                    TitleEn = "TitleEn",
                                    TitleBe = "TitleBe",
                                    TextBe = "Be1",
                                    TextEn = "En1",
                                    TextRu = "Ru1",
                                },
                                new Exhibit()
                                {
                                    Id = "123456789012345678901213",
                                    State = true,
                                    TitleRu = "TitleRu2",
                                    TitleEn = "TitleEn2",
                                    TitleBe = "TitleBe2",
                                    TextBe = "Be12",
                                    TextEn = "En2",
                                    TextRu = "Ru12",
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678901112",
                            State = true,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            TextBe = new List<string>(){ "Be1", "Be2" },
                            TextEn = new List<string>(){ "En", "En2" },
                            TextRu = new List<string>(){ "Ru1", "Ru2" },
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901313",
                                    State = true,
                                    TitleRu = "TitleRu3",
                                    TitleBe = "TitleBe3",
                                    TextBe = "Be13",
                                    TextEn = "En13",
                                    TextRu = "Ru13",
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678903111",
                            State = true,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            TextBe = new List<string>(){ "Be1", "Be2" },
                            TextEn = new List<string>(){ "En", "En2" },
                            TextRu = new List<string>(){ "Ru1", "Ru2" },
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901415",
                                    State = true,
                                    TitleRu = "TitleRu24",
                                    TitleEn = "TitleEn24",
                                    TitleBe = "TitleBe24",
                                    TextBe = "Be124",
                                    TextRu = "Ru124",
                                }
                            }
                        },
                    }
                },
                new Hall()
                {
                    Id = "123456789012345678901235",
                    State = true,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>() 
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync()).ReturnsAsync(()
                => _halls.ToList());

            mockRepoEmpty.Setup(repo =>
               repo.GetAllAsync()).ReturnsAsync(()
                => _hallsEmpty.ToList());

            mockRepo.Setup(repo =>
               repo.GetAsync(It.IsAny<string>())).ReturnsAsync((string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(id)));

            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Hall>()))
            .Callback((Hall hall) =>
            {
                _halls.Add(hall);
            });

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<Hall>()))
            .Callback((string id, Hall hallIn) =>
            {
                var hall = _halls.FirstOrDefault(h => h.Id.Equals(id));
                hall.TitleBe = hallIn.TitleBe;
                hall.TitleEn = hallIn.TitleEn;
                hall.TitleRu = hallIn.TitleRu;
                hall.State = hallIn.State;
                hall.Stands = hallIn.Stands;
            });

            mockRepo.Setup(repo => repo.RemoveAsync(It.IsAny<string>()))
            .Callback((string id) =>
            {
                var hall = _halls.FirstOrDefault(h => h.Id.Equals(id));
                _halls.Remove(hall);
            });

            var mockRepoExhibit = new Mock<IExhibitsRepository>();
            var mockRepoEmptyExhibits = new Mock<IExhibitsRepository>();

            mockRepoExhibit.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoEmptyExhibits.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoExhibit.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits?.Where(e => e.Id.Equals(id)).FirstOrDefault());


            var mockRepoStands = new Mock<IStandsRepository>();
            var mockRepoEmptyStands = new Mock<IStandsRepository>();
            mockRepoStands.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.ToList());

            mockRepoEmptyStands.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.ToList());

            mockRepoStands.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Where(s => s.Id.Equals(id)).FirstOrDefault());

            mockRepo.SetupAllProperties();
            mockRepoEmpty.SetupAllProperties();

            _exhibitsService = new ExhibitsService(mockRepoExhibit.Object);
            _standsService = new StandsService(mockRepoStands.Object, _exhibitsService);
            _service = new HallsService(mockRepo.Object, _standsService);
            _hallsController = new HallsController(mockRepo.Object, _service);

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _standsServiceEmptyRepo = new StandsService(mockRepoEmptyStands.Object, _exhibitsServiceEmptyRepo);
            _serviceEmptyRepo = new HallsService(mockRepoEmpty.Object, _standsServiceEmptyRepo);
            _hallsController2 = new HallsController(mockRepoEmpty.Object, _serviceEmptyRepo);
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf2Records_ShouldReturnListOf2Records()
        {
            // Arrange
            const int expected = 2;

            // Act
            var list = await _hallsController.GetAll();

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _hallsController2.GetAll();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public async void GetAsync_RecordWithRequestedIdExists_ShouldReturnRequestedRecord()
        {
            // Arrange
            Hall hall = _halls.First();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>());
            mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
            .ForMember(destination => destination.Title,
                    map => map.MapFrom(
                source => source.TitleEn))
            .ForMember(destination => destination.Stands,
                map => map.Ignore())
            );
            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<HallDTO>(hall);

            // Act
            var actual = await _hallsController.GetAsync(expected.Id) as OkObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_RequestRecordByIncorrectId_ShouldReturnIncorrectIdError()
        {
            // Arrange
            const Errors errorCode = Errors.Invalid_input;
            const string message = "Incorrect id length";

            // Act
            try
            {
                StatusCodeResult actual = await _hallsController.GetAsync("1") as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCode, err.ErrorCode);
                Assert.Equal(message, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_RecordDoesNotExist_ShouldReturnNotFound()
        {
            const string id = "111111112111111111111113";

            // Arrange
            const int statusCode = StatusCodes.Status404NotFound;

            // Act
            StatusCodeResult actual = await _hallsController.GetAsync(id) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_DoubleNestedRecordDoesNotContainLocalizedTextInEnligh_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678903111";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no text in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _hallsController.GetAsync(id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_DoubleNestedRecordDoesNotContainLocalizedTitleInEnligh_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678901111";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _hallsController.GetAsync(id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }
        
        [Fact]
        public async void GetAsync_NestedRecordDoesNotContainLocalizedTitleInEnligh_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678901112";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _hallsController.GetAsync(id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_RecordDoesNotContainLocalizedTitleInEnligh_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678901234";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _hallsController.GetAsync(id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void CreateAsync_NewValidRecord_ShouldAddTheRecord()
        {
            // Arrange
            Hall expected = new Hall()
            {
                Id = "211111111111111111111111",
                TitleBe = "Be",
                TitleEn = "En",
                TitleRu = "Ru",
                State = true
            };

            // Act
            var actual = await _hallsController.CreateAsync(expected) as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Contains(expected, _halls);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecord_ShouldUpdateRecord()
        {
            // Arrange
            Hall expectd = _halls.First();
            expectd.TitleEn = "Updated";

            const int statusCode = StatusCodes.Status204NoContent;

            // Act
            StatusCodeResult actualCode = await _hallsController.UpdateAsync(expectd) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);
            Assert.Contains(expectd, _halls);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecordWithIncorrectId_ShouldReturnIncorrectIdError()
        {
            // Arrange
            Hall hall = _halls.Last();
            hall.Id = "1";
            const Errors errorCodeExpected = Errors.Invalid_input;
            string messageExpected = "Incorrect id length";

            // Act
            try
            {
                var actualCode = await _hallsController.UpdateAsync(hall);
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void UpdateAsync_UpdateRecordWhichDoesNotExist_ShouldReturnNotFoundError()
        {
            // Arrange
            Hall hall = new Hall();
            hall.Id = "323232323232323232323232";
            const int statusCode = StatusCodes.Status404NotFound;

            // Act
            StatusCodeResult actualCode = await _hallsController.UpdateAsync(hall) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);

        }

        [Fact]
        public async void DeleteAsync_DelteExistingRecord_ShouldDeleteRecord()
        {
            // Arrange
            Hall exhibitToDelete = _halls.Last();

            // Act
            var actualCode = await _hallsController.DeleteAsync(exhibitToDelete.Id) as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, actualCode.StatusCode);
            Assert.DoesNotContain(exhibitToDelete, _halls);
        }

        [Fact]
        public async void DeleteAsync_RequestedIdIsIncorrect_ShouldReturnIncorrectIdError()
        {
            // Arrange
            const string id = "1";
            const Errors errorCodeExpected = Errors.Invalid_input;
            string messageExpected = "Incorrect id length";

            // Act
            try
            {
                var actualCode = await _hallsController.DeleteAsync(id);
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void DeleteAsync_RecordWithRequestedIdDeosNotExist_ShouldReturnNotFoundError()
        {
            // Arrange
            const string id = "211111111111111111111132";
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = $"There is no record with id {id}";

            // Act
            try
            {
                var actualCode = await _hallsController.DeleteAsync(id);
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }
    }
}

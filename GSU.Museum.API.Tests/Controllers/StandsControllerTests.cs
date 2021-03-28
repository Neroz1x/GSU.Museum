using AutoMapper;
using GSU.Museum.API.Controllers;
using GSU.Museum.API.Interfaces;
using GSU.Museum.API.Services;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GSU.Museum.API.Tests.Controllers
{
    public class StandsControllerTests
    {
        private readonly ExhibitsService _exhibitsService;
        private readonly ExhibitsService _exhibitsServiceEmptyRepo;
        private readonly StandsService _service;
        private readonly StandsService _serviceEmptyRepo;
        private readonly StandsController _standsController;
        private readonly StandsController _standsController2;
        private readonly List<Hall> _halls;
        private const string HallId = "123456789012345678901234";

        public StandsControllerTests()
        {
            var mockRepo = new Mock<IStandsRepository>();
            var mockRepoEmpty = new Mock<IStandsRepository>();
            List<Hall> _hallsEmpty = new List<Hall>()
            {
                new Hall()
                {
                    Id = HallId,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Photo = new PhotoInfo(),
                    Stands = new List<Stand>()
                }
            };
            _halls = new List<Hall>()
            {
                new Hall()
                {
                    Id = "123456789012345678901234",
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Photo = new PhotoInfo(),
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = "123456789012345678901111",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Photo = new PhotoInfo(),
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901212",
                                    TitleRu = "TitleRu",
                                    TitleEn = "TitleEn",
                                    TitleBe = "TitleBe",
                                    TextBe = "Be1",
                                    TextEn = "En1",
                                    TextRu = "Ru1",
                                    Photos = new List<PhotoInfo>(),
                                },
                                new Exhibit()
                                {
                                    Id = "123456789012345678901213",
                                    TitleRu = "TitleRu2",
                                    TitleEn = "TitleEn2",
                                    TitleBe = "TitleBe2",
                                    TextBe = "Be12",
                                    TextEn = "En2",
                                    TextRu = "Ru12",
                                    Photos = new List<PhotoInfo>(),
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678901112",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Photo = new PhotoInfo(),
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901313",
                                    TitleRu = "TitleRu3",
                                    TitleBe = "TitleBe3",
                                    TextBe = "Be13",
                                    TextEn = "En13",
                                    TextRu = "Ru13",
                                    Photos = new List<PhotoInfo>(),
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678903111",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Photo = new PhotoInfo(),
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901415",
                                    TitleRu = "TitleRu24",
                                    TitleEn = "TitleEn24",
                                    TitleBe = "TitleBe24",
                                    TextBe = "Be124",
                                    TextRu = "Ru124",
                                    Photos = new List<PhotoInfo>(),
                                }
                            }
                        },
                    }
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId))?.Stands.ToList());

            mockRepoEmpty.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.ToList());

            mockRepo.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Where(s => s.Id.Equals(id)).FirstOrDefault());

            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<string>(), It.IsAny<Stand>()))
            .Callback((string hallId, Stand stand) =>
            {
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Add(stand);
            })
            .ReturnsAsync(() => "123456782032345347406865");

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stand>()))
            .Callback((string hallId, string id, Stand standIn) =>
            {
                var stand = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands
                .FirstOrDefault(s => s.Id.Equals(id));
                stand.DescriptionBe = standIn.DescriptionBe;
                stand.DescriptionEn = standIn.DescriptionEn;
                stand.DescriptionRu = standIn.DescriptionRu;
                stand.TitleBe = standIn.TitleBe;
                stand.TitleEn = standIn.TitleEn;
                stand.TitleRu = standIn.TitleRu;
            });

            mockRepo.Setup(repo => repo.RemoveAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string hallId, string id) =>
            {
                var stand = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(e => e.Id.Equals(id));
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Remove(stand);
            });

            var mockRepoExhibit = new Mock<IExhibitsRepository>();
            var mockRepoEmptyExhibits = new Mock<IExhibitsRepository>();

            mockRepo.SetupAllProperties();
            mockRepoEmpty.SetupAllProperties();

            mockRepoExhibit.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoEmptyExhibits.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoExhibit.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits?.Where(e => e.Id.Equals(id)).FirstOrDefault());


            _exhibitsService = new ExhibitsService(mockRepoExhibit.Object);
            _service = new StandsService(mockRepo.Object, _exhibitsService);
            _standsController = new StandsController(mockRepo.Object, _service, new CompareService());

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _serviceEmptyRepo = new StandsService(mockRepoEmpty.Object, _exhibitsServiceEmptyRepo);
            _standsController2 = new StandsController(mockRepoEmpty.Object, _serviceEmptyRepo, new CompareService());
        }

        [Fact]
        public void GetAll_RepositoryConsistsOf3Records_ShouldReturnListOf3Records()
        {
            // Arrange
            const int expected = 3;

            // Act
            var list = _standsController.GetAll(HallId) as OkObjectResult;

            // Assert
            Assert.Equal(expected, (list.Value as List<StandDTO>).Count);
        }

        [Fact]
        public void GetAll_RecordDoesNotFound_ShouldReturnNotFound()
        {
            // Arrange
            const int expectedStatusCode = StatusCodes.Status404NotFound;

            // Act
            var actual = _standsController.GetAll("111111189012345678901313") as NotFoundResult;

            // Assert
            Assert.Equal(expectedStatusCode, actual.StatusCode);
        }

        [Fact]
        public void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = _standsController2.GetAll(HallId) as OkObjectResult;

            // Assert
            Assert.Empty(list.Value as List<StandDTO>);
        }

        [Fact]
        public async void GetAsync_RecordWithRequestedIdExists_ShouldReturnRequestedRecord()
        {
            // Arrange
            Stand stand = _halls.First().Stands.First();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>());
            mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                    map => map.MapFrom(
                source => source.TitleEn))
            .ForMember(destination => destination.Description,
                    map => map.MapFrom(
                source => source.DescriptionEn))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                                map => map.Ignore()));

            MapperConfiguration mapperConfigurationPhoto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                .ForMember(destination => destination.Description,
                    map => map.MapFrom(
                        source => source.DescriptionEn));
            });

            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<StandDTO>(stand);
            mapper = new Mapper(mapperConfigurationPhoto);
            var photoInfoDTO = mapper.Map<PhotoInfoDTO>(stand.Photo);
            expected.Photo = photoInfoDTO;
            // Act
            var actual = await _standsController.GetAsync(HallId, expected.Id, null) as OkObjectResult;

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
                StatusCodeResult actual = await _standsController.GetAsync(HallId, "1", null) as StatusCodeResult;
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
            StatusCodeResult actual = await _standsController.GetAsync(HallId,  id, null) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashEqualsWithRetrievedRecord_ShouldReturnNoContent()
        {
            // Arrange
            var statusCode = StatusCodes.Status204NoContent;
            var stand = await _standsController.GetAsync("123456789012345678901234", "123456789012345678901111", null) as OkObjectResult;

            // Act
            var objectResult = await _standsController.GetAsync("123456789012345678901234", "123456789012345678901111", (stand.Value as StandDTO).GetHashCode()) as NoContentResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashDoesNotEqualWithRetrievedRecord_ShouldReturn200Ok()
        {
            // Arrange
            var statusCode = StatusCodes.Status200OK;

            // Act
            var objectResult = await _standsController.GetAsync("123456789012345678901234", "123456789012345678901111", 1) as OkObjectResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void CreateAsync_NewValidRecord_ShouldAddTheRecord()
        {
            // Arrange
            Stand expected = new Stand()
            {
                Id = "211111111111111111111111",
                Photo = new PhotoInfo(),
                DescriptionBe = "Be1",
                DescriptionEn = "En",
                DescriptionRu = "Ru1",
                TitleBe = "Be",
                TitleEn = "En",
                TitleRu = "Ru",
            };

            // Act
            var actual = await _standsController.CreateAsync(HallId, expected) as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Contains(expected, _halls.First().Stands);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecord_ShouldUpdateRecord()
        {
            // Arrange
            Stand expectd = _halls.First().Stands.First();
            expectd.TitleEn = "Updated";

            const int statusCode = StatusCodes.Status204NoContent;

            // Act
            StatusCodeResult actualCode = await _standsController.UpdateAsync(HallId, expectd) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);
            Assert.Contains(expectd, _halls.First().Stands);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecordWithIncorrectId_ShouldReturnIncorrectIdError()
        {
            // Arrange
            Stand stand = _halls.First().Stands.Last();
            stand.Id = "1";
            const Errors errorCodeExpected = Errors.Invalid_input;
            string messageExpected = "Incorrect id length";

            // Act
            try
            {
                var actualCode = await _standsController.UpdateAsync(HallId, stand);
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
            Stand stand = new Stand();
            stand.Id = "323232323232323232323232";
            const int statusCode = StatusCodes.Status404NotFound;

            // Act
            StatusCodeResult actualCode = await _standsController.UpdateAsync(HallId, stand) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);

        }

        [Fact]
        public async void DeleteAsync_DelteExistingRecord_ShouldDeleteRecord()
        {
            // Arrange
            Stand exhibitToDelete = _halls.First().Stands.Last();

            // Act
            var actualCode = await _standsController.DeleteAsync(HallId, exhibitToDelete.Id) as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, actualCode.StatusCode);
            Assert.DoesNotContain(exhibitToDelete, _halls.First().Stands);
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
                var actualCode = await _standsController.DeleteAsync(HallId, id);
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
                var actualCode = await _standsController.DeleteAsync(HallId, id);
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

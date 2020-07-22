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
                    Photo = new PhotoInfo(),
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = "123456789012345678901111",
                            State = true,
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
                                    State = true,
                                    TitleRu = "TitleRu",
                                    TitleEn = "TitleEn",
                                    TitleBe = "TitleBe",
                                    TextBe = "Be1",
                                    TextEn = "En1",
                                    TextRu = "Ru1",
                                    Photos = new List<PhotoInfo>()
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
                                    Photos = new List<PhotoInfo>()
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
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Photo = new PhotoInfo(),
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
                                    Photos = new List<PhotoInfo>()
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
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Photo = new PhotoInfo(),
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
                                    Photos = new List<PhotoInfo>()
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
                    Photo = new PhotoInfo(),
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
            _hallsController = new HallsController(mockRepo.Object, _service, new CompareService());

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _standsServiceEmptyRepo = new StandsService(mockRepoEmptyStands.Object, _exhibitsServiceEmptyRepo);
            _serviceEmptyRepo = new HallsService(mockRepoEmpty.Object, _standsServiceEmptyRepo);
            _hallsController2 = new HallsController(mockRepoEmpty.Object, _serviceEmptyRepo, new CompareService());
        }

        [Fact]
        public async void GetAllAsync_RepositoryConsistsOf2Records_ShouldReturnListOf2Records()
        {
            // Arrange
            const int expected = 2;

            // Act
            var list = await _hallsController.GetAllAsync(null) as OkObjectResult;

            // Assert
            Assert.Equal(expected, (list.Value as List<HallDTO>).Count);
        }

        [Fact]
        public async void GetAllAsync_HashEqualsWithRetrievedRecords_ShouldReturnNoContent()
        {
            // Arrange
            var statusCode = StatusCodes.Status204NoContent;
            var halls = await _hallsController.GetAllAsync(null) as OkObjectResult;

            // Act
            var objectResult = await _hallsController.GetAllAsync(GetHash(halls.Value as List<HallDTO>)) as NoContentResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void GetAllAsync_HashDoesNotEqualWithRetrievedRecords_ShouldReturn200Ok()
        {
            // Arrange
            var statusCode = StatusCodes.Status200OK;

            // Act
            var objectResult = await _hallsController.GetAllAsync(1) as OkObjectResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void GetAllAsync_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _hallsController2.GetAllAsync(null) as OkObjectResult;

            // Assert
            Assert.Empty((list.Value as List<HallDTO>));
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
            .ForMember(destination => destination.Photo,
                map => map.Ignore())
            );
            MapperConfiguration mapperConfigurationPhoto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                .ForMember(destination => destination.Description,
                    map => map.MapFrom(
                        source => source.DescriptionRu));
            });
            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<HallDTO>(hall);

            mapper = new Mapper(mapperConfigurationPhoto);
            var photoInfoDTO = mapper.Map<PhotoInfoDTO>(hall.Photo);
            expected.Photo = photoInfoDTO;

            // Act
            var actual = await _hallsController.GetAsync(expected.Id, null) as OkObjectResult;

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
                StatusCodeResult actual = await _hallsController.GetAsync("1", null) as StatusCodeResult;
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
            StatusCodeResult actual = await _hallsController.GetAsync(id, null) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashEqualsWithRetrievedRecord_ShouldReturnNoContent()
        {
            // Arrange
            var statusCode = StatusCodes.Status204NoContent;
            var hall = await _hallsController.GetAsync("123456789012345678901234", null) as OkObjectResult;

            // Act
            var objectResult = await _hallsController.GetAsync("123456789012345678901234", (hall.Value as HallDTO).GetHashCode()) as NoContentResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashDoesNotEqualWithRetrievedRecord_ShouldReturn200Ok()
        {
            // Arrange
            var statusCode = StatusCodes.Status200OK;

            // Act
            var objectResult = await _hallsController.GetAsync("123456789012345678901234", 1) as OkObjectResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
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

        public int GetHash(List<HallDTO> halls)
        {
            unchecked
            {
                int hash = (int)2166136261;
                foreach (var hall in halls)
                {
                    hash = (hash * 16777619) ^ (hall?.GetHashCode() ?? 1);
                }
                return hash;
            }
        }
    }
}

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
using Xunit;

namespace GSU.Museum.API.Tests.Controllers
{
    public class ExhibitsControllerTests
    {
        private readonly ExhibitsService _service;
        private readonly ExhibitsService _serviceEmptyRepo;
        private readonly ExhibitsController _exhibitsController;
        private readonly ExhibitsController _exhibitsController2;
        private readonly List<Hall> _halls;
        private const string HallId = "123456789012345678901234";
        private const string StandId = "123456789012345678901111";
        public ExhibitsControllerTests()
        {
            var mockRepo = new Mock<IExhibitsRepository>();
            var mockRepoEmpty = new Mock<IExhibitsRepository>();
            List<Hall> _hallsEmpty = new List<Hall>()
            {
                new Hall()
                {
                    Id = HallId,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = StandId,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Exhibits = new List<Exhibit>()
                        }
                    }
                }
            };
            _halls = new List<Hall>()
            {
                new Hall()
                {
                    Id = HallId,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = StandId,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
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
                                },
                                new Exhibit()
                                {
                                    Id = "123456789012345678901415",
                                    TitleRu = "TitleRu24",
                                    TitleEn = "TitleEn24",
                                    TitleBe = "TitleBe24",
                                    TextBe = "Be124",
                                    TextRu = "Ru124",
                                }
                            }
                        }
                    }
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoEmpty.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepo.Setup(repo => 
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits?.Where(e => e.Id.Equals(id)).FirstOrDefault());

            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exhibit>()))
            .Callback((string hallId, string standId, Exhibit exhibit) =>
            {
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.Add(exhibit);
            });

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exhibit>()))
            .Callback((string hallId, string standId, string id, Exhibit exhibitIn) =>
            {
                var exhibit = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands
                .FirstOrDefault(s => s.Id.Equals(standId)).Exhibits
                .FirstOrDefault(e => e.Id.Equals(id));
                exhibit.TextBe = exhibitIn.TextBe;
                exhibit.TextEn = exhibitIn.TextEn;
                exhibit.TextRu = exhibitIn.TextRu;
                exhibit.TitleBe = exhibitIn.TitleBe;
                exhibit.TitleEn = exhibitIn.TitleEn;
                exhibit.TitleRu = exhibitIn.TitleRu;
            });

            mockRepo.Setup(repo => repo.RemoveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string hallId, string standId, string id) =>
            {
                var exhibit = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.FirstOrDefault(e => e.Id.Equals(id));
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands
                .FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.Remove(exhibit);
            });

            mockRepo.SetupAllProperties();
            mockRepoEmpty.SetupAllProperties();

            _service = new ExhibitsService(mockRepo.Object);
            _exhibitsController = new ExhibitsController(mockRepo.Object, _service, new CompareService());

            _serviceEmptyRepo = new ExhibitsService(mockRepoEmpty.Object);
            _exhibitsController2 = new ExhibitsController(mockRepoEmpty.Object, _serviceEmptyRepo, new CompareService());
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf2Records_ShouldReturnListOf2Records()
        {
            // Arrange
            const int expected = 2;

            // Act
            var list = await _exhibitsController.GetAll(HallId, StandId);

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RecordDoesNotContainLocalizedTitle_ShouldReturnNotFounError()
        {
            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                var actual = await _exhibitsController.GetAll(HallId, "123456789012345678901112");
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _exhibitsController2.GetAll(HallId, StandId);

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public async void GetAsync_RecordWithRequestedIdExists_ShouldReturnRequestedRecord()
        {
            // Arrange
            Exhibit exhibit = _halls.First().Stands.First().Exhibits.First();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                .ForMember(destination => destination.Title,
                        map => map.MapFrom(
                    source => source.TitleEn))
                .ForMember(destination => destination.Text,
                        map => map.MapFrom(
                    source => source.TextEn))
                );
            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<ExhibitDTO>(exhibit);

            // Act
            var actual = await _exhibitsController.GetAsync(HallId, StandId, "123456789012345678901212", null) as OkObjectResult;

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
                StatusCodeResult actual = await _exhibitsController.GetAsync(HallId, StandId, "1", null) as StatusCodeResult;
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
            StatusCodeResult actual = await _exhibitsController.GetAsync(HallId, StandId, id, null) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashEqualsWithRetrievedRecord_ShouldReturnNoContent()
        {
            // Arrange
            var statusCode = StatusCodes.Status204NoContent;
            var exhibit = await _exhibitsController.GetAsync(HallId, StandId, "123456789012345678901212", null) as OkObjectResult;
            
            // Act
            var objectResult = await _exhibitsController.GetAsync(HallId, StandId, "123456789012345678901212", (exhibit.Value as ExhibitDTO).GetHashCode()) as NoContentResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void GetAsync_HashDoesNotEqualWithRetrievedRecord_ShouldReturn200Ok()
        {
            // Arrange
            var statusCode = StatusCodes.Status200OK;

            // Act
            var objectResult = await _exhibitsController.GetAsync(HallId, StandId, "123456789012345678901212", 1) as OkObjectResult;

            // Assert
            Assert.Equal(statusCode, objectResult.StatusCode);
        }

        [Fact]
        public async void CreateAsync_NewValidRecord_ShouldAddTheRecord()
        {
            // Arrange
            Exhibit expected = new Exhibit()
            {
                Id = "211111111111111111111111",
                Photos = new List<PhotoInfo>(),
                TextBe = "3Bel",
                TextEn = "3En",
                TextRu = "3Ru"
            };

            // Act
            var actual = await _exhibitsController.CreateAsync(HallId, StandId, expected) as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Contains(expected, _halls.First().Stands.First().Exhibits);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecord_ShouldUpdateRecord()
        {
            // Arrange
            Exhibit expectd = _halls.First().Stands.First().Exhibits.First();
            expectd.TextBe = "Updated";

            const int statusCode = StatusCodes.Status204NoContent;

            // Act
            StatusCodeResult actualCode = await _exhibitsController.UpdateAsync(HallId, StandId, expectd) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);
            Assert.Contains(expectd, _halls.First().Stands.First().Exhibits);
        }

        [Fact]
        public async void UpdateAsync_UpdateRecordWithIncorrectId_ShouldReturnIncorrectIdError()
        {
            // Arrange
            Exhibit exhibit = _halls.First().Stands.First().Exhibits.Last();
            exhibit.Id = "1";
            const Errors errorCodeExpected = Errors.Invalid_input;
            string messageExpected = "Incorrect id length";

            // Act
            try
            {
                var actualCode = await _exhibitsController.UpdateAsync(HallId, StandId, exhibit);
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
            Exhibit exhibit = new Exhibit();
            exhibit.Id = "323232323232323232323232";
            const int statusCode = StatusCodes.Status404NotFound;

            // Act
            StatusCodeResult actualCode = await _exhibitsController.UpdateAsync(HallId, StandId, exhibit) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actualCode.StatusCode);

        }

        [Fact]
        public async void DeleteAsync_DelteExistingRecord_ShoulDeleteRecord()
        {
            // Arrange
            Exhibit exhibitToDelete = _halls.First().Stands.First().Exhibits.Last();

            // Act
            var actualCode = await _exhibitsController.DeleteAsync(HallId, StandId, exhibitToDelete.Id) as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, actualCode.StatusCode);
            Assert.DoesNotContain(exhibitToDelete, _halls.First().Stands.First().Exhibits);
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
                var actualCode = await _exhibitsController.DeleteAsync(HallId, StandId, id);
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
                var actualCode = await _exhibitsController.DeleteAsync(HallId, StandId, id);
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

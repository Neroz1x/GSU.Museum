﻿using AutoMapper;
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
                    State = true,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                }
            };
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
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.ToList());

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
            });

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stand>()))
            .Callback((string hallId, string id, Stand standIn) =>
            {
                var stand = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands
                .FirstOrDefault(s => s.Id.Equals(id));
                stand.TextBe = standIn.TextBe;
                stand.TextEn = standIn.TextEn;
                stand.TextRu = standIn.TextRu;
                stand.TitleBe = standIn.TitleBe;
                stand.TitleEn = standIn.TitleEn;
                stand.TitleRu = standIn.TitleRu;
                stand.State = standIn.State;
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
            _standsController = new StandsController(mockRepo.Object, _service);

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _serviceEmptyRepo = new StandsService(mockRepoEmpty.Object, _exhibitsServiceEmptyRepo);
            _standsController2 = new StandsController(mockRepoEmpty.Object, _serviceEmptyRepo);
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf3Records_ShouldReturnListOf3Records()
        {
            // Arrange
            const int expected = 3;

            // Act
            var list = await _standsController.GetAll(HallId);

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RecordDoesNotContainLocalizedTextInEnglish_ShouldReturnNotFounError()
        {
            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no text in En language";

            // Act
            try
            {
                var actual = await _standsController.GetAll(HallId);
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
            var list = await _standsController2.GetAll(HallId);

            // Assert
            Assert.Empty(list);
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
            .ForMember(destination => destination.Text,
                    map => map.MapFrom(
                source => source.TextEn))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            );
            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<StandDTO>(stand);

            // Act
            var actual = await _standsController.GetAsync(HallId, expected.Id) as OkObjectResult;

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
                StatusCodeResult actual = await _standsController.GetAsync(HallId, "1") as StatusCodeResult;
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
            StatusCodeResult actual = await _standsController.GetAsync(HallId,  id) as StatusCodeResult;

            // Assert
            Assert.Equal(statusCode, actual.StatusCode);
        }

        [Fact]
        public async void GetAsync_NestedRecordDoesNotContainLocalizedTextInEnglish_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678903111";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no text in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _standsController.GetAsync(HallId, id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_NestedRecordDoesNotContainLocalizedTitleInEnglish_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678901111";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _standsController.GetAsync(HallId, id) as StatusCodeResult;
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCodeExpected, err.ErrorCode);
                Assert.Equal(messageExpected, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_RecordDoesNotContainLocalizedTitleInEnglish_ShouldReturnNotFounError()
        {
            const string id = "123456789012345678901112";

            // Arrange
            const Errors errorCodeExpected = Errors.Not_found;
            string messageExpected = "There is no title in En language";

            // Act
            try
            {
                StatusCodeResult actual = await _standsController.GetAsync(HallId, id) as StatusCodeResult;
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
            Stand expected = new Stand()
            {
                Id = "211111111111111111111111",
                Photos = new byte[] { 1, 2 },
                TextBe = new List<string>() { "Be", "Be2" },
                TextEn = new List<string>() { "En", "En2" },
                TextRu = new List<string>() { "Ru", "Ru2" },
                TitleBe = "Be",
                TitleEn = "En",
                TitleRu = "Ru",
                State = true
            };

            // Act
            var actual = await _standsController.CreateAsync(HallId, expected) as StatusCodeResult;

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
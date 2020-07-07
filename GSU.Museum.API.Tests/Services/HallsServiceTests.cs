using AutoMapper;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using GSU.Museum.API.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GSU.Museum.API.Tests.Services
{
    public class HallsServiceTests
    {
        private readonly ExhibitsService _exhibitsService;
        private readonly ExhibitsService _exhibitsServiceEmptyRepo;
        private readonly StandsService _standsService;
        private readonly StandsService _standsServiceEmptyRepo;
        private readonly HallsService _service;
        private readonly HallsService _serviceEmptyRepo;
        private readonly HttpRequest httpRequest;

        private readonly List<Hall> _halls;
        private const string HallId = "123456789012345678901234";

        public HallsServiceTests()
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

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _standsServiceEmptyRepo = new StandsService(mockRepoEmptyStands.Object, _exhibitsServiceEmptyRepo);
            _serviceEmptyRepo = new HallsService(mockRepoEmpty.Object, _standsServiceEmptyRepo);
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf2Records_ShouldReturnListOf2Records()
        {
            // Arrange
            const int expected = 2;

            // Act
            var list = await _service.GetAllAsync(httpRequest);

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _serviceEmptyRepo.GetAllAsync(httpRequest);

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
            var actual = await _service.GetAsync(httpRequest, expected.Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
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
                var actual = await _service.GetAsync(httpRequest, "1");
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCode, err.ErrorCode);
                Assert.Equal(message, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_RecordDoesNotExist_ShouldReturnNull()
        {
            const string id = "111111112111111111111113";

            // Arrange
            HallDTO expected = null;

            // Act
            var actual = await _service.GetAsync(httpRequest, id);

            // Assert
            Assert.Equal(expected, actual);
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
                var actual = await _service.GetAsync(httpRequest, id);
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
                var actual = await _service.GetAsync(httpRequest, id);
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
                var actual = await _service.GetAsync(httpRequest, id);
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
                var actual = await _service.GetAsync(httpRequest, id);
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

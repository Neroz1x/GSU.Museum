using AutoMapper;
using GSU.Museum.CommonClassLibrary.Models;

namespace GSU.Museum.API.AutoMapper.MappingConfigurations
{
    public class ExhibitsMappingConfigurations
    {
        public static MapperConfiguration GetAllRuConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
            source => source.TitleRu))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionRu))
            .ForMember(destination => destination.Text,
               map => map.MapFrom(
                   source => source.TextRu))
            .ForMember(destination => destination.Photos,
            map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllRuPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionRu));
        });

        public static MapperConfiguration GetAllEnConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
            source => source.TitleEn))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionEn))
            .ForMember(destination => destination.Text,
               map => map.MapFrom(
                   source => source.TextEn))
            .ForMember(destination => destination.Photos,
            map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllEnPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionEn));
        });

        public static MapperConfiguration GetAllByConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
            source => source.TitleBe))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionBe))
            .ForMember(destination => destination.Text,
               map => map.MapFrom(
                   source => source.TextBe))
            .ForMember(destination => destination.Photos,
            map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllByPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionBe));
        });

        public static MapperConfiguration GetRuConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleRu))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionRu))
            .ForMember(destination => destination.Text,
                map => map.MapFrom(
                    source => source.TextRu))
            .ForMember(destination => destination.ExhibitType,
                map => map.MapFrom(
                    source => source.ExhibitType))
            .ForMember(destenation => destenation.Photos,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetEnConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
           .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleEn))
           .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionEn))
           .ForMember(destination => destination.Text,
                map => map.MapFrom(
                    source => source.TextEn))
           .ForMember(destination => destination.ExhibitType,
                map => map.MapFrom(
                    source => source.ExhibitType))
           .ForMember(destenation => destenation.Photos,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetByConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exhibit, ExhibitDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleBe))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
            source => source.DescriptionBe))
            .ForMember(destination => destination.Text,
                map => map.MapFrom(
                    source => source.TextBe))
            .ForMember(destination => destination.ExhibitType,
                map => map.MapFrom(
                    source => source.ExhibitType))
            .ForMember(destenation => destenation.Photos,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetRuPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionRu));
        });

        public static MapperConfiguration GetEnPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionEn));
        });

        public static MapperConfiguration GetByPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionBe));
        });
    }
}

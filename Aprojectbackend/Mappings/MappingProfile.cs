using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Models;
using AutoMapper;
namespace Aprojectbackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 定義 TUserPrefer 到 UserProfileDTO 的映射
            CreateMap<TUserPrefer, UserProfileDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.FUserId))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.FHeight))
                .ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(src => src.FPhotoPath))
                .ForMember(dest => dest.Info, opt => opt.MapFrom(src => src.FInfo))
                .ForMember(dest => dest.SelectedHobbies, opt => opt.MapFrom(src => src.TUserHobbies != null ? src.TUserHobbies.Select(h => h.FHobby.FHobbyName).ToList() : new List<string>()))
                .ForMember(dest => dest.SelectedTraits, opt => opt.MapFrom(src => src.TUserTraits != null ? src.TUserTraits.Select(t => t.FTraits.FTraitsName).ToList() : new List<string>()));


            // 定義 UserProfileDTO 到 TUserPrefer 的映射，適用於資料新增
            CreateMap<UserProfileDTO, TUserPrefer>()
                .ForMember(dest => dest.FUserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FHeight, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.FPhotoPath, opt => opt.MapFrom(src => src.PhotoPath))
                .ForMember(dest => dest.FInfo, opt => opt.MapFrom(src => src.Info))
                .ForMember(dest => dest.TUserHobbies, opt => opt.Ignore()) // 忽略 TUserHobbies，需手動處理
                .ForMember(dest => dest.TUserTraits, opt => opt.Ignore()) // 忽略 TUserTraits，需手動處理
                .ForMember(dest => dest.FMaxHeight, opt => opt.Ignore()) // 忽略 FMaxHeight
                .ForMember(dest => dest.FMinHeight, opt => opt.Ignore()) // 忽略 FMinHeight
                .ForMember(dest => dest.FMaxAge, opt => opt.Ignore()) // 忽略 FMaxAge
                .ForMember(dest => dest.FMinAge, opt => opt.Ignore()); // 忽略 FMinAge
        }
    }
}



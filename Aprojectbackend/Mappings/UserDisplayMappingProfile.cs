using AutoMapper;
using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Models;
using System;
using System.Linq;

public class UserDisplayMappingProfile : Profile
{
    public UserDisplayMappingProfile()
    {
        CreateMap<TUserPrefer, UserProfileDisplayDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.FUser.FUserId))
            .ForMember(dest => dest.UserNickName ,opt => opt.MapFrom(src => src.FUser.FUserNickName)) // 從 TUser 中映射暱稱
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.FAge))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
           src.FUser.FUserGender.HasValue ? (src.FUser.FUserGender.Value ? "男" : "女") : "未指定"))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.FHeight))
            .ForMember(dest => dest.Hobbies, opt => opt.MapFrom(src =>
                src.TUserHobbies.Select(h => h.FHobby.FHobbyName).ToList()))
            .ForMember(dest => dest.Traits, opt => opt.MapFrom(src =>
                src.TUserTraits.Select(t => t.FTraits.FTraitsName).ToList()))
        .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.FInfo)); // 確保 fInfo 映射到 bio
    }


}
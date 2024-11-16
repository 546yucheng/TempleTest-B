using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Models;
using Aprojectbackend.Service.Conmon;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aprojectbackend.Services
{
    public class UserProfileService
    {
        private readonly AprojectContext _context;
        private readonly IMapper _mapper;

        public UserProfileService(AprojectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdatePhotoPathAsync(int userId, string photoPath)
        {
            // 查找用戶的 TUserPrefer 記錄
            var userPrefer = await _context.TUserPrefers.FirstOrDefaultAsync(up => up.FUserId == userId);
            if (userPrefer == null)
            {
                return false; // 如果找不到記錄，返回 false
            }

            // 更新照片路徑
            userPrefer.FPhotoPath = photoPath;
            //userPrefer.FPhotoPath = "";

            // 保存變更到資料庫
            try
            {
                await _context.SaveChangesAsync();
                return true; // 更新成功
            }
            catch (Exception ex)
            {
                // 捕獲異常，例如資料庫連線錯誤等
                return false; // 更新失敗
            }
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(int userId)
        {
            var userProfile = await _context.TUserPrefers
                .Include(u => u.TUserHobbies)
                 .ThenInclude(uh => uh.FHobby) // 確保興趣關聯被正確加載
                .Include(u => u.TUserTraits)
                 .ThenInclude(ut => ut.FTraits) // 確保特質關聯被正確加載
                .FirstOrDefaultAsync(u => u.FUserId == userId);

            if (userProfile == null)
            {
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.FUserId == userId);
                if (user == null)
                {
                    return null;
                }
                return new UserProfileDTO
                {
                    UserId = user.FUserId,
                    UserNickName = user.FUserNickName,
                    Height = null,
                    PhotoPath = string.Empty,
                    Info = string.Empty,
                    SelectedHobbies = new List<string>(),
                    SelectedTraits = new List<string>()
                };
            }

            // 使用 AutoMapper 進行基本屬性映射
            var userProfileDto = _mapper.Map<UserProfileDTO>(userProfile);

            matchphoto matchphoto = new matchphoto();
            userProfileDto.PhotoPath = matchphoto.Base64ImageData(userProfile.FPhotoPath);

            // 查找用戶的暱稱
            var userEntity = await _context.TUsers.FirstOrDefaultAsync(u => u.FUserId == userId);
            if (userEntity != null)
            {
                userProfileDto.UserNickName = userEntity.FUserNickName;
            }
            // 手動設置 SelectedHobbies 和 SelectedTraits
            userProfileDto.SelectedHobbies = userProfile.TUserHobbies.Select(h => h.FHobby.FHobbyName).ToList();
            userProfileDto.SelectedTraits = userProfile.TUserTraits.Select(t => t.FTraits.FTraitsName).ToList();

            // 添加日誌輸出檢查
            Console.WriteLine($"Selected Hobbies: {string.Join(", ", userProfileDto.SelectedHobbies)}");
            Console.WriteLine($"Selected Traits: {string.Join(", ", userProfileDto.SelectedTraits)}");

            return userProfileDto;
        }


        public async Task<bool> UpdateUserProfileAsync(UserProfileDTO userProfileDto)
        {
            var userProfile = await _context.TUserPrefers
                .Include(u => u.TUserHobbies)
                .Include(u => u.TUserTraits)
                .FirstOrDefaultAsync(u => u.FUserId == userProfileDto.UserId);

            if (userProfile == null)
            {
                Console.WriteLine($"找不到會員資料 FUserId: {userProfileDto.UserId}");
                return false;
            }

            // 使用 AutoMapper 映射非集合屬性
            _mapper.Map(userProfileDto, userProfile);

            // 刪除現有興趣和特質
            _context.TUserHobbies.RemoveRange(userProfile.TUserHobbies);
            _context.TUserTraits.RemoveRange(userProfile.TUserTraits);

            // 添加新的興趣和特質
            foreach (var hobbyName in userProfileDto.SelectedHobbies)
            {
                var hobby = await _context.THobbies.FirstOrDefaultAsync(h => h.FHobbyName == hobbyName);
                if (hobby != null)
                {
                    userProfile.TUserHobbies.Add(new TUserHobby { FUserPreferId = userProfile.FUserPreferId, FHobbyId = hobby.FHobbyId });
                }
            }

            foreach (var traitName in userProfileDto.SelectedTraits)
            {
                var trait = await _context.TTraits.FirstOrDefaultAsync(t => t.FTraitsName == traitName);
                if (trait != null)
                {
                    userProfile.TUserTraits.Add(new TUserTrait { FUserPreferId = userProfile.FUserPreferId, FTraitsId = trait.FTraitsId });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新用戶資料時發生錯誤: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateUserProfileAsync(UserProfileDTO userProfileDto)
        {
            var existingUserProfile = await _context.TUserPrefers.FirstOrDefaultAsync(up => up.FUserId == userProfileDto.UserId);
            if (existingUserProfile != null)
            {
                return false;
            }

            var userProfile = _mapper.Map<TUserPrefer>(userProfileDto);

            try
            {
                await _context.TUserPrefers.AddAsync(userProfile);
                await _context.SaveChangesAsync();

                // 手動添加興趣和特質
                userProfile.TUserHobbies = new List<TUserHobby>();
                userProfile.TUserTraits = new List<TUserTrait>();

                foreach (var hobbyName in userProfileDto.SelectedHobbies)
                {
                    var hobby = await _context.THobbies.FirstOrDefaultAsync(h => h.FHobbyName == hobbyName);
                    if (hobby != null)
                    {
                        userProfile.TUserHobbies.Add(new TUserHobby { FUserPreferId = userProfile.FUserPreferId, FHobbyId = hobby.FHobbyId });
                    }
                }

                foreach (var traitName in userProfileDto.SelectedTraits)
                {
                    var trait = await _context.TTraits.FirstOrDefaultAsync(t => t.FTraitsName == traitName);
                    if (trait != null)
                    {
                        userProfile.TUserTraits.Add(new TUserTrait { FUserPreferId = userProfile.FUserPreferId, FTraitsId = trait.FTraitsId });
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"創建用戶資料時發生錯誤: {ex.Message}");
                return false;
            }
        }
        public async Task<TUserPrefer> GetUserPreferByUserIdAsync(int userId)
        {
            // 檢查 UserPrefer 表中是否有與 userId 對應的記錄
            return await _context.TUserPrefers.FirstOrDefaultAsync(up => up.FUserId == userId);
        }
    }
}


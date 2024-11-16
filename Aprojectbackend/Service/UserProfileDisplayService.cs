using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Models;
using Aprojectbackend.Service.Conmon;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class UserProfileDisplayService
{
    private readonly AprojectContext _context;
    private readonly IMapper _mapper;

    public UserProfileDisplayService(AprojectContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // 獲取單個用戶資料
    public UserProfileDisplayDTO GetUserProfileDisplay(int userId)
    {
        var userPrefer = _context.TUserPrefers
    .Include(up => up.FUser) // 載入 TUser 以取得性別、年齡、暱稱
    .Include(up => up.TUserHobbies)
        .ThenInclude(uh => uh.FHobby)
    .Include(up => up.TUserTraits)
        .ThenInclude(ut => ut.FTraits)
    .FirstOrDefault(up => up.FUserId == userId);

        // 如果找不到對應的 TUserPrefer 記錄，則返回 null
        if (userPrefer == null) return null;

        // 使用 AutoMapper 將 TUserPrefer 映射到 UserProfileDisplayDTO
        var userProfileDisplayDTO = _mapper.Map<UserProfileDisplayDTO>(userPrefer);

        return userProfileDisplayDTO;
    }

    // 獲取所有用戶資料
    public IEnumerable<UserProfileDisplayDTO> GetAllUserProfiles()
    {
        var userPrefers = _context.TUserPrefers
            .Include(up => up.FUser) // 載入 TUser 以取得性別、年齡、暱稱
            .Include(up => up.TUserHobbies)
                .ThenInclude(uh => uh.FHobby)
            .Include(up => up.TUserTraits)
                .ThenInclude(ut => ut.FTraits)
            .ToList();

        //UserProfileDisplayDTO dto = new UserProfileDisplayDTO();
        //// 建構子內的設定
        //var a = dto.Age;

        var myResult = userPrefers
            .Select(e => new UserProfileDisplayDTO(e))
            .ToList();

        var result = _mapper.Map<IEnumerable<UserProfileDisplayDTO>>(userPrefers);

        // =======================================
        matchphoto matchphoto = new matchphoto();
        foreach (var item in result)
        {

            item.Base64ImageData = matchphoto.Base64ImageData(item.FPhotoPath);
        }

        return result;
    }

    //private string GetMimeType(string extension)
    //{
    //    return extension switch
    //    {
    //        ".jpg" or ".jpeg" => "image/jpeg",
    //        ".png" => "image/png",
    //        ".bmp" => "image/bmp",
    //        ".gif" => "image/gif",
    //        ".webp" => "image/webp",
    //        _ => null,
    //    };
    //}

    //private string Base64ImageData(string photoPath)
    //{
    //    string result = "";

    //    string defaultImagePath = "uploads/預設.jpg";
    //    string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    //    string filePath = string.IsNullOrEmpty(photoPath) ? Path.Combine(uploadsFolderPath, defaultImagePath) : Path.Combine(uploadsFolderPath, photoPath);

    //    if (File.Exists(filePath))
    //    {
    //        byte[] imageBytes = File.ReadAllBytes(filePath);
    //        string fileExtension = Path.GetExtension(filePath).ToLower();
    //        string mimeType = GetMimeType(fileExtension);

    //        if (!string.IsNullOrEmpty(mimeType))
    //        {
    //            result = $"data:{mimeType};base64,{Convert.ToBase64String(imageBytes)}";
    //        }
    //    }
    //    return result;
    //}
}


// =======================================


// 使用 AutoMapper 將所有 TUserPrefer 映射到 UserProfileDisplayDTO
//        return result;
//    }
//}
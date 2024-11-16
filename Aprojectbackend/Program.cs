global using SendEmailService.Service.EmailService;
global using SendEmailService.Models;
using Aprojectbackend.Mappings;
using Aprojectbackend.Models;
using Aprojectbackend.Service.orderInterface;



using Aprojectbackend.Service.orderService;
using Aprojectbackend.Service.User;
using Aprojectbackend.Service.Conmon;


//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Aprojectbackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;
using Aprojectbackend.Service.LinepayService;
using Aprojectbackend.Service.Linepayinterface;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Aprojectbackend.Models.PartialClass;
using Aprojectbackend.DTO.UserDTO;

//yu
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

//Line Pay
builder.Services.AddHttpClient("LinePayClient", client =>
{
    client.BaseAddress = new Uri("https://sandbox-api-pay.line.me/v2/");
});

builder.Services.AddSingleton(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("LinePayClient");
    return new LinePayClient(httpClient, "2006573163", "33d464feb1fe3f08fc48aa607891bebd");
});

//builder.Services.AddHttpClient<LinePayClient>(client =>
//{
//    client.BaseAddress = new Uri("https://sandbox-api-pay.line.me/v2/"); // Sandbox 環境 URL
//}).AddSingleton(sp => new LinePayClient(
//    sp.GetRequiredService<HttpClient>(),
//    "YourChannelId",       // 替換為您的 LINE Pay Channel ID
//    "YourChannelSecret"    // 替換為您的 LINE Pay Channel Secret
//));

//yu
builder.Services.AddScoped<UserProfileService>(); // 註冊 UserProfileService
builder.Services.AddScoped<UserProfileDisplayService>(); // 註冊 UserProfileDisplayService
builder.Services.AddScoped<IEmailService, EmailService>(); // 註冊 EmailService
builder.Services.AddScoped<LinepayService>();
builder.Services.AddScoped<ILinepayService, LinepayService>();

//Jia
// ���U�A��
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderEmailService, OrderEmailService>();

//Jarek
// �b builder.Services �϶����K�[�G
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.AddScoped<IUserEmailService, UserEmailService>();
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddDbContext<AprojectContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Aproject"));
});

//定Cors策略 王老師教的
string PolicyName = "All";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:4200").WithMethods("*").WithHeaders("*");
    });
});

//Jarek

builder.Services.AddScoped<IPasswordService, PasswordService>();

//JWT

// 加入 JWT 驗證服務
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "未授權",
                message = "您需要登入才能訪問此資源"
            });
        }
    };
});
//JWTmiddleware
// 加入 JWT 設定
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));



// Cors Angular學的
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
//yu
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>(); // 原本的 MappingProfile
    cfg.AddProfile<UserDisplayMappingProfile>(); // 新的 UserProfileDisplayMappingProfile
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        // 設定 XML 註解文件路徑
        var xmlFile = $"Aprojectbackend.xml"; // 這裡的名稱要正確
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//()沒寫表示沒有指定的策略，等於是在各個controller套用
//若( )有填會影響全部的Controller
app.UseCors("All");
//app.UseCors();

app.UseAuthentication();

// 在 app.UseAuthorization(); 之前加入
app.UseMiddleware<UserContextMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

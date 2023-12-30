using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Data;
using DndDungeons.API.Mappings;
using DndDungeons.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using DndDungeons.API.Middlewares;
using NZWalks.API.Repositories;

//var builder = WebApplication.CreateBuilder(args);
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Serilog.Core.Logger logger = new LoggerConfiguration().WriteTo.Console().MinimumLevel.Information().CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// Now we are going to add Authorization so we don't have to be using Postman:
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DnD Dungeons API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<DndDungeonsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DndDungeonsConnectionString")));

builder.Services.AddDbContext<DndDungeonsAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DndDungeonsAuthConnectionString")));

// Repositories need to be added to Program.cs:
builder.Services.AddScoped<IDnDRegionRepository, SQLDnDRegionRepository>();
builder.Services.AddScoped<IDungeonRepository, SQLDungeonRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Configuring the identity of the users:
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("DndDungeons")
    .AddEntityFrameworkStores<DndDungeonsAuthDbContext>()
    .AddDefaultTokenProviders();
//.AddEntityFrameworkStores<NZWalksDbContext>() THIS THROWS AN ERROR!

// Configuring the requirement for the password of the users:
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    //options.Password.RequiredUniqueChars = 1 // We won't require any unique chars
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Comes from the Jwt object inside appsettings.json file
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

// Before Authorization, we need Authentication, so let's add it!
app.UseAuthentication();
app.UseAuthorization();

// Images, CSS and HTML are all static files, so we need to enable it on Program.cs:
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    // https://localhost:1234/Images
});

app.MapControllers();

app.UseCors("AllowSwagger");

app.Run();


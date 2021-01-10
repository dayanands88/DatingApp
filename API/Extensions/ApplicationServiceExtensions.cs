using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SingnaIR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ItokenService,Tokenservice>();
            services.AddScoped<IPhotoService,PhotoService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository,MessageRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                
            });
            
            
            //  services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            //  ,sqlOptions => {  
            //         sqlOptions.AddRowNumberSupport() ;
            //     }
            //  );
//              services
//    .AddDbContext<DemoDbContext>(builder => builder
//          .UseSqlServer("conn-string", sqlOptions =>
//           {
//                 sqlOptions.AddRowNumberSupport();
//           });
            return services;
        }
    }
}
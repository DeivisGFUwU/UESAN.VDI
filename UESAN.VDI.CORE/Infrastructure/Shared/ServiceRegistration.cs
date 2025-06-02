using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Services;
using UESAN.VDI.CORE.Core.Settings;
using UESAN.VDI.CORE.Infrastructure.Repositories;

namespace UESAN.VDI.CORE.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<JWTSettings>(_config.GetSection("JWTSettings"));

            services.AddTransient<IJWTService, JWTService>();
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();
            services.AddTransient<IUsuariosService, UsuariosService>();
            services.AddTransient<IProfesoresRepository, ProfesoresRepository>();
            services.AddTransient<IProfesoresService, ProfesoresService>((provider) =>
            {
                var profesoresRepo = provider.GetRequiredService<IProfesoresRepository>();
                var usuariosRepo = provider.GetRequiredService<IUsuariosRepository>();
                return new ProfesoresService(profesoresRepo, usuariosRepo);
            });
            services.AddTransient<IProyectosRepository, ProyectosRepository>();
            services.AddTransient<IProyectosService, ProyectosService>();
            services.AddTransient<IPublicacionesRepository, PublicacionesRepository>();
            services.AddTransient<IPublicacionesService, PublicacionesService>();
            services.AddTransient<IRevistasRepository, RevistasRepository>();
            services.AddTransient<IRevistasService, RevistasService>();
            services.AddTransient<IFormulariosInvestigacionRepository, FormulariosInvestigacionRepository>();
            services.AddTransient<IFormulariosInvestigacionService, FormulariosInvestigacionService>();
            services.AddTransient<IAsignacionProyectoRepository, AsignacionProyectoRepository>();
            services.AddTransient<IAsignacionProyectoService, AsignacionProyectoService>();
            services.AddTransient<IAutoresPublicacionRepository, AutoresPublicacionRepository>();
            services.AddTransient<IAutoresPublicacionService, AutoresPublicacionService>();
            services.AddTransient<ISesionChatRepository, SesionChatRepository>();
            services.AddTransient<ISesionChatService, SesionChatService>();
            services.AddTransient<IMensajeChatRepository, MensajeChatRepository>();
            services.AddTransient<IMensajeChatService, MensajeChatService>();

            var issuer = _config["JWTSettings:Issuer"];
            var audience = _config["JWTSettings:Audience"];
            var secretKey = _config["JWTSettings:SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

               .AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,

                       ValidIssuer = issuer,
                       ValidAudience = audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                   };
               });
        }
    }
}

using System.Reflection;

namespace ParkingApi.Extensions;

public static class SwaggerGenWithBearer
{
    public static void SwaggerWithBearerToken(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Por favor, ingrese el token JWT como 'Bearer <token>' en la cabecera."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            var info = new OpenApiInfo()
            {
                Title = "ParqueaderoAPI",
                Version = "v1",
                Description = "API Rest para el control de vehículos por parqueaderos.",
                Contact = new OpenApiContact()
                {
                    Name = "Laura Daniela Pumarejo Garcia",
                    Email = "lauradanielapg97@gmail.com",
                }

            };

            options.SwaggerDoc("v1", info);
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }        
}

using Microsoft.OpenApi.Models;
using System.Text;

namespace University.API.Utils
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //when an endpoint is authorized, required a security token for testing that endpoint
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversityManagement API", Version = "v1" });
                c.CustomSchemaIds(GenerateSchemaId);
            });
            return services;
        }

        private static string GenerateSchemaId(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }
            var genericType = type.GetGenericTypeDefinition();
            var genericArguments = type.GetGenericArguments();
            StringBuilder sb = new StringBuilder();
            sb.Append(RemoveGenericTypeParameterCount(genericType.Name));
            sb.Append("<");

            for (var i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");

                }
                sb.Append(GenerateSchemaId(genericArguments[i]));
            }

            sb.Append(">");
            return sb.ToString();
        }

        private static string RemoveGenericTypeParameterCount(string typeName)
        {
            // Remove the backtick and number from the type name ("BaseQueryListResponse`1" -> "BaseQueryListResponse")
            int backtickIndex = typeName.IndexOf('`');
            return backtickIndex >= 0 ? typeName.Substring(0, backtickIndex) : typeName;
        }
    }
}

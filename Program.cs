using API_Eventos.DataContext;
using API_Eventos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API_Eventos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Migrar banco de dados (se necessário)
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApiDataContext>();
                dbContext.Database.Migrate();
            }

            // Executar a aplicação
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        // Configurar o banco de dados
                        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                        services.AddDbContext<ApiDataContext>(options => options.UseSqlServer(connectionString));

                        // Configurar os serviços adicionais
                        services.AddScoped<UserService>();

                        services.AddControllers();

                        // Configurar a documentação da API com Swagger
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Eventos", Version = "v1" });

                            // Configurar a autenticação JWT para o Swagger
                            var securityScheme = new OpenApiSecurityScheme
                            {
                                Name = "Authorization",
                                Description = "Informe o token de autenticação JWT no formato 'Bearer {token}'",
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                BearerFormat = "JWT"
                            };

                            c.AddSecurityDefinition("Bearer", securityScheme);
                            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                                    new List<string>()
                                }
                            });
                        });


                        // Configurar a autenticação JWT
                        var jwtConfig = hostContext.Configuration.GetSection("Jwt");
                        var secretKey = Encoding.ASCII.GetBytes(jwtConfig["Secret"]);
                        services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                        });
                    })
                    .Configure((hostContext, app) =>
                    {
                        if (hostContext.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Eventos v1");
                            c.RoutePrefix = string.Empty;
                        });

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}

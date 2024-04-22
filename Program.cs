using JWT_Authentication.Models;
using JWT_Authentication.Serives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace JWT_Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                // Add security definition for JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Authentication using JWT Token",
                    Type = SecuritySchemeType.Http
                });
                // Add security requirement for endpoints requiring authorization
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            // Configure JWT authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    };
                }
            );

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<IBookService, BookService>();
            builder.Services.AddSingleton<UserService>();

            var app = builder.Build();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Authentication");
                c.RoutePrefix = string.Empty;
            }); // automatically transfer to SwaggerUI

            // Map endpoints
            app.MapPost("/login", (UserLogin user, UserService service) => Login(user, service, builder));
            app.MapPost("/register", (User user, UserService service) => Register(user, service));
            app.MapPost("/createBook",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (Book book, IBookService service) => Create(book, service));
            app.MapGet("/getBooks",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, user")]
            (int id, IBookService service) => Get(id, service));
            app.MapGet("/getAllBooks", (IBookService service) => GetAll(service));
            app.MapPut("/updateBook",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (Book book, IBookService service) => Update(book, service));
            app.MapDelete("/deleteBook",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (int id, IBookService service) => Delete(id, service));

            app.Run();
        }

        // Method for handling user login
        private static IResult Login(UserLogin user, UserService service, WebApplicationBuilder builder)
        {
            // Validate user input
            if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.password))
            {
                // Authenticate user
                User loggedInUser = service.Get(user);
                if (loggedInUser == null) return Results.NotFound("User is not found");

                // Generate JWT token
                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.username),
                    new Claim(ClaimTypes.Email, loggedInUser.email),
                    new Claim(ClaimTypes.GivenName, loggedInUser.firstName),
                    new Claim(ClaimTypes.Surname, loggedInUser.lastName),
                    new Claim(ClaimTypes.Uri, loggedInUser.facebook),
                    new Claim(ClaimTypes.Role, loggedInUser.role),
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: builder.Configuration["Jwt:Issuer"],
                    audience: builder.Configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(tokenString);
            }
            return Results.BadRequest("Something went wrong");
        }

        // Method for handling user registration
        private static IResult Register(User user, UserService userService)
        {
            // Register user
            User result = userService.Post(user);
            if (result == null) return Results.BadRequest("Check your data format");
            return Results.Ok(user);
        }

        // Method for creating a book
        private static IResult Create(Book book, IBookService service)
        {
            // Create book
            Book result = service.Create(book);
            return Results.Ok(result);
        }

        // Method for getting a book by ID
        private static IResult Get(int id, IBookService service)
        {
            // Get book
            Book result = service.Get(id);
            if (result == null) return Results.NotFound("Book is not found");
            return Results.Ok(result);
        }

        // Method for getting all books
        private static IResult GetAll(IBookService service)
        {
            // Get all books
            List<Book> results = service.GetAll();
            if (results == null) return Results.NotFound("There are no books");
            return Results.Ok(results);
        }

        // Method for updating a book
        private static IResult Update(Book book, IBookService service)
        {
            // Update book
            Book result = service.Update(book);
            return Results.Ok(result);
        }

        // Method for deleting a book
        private static IResult Delete(int id, IBookService service)
        {
            // Delete book
            bool result = service.Delete(id);
            if (!result) return Results.NotFound("Book is not found");
            return Results.Ok(result);
        }
    }
}

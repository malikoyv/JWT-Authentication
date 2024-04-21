using JWT_Authentication.Models;
using JWT_Authentication.Serives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace JWT_Authentication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Authentification using JWT Token",
                Type = SecuritySchemeType.Http
            });
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
        builder.Services.AddSingleton<IBookService , BookService>();
        builder.Services.AddSingleton<UserService>();

        var app = builder.Build();
        app.UseAuthorization();
        app.UseAuthentication();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Authentification");
            c.RoutePrefix = string.Empty;
        }); // automatically transfer to SwaggerUI

        app.MapPost("/login", (UserLogin user, UserService service) => Login(user, service, builder));
        app.MapPost("/createBook",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (Book book, IBookService service) => Create(book, service));
        app.MapGet("/getBooks",
            (int id, IBookService service) => Get(id, service));
        app.MapGet("/getAllBooks",
            (IBookService service) => GetAll(service));
        app.MapPut("/updateBook",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (Book book, IBookService service) => Update(book, service));
        app.MapDelete("/deleteBook",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
            (int id, IBookService service) => Delete(id, service));

        app.Run();
    }
    private static IResult Login(UserLogin user, UserService service, WebApplicationBuilder builder)
    {
        if (!string.IsNullOrEmpty(user.username) &&  !string.IsNullOrEmpty(user.password))
        {
            User loggedInUser = service.Get(user);
            if (loggedInUser == null) return Results.NotFound("User is not found");
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.username),
                new Claim(ClaimTypes.Email, loggedInUser.email),
                new Claim(ClaimTypes.GivenName, loggedInUser.firstName),
                new Claim(ClaimTypes.Surname, loggedInUser.lastName),
                new Claim(ClaimTypes.Uri, loggedInUser.facebook),
                new Claim(ClaimTypes.Role, loggedInUser.role)
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
    private static IResult Create(Book book, IBookService service)
    {
        Book result = service.Create(book);
        return Results.Ok(result);
    }
    private static IResult Get(int id, IBookService service)
    {
        Book result = service.Get(id);
        if (result == null) return Results.NotFound("Book is not found");
        return Results.Ok(result);
    }
    private static IResult GetAll(IBookService service)
    {
        List<Book> results = service.GetAll();
        if (results == null) return Results.NotFound("There is any book");
        return Results.Ok(results);
    }
    private static IResult Update(Book book, IBookService service)
    {
        Book result = service.Update(book);
        return Results.Ok(result);
    }
    private static IResult Delete(int id, IBookService service)
    {
        bool result = service.Delete(id);
        if (!result) return Results.NotFound("Book is not found");
        return Results.Ok(result);
    }
}

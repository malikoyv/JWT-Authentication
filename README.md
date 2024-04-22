# JWT Authentication with ASP.NET Core
This repository provides a comprehensive implementation of JWT (JSON Web Token) authentication/authorization in ASP.NET Core, showcasing secure user authentication, registration, and authorization for accessing protected endpoints. The project utilizes the latest features of .NET 6.0 and integrates Swagger for a user-friendly interface to test endpoints.

## Features
- **JWT Authentication:** Secure user authentication using JSON Web Tokens.
- **User Registration and Login:** User registration and login endpoints with password hashing for enhanced security.
- **Role-Based Authorization:** Role-based access control for endpoints (admin vs. user).
- **Swagger Integration:** Integration of Swagger UI for easy testing and documentation of API endpoints.

## Installation
1. **Clone the Repository:** Clone this repository to your local machine using Git. \
`git clone https://github.com/your_username/repo_name.git`
2. **Install Dependencies:** Install the required dependencies using NuGet Package Manager.
```
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 6.0.x
Install-Package Microsoft.IdentityModel.Tokens
Install-Package System.IdentityModel.Tokens.Jwt
Install-Package Swashbuckle.AspNetCore
Install-Package BCrypt.Net-Core
```
3. **Configure JWT Settings:** Customize JWT issuer, audience, and key settings in the `appsettings.json` file.
4. **Build and Run:** Build and run the project using Visual Studio or the .NET CLI.
```
dotnet build
dotnet run
```

## Usage
- **Swagger UI:** Access the Swagger UI interface at /swagger/index.html to test and explore the API endpoints.
- **User Registration:** Register new users using the /register endpoint with a POST request.
- **User Login:** Authenticate users using the /login endpoint with a POST request to obtain a JWT token.
- **Protected Endpoints:** Access protected endpoints by including the JWT token in the Authorization header with the Bearer scheme.

## Configuration
Customize the application settings, JWT configuration, and other parameters in the `appsettings.json` file according to your requirements.
```
{
  "Jwt": {
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "Key": "YourSecretKey"
  }
}
```

## Contributing
Contributions are welcome! If you encounter any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.

## License
This project is licensed under the GNU License.
#
**Author:** Yehor Malikov \
**Email:** miggabit@gmail.com \
**LinkedIn:** [Yehor Malikov](https://www.linkedin.com/in/yehormalikov/)
#

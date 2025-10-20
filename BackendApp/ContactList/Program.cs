using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using ContactList.Entities;
using ContactList.DTOs;
using MiniValidation;
using ContactList.Helpers;
using ContactList.Interfaces;
using ContactList.Repositories;
using ContactList.BusinessLogicServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Update;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ContactDb")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DbContext>()
.AddSignInManager<SignInManager<User>>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ContactAPI";
    config.Title = "ContactAPI v1";
    config.Version = "v1";

    config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Name = "Authorization",
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    config.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();

app.MapPost("/contacts", async (CreateContactRequest contactRequest, IContactRepository repository) =>
{
    if (!MiniValidator.TryValidate(contactRequest, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    if (!Enum.TryParse<ContactType>(contactRequest.Category, true, out var parsedCategory))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>() { { "Category", new string[] { $"Invalid category: {contactRequest.Category}" } } });
    }

    var contact = new Contact
    {
        FirstName = contactRequest.FirstName,
        LastName = contactRequest.LastName,
        Email = contactRequest.Email,
        DateOfBirth = contactRequest.DateOfBirth,
        Category = parsedCategory,
        SubCategory = contactRequest.SubCategory,
        TelephoneNumber = contactRequest.TelephoneNumber
    };

    await repository.InsertContactAsync(contact);
    return Results.Created($"/contacts/{contact.Id}", contact);
})
.RequireAuthorization();

app.MapGet("/contacts", async (IContactRepository repository) =>
{
    return Results.Ok(await repository.GetAllContactsAsync());
});
app.MapGet("/contacts/{id:int}", async (int id, IContactRepository repository) =>
{
    var contact = await repository.GetContactByIdAsync(id);
    if (contact == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(contact);
});

app.MapPut("/contacts", async (EditContactRequest req, IContactRepository repository) =>
{
    if (!MiniValidator.TryValidate(req, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    var dbContact = await repository.GetContactByIdAsync(req.Id);
    if (dbContact == null)
    {
        return Results.NotFound();
    }

    if (!Enum.TryParse<ContactType>(req.Category, true, out var parsedCategory))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>() { { "Category", new string[] { $"Invalid category: {req.Category}" } } });
    }

    dbContact.DateOfBirth = req.DateOfBirth;
    dbContact.FirstName = req.FirstName;
    dbContact.LastName = req.LastName;
    dbContact.Email = req.Email;
    dbContact.TelephoneNumber = req.TelephoneNumber;
    dbContact.Category = parsedCategory;
    dbContact.SubCategory = req.SubCategory;

    await repository.SaveContactChangesAsync();
    return Results.Ok(dbContact);
})
.RequireAuthorization();

app.MapDelete("/contacts/{id:int}", async (int id, IContactRepository repository) =>
{
    var contact = await repository.GetContactByIdAsync(id);
    if (contact == null)
    {
        return Results.NotFound();
    }

    await repository.DeleteContactAsync(contact);
    return Results.Ok();
})
.RequireAuthorization();

app.MapPost("/register", async (RegisterRequest req, IUserService userService) =>
{
    if (!MiniValidator.TryValidate(req, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    var result = await userService.Register(req.UserName, req.Email, req.Password);

    if (!result.Succeeded)
    {
        return Results.BadRequest(result.Errors);
    }

    return Results.Ok();
});

app.MapPost("/login", async (LoginRequest req, IUserService userService) =>
{
    if (!MiniValidator.TryValidate(req, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    var token = await userService.Login(req.UserName, req.Password);
    if (token == null) return Results.Unauthorized();
    return Results.Ok(new { token });
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();

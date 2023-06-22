using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
// add Cors
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy=>
            {
                //policy.WithOrigins("https://24h.com.vn");
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }
            );
    }
);
// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options
    => options.SerializerSettings.ReferenceLoopHandling
    = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<T2203E_API.Entities.T2203eApiContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("T2203E_API"))
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// add Handlers
builder.Services.AddSingleton<IAuthorizationHandler, T2203E_API.Handlers.ValidBirthdayHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireUserName("rootadmin"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));

    options.AddPolicy("Dev", policy => policy.RequireClaim("IT","Developer"));
    options.AddPolicy("QA", policy => policy.RequireClaim("IT","QA"));
    options.AddPolicy("BA", policy => policy.RequireClaim("IT", "BA"));

    options.AddPolicy("DEV_QA", policy => policy.RequireAssertion(
        context => context.User.HasClaim(claim => claim.Type == "IT"
        && (claim.Value == "QA" || claim.Value == "Developer")
        )));

    options.AddPolicy("DEV_ADMIN", policy => policy.RequireAssertion(
        context => context.User.HasClaim(claim => claim.Type == "IT" && claim.Value == "Developer")
           && context.User.IsInRole("Admin")
        ));
    options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
    var min = builder.Configuration["ValidYearOld:Min"];
    options.AddPolicy("ValidYearOld", policy => policy.AddRequirements(
        new T2203E_API.Requirements.YearOldRequirement(Convert.ToInt32(builder.Configuration["ValidYearOld:Min"]),
                                Convert.ToInt32(builder.Configuration["ValidYearOld:Max"]))
        ));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


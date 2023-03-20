using ICS.User.API.Odata;
using ICS.User.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => {
        opt.Conventions.Remove(opt.Conventions.OfType<MetadataRoutingConvention>().First());
        opt.AddRouteComponents("odata", EdmModel.GetEdmModel()).Filter().Expand().SkipToken().OrderBy().Count().Select(); }
    )
    .AddJsonOptions(x => {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpClient();

var environment = builder.Environment;

if (environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
else
    builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructure(builder.Configuration);

// get assemblies in runtime
var assembliesInRuntime = typeof(Program).Assembly.GetTypes();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes("secret.secret.secret.secret.secret.secret.secret.secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                      ForwardedHeaders.XForwardedProto
});

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

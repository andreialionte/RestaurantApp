using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Repository;
using RestaurantApp.Repository.IRepository;
using RestaurantApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
/*builder.Services.AddScoped<IRepository<User>, Repository<User>>();*/
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantApp API", Version = "v1" });

    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddCors(ops =>
{
    ops.AddPolicy("DevCors", opts =>
    {
        opts.AllowAnyHeader();
        opts.AllowAnyMethod();
        opts.AllowCredentials();
        opts.WithOrigins("http://localhost:4200");
    });
    ops.AddPolicy("ProdCors", opts =>
    {
        opts.AllowAnyMethod();
        opts.AllowAnyHeader();
        opts.AllowCredentials();
        opts.WithOrigins("https://restaurant-app-frontend-three.vercel.app");
    });
});


//Other method: adding the DefaultConnection ConnectionString in DataContextEf
builder.Services.AddDbContext<DataContextEf>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var awsSettings = builder.Configuration.GetSection("AWS");
var accessKeyId = awsSettings["AccessKeyId"];
var secretAccessKey = awsSettings["SecretAccessKey"];
var awsRegion = awsSettings["Region"];

var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = credentials;
awsOptions.Region = RegionEndpoint.GetBySystemName(awsRegion);

/*builder.Services.AddAWSService<IAmazonS3>();*/
builder.Services.AddAWSService<IAmazonS3>(awsOptions);

builder.Services.AddDefaultAWSOptions(awsOptions);

builder.Services.AddTransient<IUploadService, UploadService>();

//test
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
        /*policy.RequireClaim()*/ //we should use claims to get the userId or userName or userRole
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TokenKey"))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ProdCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

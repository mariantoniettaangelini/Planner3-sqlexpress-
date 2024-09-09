using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.REPO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// JTW ->
// Microsoft.AspNetCore.Authentication.JwtBearer
// Microsoft.IdentityModel.JsonWebTokens
//builder.Services.AddAuthenticationScheme(opt =>
//{
//    opt.DefaultAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(opt =>
//{
//    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidIssuer = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//    };
//});

// builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurazione DbContext
builder.Services.AddDbContext<PlannerContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IWorkoutSessionRepo, WorkoutSessionRepo>();
builder.Services.AddScoped<IExerciseRepo, ExerciseRepo>();
builder.Services.AddScoped<IProgressRepo, ProgressRepo>();

// Configurazione autenticazione 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/user/login"; // Path per il login
        options.Cookie.SameSite = SameSiteMode.None; // Necessario per consentire l'invio dei cookie in richieste cross-origin
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Necessario per HTTPS
    });

// Aggiunta autorizzazione
builder.Services.AddAuthorization();

// Aggiungi CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsBuilder => corsBuilder
            .WithOrigins("http://localhost:4200") // Sostituisci con l'URL del tuo frontend
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy"); // Applica la policy CORS

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

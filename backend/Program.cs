using LoanStream.Api.Data;
using LoanStream.Api.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddEnvironmentVariables();

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.WebHost.UseSetting("linuxChmodOnStartup", "false");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<AppSettings>>().Value);

builder.Services.AddSingleton<ILeadRepository, SqlLeadRepository>();
builder.Services.AddSingleton<IContactRepository, SqlContactRepository>();
builder.Services.AddSingleton<ILeadIngestionService, LeadIngestionService>();
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddSingleton<IAdminService, AdminService>();

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/thankyou", () => Results.Redirect("/thank-you"));
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

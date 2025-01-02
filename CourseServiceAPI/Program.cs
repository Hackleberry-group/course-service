using Azure.Data.Tables;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Services;
using CourseServiceAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using CourseServiceAPI.Middleware;
using CourseServiceAPI.Services.Commands;
using CourseServiceAPI.Services.Queries;
using MassTransit;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



    // TODO: Remove in the future
// Add logging
var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting application");
logger.LogInformation("Environment: {Environment}", builder.Environment.EnvironmentName);
logger.LogInformation("Content root path: {ContentRoot}", builder.Environment.ContentRootPath);
logger.LogInformation("Web root path: {WebRoot}", builder.Environment.WebRootPath);
logger.LogInformation("Application name: {ApplicationName}", builder.Environment.ApplicationName);
logger.LogInformation("Config Size {ConfigSize}", builder.Configuration.AsEnumerable().Count());

// Log all configuration settings
foreach (var config in builder.Configuration.AsEnumerable())
{
    logger.LogInformation("{Key}: {Value}", config.Key, config.Value);
}

builder.Services.AddSingleton(new TableServiceClient(builder.Configuration["ConnectionString"]));

builder.Services.AddScoped<ITableStorageCommandService, TableStorageCommandService>();
builder.Services.AddScoped<ITableStorageQueryService, TableStorageQueryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Disable the default model validation (todo:Don't forget to remove this if we want to use the default model validation)
    options.ModelValidatorProviders.Clear();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ExerciseDtoValidator>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});

// Add CORS services TODO: Remove this if we don't need CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthorization();

// Use CORS middleware
app.UseCors("AllowAllOrigins");

app.MapMetrics();

app.MapControllers();

app.Run();
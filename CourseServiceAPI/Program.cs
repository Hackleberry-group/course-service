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

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the TableServiceClient to the services collection
builder.Services.AddSingleton(new TableServiceClient(builder.Configuration["Azure:Storage:ConnectionString"]));
builder.Services.AddScoped<ITableStorageCommandService, TableStorageCommandService>();
builder.Services.AddScoped<ITableStorageQueryService, TableStorageQueryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ITopicService, TopicService>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Disable the default model validation (todo:Don't forget to remove this if we want to use the default model validation)
    options.ModelValidatorProviders.Clear();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ExerciseDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ValidationExceptionMiddleware>(); // Ensure this is before UseAuthorization

app.UseAuthorization();

app.MapControllers();

app.Run();
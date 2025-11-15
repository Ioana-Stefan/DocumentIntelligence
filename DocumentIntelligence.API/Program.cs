using DocumentIntelligence.Application.DependencyInjection;
using DocumentIntelligence.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

// Add controllers
builder.Services.AddControllers();


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Important**: authentication middleware must come before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

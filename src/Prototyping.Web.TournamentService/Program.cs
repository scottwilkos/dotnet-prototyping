using MediatR;
using Protottyping.Business.SetupService;
using Prototyping.Business.Cqrs;
using Prototyping.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

var databaseSetupService = new DatabaseSetupService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

databaseSetupService.ConfigureSql(builder);

builder.Services.AddMediatR(typeof(AddTournamentHandler));
builder.Services.AddScoped<ITournamentRepository, TournamentEFRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var clearDatabases = builder.Configuration.GetValue("ClearDatabases", "false");

var servicesProvider = app.Services.GetService<IServiceProvider>() ?? throw new Exception("Could not get service provider");
using (var scope = servicesProvider.CreateScope() ?? throw new NullReferenceException("Could not create scope"))
{
    var scopedProvider = scope.ServiceProvider.GetRequiredService<IServiceProvider>();
    if (clearDatabases == "true")
    {
        Console.WriteLine("Clearing databases...");
        databaseSetupService.ClearMongoDb(builder);
        await databaseSetupService.ClearSqlDatabase(builder, scopedProvider);
    }
    await databaseSetupService.CreateAndLoadDatabase(builder, scopedProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine("Ready for Requests");

app.Run();
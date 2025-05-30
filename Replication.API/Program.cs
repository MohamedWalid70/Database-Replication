using Rebus.Bus;
using Replication.API;
using Replication.Application;
using Replication.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();


builder.Services.
    AddInfrastructure(builder.Configuration).
    AddApplication().
    AddAPI(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.Lifetime.ApplicationStarted.Register(() =>
{

    var bus = app.Services.GetRequiredService<IBus>();

});

app.MapControllers();

app.Run();

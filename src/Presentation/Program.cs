using Application;
using Infrastructure;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);


var app = builder.Build();


app.UseInfrastructure();
app.UsePresentation(builder.Configuration);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MirthInviteEngine;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// We will add logic here in Day 2
app.MapPost("/api/patient", () => Results.Ok("Endpoint is active."));

app.Run();

using MirthInviteEngine;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/patient", ([FromBody] Patient patient) =>
{
    if (patient is not null)
    {
        // Format the patient's full name for the log message.
        var patientName = $"{patient.FirstName} {patient.LastName}";

        // Log the confirmation message to the console.
        Console.WriteLine($"✅ HL7 Patient Record for [{patientName}] successfully processed.");

        // Return a success response.
        return Results.Ok("Patient record processed successfully.");
    }

    // Return a failure response if the patient object is null.
    return Results.BadRequest("Invalid patient data provided.");
});

app.Run();
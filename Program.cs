using phrasal_verbs.Models;
using phrasal_verbs.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<PhrasalVerbService>();

var hostUrl = builder.Configuration["WebHost:Url"];
var port = builder.Configuration["WebHost:Port"];
builder.WebHost.UseUrls($"{hostUrl}:{port}");
var app = builder.Build();

app.MapGet("/random", (PhrasalVerbService service) =>
{
    var (phrasalVerb, meanings) = service.GetRandomPhrasalVerbWithMeanings();
    return Results.Ok(new { phrasalVerb.Verb, Meanings = meanings });
});

app.MapPost("/check", (CheckMeaningRequest request, PhrasalVerbService service) =>
{
    var phrasalVerb = service.CheckMeaning(request.Verb, request.SelectedMeaning);
    var isCorrect = phrasalVerb?.Meaning == request.SelectedMeaning;
    return Results.Ok(new { isCorrect, CorrectMeaning = phrasalVerb?.Meaning });
});

app.Run();

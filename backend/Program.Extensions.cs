using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using TgiControl.Engines;

namespace TgiControl;

public static class EngineExtensions
{
    public static IServiceCollection AddTgiEngines(this IServiceCollection s, IConfiguration c)
    {
        if ((c["Auth:Mode"] ?? "Demo") == "Entra")
            s.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(c.GetSection("AzureAd"));

        s.AddAuthorization();
        s.AddSingleton<AuthorizationEngine>();
        s.AddSingleton<OperationalEngine>();
        s.AddSingleton<SimopsEngine>();
        s.AddSingleton<ExposureEngine>();
        s.AddSingleton<HandoverEngine>();
        s.AddScoped<IDocumentEngine, DocumentEngine>();

        return s;
    }

    public static IEndpointRouteBuilder MapTgiEngines(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/engines/operational/evaluate",
            (OperationalRequest x, OperationalEngine e) =>
                e.Evaluate(x.Work, x.Condition, x.Sst, x.Supervisor, x.Field, x.GasValid));

        app.MapPost("/api/engines/simops/detect",
            (List<WorkContext> x, SimopsEngine e) => e.Detect(x));

        app.MapPost("/api/engines/exposure/calculate",
            (ExposureRequest x, SimopsEngine s, ExposureEngine e) =>
                e.Calculate(x.Works, x.Condition, s.Detect(x.Works)));

        app.MapPost("/api/engines/handover/validate",
            (HandoverDraft x, HandoverEngine e) => e.Validate(x));

        return app;
    }
}

public record OperationalRequest(
    WorkContext Work,
    OperationalCondition Condition,
    bool Sst,
    bool Supervisor,
    bool Field,
    bool GasValid);

public record ExposureRequest(
    List<WorkContext> Works,
    OperationalCondition Condition);
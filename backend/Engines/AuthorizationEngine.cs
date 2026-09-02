using System.Security.Claims;

namespace TgiControl.Engines;

public sealed class AuthorizationEngine
{
    private readonly IConfiguration _config;

    public AuthorizationEngine(IConfiguration config)
    {
        _config = config;
    }

    static readonly Dictionary<string, string[]> RoleActions = new()
    {
        ["Contratista"] = ["permit.create", "permit.correct", "permit.requestClose"],
        ["Ejecutor directo"] = ["permit.create", "permit.progress", "permit.requestClose"],
        ["Profesional SST"] = ["permit.sstReview", "permit.suspend"],
        ["Supervisor / Superintendente"] = ["permit.supervisorApprove", "simops.authorize", "center.restrict"],
        ["Operador / Autoridad de area"] = ["permit.fieldValidate", "permit.activate", "permit.suspend", "handover.sign"],
        ["Gerencia"] = ["management.read"],
        ["Admin"] = ["admin.configure", "management.read"]
    };

    public UserContext Resolve(ClaimsPrincipal p, string role, string center, string company)
    {
        var mode = _config["Auth:Mode"] ?? "Demo";
        if (mode == "Entra" && p.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("Microsoft Entra token required");

        var email = p.FindFirstValue("preferred_username") ?? p.FindFirstValue(ClaimTypes.Email) ?? "demo.user@tgi.com.co";
        var roles = p.FindAll("roles").Select(x => x.Value).Distinct().ToList();
        var centers = p.FindAll("center").Select(x => x.Value).Distinct().ToList();

        if (mode == "Demo")
        {
            if (roles.Count == 0)
                roles.AddRange(RoleActions.Keys);
            if (centers.Count == 0)
                centers.Add("*");
        }

        if (!roles.Contains(role))
            throw new UnauthorizedAccessException("Rol no autorizado");
        if (!centers.Contains("*") && !centers.Contains(center))
            throw new UnauthorizedAccessException("Centro no autorizado");

        return new(email, role, center, company, roles.ToArray(), centers.ToArray(), true);
    }

    public void Demand(UserContext u, string action)
    {
        if (!RoleActions.TryGetValue(u.Role, out var a) || !a.Contains(action))
            throw new UnauthorizedAccessException($"{u.Role} no puede ejecutar {action}");
    }
}
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TgiControl.Engines;

public sealed class HandoverEngine
{
    public HandoverValidation Validate(HandoverDraft d)
    {
        var m = new List<string>();

        void Req(bool ok, string x)
        {
            if (!ok) m.Add(x);
        }

        Req(d.Variables.Count > 0, "Variables operacionales");
        Req(d.Units.Count > 0, "Estado de unidades");
        Req(d.Permits.Count > 0, "Permisos activos/suspendidos");
        Req(d.Risks.Count > 0, "Riesgos críticos/SIMOPS");
        Req(d.Actions.All(x => !string.IsNullOrWhiteSpace(x.Owner) && x.DueDate.HasValue),
            "Responsable y fecha de acciones");
        Req(!string.IsNullOrWhiteSpace(d.OutgoingOperator), "Operador saliente");
        Req(!string.IsNullOrWhiteSpace(d.IncomingOperator), "Operador entrante");

        var critical = d.Risks.Count(x => x.Level >= RiskLevel.Alto) +
                       d.Permits.Count(x => x.Risk >= RiskLevel.Alto);

        return new(
            m.Count == 0,
            m.ToArray(),
            critical,
            d.Actions.Count(x => x.Status != "Cerrada"),
            Math.Clamp(100 - m.Count * 8, 0, 100)
        );
    }

    public HandoverSnapshot Sign(HandoverDraft d, UserContext u, string meaning)
    {
        var v = Validate(d);
        if (!v.Valid)
            throw new InvalidOperationException(string.Join(", ", v.Missing));

        var json = JsonSerializer.Serialize(d);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json)));

        return new(
            d.Code,
            d.Center,
            d.Shift,
            u.Email,
            DateTime.UtcNow,
            meaning,
            hash,
            json,
            v
        );
    }
}
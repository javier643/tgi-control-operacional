namespace TgiControl.Engines;

public sealed class ExposureEngine
{
    public ExposureResult Calculate(
        IEnumerable<WorkContext> works,
        OperationalCondition c,
        IEnumerable<SimopsConflict> conflicts)
    {
        var w = works.ToArray();
        var s = conflicts.ToArray();

        var comp = new Dictionary<string, int>
        {
            { "Permisos", w.Length == 0 ? 0 : w.Max(x => (int)x.ResidualRisk) * 18 },
            { "Condición", c.Status switch {
                "Emergencia" => 25,
                "Fuera de servicio" => 20,
                "Restringida" => 14,
                _ => 0
            } },
            { "SIMOPS", Math.Min(20, s.Length * 8) },
            { "Aislamientos", Math.Min(12, c.Isolations.Length * 3) },
            { "Inhibiciones", Math.Min(12, c.Inhibitions.Length * 6) },
            { "Equipos indisponibles", Math.Min(12, c.UnavailableEquipment.Length * 6) },
            { "Alarmas", Math.Min(8, c.Alarms.Length * 2) }
        };

        var score = Math.Clamp(comp.Values.Sum(), 0, 100);

        return new(
            score,
            OperationalEngine.Level(score),
            comp,
            comp.Where(x => x.Value > 0).OrderByDescending(x => x.Value)
                .Select(x => $"{x.Key}: {x.Value}").ToArray(),
            DateTime.UtcNow
        );
    }
}
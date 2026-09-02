namespace TgiControl.Engines;

public sealed class SimopsEngine
{
    public IReadOnlyCollection<SimopsConflict> Detect(IEnumerable<WorkContext> source)
    {
        var w = source.ToArray();
        var r = new List<SimopsConflict>();

        for (int i = 0; i < w.Length; i++)
        {
            for (int j = i + 1; j < w.Length; j++)
            {
                var a = w[i];
                var b = w[j];

                if (!(a.Start < b.End && b.Start < a.End) ||
                    (a.Center != b.Center) ||
                    (a.Area != b.Area))
                    continue;

                Add(a, b, a.HotWork && (b.LineBreak || b.Hazards.Contains("Gas")),
                    "Trabajo en caliente vs liberación de hidrocarburo", "Crítica",
                    "Segregar, inertizar, monitoreo continuo y autorización SIMOPS", r);

                Add(a, b, a.Lifting && (b.Electrical || a.Area == b.Area),
                    "Izaje sobre trabajo activo", "Alta",
                    "Zona de exclusión y secuencia coordinada", r);

                Add(a, b, a.ConfinedSpace && (b.HotWork || b.LineBreak),
                    "Espacio confinado incompatible", "Crítica",
                    "Detener simultaneidad y asegurar rescate", r);

                Add(a, b, a.Excavation && b.Electrical,
                    "Excavación vs servicios eléctricos", "Crítica",
                    "Localizar servicios y aislar energía", r);
            }
        }

        return r;
    }

    static void Add(WorkContext a, WorkContext b, bool ok, string rule, string sev,
        string control, List<SimopsConflict> r)
    {
        if (ok)
            r.Add(new(a.PermitId, b.PermitId, a.Area, rule, sev, control, true));
    }
}
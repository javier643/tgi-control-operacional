namespace TgiControl.Engines;

public sealed class OperationalEngine
{
    public Decision Evaluate(
        WorkContext w,
        OperationalCondition c,
        bool sst,
        bool supervisor,
        bool field,
        bool gasValid)
    {
        var f = new List<Finding>();

        void Add(string code, string sev, string msg, string act, bool block = false) =>
            f.Add(new(code, sev, msg, act, block));

        if (!sst)
            Add("SST", "Alta", "Revisión SST pendiente", "Completar revisión", true);
        if (!supervisor)
            Add("SUP", "Alta", "Aprobación del Supervisor pendiente", "Obtener aprobación", true);
        if (!field)
            Add("FIELD", "Alta", "Validación física pendiente", "Inspeccionar sitio", true);

        if ((w.HotWork || w.LineBreak || w.ConfinedSpace) && !gasValid)
            Add("GAS", "Crítica", "Prueba de gases ausente o vencida", "Registrar prueba vigente", true);

        if (c.Status is "Emergencia" or "Fuera de servicio")
            Add("UNIT", "Crítica", "Condición operacional incompatible", "Suspender o reprogramar", true);

        if (c.Inhibitions.Length > 0)
            Add("INHIB", "Alta", "Protecciones inhibidas", "Validar control compensatorio");
        if (c.UnavailableEquipment.Length > 0)
            Add("EQUIP", "Alta", "Equipo esencial indisponible", "Verificar capacidad de respuesta");
        if (w.ResidualRisk == RiskLevel.Critico)
            Add("RISK", "Crítica", "Riesgo residual crítico", "Reducir riesgo", true);

        var score = Math.Clamp(
            (int)w.ResidualRisk * 20 + c.Inhibitions.Length * 8 + c.UnavailableEquipment.Length * 8 + c.Isolations.Length * 3,
            0, 100);
        var blocked = f.Any(x => x.BlocksWork);

        return new(
            !blocked,
            blocked ? "Bloqueado" : "Listo para activar",
            score,
            Level(score),
            f,
            ["SST", "Supervisor / Superintendente", "Autoridad de área"]
        );
    }

    public static RiskLevel Level(int x) =>
        x >= 80 ? RiskLevel.Critico : x >= 60 ? RiskLevel.Alto : x >= 30 ? RiskLevel.Medio : RiskLevel.Bajo;
}
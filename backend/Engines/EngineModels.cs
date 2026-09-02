namespace TgiControl.Engines;

public enum RiskLevel { Bajo = 0, Medio = 1, Alto = 2, Critico = 3 }

public record WorkContext(
    int PermitId,
    string Center,
    string Area,
    string Activity,
    RiskLevel ResidualRisk,
    DateTime Start,
    DateTime End,
    string[] Hazards,
    string[] Controls,
    bool HotWork,
    bool LineBreak,
    bool ConfinedSpace,
    bool Electrical,
    bool Lifting,
    bool Excavation
);

public record OperationalCondition(
    string Center,
    string Area,
    string Unit,
    string Status,
    Dictionary<string, double> Variables,
    string[] Alarms,
    string[] Inhibitions,
    string[] UnavailableEquipment,
    string[] Isolations
);

public record Finding(
    string Code,
    string Severity,
    string Message,
    string RequiredAction,
    bool BlocksWork = false
);

public record Decision(
    bool Allowed,
    string Status,
    int Score,
    RiskLevel Level,
    IReadOnlyCollection<Finding> Findings,
    string[] RequiredApprovals
);

public record SimopsConflict(
    int LeftPermitId,
    int RightPermitId,
    string Area,
    string Rule,
    string Severity,
    string Control,
    bool BlocksStart
);

public record ExposureResult(
    int Score,
    RiskLevel Level,
    IReadOnlyDictionary<string, int> Components,
    string[] Drivers,
    DateTime CalculatedAtUtc
);

public record UserContext(
    string Email,
    string Role,
    string Center,
    string Company,
    string[] Roles,
    string[] Centers,
    bool Active
);

public record HandoverDraft(
    string Code,
    string Center,
    string Shift,
    string OutgoingOperator,
    string IncomingOperator,
    List<VariableEntry> Variables,
    List<UnitEntry> Units,
    List<PermitTransfer> Permits,
    List<RiskTransfer> Risks,
    List<ActionEntry> Actions,
    List<string> AlarmsAndInhibitions,
    List<string> Isolations,
    List<string> UnavailableEquipment,
    List<string> Novelties,
    List<string> DocumentIds
);

public record VariableEntry(
    string Unit,
    string Name,
    double Value,
    string Uom,
    double? Low,
    double? High,
    string Trend,
    string Comment
);

public record UnitEntry(
    string Unit,
    string Status,
    string Restriction,
    string Comment
);

public record PermitTransfer(
    int PermitId,
    string Status,
    RiskLevel Risk,
    int Progress,
    string ContinuityCondition
);

public record RiskTransfer(
    string Scenario,
    string Area,
    RiskLevel Level,
    string Controls,
    string StopTrigger
);

public record ActionEntry(
    string Action,
    string Owner,
    DateTime? DueDate,
    string Priority,
    string Status
);

public record HandoverValidation(
    bool Valid,
    string[] Missing,
    int CriticalItems,
    int OpenActions,
    int Completeness
);

public record HandoverSnapshot(
    string Code,
    string Center,
    string Shift,
    string SignedBy,
    DateTime SignedAtUtc,
    string Meaning,
    string Sha256,
    string Payload,
    HandoverValidation Validation
);

public record DocumentRecord(
    Guid Id,
    string EntityType,
    string EntityId,
    string FileName,
    string ContentType,
    string StorageUri,
    string UploadedBy,
    DateTime UploadedAtUtc,
    string Center
);
# GUÍA DE INTEGRACIÓN DE MOTORES

## Resumen

Este documento describe cómo integrar los motores operacionales al proyecto TGI Control Operacional.

## Motores Incluidos

### 1. AuthorizationEngine
**Propósito**: Validar credenciales y permisos de usuario  
**Ubicación**: `backend/Engines/AuthorizationEngine.cs`

```csharp
var authEngine = new AuthorizationEngine(configuration);
var userContext = authEngine.Resolve(claimsPrincipal, role, center, company);
authEngine.Demand(userContext, "permit.create");
```

**Roles soportados**:
- Contratista
- Ejecutor directo
- Profesional SST
- Supervisor / Superintendente
- Operador / Autoridad de área
- Gerencia
- Admin

---

### 2. OperationalEngine
**Propósito**: Evaluar si un permiso puede ejecutarse  
**Ubicación**: `backend/Engines/OperationalEngine.cs`

```csharp
var engine = new OperationalEngine();
var decision = engine.Evaluate(
    work: workContext,
    condition: operationalCondition,
    sst: true,
    supervisor: true,
    field: true,
    gasValid: true
);

if (!decision.Allowed) {
    // Bloquear ejecución
}
```

**Retorna**:
- `Decision.Allowed`: bool si puede ejecutarse
- `Decision.Score`: 0-100 nivel de riesgo
- `Decision.Level`: Bajo/Medio/Alto/Crítico
- `Decision.Findings`: Lista de hallazgos

---

### 3. SimopsEngine
**Propósito**: Detectar conflictos entre trabajos simultáneos  
**Ubicación**: `backend/Engines/SimopsEngine.cs`

```csharp
var engine = new SimopsEngine();
var conflicts = engine.Detect(new List<WorkContext> {
    workA,
    workB,
    workC
});

foreach (var conflict in conflicts) {
    Console.WriteLine($"SIMOPS: {conflict.Rule} - {conflict.Severity}");
}
```

**Conflictos detectados**:
- Trabajo en caliente vs liberación de HC
- Izaje sobre trabajo activo
- Espacio confinado incompatible
- Excavación vs servicios eléctricos

---

### 4. ExposureEngine
**Propósito**: Calcular exposición operacional global (0-100)  
**Ubicación**: `backend/Engines/ExposureEngine.cs`

```csharp
var simopsEngine = new SimopsEngine();
var exposureEngine = new ExposureEngine();

var conflicts = simopsEngine.Detect(works);
var exposure = exposureEngine.Calculate(works, condition, conflicts);

Console.WriteLine($"Exposición: {exposure.Score} ({exposure.Level})");
Console.WriteLine($"Drivers: {string.Join(", ", exposure.Drivers)}");
```

**Componentes**:
- Permisos (max 18%)
- Condición de unidad (max 25%)
- SIMOPS (max 20%)
- Aislamientos (max 12%)
- Inhibiciones (max 12%)
- Equipos indisponibles (max 12%)
- Alarmas (max 8%)

---

### 5. HandoverEngine
**Propósito**: Validar y firmar entrega/recibo de turno  
**Ubicación**: `backend/Engines/HandoverEngine.cs`

```csharp
var engine = new HandoverEngine();

// Validar integridad
var validation = engine.Validate(handoverDraft);
if (!validation.Valid) {
    Console.WriteLine($"Falta: {string.Join(", ", validation.Missing)}");
}

// Firmar (genera hash SHA-256)
var snapshot = engine.Sign(handoverDraft, userContext, "Entrega turno diurno");
Console.WriteLine($"SHA-256: {snapshot.Sha256}");
```

**Validaciones**:
- Variables operacionales presentes
- Estado de unidades reportado
- Permisos activos/suspendidos
- Riesgos críticos/SIMOPS
- Acciones con responsable y fecha
- Operadores de salida/entrada

---

### 6. DocumentEngine
**Propósito**: Almacenar documentos en Local o Azure Blob  
**Ubicación**: `backend/Engines/DocumentEngine.cs`

```csharp
var engine = new DocumentEngine(configuration, environment);

await using var stream = file.OpenReadStream();
var document = await engine.UploadAsync(
    s: stream,
    name: file.FileName,
    type: file.ContentType,
    entity: "Permit",
    id: permitId.ToString(),
    user: userContext,
    ct: cancellationToken
);

Console.WriteLine($"Almacenado: {document.StorageUri}");
```

**Proveedores soportados**:
- **Local**: `App_Data/documents/{center}/{entity}/{id}/`
- **Blob**: Azure Blob Storage con `DefaultAzureCredential`

---

## Integración en Program.cs

```csharp
// En Program.cs después de AddDbContext
builder.Services.AddTgiEngines(builder.Configuration);

// En app.MapEndpoints
app.MapTgiEngines();
```

### Endpoints Expuestos

```
POST /api/engines/operational/evaluate
POST /api/engines/simops/detect
POST /api/engines/exposure/calculate
POST /api/engines/handover/validate
```

---

## Configuración appsettings.json

```json
{
  "Auth": {
    "Mode": "Demo"  // o "Entra" para producción
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "YOUR_TENANT_ID",
    "ClientId": "YOUR_API_CLIENT_ID",
    "Audience": "api://YOUR_API_CLIENT_ID"
  },
  "Documents": {
    "Provider": "Local",  // o "Blob"
    "BlobAccount": "YOUR_STORAGE_ACCOUNT",
    "Container": "operational-documents"
  }
}
```

---

## Transición a Producción

### Entra ID
1. Registrar SPA en Azure AD Portal
2. Registrar API como recurso
3. Exponer scope `access_as_user`
4. Crear app roles y asignaciones
5. Establecer `Auth:Mode = "Entra"`

### Azure Blob
1. Crear Storage Account
2. Crear contenedor privado
3. Establecer `Documents:Provider = "Blob"`
4. Configurar credenciales de Managed Identity

### Pesos Parametrizables

Actualmente los pesos en ExposureEngine están en código. Para producción:

```sql
CREATE TABLE EngineWeights (
    ComponentName NVARCHAR(100) PRIMARY KEY,
    Weight INT,
    MaxValue INT,
    Active BIT,
    LastUpdated DATETIME2
);
```

Luego refactorizar ExposureEngine.Calculate() para leer de BD.

---

## Testing

### Unit Tests Recomendados

```csharp
[TestClass]
public class OperationalEngineTests
{
    [TestMethod]
    public void HotWorkWithoutGasTest_ShouldBlock() {
        // Arrange
        var work = new WorkContext(..., HotWork: true, ...);
        var engine = new OperationalEngine();
        
        // Act
        var decision = engine.Evaluate(work, ..., gasValid: false);
        
        // Assert
        Assert.IsFalse(decision.Allowed);
    }
}
```

---

## Referencias

- `EngineModels.cs`: Definiciones de records
- `Program.Extensions.cs`: Endpoints y configuración
- `appsettings.engines.json`: Variables de entorno


# Centro de Control Operacional TGI

MVP corporativo para permisos de trabajo, control operacional, mapa de calor 5x5 y entrega/recibo de turno.

## Arquitectura
- Frontend: React 18 + TypeScript + Vite
- Backend: .NET 8 Minimal API + Entity Framework Core
- Base de datos: SQL Server 2022
- Autenticacion: modo demostrativo por roles, preparado para Microsoft Entra ID
- Ejecucion: Docker Compose

## Inicio rapido con Docker
1. Copie `.env.example` como `.env`.
2. Ejecute `docker compose up --build`.
3. Abra `http://localhost:5173`.
4. API y Swagger: `http://localhost:8080/swagger`.

## Credenciales demostrativas
No requiere contrasena. Seleccione rol, empresa, centro y turno en el acceso inicial.

## Flujo de permisos
Borrador > Radicado > Revision SST > Aprobacion Supervisor > Validacion de Area > En ejecucion > Suspendido/Revalidado/Transferido/Cerrado.

## Seguridad
El modo demo no debe utilizarse en produccion. Para ambiente corporativo, active Entra ID y reemplace secretos mediante Azure Key Vault.

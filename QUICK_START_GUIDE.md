# GUÍA DE INICIO RÁPIDO - TGI Control Operacional

## Requisitos Previos

- Docker Desktop (o Docker + Docker Compose)
- Git
- Navegador web (Chrome, Firefox, Edge)

## 1. Clonar el Repositorio

```bash
git clone https://github.com/javier643/tgi-control-operacional.git
cd tgi-control-operacional
```

## 2. Configurar Variables de Entorno

```bash
# Copiar archivo de ejemplo
cp .env.example .env

# Editar si es necesario (valores por defecto son válidos)
# cat .env
```

## 3. Iniciar la Aplicación

### Opción A: Docker Compose (Recomendado)

```bash
docker compose up --build
```

**Esto levantará automáticamente**:
- 📊 **SQL Server 2022** en puerto 1433
- 🔗 **Backend API** en puerto 8080 (esperará BD healthy)
- 🎨 **Frontend React** en puerto 5173 (construido automáticamente)

**Tiempos típicos**:
- SQL Server initialization: 30-40 segundos
- Backend startup: 20 segundos
- Frontend build: 30-60 segundos
- **Total**: ~2-3 minutos

### Opción B: Desarrollo Local (Avanzado)

```bash
# Terminal 1: Base de datos
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=Tgi.Demo.2026!Secure \
  -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

# Terminal 2: Backend
cd backend
dotnet restore
dotnet run

# Terminal 3: Frontend
cd frontend
npm install
npm run dev
```

## 4. Acceder a la Aplicación

### 🎨 Frontend
- **URL**: http://localhost:5173
- **Disponible**: Después de ver "VITE v5.0.0 ready in Xs"

### 🔗 Backend API
- **URL**: http://localhost:8080
- **Swagger Docs**: http://localhost:8080/swagger
- **Health Check**: http://localhost:8080/health

### 📊 Base de Datos
- **Host**: localhost
- **Puerto**: 1433
- **Usuario**: sa
- **Contraseña**: Tgi.Demo.2026!Secure
- **Database**: TgiControl

## 5. Login en la Aplicación

1. Abre http://localhost:5173
2. **Sin contraseña** - Solo ingresa:
   - **Nombre de usuario**: Cualquier nombre (ej: "javier.test")
   - **Rol**: Elige uno:
     - Admin
     - Supervisor
     - Profesional SST
     - Operador
     - Contratista
     - Visualizador
   - **Empresa**: TGI Demo (default)
   - **Centro**: Centro Principal (default)
   - **Turno**: Diurno (default)
3. Haz clic en **"Acceder al Sistema"**

## 6. Funcionalidades Disponibles

### Dashboard
- Estadísticas de permisos activos
- Permisos en revisión/aprobación/cierre
- Tabla de permisos recientes

### Menú de Navegación
- 📋 **Dashboard**: Resumen operacional
- 📝 **Permisos**: Gestión de permisos de trabajo (en desarrollo)
- 🔄 **Turnos**: Entrega y recibo de turnos (en desarrollo)

## 7. Solución de Problemas

### "No puedo acceder a http://localhost:5173"
```bash
# Verifica que el contenedor esté corriendo
docker ps

# Ver logs
docker compose logs web
```

### "Error de conexión a la BD"
```bash
# Verifica SQL Server
docker compose logs db

# Espera ~40 segundos más y reintentar
```

### "Backend devuelve error 500"
```bash
# Ver logs del API
docker compose logs api

# Verificar aplicaciones reconstruyen correctamente
docker compose down
docker compose up --build
```

### Limpiar todo (para empezar de cero)
```bash
# Detener contenedores
docker compose down

# Eliminar volumen de BD (CUIDADO: borra datos)
docker volume rm tgi-control-operacional_sql-data

# Reiniciar
docker compose up --build
```

## 8. Desarrollo Posterior

### Backend (.NET 8)
```bash
cd backend

# Agregar modelo
dotnet new class -n MyModel -o Models

# Agregar migración
dotnet ef migrations add MyMigration
dotnet ef database update

# Ejecutar tests
dotnet test
```

### Frontend (React + TypeScript)
```bash
cd frontend

# Instalar dependencias
npm install

# Desarrollo con hot reload
npm run dev

# Build para producción
npm run build

# Linting
npm run lint
```

## 9. Estructura de Carpetas

```
tgi-control-operacional/
├── backend/
│   ├── Engines/              # Motores operacionales
│   ├── Models/               # Entidades de datos
│   ├── Data/                 # DbContext
│   ├── Services/             # Lógica de negocio
│   ├── Program.cs            # Configuración principal
│   ├── Program.Extensions.cs # Extensiones de motores
│   ├── Dockerfile            # Imagen Docker
│   └── TgiControl.csproj    # Dependencias
│
├── frontend/
│   ├── src/
│   │   ├── components/       # Componentes React
│   │   ├── pages/            # Páginas (Login, Dashboard, etc)
│   │   ├── store/            # Zustand stores
│   │   ├── App.tsx           # Routing
│   │   └── main.tsx          # Entry point
│   ├── Dockerfile            # Imagen Docker
│   ├── nginx.conf            # Configuración Nginx
│   └── package.json          # Dependencias
│
├── database/
│   ├── 001_CreateInitialSchema.sql   # Schema
│   └── 002_InsertDemoData.sql        # Datos demo
│
├── docker-compose.yml        # Orquestación de servicios
├── .env                      # Variables de entorno
├── .env.example              # Plantilla
└── README.md                 # Este archivo
```

## 10. URLs Importantes

| Componente | URL | Notas |
|-----------|-----|-------|
| Frontend | http://localhost:5173 | Interfaz de usuario |
| Backend API | http://localhost:8080 | API REST |
| Swagger Docs | http://localhost:8080/swagger | Documentación interactiva |
| Health Check | http://localhost:8080/health | Estado del API |
| SQL Server | localhost:1433 | Conector SSMS/Azure Data Studio |

## 11. Próximos Pasos

- [ ] Validar flujos de permisos con datos reales
- [ ] Integrar autenticación Microsoft Entra ID
- [ ] Configurar almacenamiento en Azure Blob
- [ ] Crear tests unitarios
- [ ] Documentar reglas de negocio
- [ ] Capacitar a usuarios finales
- [ ] Validar con SST y Seguridad de Procesos

## 12. Soporte

Para más información:
- Ver `README.md`
- Ver `VALIDATION_CHECKLIST.md`
- Ver `ENGINES_INTEGRATION_GUIDE.md`
- Revisar comentarios en código

---

**Versión**: 0.1.0 MVP  
**Última actualización**: 2026-09-02  
**Estado**: ✅ Listo para usar

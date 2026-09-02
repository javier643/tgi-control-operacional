-- TGI Control Operacional - Datos Demo
USE TgiControl;
GO

-- Insertar Usuarios Demo
INSERT INTO [dbo].[Users] ([Username], [Email], [FirstName], [LastName], [Role], [Company], [OperationalCenter], [IsActive])
VALUES
    ('admin.demo', 'admin@tgi.demo', 'Admin', 'Demo', 'Admin', 'TGI Demo', 'Centro Principal', 1),
    ('supervisor.demo', 'supervisor@tgi.demo', 'Supervisor', 'Demo', 'Supervisor / Superintendente', 'TGI Demo', 'Centro Principal', 1),
    ('sst.demo', 'sst@tgi.demo', 'Profesional', 'SST', 'Profesional SST', 'TGI Demo', 'Centro Principal', 1),
    ('operador.demo', 'operador@tgi.demo', 'Operador', 'Demo', 'Operador / Autoridad de area', 'TGI Demo', 'Centro Principal', 1),
    ('contratista.demo', 'contratista@tgi.demo', 'Contratista', 'Demo', 'Contratista', 'Empresa Contratista', 'Centro Principal', 1);
GO

-- Insertar Permisos Demo
INSERT INTO [dbo].[Permits] ([Number], [Description], [StartDate], [EndDate], [Status], [RequestedBy], [ApprovedBy], [Center], [Area], [Activity], [ResidualRisk])
VALUES
    ('PT-2026-001', 'Revisión de línea de producción', DATEADD(DAY, 1, GETUTCDATE()), DATEADD(DAY, 2, GETUTCDATE()), 'Filed', 'contratista.demo@tgi.demo', NULL, 'Centro Principal', 'Producción', 'Inspección', 1),
    ('PT-2026-002', 'Trabajo en altura - Estructura A', DATEADD(DAY, 2, GETUTCDATE()), DATEADD(DAY, 3, GETUTCDATE()), 'SSTReview', 'contratista.demo@tgi.demo', NULL, 'Centro Principal', 'Mantenimiento', 'Reparación', 2),
    ('PT-2026-003', 'Soldadura de tuberías', DATEADD(DAY, 3, GETUTCDATE()), DATEADD(DAY, 4, GETUTCDATE()), 'SupervisorApproval', 'contratista.demo@tgi.demo', 'supervisor@tgi.demo', 'Centro Principal', 'Soldadura', 'Mantenimiento', 2);
GO

-- Insertar Turnos Demo
INSERT INTO [dbo].[Shifts] ([Date], [ShiftType], [OperationalCenter], [Company], [HeadCount], [DeliveredBy], [HandoverNotes])
VALUES
    (CAST(GETUTCDATE() AS DATE), 'Diurno', 'Centro Principal', 'TGI Demo', 25, 'operador.demo@tgi.demo', 'Turno sin incidentes'),
    (CAST(GETUTCDATE() AS DATE), 'Vespertino', 'Centro Principal', 'TGI Demo', 22, 'supervisor@tgi.demo', 'Se reportaron dos alertas menores');
GO

-- Insertar Condiciones Operacionales Demo
INSERT INTO [dbo].[OperationalConditions] ([Center], [Area], [Unit], [Status], [Inhibitions], [Isolations], [UnavailableEquipment], [Alarms])
VALUES
    ('Centro Principal', 'Producción', 'Unidad-A', 'Normal', '[]', '[]', '[]', '[]'),
    ('Centro Principal', 'Mantenimiento', 'Unidad-B', 'Restringida', '["Válvula PSV-01"]', '[]', '["Compresor-02"]', '["Alarma de presión"]');
GO

PRINT 'Datos demo insertados exitosamente';
GO
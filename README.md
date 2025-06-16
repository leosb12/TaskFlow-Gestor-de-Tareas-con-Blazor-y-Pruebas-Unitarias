# TaskFlow: Gestor de Tareas con Blazor y Pruebas Unitarias

TaskFlow es una aplicación web moderna y minimalista construida con **Blazor Server**, que permite a los usuarios gestionar sus tareas personales con autenticación y roles. Incluye funcionalidades de creación, actualización y eliminación de tareas, así como un completo **Panel de Administración** para la gestión de usuarios, roles y tareas.

---

## Tecnologías y Competencias Demostradas

### ✅ C# y .NET

- Lógica construida completamente en **C# 100% tipado y estructurado**.
- Arquitectura limpia basada en servicios y dependencias inyectadas.
- Uso de `async/await`, patrones `Repository` y `Dependency Injection`.

### ✅ Blazor Server

- Interfaz 100% interactiva con **componentes reactivos y data binding**.
- Navegación segura con rutas protegidas y renderizado condicional.
- Integración de `EditForm`, validaciones, y `@inject` para servicios.

### ✅ SQL y Bases de Datos Relacionales

- Arquitectura de datos basada en SQL Server, utilizando una instancia en Azure SQL Database como backend relacional. La integración se realiza mediante Entity Framework Core, con mapeo de entidades, consultas LINQ y migraciones automáticas para mantener la estructura sincronizada.

- Modelo de datos relacional diseñado con:

- Integridad referencial estricta

- Relaciones uno a muchos (1:N) entre Usuario → Tarea, y Rol → Usuario

- Normalización y claridad en los esquemas.

- Uso avanzado de DbContext, DbSet, y LINQ para consultas eficientes y seguras.

### ✅ Pruebas Unitarias y QA

- Proyecto de pruebas `TaskFlow.Tests` implementado con `xUnit` y `Bunit`.
- Cobertura de pruebas sobre servicios (`AuthService`, `UsuarioService`, `TareaService`) y componentes Blazor (`Admin.razor`).
- Pruebas unitarias con mocks (`Moq`) y pruebas de interfaz con `Bunit` que simulan clicks, selecciones y cambios de estado.
- Reportes generados con `ReportGenerator` y `coverlet`, incluyendo:
  - **Line Coverage**
  - **Branch Coverage**
  - Identificación de Hotspots

---

## Características Principales

### Para Usuarios

- Iniciar sesión con validación.
- Ver y gestionar solo sus tareas.
- Marcar tareas como completadas o eliminarlas.

### Para Administradores

- Acceder al panel con:
  - Tabla de usuarios y sus tareas.
  - Cambio de roles.
  - Eliminación de usuarios.
- Gestión de tareas globales.

---

## Cobertura y QA

- 29 pruebas automatizadas exitosas.
- Cobertura visual generada (HTML) con `ReportGenerator`.
- Lógica cubierta:
  - Autenticación (login, registro, roles).
  - Creación y actualización de tareas.
  - Condicionales visuales de interfaz Blazor.
  - Gestión de administración (edición y eliminación de usuarios).

---

## Tecnologías Usadas

- `C#` y `.NET 8.0`
- `Blazor Server`
- `Entity Framework Core`
- `xUnit`, `Bunit`, `Moq`
- `SQL Server` 
- `coverlet` + `ReportGenerator`

---

## Acceder

TaskFlow está desplegado y accesible públicamente en:

🔗 [https://taskflow-production-03d1.up.railway.app](https://taskflow-production-03d1.up.railway.app)

---


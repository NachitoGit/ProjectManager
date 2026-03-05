# 🚀 TaskFlow API - Gestión de Proyectos con Clean Architecture

Este proyecto es una API robusta para la gestión de proyectos y tareas, construida siguiendo los principios de **Clean Architecture**, **Domain-Driven Design (DDD)** y **CQRS**. 

El objetivo principal de este repositorio es demostrar la implementación de un sistema escalable, testeable y desacoplado utilizando las últimas tecnologías de .NET.

## 🏗️ Arquitectura del Sistema

La solución está dividida en 4 capas principales para garantizar la separación de responsabilidades:

* **Domain**: Entidades de negocio, excepciones de dominio e interfaces de repositorio. Contiene la lógica central sin dependencias externas.
* **Application**: Implementación de **CQRS con MediatR**. Aquí residen los Handlers, DTOs, validaciones (FluentValidation) y la lógica de orquestación.
* **Infrastructure**: Implementación de acceso a datos con **Entity Framework Core**, configuración de la base de datos y servicios externos.
* **WebAPI**: Punto de entrada del sistema, controladores REST y configuración de Inyección de Dependencias.



## 🌟 Características Destacadas

### 1. Sistema de Auditoría mediante Domain Events
Para mantener un historial de actividad sin ensuciar la lógica de negocio, implementé un sistema de **Eventos de Dominio**. 
* Cuando se crea una tarea o se cambia un estado, se dispara un evento.
* Un **NotificationHandler** captura el evento y registra la actividad de forma asíncrona en una tabla de auditoría.

### 2. Seguridad Transversal
No se trata solo de estar autenticado con **JWT**. El sistema valida la propiedad de los recursos:
* Un usuario solo puede ver o modificar tareas de proyectos donde es **miembro**.
* Implementación de un `PermissionService` inyectado en los Handlers para validar reglas de negocio antes de ejecutar cualquier comando.

### 3. Testing Automatizado
Capa de pruebas unitarias robusta utilizando **xUnit**, **Moq** y **FluentAssertions**.
* Cobertura de "Happy Paths" y casos de error (seguridad y validación).
* Uso de Mocks para aislar la lógica de aplicación de la base de datos y servicios de identidad.

## 🛠️ Tech Stack
* **.NET 8**
* **Entity Framework Core** (SQL Server)
* **MediatR** (CQRS & Domain Events)
* **AutoMapper** (Mapeo de objetos)
* **FluentValidation** (Validación de entrada)
* **xUnit & Moq** (Unit Testing)

## 🚀 Cómo ejecutar el proyecto

1.  Clonar el repositorio:
    ```bash
    git clone [https://github.com/tu-usuario/ProjectManager.git](https://github.com/tu-usuario/ProjectManager.git)
    ```
2.  Actualizar la cadena de conexión en `appsettings.json` dentro de `ProjectManager.WebAPI`.
3.  Ejecutar las migraciones:
    ```bash
    dotnet ef database update --project src/ProjectManager.Infrastructure --startup-project src/ProjectManager.WebAPI
    ```
4.  Correr los tests para verificar la integridad:
    ```bash
    dotnet test
    ```

---
Creado con fines de portfolio por Ignacio Merlo - 2026.
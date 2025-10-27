# vilkatea-angular
FULLSTACK NET – ANGULAR JS Crear una web y un microservicio para dar mantenimiento

## Contenido
- `backend/Clientes.Api/` – API .NET 8 (Minimal API + EF Core Oracle).
- `sql/init.sql` – Script para crear esquema y tabla `CLIENTE` en Oracle XE.
- `frontend/setup.ps1` – Script para generar el proyecto Angular y copiar los archivos de la feature `clientes`.
- `http/clientes.http` – Requests para probar la API (VS Code REST Client).

## Requisitos
- Windows 11
- Node LTS + Angular CLI
- .NET 8 SDK
- Oracle XE (Docker o instalador)
- Git + VS Code

## Pasos Rápidos

### 1) Base de datos Oracle
Ejecuta `sql/init.sql` con un cliente Oracle:

```
-- Ajusta la contraseña si gustas
@sql/init.sql
```

Datos de conexión por defecto en el API:
- Host: `localhost`
- Puerto: `1521`
- Service Name: `XEPDB1`
- Usuario: `app_clientes`
- Password: `App_Cl1entes#2025`

### 2) Backend (.NET 8)
```powershell
cd backend/Clientes.Api
dotnet restore
dotnet run
```
Swagger: se mostrará en la consola (ej. https://localhost:7xxx/swagger).

### 3) Frontend (Angular)
```powershell
cd frontend
# genera el proyecto y aplica el módulo clientes
./setup.ps1
# inicia la app
cd cliente-app
npm install
ng serve -o
```

El Front usa `environment.api = "https://localhost:7080/api"` por defecto. Cambia el puerto si tu API corre en otro.

¡Listo!

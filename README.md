# Proyecto Prueba-Desarrollador-Excellentiam

Este proyecto está desarrollado en **.NET 8** y utiliza **MudBlazor** para el frontend. La base de datos se encuentra en SQL Server y se incluye un backup para restauración.

---

## Requisitos previos

- **Visual Studio 2022**  
- **SQL Server Management Studio 20**  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## Configuración de la base de datos

1. La base de datos se encuentra en la carpeta `Backup` del proyecto:  
   - Archivo de backup: `MiBaseDeDatos.bak`  
   - Trigger de ejemplo: `TR_Auditoria_Facturas.sql` (no es necesario ejecutarlo inicialmente, ya que los triggers ya existen en la base, pero se incluye para referencia o previsualización).

2. Para conectar el proyecto a tu base de datos, modifica la sección de **ConnectionString** en `WebApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
}

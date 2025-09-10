# RealEstate API

API para gestión de propiedades, imágenes, propietarios y trazabilidad de propiedades.

## Arquitectura

Basado en Clean Architecture con cuatro capas principales:

- **RealEstate.Api** – Capa de presentación y endpoints.
- **RealEstate.Application** – Lógica de negocio, servicios y DTOs.
- **RealEstate.Domain** – Entidades, interfaces y reglas de negocio.
- **RealEstate.Infrastructure** – Acceso a datos y almacenamiento.
- **RealEstate.Tests** – Pruebas unitarias de servicios.

## Instalación

1. Clona el repositorio:
   git clone https://github.com/everllanos46/RealEstate_api.git
   cd RealEstate_api

2. Restaurar paquetes:
    dotnet restore

3. Configura el archivo appsettings.json para las conexiones a base de datos y colocar el firebase-key.json (https://drive.google.com/file/d/1GlkS7M_WzYoHzNEVrM3Skqp2wpC0F6oN/view?usp=sharing) en la misma ruta

4. Ejecución de la aplicación
    cd RealEstate.Api
    dotnet run

5. Run de tests
   dotnet test
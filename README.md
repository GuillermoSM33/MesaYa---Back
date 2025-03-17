MesaYa - Backend

♨ MesaYa - Plataforma integral para la gestión de clientes en restaurantes.

📖 Problemática

En la actualidad, muchos restaurantes enfrentan dificultades en la gestión de reservas y la actualización de menús, lo que genera una experiencia fragmentada tanto para clientes como para administradores. MesaYa surge como una solución digital innovadora para optimizar y automatizar estos procesos, permitiendo una gestión más eficiente y personalizada de los restaurantes.

👥 Integrantes del Proyecto

Garcia Canul Guillermo De Jesus – 22393204

Guzman Salazar Abelardo Geovani – 22393178

Hernandez Gonzalez Jonathan – 22393180

Lopez Rodriguez Cristian – 22393176

Tuz Carrillo Daniel Alejandro – 22393190

🛠 Librerías utilizadas

Para el desarrollo del backend en .NET Core:

Microsoft.EntityFrameworkCore.SqlServer → ORM para la base de datos.

Microsoft.EntityFrameworkCore.Design → Soporte para migraciones.

Swashbuckle.AspNetCore → Documentación API con Swagger.

Microsoft.AspNetCore.Identity → Gestión de usuarios y roles.

🚀 Cómo correr el proyecto

🔧 Backend (API en .NET Core)

Clonar el repositorio:

git clone https://github.com/tu-repo/MesaYa.git

Navegar al directorio del proyecto:

cd MesaYa

Instalar las dependencias necesarias:

dotnet restore

Aplicar migraciones y actualizar la base de datos:

dotnet ef database update

Ejecutar el proyecto:

dotnet run

La API estará disponible en http://localhost:5000 o el puerto configurado.

📌 Notas

Asegúrate de configurar correctamente la conexión a la base de datos en appsettings.json.

Puedes acceder a la documentación Swagger de la API en http://localhost:5000/swagger.

¡Gracias por usar MesaYa! 🍽️


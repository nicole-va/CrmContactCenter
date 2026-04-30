En este archivo se explica cómo Visual Studio creado el proyecto.

Se usaron las siguientes herramientas para generar este proyecto:
- create-vite

Los pasos siguientes se usaron para generar este proyecto:
- Cree un proyecto vue con create-vite: `npm init --yes vue@latest crmcontactcenter.client -- --eslint `.
- Actualizar `vite.config.js` para configurar el proxy y los certificados.
- Actualice el componente `HelloWorld` para capturar y mostrar información meteorológica.
- Crear archivo de proyecto (`crmcontactcenter.client.esproj`).
- Crear `launch.json` para habilitar la depuración.
- Agregue el proyecto a la solución.
- Actualice el punto de conexión de proxy para que sea el punto de conexión del servidor back-end.
- Agregar proyecto a la lista de proyectos de inicio.
- Escriba este archivo.

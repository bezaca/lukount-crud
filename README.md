# lukount-crud
Este projecto ha sido creado para la prueba tecnica de Lukount, consiste en un crud implementado en .net5 que utiliza como base de datos Mongodb y el framework Xunit para las pruebas unitarias implementando el patron AAA. 

## Configuración de la base de datos 
Mongo ha sido implementado mediante un contenedor de docker, se puede ejecutar este contenedor mediante el comando: 
```bash
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo
```
### `Nota:`
Este contenedor es fundamental para el correcto funcionamiento del crud, por lo que es recomendable ejecutar el contenedor antes del proyecto.

## Documentación
Se pueden consultar los endpoints del crud mediante la documentación de Postman o mediante swagger en la aplicación:
```
https://documenter.getpostman.com/view/14940513/Tzm6jv1V
```

<p align="center">
  <img src="img/swagger.png" alt="swagger" width="700"/>
 </p>

## Pruebas Unitarias
Las pruebas pueden ser ejecutadas ingresando a la carpeta `Lukount.UnitTests` y posteriormente, mediante el comando:
```bash
dotnet test 
```
Estas pruebas estan enfocadas en las funcionalidad de las peticiones HTTP.

## Despliegue con Docker:
Gracias a docker podemos correr nuestro crud mediante contenedores.
Primero, debemos crear una network para nuestros contenedores mediante:
```
docker network create "nombre"
```
Posteriormente debemos ejecutar nuestra base de datos agregandole el network recien creado:
```bash
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db --network="nombre" mongo
```
Y por ultimo, debemos ejecutar nuestro crud utilizando:
```
docker run -it --rm -p 8080:8080 -e MongoDbSettings:Host=mongo --network="nombre" fcsource/lukountcrud:v2
```
Y podremos utilizar `http://localhost:8080` para nuestras peticiones.




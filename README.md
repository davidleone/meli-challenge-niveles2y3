# meli-challenge-niveles2y3
Repositorio exclusivo para los desafios 2 y 3 del challenge (REST API)

## Arquitectura del microservicio ChallengeMeliServices:
El microservicio está dividido en 3 proyectos principales, separando la implementación en capas:
- __DataAccess__: capa de acceso a base de datos. Esta capa contiene:
    - Daos: clases específicas para queries "en crudo" contra la base, sin resolver (para reutilizacion desde los repositorios).
	- Repositorios: clases que utilizan los Daos y le agregan filtros, ordenamientos, agregaciones, etc. y resuelven las queries.
	- SessionManager (clase única): manejo de sesiones, conexiones contra la base.
	- Modelos: entidades de negocio y persistencia.
	- Maps: mapeos (por fluentnhibernate) de las entidades persistibles.
- __Services__: capa de lógica de negocio. Esta capa contiene:
	- Excepciones: excepciones de validaciones.
	- Servicios: clases que manejan la lógica de negocio de cada entidad.
- __Web__: capa de mapeo externo y puntos de acceso a la vertical.
    - AutoMapperWeb (clase única): clase que gestiona el mapeo entre Dtos y Entidades.
	- Controladores: clases que contienen los puntos de acceso a la vertical.
	- Modelos: dtos.
	- UnityConfig (clase única): registro de tipos para inyección de dependencia.
	
A su vez cada proyecto tiene también su correspondiente proyecto de UnitTest.

## Reporte de Code Coverage:
[a relative link](CodeCoverage/index.html)

## Instrucciones de uso:
Se pueden consultar 2 endpoints desde cualquier programa de envio de peticiones HTTP REST, por ej Postman.

- __POST__ http://prod.ywtpjv956u.sa-east-1.elasticbeanstalk.com/api/challenge-meli/v1/mutant <br />
Json de ejemplo a enviar en el Body: <br />
{ <br />
"dna":["TTGCGA","CAGTAC","TTATGT","AGAAGG","CCTCTA","TCACTG"] <br />
} <br />
Endpoint para consulta de si un adn es mutante o no, devuelve:
    - __200 OK__: en caso que el adn corresponda a un mutante.
    - __403 FORBIDDEN__: en caso que el adn corresponda a un humano.
    - __400 BAD REQUEST__: en caso que el adn no sea válido (para serlo debe contener elementos y ser de NxN).
    - __500 INTERNAL SERVER ERROR__: en caso que ocurra cualquier error inesperado.

- __GET__ http://prod.ywtpjv956u.sa-east-1.elasticbeanstalk.com/api/challenge-meli/v1/stats <br />
    Endpoint para consulta de estadísticas.

## Principales nugets utilizados:
- Unity => para inyección de dependencia.
- AutoMapper => para mapeo externo entre DTO y Entidad.
- NHibernate => ORM para persistencia.
- FluentNHibernate => para mapeo contra la base (en lugar de XMLs).
- Npgsql => para conexión con PostgreSQL (gestor elegido de BD).
- MS.Ext.Caching.Memory => para cache en memoria.
- Moq => para mockeos de UnitTest.
- AxoCover (extension) => code coverage de testing.

## Mejoras y Consideraciones:
- Contemplar mensajes de error en caso de DNA inválido, tal como está hecho en el Desafío Nivel 1.
- Agregar Logging de errores para facilitar trackeo de problemas.
- No se agregó nada respecto a seguridad y autenticación debido a que quiero que los endpoints sean completamente públicos y accesibles por cualquiera.
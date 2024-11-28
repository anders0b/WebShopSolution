## Labb 1 - WebShop | Anders Öberg

Initiera Docker-compose:

`docker-compose up --build`

Starta webshop.api-1 och gå till

`http://localhost:5000/swagger/index.html`

## Implementerade designmönster:
- Observer pattern
- Factory pattern

## Verktyg
| Package     | Use                                  |
| ----------- | ------------------------------------ |
| SQL Server  | Database                             |
| Minimal-API | Endpoints                            |
| Swagger     | Endpoint management                  |
| Dapper      | micro-ORM                            |
| DbUp        | Database migration and version check |
| xUnit       | Testing                              |
| FakeItEasy  | Mocking                              |

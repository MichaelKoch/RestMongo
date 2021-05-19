# RestMongo

RestMongo aims to provide full dotnet ecosystem integration for straight forward mongodb development.
The main focus lies on providing an easy entrypoint to start implementing in DDD Pattern.
It supports out of the box domain services for given entities and dtos, 
this can be used to provide CRUD functionality in a simple matter.

## Origin

The project was initally started by [MichaelKoch](https://github.com/MichaelKoch) at [RestMongo](https://github.com/MichaelKoch/RestMongo) and [Domain.CartService](https://github.com/MichaelKoch/Domain.CartService).

## Components

| Package Name									| Purpose			|
| ----------------------------------------------| ----------------- |
| `RestMongo`									| Contains all core packages `RestMongo.Data`, `RestMongo.Domain`, `RestMongo.Web` and `RestMongo.Extensions`|
| `RestMongo.Data`								| Contains Implementations for mongo data layer |
| `RestMongo.Data.Abstractions`					| Contains Abstractions for data layer |
| `RestMongo.Domain`							| Contains Implementations for domain layer |
| `RestMongo.Domain.Abstractions`					| Contains Abstractions for domain layer |
| `RestMongo.Web`								| Contains Implementations for ASP.Net Core integration |
| `RestMongo.Extensions`						| Contains Extensions for integrating RestMongo in dotnet ecosystem  |

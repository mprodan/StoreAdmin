# StoreAdmin
StoreAdmin example of REST API

## Main points:
- Simple web application with an API and data layer using .NET C#, ASP.NET MVC, Web API, and a relational database
- Clean Architecture principles and Test-Driven Development, methodologies.
- Create an informal user story - CRUD
- Also create a user, login as the user, the user information is stored in the DBdatabase.
- Cannot use Entity Framework to complete this assignment.

## Layers:
- Data layer: relational database 2 tables, one with a key and 2 fields, second user/security data.
- Business layer: services and validators.
- API layer: CRUD for data and a second API should include endpoints for user creation, user login, and authorized and non-authorized endpoints.
- Core layer: models, use interfaces.
Unit-Test 

## Thoughts
- Create the user story and think about the design.
- Create the structure of projects. Thinking in use IoC and Interfaces.
- Create tests first in order to use TDD.
- Add the CRUD of users in the repository.
- Add the CRUD of stores in the repository.
- Add a default user when the DB no exists, to allow the first login.
- Add the interfaces in Core layer.
- Add Swagger to see the API.
- Add Dockerfile 
- Add git init 
- Use DRY in repository
- SOLID (IoC, Separation of concerns) 


## For dotnet:
In folder: /c/tests/StoreAPI/src
dotnet test
dotnet run

## For docker:
In folder: /c/tests/StoreAPI/src
docker build --tag store-api .
docker run --publish 5000:80 store-api

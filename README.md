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
- SOLID (IoC, Separation of concerns) using Packages.cs in each component.
- All the interfaces in Core layer.
- Create the structure of projects. Thinking in use IoC and Interfaces.
- Create tests first in order to use TDD.
- I will use for the database sqlite.
- Add the CRUD of users in the repository.
- Add the CRUD of stores in the repository.
- Add a default user when the DB no exists, to allow the first login. {"username": "admin",  "password": "admin1234"}
- For Authenticate the User I will use BCrypt, and generate a token for Authorization of the endpoints.
- Add Swagger to see the API.
- Add Dockerfile 
- Upload to git 
- Use DRY in store repository with generics
- Add the test in the API layer, this are end to end test that goes through all the layers (StoreAdmin.WebAPI.Test) 


## For run dotnet:
- dotnet test (folder ./src)
- dotnet run (folder ./src/StoreAdmin.WebAPI)

## For run docker:
- docker build --tag store-api . (folder ./src)
- docker run --publish 5000:80 store-api (folder ./src)

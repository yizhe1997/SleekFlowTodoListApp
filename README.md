# SleekFlowTodoListApp

Create a simple TODO List web application that allows user to manage their TODOs. 

![image](https://user-images.githubusercontent.com/81303202/215345188-c252f925-0278-431c-91ca-8530a33e75d9.png)

- GitHub for Version control.
- .NET 6 as our programming framework for our API.

## You'll need

For hosting on Azure
- Azure account

For local developer environment

- Visual Studio 2022 and the necessary .Net SDKs
- Docker
- SQL server
- Postman

## How to get started Backend Web App(Local)

### How to setup Azure AD B2C for authorization (optional)
 - WORK IN PROGRESS (still writing the document, but theres already good explanation in other projects like [this](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/blob/master/4-WebApp-your-API/4-2-B2C/README.md))

### Get the code and environment

1. [Fork the repository](https://github.com/yizhe1997/SleekFlowTodoListApp.git) so you can have your own copy of it. And load the solution in Visual Studio 2022, download the necessary SDKs if prompted.

### Configuration for the Backend Web App
1. If hosting locally user can choose to fill in the appsettings.json or use the user secrets by left clicking the SleekFlowTodoListCore.csproj and select "Manage User Secrets".
![image](https://user-images.githubusercontent.com/81303202/215346110-c5c904d8-4c60-4a22-aa28-7c1696b17d66.png)
2. Create a new SQL Server instance for your local environment.
3. Go through the [README.md](https://github.com/yizhe1997/SleekFlowTodoListApp/blob/master/SleekFlowTodoListAPI/README.md) to understand the purpose of each configuration options. There are empty templates in the README.md readily available for use.
4. Clean and Rebuild the project. 
5. Select SleekFlowTodoListAPI as the startup project.
![image](https://user-images.githubusercontent.com/81303202/215347856-5f12d718-2f0b-42e5-a79e-d3a41fbbbb6b.png)
6. Run the application.The first time you run the application, it will seed the AspNetUsers table in your database with an admin user and a dummy user.
7. You should be able to make requests to localhost:7149 ([please change the port if necessary](https://github.com/yizhe1997/SleekFlowTodoListApp/blob/master/SleekFlowTodoListAPI/Properties/launchSettings.json)) for the Backend API project once the swagger UI appears. If you have any problems, especially with viewing the swagger UI or security issues, try using a different browser and [trusting the local development cert](https://go.microsoft.com/fwlink/?linkid=848054).
8. To test the APIs in postman download the [collection](https://github.com/yizhe1997/SleekFlowTodoListApp/blob/master/Deliverables/SleekFlowTodoListAPICollection.postman_collection.json) and [environment](https://github.com/yizhe1997/SleekFlowTodoListApp/blob/master/Deliverables/SleekFlowTodoListEnvVars.postman_environment.json) files and load it into postman. Theres explanation in the Body section on what to do. Use the Authenticate -> Authorize POST service so that the Tests script can auto populate the postman environment variables, afterwards you can start testing the TODO APIs.
![image](https://user-images.githubusercontent.com/81303202/215348784-f7b05a85-7c08-46a5-9f06-e634b9238e72.png)
9. You can directly test the APIs from the swagger UI as well. Just follow the same steps as (8.) to test the TODO APIs.
![image](https://user-images.githubusercontent.com/81303202/215348643-bee77c55-f4d5-4d0d-a9cd-e77e5aaa76ca.png)

## How to get started Frontend App(Local)
 - WORK IN PROGRESS
 - 
## How to host on Azure
 - WORK IN PROGRESS

## List of pending tasks:
 -  Swagger documentation not enough documentation (description, example inputs, etc.)
 -  Hosting on Azure (setup Azure AD B2C, SQL Server, Containers, etc.) so that users can try it out online
 -  Frontend Blazor WebAssembly still no working example yet
 -  In depth documentation of code
 -  Unit testing
 -  Technical design review
 -  SleekFlowTodoListAPI.csproj startup logging missing information

Conclusion: Its not entirely complete yet....

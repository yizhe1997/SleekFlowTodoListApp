# SleekFlowTodoListAPI

## 1. Configuration

To avoid unexpected errors during startup or runtime, the configuration should be configured before hosting. The backend API can be configured by either setting environment variables on the hosting server or user secrets on the local host. Currently, the following configurations are available in the API:

| Configurations    | Description                                                                                                                                  |
| ----------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ConnectionStrings | Configure connection strings of databases and Azure storage.                                                                                 |
| Database          | Configure option to seed database and setup the dealer portal's admin credenetial.                                                           |
| Jwt               | Configure JWT options for securely transmitting information between parties as a JSON object.                                                |
| AzureADB2CApi     | Configure Azure AD B2C options for backend API.                                                                                              |
| AzureADB2CSwagger | Configure Azure AD B2C options for client Swagger.                                                                                           |

### 1.1 Configuration options

Whether the options are left out or setup wrongly during setup, the appropriate error msg will be displayed in the hosting environment's log stream or the frontend application's dialog box when calling certain API services. The available options for the configurations are the following:

#### 1.1.1 ConnectionStrings

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ConnectionStrings__Primary           | The Database(DB) connection string for SleekFlowTodoListAPI server. Backend I/O only supports sql server.                                    |

#### 1.1.2 Database

Only one SleekFlowTodoListAPI server application user admin exists. 

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| Database__AdminEmail			       | The server application's user admin's email.								                                                                  |
| Database__AdminPassword              | The server application's user admin's password.							                                                                  |
| Database__DefaultPassword            | The server application's non-admin user default password.							                                                          |

#### 1.1.3 JWT

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| Jwt__SecretKey                       | The secret key used to create signing credentials.                                                                                           |
| Jwt__ValidForDays                    | Set the JWT token timespan in days. The default values is "14".                                                                              |

#### 1.1.4 AzureADB2CApi

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADB2CApi__ClientId              | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADB2CApi__Instance              | Tenant name copied from the Azure portal.                                                                                                    |
| AzureADB2CApi__SignUpSignInPolicyId  | Sign up and sign in policy or user flow created in Azure portal.                                                                             |
| AzureADB2CApi__Domain                | Azure AD tenant name.                                                                                                                        |

#### 1.1.5 AzureADB2CSwagger

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADB2CSwagger__ClientId          | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADB2CSwagger__ClientSecret      | The secret to prove identity when requesting token.                                                                                          |
| AzureADB2CSwagger__Scope             | Scopes to acces data and functionality provided by backend API.                                                                              |
| AzureADB2CSwagger__AppName           | App name registered in Azure portal.                                                                                                         |

*disclaimer: certain configuration options are not included because they are not/rarely used or have the appropriate default values already.
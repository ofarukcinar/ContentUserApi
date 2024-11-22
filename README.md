
# ContentUserApi Project

## Overview

This project is a microservices-based application built using .NET Core. It consists of multiple microservices that manage different parts of the application, such as content and user data, with a centralized API Gateway for routing. The solution is containerized using Docker for easy deployment and scalability.

---

## Technologies Used

- **Programming Language**: C#
- **Framework**: .NET 8
- **API Gateway**: Ocelot
- **Authentication**: JWT (JSON Web Tokens)
- **Dependency Injection**: Built-in .NET DI container
- **Logging**: Console logging with .NET Logging Abstractions
- **Containerization**: Docker & Docker Compose
- **Unit Testing**: xUnit
- **Version Control**: Git
- **IDE Support**: Visual Studio, Visual Studio Code, Rider

---

## Project Structure

### 1. Microservices
- **ContentApi**: Manages content-related operations such as creating, updating, retrieving, and deleting content.
- **UserApi**: Handles user-related functionalities including login, registration, and user profile management.

### 2. API Gateway
- Centralized routing for the microservices using Ocelot.
- Authentication and request forwarding.

### 3. Docker Support
- **`docker-compose.yml`**: Contains configuration for running the application in a containerized environment.

### 4. Tests
- **Tests Folder**: Includes unit and integration tests for microservices.

---

## Installation

### Prerequisites
1. Install [.NET SDK](https://dotnet.microsoft.com/download) version 8.0 or higher.
2. Install [Docker](https://www.docker.com/) and Docker Compose.
3. Clone this repository using Git.

### Steps
1. **Clone the repository**:
   ```bash
   git clone https://github.com/ofarukcinar/ContentUserApi.git
   ```

2. **Navigate to the project directory**:
   ```bash
   cd MicroservicesProjectCsprojUpdated
   ```

3. **Build and Run using Docker Compose**:
Migratons must be sent to the database after running docker to complete the migrations and create test data in the database.
   ```bash
   docker-compose up --build 
   dotnet ef database update --project ContentApi
   dotnet ef database update --project UserApi
   ```

4. **Access Services**:
   - API Gateway: `http://localhost:5050`
   - Swagger for `ContentApi`: `http://localhost:5001/swagger`
   - Swagger for `UserApi`: `http://localhost:5002/swagger`

---

## API Endpoints

### ContentApi
| HTTP Method | Endpoint         | Description                   |
|-------------|------------------|-------------------------------|
| GET         | /contents        | Retrieve all contents         |
| POST        | /contents        | Create new content            |
| GET         | /contents/{id}   | Retrieve content by ID        |
| PUT         | /contents/{id}   | Update content by ID          |
| DELETE      | /contents/{id}   | Delete content by ID          |

### UserApi
| HTTP Method | Endpoint        | Description                      |
|-------------|-----------------|----------------------------------|
| POST        | /users/login    | Authenticate and generate token |
| GET         | /users          | Retrieve all users              |
| POST        | /users          | Create new user                 |
| GET         | /users/{id}     | Retrieve user by ID             |
| PUT         | /users/{id}     | Update user by ID               |
| DELETE      | /users/{id}     | Delete user by ID               |

---

## Development

### Opening the Solution
1. Open `ContentUserApiTask.sln` in Visual Studio or any compatible IDE.

### Running Locally
1. Use Visual Studio to build and run the solution.
2. Run the API Gateway project and microservices individually.

---

## Testing

- Navigate to the `Tests` folder.
- Run tests using the following command:
  ```bash
  dotnet test
  ```

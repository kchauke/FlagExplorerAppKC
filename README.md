## Flag Explorer App

## Overview
The Flag Explorer App is designed to showcase your skills in building a modern software application. The project involves developing a backend API, integrating a frontend that retrieves and displays country flags, and creating a user interface with two distinct views.

## Requirements

### Part 1: Backend API
- **Technology**: ASP.NET Core Web API
- **Endpoints**:
  - `/countries`: Retrieve a list of countries.
  - `/countries/{name}`: Retrieve details about a specific country.
- **Architecture**: Use modern backend architecture patterns (e.g., MVC or Clean Architecture).
- **Testing**: Includes unit and integration tests.

### Part 2: Frontend Application
- **Technology**: Angular
- **Features**:
  - **Home Screen**: Display all country flags in a grid layout. Fetch flag images using the Open API: `https://restcountries.com/v3.1/all`.
  - **Detail Screen**: Display details about the country when a flag is clicked, including country name, population, and capital.
- **Testing**: Includes unit and integration tests.

### Part 3: Pipeline Integration
- **CI/CD Pipeline**: YAML configuration file for setting up a CI/CD pipeline.
  - **Tasks**:
    - Run automated tests for both frontend and backend.
    - Build the application.
    - Package the frontend and backend for deployment.

## Project Structure

### Backend (C#)
- **Controllers**: Contains API controllers.
- **Models**: Defines data models.
- **Services**: Implements business logic and data fetching.
- **Startup.cs**: Configures services and middleware.
- **Program.cs**: Entry point of the application.

### Frontend (Angular)
- **Components**: Contains Angular components for Home and Detail views.
- **Services**: Handles HTTP requests to the backend API.
- **Routing**: Configures application routes.

## Setup and Run

### Backend
1. Clone the repository.
2. Navigate to the backend project directory.
3. Restore dependencies: `dotnet restore`
4. Build the project: `dotnet build`
5. Run the application: `dotnet run`

### Frontend
1. Navigate to the frontend project directory.
2. Install dependencies: `npm install`
3. Start the development server: `ng serve`

## Testing
To do

### Backend
- Run unit tests: `dotnet test`

### Frontend
- Run unit tests: `ng test`

## CI/CD Pipeline
- The pipeline configuration is provided in the `pipeline.yml` file.

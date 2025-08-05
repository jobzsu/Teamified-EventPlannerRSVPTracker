# Teamified-EventPlannerRSVPTracker

This is a backend-only project for a web application. Due to time constraints, the frontend has not been developed.

## Prerequisites

Before you can run this project, you need to have the following installed:

* **Visual Studio**: A powerful IDE for building .NET applications.
* **NPM (Node Package Manager)**: Used for managing frontend dependencies, though not currently utilized in this backend-only project.
* **MSSQLLocalDB**: A lightweight, self-contained version of SQL Server Express that is often included with Visual Studio.

## Getting Started

Follow these steps to get the project up and running on your local machine:

1.  **Restore NuGet Packages**: Ensure all the necessary NuGet packages are restored. Visual Studio should do this automatically when you open the solution, but you can also right-click on the solution in the Solution Explorer and select "Restore NuGet Packages."

2.  **Update the Database**: Open the Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console) and run the following command to apply the latest database migrations:

    ```powershell
    Update-Database -Verbose
    ```

    Alternatively, you can navigate to the API project directory in your terminal (CMD or PowerShell) and run the `dotnet ef database update` command.

3.  **Run the Project**: Press `F5` in Visual Studio to start the API. This will launch the application and open a browser window with the Swagger/OpenAPI documentation, where you can explore and test the available endpoints.

## Testing Endpoints

Once the API is running, you can use the Swagger UI provided to test the various endpoints. The UI is accessible at the root URL of your running application (e.g., `https://localhost:7001/swagger/index.html`).

## Note on Frontend

This project is the backend component only. A separate repository or project would be required for the frontend application that consumes this API.

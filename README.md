# Teamified-EventPlannerRSVPTracker

This project consists of both a backend API and a frontend application.

## Prerequisites

Before you can run this project, you need to have the following installed:

* **Visual Studio**: A powerful IDE for building .NET applications.
* **NPM (Node Package Manager)**: Used for managing frontend dependencies.
* **MSSQLLocalDB**: A lightweight, self-contained version of SQL Server Express that is often included with Visual Studio.

## Backend - Getting Started

Follow these steps to get the API up and running on your local machine:

1.  **Restore NuGet Packages**: Ensure all the necessary NuGet packages are restored. Visual Studio should do this automatically when you open the solution, but you can also right-click on the solution in the Solution Explorer and select "Restore NuGet Packages."

2.  **Update the Database**: Open the Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console) and run the following command to apply the latest database migrations:

    ```powershell
    Update-Database -Verbose
    ```

    Alternatively, you can navigate to the API project directory in your terminal (CMD or PowerShell) and run the `dotnet ef database update` command.

3.  **Run the Project**: Press `F5` in Visual Studio to start the API. This will launch the application and open a browser window with the Swagger/OpenAPI documentation, where you can explore and test the available endpoints.

---

## Backend - Architectural Details

The backend is built with a **Clean Architecture** (also known as Onion Architecture) design, which separates the application into layers for better maintainability and scalability.

* **Entity Framework Core**: We're using a **code-first approach** with EF Core. The domain models are configured using the **Fluent API** to define their relationships and properties.
* **Repository Pattern**: The data access layer uses the Repository Pattern to abstract the logic for working with data, making it easier to switch data sources in the future.
* **Database Migrations**: We use EF Core migrations to manage changes to the database schema over time.
* **Logging**: **Serilog** is implemented for structured logging of all incoming HTTP requests to our endpoints, which is useful for debugging and monitoring.

---

## Frontend - Getting Started

The frontend is a separate project built with ReactJS. Follow these steps to run it:

1.  **Navigate to the Frontend Directory**: Open your terminal and change to the directory of the frontend project.

2.  **Restore NPM Packages**: Install all the required packages by running:

    ```bash
    npm install
    ```

3.  **Start the Frontend**: Run the following command to start the development server:

    ```bash
    npm start
    ```

---

## Frontend - Technology Stack

The frontend is built using **ReactJS** and leverages the following libraries for a smooth user experience:

* **react-router-dom**: Used for handling client-side routing and navigation between different pages or components.
* **useState Hooks**: Manages the state of components, allowing for dynamic and interactive user interfaces.
* **react-toastify**: Provides toast notifications to give users feedback on actions like successful API calls or errors.
* **Axios**: A promise-based HTTP client used to make API calls to the backend.

---

## Testing Endpoints

Once the API and frontend are running, you can use the Swagger UI provided by the backend to test the various endpoints. The UI is accessible at the root URL of your running backend application (e.g., `https://localhost:7001/swagger/index.html`).

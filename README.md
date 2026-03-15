# Restaurant Application

This is a full-stack web application for a fictional restaurant. It features a public-facing storefront for customers to browse products and a backend system for administrators to manage the menu. The project is intended as a portfolio piece to demonstrate skills in full-stack development with a C#/.NET backend and a static HTML/CSS/JS frontend.

## Tech Stack

*   **Backend:**
    *   [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
    *   [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
    *   Minimal APIs
    *   [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
    *   JWT-based Authentication
*   **Database:**
    *   [MySQL](https://www.mysql.com/)
*   **Frontend:**
    *   HTML5
    *   CSS3
    *   JavaScript (with jQuery)
    *   Bootstrap 5

## Project Structure

The project follows a standard ASP.NET Core structure with some key directories:

```
/
│
├─── Data/              # Contains the Entity Framework DbContext and migrations.
├─── Models/            # Contains C# model classes (e.g., Product, User, Order).
├─── Properties/        # Contains launch settings for development.
├─── Routes/            # Contains API route definitions (e.g., AuthRoutes, ProductRoutes).
├─── wwwroot/           # Contains all static frontend assets (HTML, CSS, JS, images).
│
├─── .gitignore         # Specifies intentionally untracked files to ignore.
├─── appsettings.json   # Configuration settings for the application.
├─── Program.cs         # The main entry point, configures services and middleware.
├─── README.md          # This file.
└─── Restaurant-Application.csproj # The C# project file, defines dependencies and properties.
```

## Getting Started

Follow these instructions to get the project running on your local machine.

### Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download) (Version 9.0 or later)
*   [MySQL Server](https://dev.mysql.com/downloads/mysql/)
*   A code editor (e.g., VS Code, Visual Studio)

### Installation & Running

1.  **Clone the repository:**
    ```sh
    git clone <repository-url>
    cd Restaurant-Application
    ```

2.  **Configure the database connection:**
    *   Open the `appsettings.json` file.
    *   Modify the `DefaultConnection` string to point to your local MySQL instance and database.
    *   The application will create the database and tables on first run if they don't exist.

3.  **Restore dependencies and run the application:**
    ```sh
    dotnet run
    ```
    The .NET CLI will automatically restore dependencies. The application will start, and you can access it at the URL specified in the console output (e.g., `http://localhost:5183`).

## API Endpoints

The application exposes a set of RESTful APIs for frontend interaction.

### Authentication API (`/api/auth`)

*   **`POST /register`**: Creates a new user.
    *   **Body:** `{ "email": "user@example.com", "passwordHash": "your_password", "userName": "your_username" }`
*   **`POST /login`**: Authenticates a user and returns a JWT.
    *   **Body:** `{ "email": "user@example.com", "passwordHash": "your_password" }`
*   **`GET /verify`**: (Requires Authentication) Verifies the current user's token and returns their details (ID, email, role).

### Products API (`/api/products`)

*   **`GET /`**: Retrieves a list of all products.
*   **`GET /{id}`**: Retrieves a single product by its ID.
*   **`POST /`**: (Requires Admin Authentication) Creates a new product.
*   **`PUT /{id}`**: (Requires Admin Authentication) Updates an existing product.
*   **`DELETE /{id}`**: (Requires Admin Authentication) Deletes a product.

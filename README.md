## Overview

The Reservation App is a part of a larger system designed to manage reservations, users, and payments. It follows the principles of Clean Architecture, utilizing the Mediator Pattern, Repository Pattern, and Command Query Responsibility Segregation (CQRS) to ensure a maintainable and scalable codebase.

## Purpose

The purpose of the Reservation App is to create a comprehensive reservation management platform where users can:

- Make and manage reservations
- Manage user profiles and view reservation history
- Handle payments and transactions

## Project Structure

The project is organized into several layers, each with a specific responsibility:

- **Reservation.App.Api**: Contains the API controllers and middleware for handling HTTP requests.
- **Reservation.App.Application**: Contains the application logic, including commands, queries, validators, and various contracts.
- **Reservation.App.Domain**: Contains the domain entities and shared enums.
- **Reservation.App.Infrastructure**: Contains the implementation of repositories, database context, and contracts for communicating with external APIs.

## Key Concepts

### Clean Architecture

The project is structured to follow Clean Architecture principles, ensuring separation of concerns and making the codebase easier to maintain and test.

### Mediator Pattern

The Mediator Pattern is used to handle requests and responses within the application. This is achieved using the MediatR library, which decouples the request handlers from the controllers.

### Repository Pattern

The Repository Pattern is used to abstract the data access logic, providing a clean API for the application layer to interact with the database.

### CQRS (Command Query Responsibility Segregation)

CQRS is used to separate the read and write operations, ensuring that each operation is handled by a dedicated handler. This improves the scalability and maintainability of the application.

## Technologies Used

### Frontend

- **React**: A JavaScript library for building user interfaces, used for previewing the reservations.
- **TypeScript**: A typed superset of JavaScript that compiles to plain JavaScript.
- **Redux**: A predictable state container for JavaScript apps.
- **Axios**: A promise-based HTTP client for the browser and Node.js.

### Backend

- **ASP.NET Core**: A cross-platform, high-performance framework for building modern, cloud-based, Internet-connected applications.
- **Entity Framework Core**: An object-database mapper for .NET, enabling developers to work with a database using .NET objects.
- **SQL Server**: A relational database management system developed by Microsoft.

### Authentication

- **ASP.NET Core Identity**: Provides authentication and authorization with roles

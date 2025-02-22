# BudgetWise

## Overview

BudgetWise is a full-stack application designed to help users manage their finances efficiently. The project consists of a frontend built with React and a backend powered by .NET Core.

## Getting Started

### Frontend

To start the frontend, navigate to the `frontend` directory and run:

```sh
cd frontend
npm install  # Install dependencies
npm start    # Start the development server
```

### Backend

To run the backend, navigate to the `backend` directory and execute:

```sh
cd backend
dotnet ef migrations add InitialCreate --project BudgetWise.Infrastructure --startup-project BudgetWise.API  # Create the database migration
cd .\BudgetWise.API\ 
dotnet ef database update               # Apply the migration to create the database
dotnet run                               # Start the backend server
```

## Features

- **Budget Tracking**: Track income and expenses
- **Category Management**: Organize transactions into categories
- **Real-time Updates**: Sync data between frontend and backend

## Tech Stack

- **Frontend**: React, Tailwind CSS
- **Backend**: .NET Core, Entity Framework
- **Database**: SQL Server

## Cause

This is created by Marc Sorial for a task interview

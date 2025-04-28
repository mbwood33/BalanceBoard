#!/bin/bash
# C:\Dev\BalanceBoard\BalanceBoard\entrypoint.sh

# Exit immediately if a command exits with a non-zero status.
set -e

# --- Wait for the database to be ready ---
# Using wait-for-it.sh to ensure the database container is accepting connections.
# You might need to install wait-for-it.sh in your project if you haven't.
echo "Waiting for database (postgres:5432)..."
# Adjust the path to wait-for-it.sh if it's not in the same directory
/app/wait-for-it.sh postgres:5432 --timeout=30 --strict -- echo "Database is up!"

# --- Apply Entity Framework Core Migrations ---
echo "Applying database migrations..."
# Execute the dotnet ef command to update the database
# Ensure the project name and path are correct relative to the WORKDIR /app in your Dockerfile
# If your .csproj is at /app/BalanceBoard/BalanceBoard.csproj, adjust the path here.
dotnet ef database update --project BalanceBoard.csproj

echo "Migrations applied successfully."

# --- Start the application ---
echo "Starting application..."
# Execute the command to run your application's main assembly
# Adjust the dll name to match your project's output assembly name
dotnet BalanceBoard.dll
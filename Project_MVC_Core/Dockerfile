# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy the .csproj file and restore dependencies
COPY ["Project_MVC_Core.csproj", "./"]
RUN dotnet restore "Project_MVC_Core.csproj"
# Copy the rest of the application code
COPY . .
# Publish the application for release
RUN dotnet publish "Project_MVC_Core.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
# Copy the published output from the build stage
COPY --from=build /app/publish .
# Expose the port Kestrel listens on. ASP.NET Core 8 defaults to 8080.
EXPOSE 8080
# Set the ASP.NET Core URLs environment variable to listen on all interfaces
ENV ASPNETCORE_URLS=http://+:8080
# Define the entry point for the application
ENTRYPOINT ["dotnet", "Project_MVC_Core.dll"]

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the project files and restore any dependencies
COPY *.csproj ./  
RUN dotnet restore

# Copy the remaining files and build the application
COPY . ./  
RUN dotnet publish BoardGameBackend.csproj -c Release -o out   

RUN mkdir -p /app/Downloads
ADD Downloads/MyApp.rar /opt/
COPY Downloads/MyApp.rar /app/Downloads/MyApp.rar

# Use the official ASP.NET Core runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .   

COPY --from=build-env /app/Downloads /app/Downloads

# Set the entry point for the application
ENTRYPOINT ["dotnet", "BoardGameBackend.dll"]
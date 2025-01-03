# Use the .NET ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID

## Install additional tools if needed
#RUN apt-get update \
#    && apt-get install -y curl iputils-ping 

WORKDIR /app
EXPOSE 1030

# Set the environment variable to ensure the application listens on 0.0.0.0:1030
ENV ASPNETCORE_URLS=http://0.0.0.0:1030

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project files and submodule references
COPY ["CourseServiceAPI/CourseServiceAPI.csproj", "CourseServiceAPI/"]
COPY ["SharedLibraries/Exceptions/HackleberryExceptions.csproj", "SharedLibraries/Exceptions/"]
COPY ["SharedLibraries/HackleberrySharedModels/HackleberryModels.csproj", "SharedLibraries/HackleberrySharedModels/"]
COPY ["SharedLibraries/HackleberryServices/HackleberryServices.csproj", "SharedLibraries/HackleberryServices/"]

# Restore dependencies
RUN dotnet restore "CourseServiceAPI/CourseServiceAPI.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/CourseServiceAPI"

# Build the project
RUN dotnet build "CourseServiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CourseServiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

# Use the ASP.NET runtime as the final stage
FROM base AS final
WORKDIR /app

# Copy published output and source files (if needed for debugging)
COPY --from=publish /app/publish .

# Define the entry point
ENTRYPOINT ["dotnet", "CourseServiceAPI.dll"]

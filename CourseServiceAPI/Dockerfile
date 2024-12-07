FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update \
	&& apt-get install -y curl iputils-ping 

WORKDIR /app
EXPOSE 1030   

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["CourseServiceAPI/CourseServiceAPI.csproj", "CourseServiceAPI/"]
COPY ["SharedLibraries/Exceptions/HackleberryExceptions.csproj", "SharedLibraries/Exceptions/"]
COPY ["SharedLibraries/HackleberrySharedModels/HackleberryModels.csproj", "SharedLibraries/HackleberrySharedModels/"]
COPY ["SharedLibraries/HackleberryServices/HackleberryServices.csproj", "SharedLibraries/HackleberryServices/"]
RUN dotnet restore "./CourseServiceAPI/CourseServiceAPI.csproj"
COPY . .

WORKDIR "/src/CourseServiceAPI"
RUN dotnet build "./CourseServiceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./CourseServiceAPI.csproj" -c Release -o /app/publish --no-restore /p:PublishWithAspNetCoreTargetManifest=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src /app/src
ENTRYPOINT ["dotnet", "CourseServiceAPI.dll"]
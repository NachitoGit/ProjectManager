# 1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2
COPY ["src/ProjectManager.WebAPI/ProjectManager.WebAPI.csproj", "src/ProjectManager.WebAPI/"]
COPY ["src/ProjectManager.Application/ProjectManager.Application.csproj", "src/ProjectManager.Application/"]
COPY ["src/ProjectManager.Domain/ProjectManager.Domain.csproj", "src/ProjectManager.Domain/"]
COPY ["src/ProjectManager.Infrastructure/ProjectManager.Infrastructure.csproj", "src/ProjectManager.Infrastructure/"]
RUN dotnet restore "src/ProjectManager.WebAPI/ProjectManager.WebAPI.csproj"

# 3
COPY . .
WORKDIR "/app/src/ProjectManager.WebAPI"
RUN dotnet build "ProjectManager.WebAPI.csproj" -c Release -o /app/build

# 4
FROM build AS publish
RUN dotnet publish "ProjectManager.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManager.WebAPI.dll"]
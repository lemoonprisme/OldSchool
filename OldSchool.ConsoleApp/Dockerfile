FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OldSchool.ConsoleApp/OldSchool.ConsoleApp.csproj", "OldSchool.ConsoleApp/"]
RUN dotnet restore "OldSchool.ConsoleApp/OldSchool.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/OldSchool.ConsoleApp"
RUN dotnet build "OldSchool.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OldSchool.ConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OldSchool.ConsoleApp.dll"]

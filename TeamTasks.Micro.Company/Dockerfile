FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TeamTasks.Micro.Company/TeamTasks.Micro.Company.csproj", "TeamTasks.Micro.Company/"]
RUN dotnet restore "TeamTasks.Micro.Company/TeamTasks.Micro.Company.csproj"
COPY . .
WORKDIR "/src/TeamTasks.Micro.Company"
RUN dotnet build "TeamTasks.Micro.Company.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamTasks.Micro.Company.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamTasks.Micro.Company.dll"]

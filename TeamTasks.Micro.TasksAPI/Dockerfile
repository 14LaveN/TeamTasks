FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7002
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TeamTasks.Micro.TasksAPI/TeamTasks.Micro.TasksAPI.csproj", "TeamTasks.Micro.TasksAPI/"]
RUN dotnet restore "TeamTasks.Micro.TasksAPI/TeamTasks.Micro.TasksAPI.csproj"
COPY . .
WORKDIR "/src/TeamTasks.Micro.TasksAPI"
RUN dotnet build "TeamTasks.Micro.TasksAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamTasks.Micro.TasksAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamTasks.Micro.TasksAPI.dll"]

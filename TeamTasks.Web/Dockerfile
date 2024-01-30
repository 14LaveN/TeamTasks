FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TeamTasks.Web/TeamTasks.Web.csproj", "TeamTasks.Web/"]
RUN dotnet restore "TeamTasks.Web/TeamTasks.Web.csproj"
COPY . .
WORKDIR "/src/TeamTasks.Web"
RUN dotnet build "TeamTasks.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamTasks.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamTasks.Web.dll"]

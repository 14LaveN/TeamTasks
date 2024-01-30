FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7210
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TeamTasks.Micro.Identity/TeamTasks.Micro.Identity.csproj", "TeamTasks.Micro.Identity/"]
RUN dotnet restore "TeamTasks.Micro.Identity/TeamTasks.Micro.Identity.csproj"
COPY . .
WORKDIR "/src/TeamTasks.Micro.Identity"
RUN dotnet build "TeamTasks.Micro.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamTasks.Micro.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamTasks.Micro.Identity.dll"]

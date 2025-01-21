# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5015
EXPOSE 5016


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChatsService/ChatsService.API/ChatsService.API.csproj", "ChatsService/ChatsService.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["ChatsService/ChatsService.BLL/ChatsService.BLL.csproj", "ChatsService/ChatsService.BLL/"]
COPY ["ChatsService/ChatsService.DAL/ChatsService.DAL.csproj", "ChatsService/ChatsService.DAL/"]
RUN dotnet restore "./ChatsService/ChatsService.API/ChatsService.API.csproj"
COPY . .
WORKDIR "/src/ChatsService/ChatsService.API"
RUN dotnet build "./ChatsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatsService.API.dll"]
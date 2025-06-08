# Используем официальный образ .NET 8 для запуска приложения (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7878
EXPOSE 443
# Для HTTPS

# Для сборки приложения добавляем SDK образ .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR "/src"

# Копируем .csproj для проекта MatchMakingService
#COPY ["Projects/MatchMakingService/MatchMakingService/MatchMakingService.csproj", "MatchMakingService/"]

# Копируем все исходные файлы проекта
COPY ["Projects/MatchMakingService/", "MatchMakingService/"]

WORKDIR "/src/MatchMakingService"
# Восстанавливаем зависимости
RUN dotnet restore "MatchMakingService/MatchMakingService.csproj"

# Собираем проект MatchMakingService
RUN dotnet build "MatchMakingService/MatchMakingService.csproj" -c Release -o /app/build

# Публикуем проект MatchMakingService
FROM build AS publish
WORKDIR /src
RUN dotnet publish "MatchMakingService/MatchMakingService/MatchMakingService.csproj" -c Release -o /app/publish

# Финальный образ для MatchMakingService
FROM base AS final
WORKDIR /app

# Копируем опубликованное приложение
COPY --from=publish /app/publish /app

# Запускаем MatchMakingService
ENTRYPOINT ["dotnet", "MatchMakingService.dll", "--urls=http://0.0.0.0:7878"]
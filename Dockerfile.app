# Используем официальный образ .NET 8 для запуска приложения (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443  
# Для HTTPS

# Для сборки приложения добавляем SDK образ .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR "/src"

# Копируем .csproj для проекта SessionApi
#COPY ["Projects/SnakeServer/SnakeServer/SnakeApi/SessionApi.csproj", "SnakeServer/SnakeApi/"]

# Копируем все исходные файлы проекта
COPY Projects/SnakeServer/ . 

# Восстанавливаем зависимости
RUN dotnet restore "SnakeServer/SnakeApi/SessionApi.csproj"

# Собираем проект SessionApi
WORKDIR "/src/SnakeServer/SnakeApi"
RUN dotnet build "SessionApi.csproj" -c Release -o /app/build

# Публикуем проект SessionApi
FROM build AS publish
WORKDIR /src
RUN dotnet publish "SnakeServer/SnakeApi/SessionApi.csproj" -c Release -o /app/publish

# Финальный образ для SessionApi
FROM base AS final
WORKDIR /app

# Копируем опубликованное приложение
COPY --from=publish /app/publish /app

# Запускаем SessionApi
ENTRYPOINT ["dotnet", "SessionApi.dll", "--urls=http://0.0.0.0:8080"]
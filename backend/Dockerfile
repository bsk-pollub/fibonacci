#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FS_Backend/FS_Backend.csproj", "FS_Backend/"]
RUN dotnet restore "FS_Backend/FS_Backend.csproj"
COPY . .
WORKDIR "/src/FS_Backend"
RUN dotnet build "FS_Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FS_Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FS_Backend.dll"]

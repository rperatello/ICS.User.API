#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ICS.User.API/ICS.User.API.csproj", "ICS.User.API/"]
RUN dotnet restore "ICS.User.API/ICS.User.API.csproj"
COPY . .
WORKDIR "/src/ICS.User.API"
RUN dotnet build "ICS.User.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ICS.User.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ICS.User.API.dll"]
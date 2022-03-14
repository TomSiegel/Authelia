#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Authelia.Server.Interface/Authelia.Server.Interface.csproj", "Authelia.Server.Interface/"]
RUN dotnet restore "Authelia.Server.Interface/Authelia.Server.Interface.csproj"
COPY . .
WORKDIR "/src/Authelia.Server.Interface"
RUN dotnet build "Authelia.Server.Interface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authelia.Server.Interface.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authelia.Server.Interface.dll"]
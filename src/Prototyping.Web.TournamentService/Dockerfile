#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Prototyping.Web.TournamentService/Prototyping.Web.TournamentService.csproj", "Prototyping.Web.TournamentService/"]
RUN dotnet restore "Prototyping.Web.TournamentService/Prototyping.Web.TournamentService.csproj"
COPY . .
WORKDIR "/src/Prototyping.Web.TournamentService"
RUN dotnet build "Prototyping.Web.TournamentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prototyping.Web.TournamentService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prototyping.Web.TournamentService.dll"]
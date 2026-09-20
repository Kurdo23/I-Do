FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY IDotAPI/IDotAPI.csproj IDotAPI/
RUN dotnet restore IDotAPI/IDotAPI.csproj
COPY IDotAPI/ IDotAPI/
WORKDIR /src/IDotAPI
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "IDotAPI.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY IDotAPI/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "IDotAPI.dll"]
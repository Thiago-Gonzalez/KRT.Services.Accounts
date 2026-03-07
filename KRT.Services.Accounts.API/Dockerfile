FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "KRT.Services.Accounts.API/KRT.Services.Accounts.API.csproj"
RUN dotnet publish "KRT.Services.Accounts.API/KRT.Services.Accounts.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "KRT.Services.Accounts.API.dll"]
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend/LoanStream.Api.csproj ./backend/
RUN dotnet restore ./backend/LoanStream.Api.csproj

COPY backend ./backend
RUN dotnet publish ./backend/LoanStream.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
EXPOSE 8080
ENTRYPOINT ["dotnet", "LoanStream.Api.dll"]

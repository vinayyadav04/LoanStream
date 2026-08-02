FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LoanStream.Api.csproj", "./"]
RUN dotnet restore "LoanStream.Api.csproj"

COPY . .
RUN dotnet publish "LoanStream.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
EXPOSE 8080
ENTRYPOINT ["dotnet", "LoanStream.Api.dll"]

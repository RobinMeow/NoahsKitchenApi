FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# this is for Cors
ARG MY_ALLOWED_ORIGINS
ENV ALLOWED_ORIGINS=${MY_ALLOWED_ORIGINS}

ENV ASPNETCORE_ENVIRONMENT=Production

ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "api.dll"]

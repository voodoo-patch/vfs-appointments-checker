# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./*.csproj .
RUN dotnet restore --use-current-runtime

# copy everything else and build app
COPY ./. .
RUN mv -f appsettings.local.json appsettings.json

# publish
RUN dotnet publish \
  -c Release \
  --self-contained true \
  --no-restore \
  -o /app

FROM alpine:3.17 as final

# Add some libs required by .NET runtime 
RUN apk add --no-cache libstdc++ libintl icu

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./TimedChecker.Bot"]
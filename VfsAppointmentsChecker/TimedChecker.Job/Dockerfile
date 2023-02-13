ARG PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# renewing arg
ARG PLAYWRIGHT_BROWSERS_PATH
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

# install playwright
ENV PLAYWRIGHT_BROWSERS_PATH=${PLAYWRIGHT_BROWSERS_PATH}
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools/"
RUN playwright install --with-deps chromium 

FROM mcr.microsoft.com/playwright:v1.30.0-focal as final

EXPOSE 80
EXPOSE 443

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./TimedChecker.Job"]
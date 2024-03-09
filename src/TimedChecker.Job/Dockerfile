ARG PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# renewing arg
ARG PLAYWRIGHT_BROWSERS_PATH
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/TimedChecker.Job
COPY ["TimedChecker.Job.csproj", "./"]
RUN dotnet restore "TimedChecker.Job.csproj"
COPY . .
RUN dotnet build "TimedChecker.Job.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish
RUN dotnet publish "TimedChecker.Job.csproj" \
  -c $BUILD_CONFIGURATION \
  --self-contained true \
  -o /app/publish

# install playwright
ENV PLAYWRIGHT_BROWSERS_PATH=${PLAYWRIGHT_BROWSERS_PATH}
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools/"
RUN playwright install --with-deps chromium 

FROM mcr.microsoft.com/playwright:v1.42.1-focal as final

EXPOSE 80
EXPOSE 443

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["./TimedChecker.Job"]
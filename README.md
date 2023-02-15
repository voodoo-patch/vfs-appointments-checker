# vfs-appointments-checker

This project is comprised of two main components:
- A CRON scheduled Job in charge of checking for appointments
- A Telegram Bot that offers a user friendly way of remotely control the Job through its APIs

The Job leverages Playwright to simulate user interaction with the VFS website and sends a message to a Telegram Bot in a specified channel when a new appointment is available.




## Build Docker Images

```sh
# TimedChecker.Bot
(cd VfsAppointmentsChecker/TimedChecker.Bot; \
docker build -t vfs-checker-bot .  --progress=plain)
```

```sh
# TimedChecker.Job
(cd VfsAppointmentsChecker/TimedChecker.Job; \
docker build -t vfs-checker .  --progress=plain)
```

## Run Docker Containers

To configure the containers you can use environment variables (dotenv files) supplied through `docker run` command

```sh
# TimedChecker.Bot
docker run \
  -d \
  --restart unless-stopped \
  --env-file ./vfs-checker-bot.env \
  vfs-checker-bot:latest
```

```sh
# TimedChecker.Job
docker run \
  -d \
  --restart unless-stopped \
  -p 5000:80 \
  --env-file ./vfs-checker-job.env \
  vfs-checker:latest
```

# TimedChecker.Bot

## Configuration

```dotenv
TelegramSettings__BotId={my-bot-id}

AppointmentCheckerSettings__ApiBaseLocation=http://localhost:5000/

AppointmentCheckerSettings__Check__Method=POST
AppointmentCheckerSettings__Check__Path=trigger

AppointmentCheckerSettings__Pause__Method=POST
AppointmentCheckerSettings__Pause__Path=stop

AppointmentCheckerSettings__Resume__Method=POST
AppointmentCheckerSettings__Resume__Path=start
```

# TimedChecker.Job

## APIs

### /trigger

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /start

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /stop

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |


## Configuration

```dotenv
ASPNETCORE_URLS=http://+:80
EnableSwagger=true

TelegramSettings__BotApiEndpoint=https://api.telegram.org/bot
TelegramSettings__BotId={my-bot-id}
TelegramSettings__Channels__0={my-tg-channel}

VfsSettings__Account__Email={my-vfs-email}
VfsSettings__Account__Password={my-vfs-pw}
VfsSettings__Urls__Authentication=https://visa.vfsglobal.com/gbr/en/ita/login

JobSettings__CronSchedule={my-CRON-expression}
JobSettings__RunOnStartup=false
JobSettings__Headless=true
```

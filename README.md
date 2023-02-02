# Task scheduler using Quartz

## Run using docker

### Run the mysql db

- cd into `compose/quartz-mysql`
- run the service

```bash
docker compose up -d
```

### Run the api

- cd into `compose/api`
- copy and rename ./samples/.env.sample to .env
- set the env as

```bash
SIG_SECRET=<your_secret_key>
```

- run the service

```bash
docker compose up -d
```

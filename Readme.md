# ClashStats

Бэкенд-сервис на .NET для автоматического сбора, хранения и анализа статистики из Clash of Clans.

Приложение взаимодействует с API игры, собирает данные по кланам, участникам, обычным войнам (CW) и лигам войн кланов (CWL),
после чего сохраняет историю в базу данных для дальнейшей агрегации.

## Стек

- **Платформа:** C#, .NET (Web API, Worker Service)
- **База данных:** PostgreSQL, Entity Framework Core
- **Инфраструктура:** Docker, Docker Compose
- **Архитектура:** Clean Architecture

## Структура проекта

- `Domain` — доменные модели (Clan, Player, War), константы и структуры данных.
- `Application` — бизнес-логика, сервисы (синхронизация, обработка лиг), интерфейсы и DTO.
- `Infrastructure` — контекст EF Core, SQL-миграции, реализация HTTP-клиента к Clash API.
- `App` — точка входа, конфигурация DI, Minimal API эндпоинты (`SummaryEndpoints`) и фоновые задачи (`StatsUpdateWorker`).

## Требования

- .NET SDK (10.0+)
- Docker и Docker Compose
- Токен разработчика от [Clash of Clans API](https://developer.clashofclans.com/)

## Запуск (Docker)

1. Создайте файл `.env` в корне проекта (можно скопировать из `.env.example`) и заполните его

2. Поднимите базу данных и приложение одной командой:
    ```bash
    docker-compose up -d
    ```
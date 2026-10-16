# TaskApi

REST API для управления задачами на ASP.NET Core Web API.

## Стек

- C# / .NET 10
- ASP.NET Core Web API — контроллеры, DI
- Entity Framework Core — ORM, миграции
- SQL Server — база данных
- Service Layer — бизнес-логика отделена от контроллеров
- Global Exception Handling — middleware
- JWT-аутентификация (Microsoft.AspNetCore.Authentication.JwtBearer)
- BCrypt для хэширования паролей

## Функциональность

- CRUD для проектов, исполнителей, задач
- Связь многие-ко-многим с дополнительным полем (Project ↔ Executor через TaskItem)
- Количество задач у каждого проекта и исполнителя
- Фильтрация задач по проекту, исполнителю, статусу
- Сервисный слой — вся логика в сервисах
- Глобальная обработка исключений через middleware
- DTO для запросов и ответов
- Валидация входных данных (Data Annotations)
- Swagger UI для тестирования
- JWT-аутентификация (регистрация, логин, защита эндпоинтов)

## Эндпоинты

### Projects

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/projects | Все проекты с количеством задач |
| GET | /api/projects/{id} | Проект по Id |
| POST | /api/projects | Создать проект |
| PUT | /api/projects/{id} | Обновить проект |
| DELETE | /api/projects/{id} | Удалить проект вместе с задачами |

### Executors

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/executors | Все исполнители с количеством задач |
| GET | /api/executors/{id} | Исполнитель по Id |
| POST | /api/executors | Создать исполнителя |
| PUT | /api/executors/{id} | Обновить исполнителя |
| DELETE | /api/executors/{id} | Удалить исполнителя вместе с задачами |

### TaskItems

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/taskitems | Все задачи с именами проекта и исполнителя |
| GET | /api/taskitems/{id} | Задача по Id |
| POST | /api/taskitems | Создать задачу |
| PUT | /api/taskitems/{id} | Обновить задачу |
| DELETE | /api/taskitems/{id} | Удалить задачу |
| GET | /api/taskitems/project/{projectId} | Задачи проекта |
| GET | /api/taskitems/executor/{executorId} | Задачи исполнителя |
| GET | /api/taskitems/status/{status} | Задачи с определённым статусом |

### Auth

| Метод | URL | Описание |
|-------|-----|----------|
| POST | /api/auth/register | Регистрация |
| POST | /api/auth/login | Логин (возвращает JWT-токен) |

## Архитектура

- Models — сущности БД (Project, Executor, TaskItem)
- DTOs — контракты API
- Data — DbContext
- Services — бизнес-логика
- Exceptions — собственные исключения + middleware
- Controllers — обработка HTTP-запросов

## Как запустить

1. Установить .NET 10 SDK и SQL Server Express.
2. Клонировать репозиторий: git clone https://github.com/danila-markevich/TaskApi.git
3. Настроить строку подключения в Program.cs.
4. Применить миграции: dotnet ef database update
5. Запустить: dotnet run
6. Открыть Swagger: https://localhost:XXXX/swagger

## Автор

Danila Markevich — https://github.com/danila-markevich
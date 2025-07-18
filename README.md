Тестовое десктопное приложение на WPF для управления заказами, сотрудниками и контрагентами в компании по доставке воды.

## Стек

*   **Платформа:** .NET 
*   **Язык:** C#
*   **UI Framework:** WPF
*   **Архитектурный паттерн:** MVVM (с использованием `CommunityToolkit.Mvvm`)
*   **ORM:** NHibernate
*   **DI Контейнер:** `Microsoft.Extensions.DependencyInjection`

## Архитектура

Проект построен с использованием принципов чистой архитектуры (Clean Architecture), разделенной на следующие слои:

*   `WaterCarrier.Domain`: Содержит основные доменные модели (`Employee`, `Order`, `Counterparty`) и бизнес-логику, инкапсулированную в этих моделях. Этот слой не зависит ни от каких других слоев в проекте.
*   `WaterCarrier.Application`: Содержит интерфейсы репозиториев (`IEmployeeRepository` и др.), определяющие контракты для работы с данными.
*   `WaterCarrier.Infrastructure`: Реализует слой доступа к данным. Содержит реализации репозиториев с использованием NHibernate, маппинги сущностей и конфигурацию для подключения к базе данных.
*   `WaterCarrier` (UI): Слой представления, реализованный на WPF. Содержит View, ViewModels и сервисы, такие как `DialogService`.

## Основной функционал

*   **Сотрудники:** Просмотр, добавление, редактирование и удаление сотрудников.
*   **Контрагенты:** Просмотр, добавление, редактирование и удаление контрагентов с привязкой к сотруднику-куратору.
*   **Заказы:** Просмотр, добавление, редактирование и удаление заказов с привязкой к сотруднику и контрагенту.
*   **Валидация:** Реализована логика валидации в доменных моделях для обеспечения целостности данных.
*   **Обработка ошибок:** Реализованы проверки для предотвращения удаления связанных данных (например, нельзя удалить сотрудника, если он является куратором или связан с заказами).

## Как запустить

1.  **Клонируйте репозиторий:**
    ```bash
    git clone https://github.com/cofheim/WaterCarrier
    ```
2.  **Откройте решение:**
    Откройте файл `WaterCarrier.sln` в Visual Studio.
3.  **База данных:**
    Проект использует **MariaDB** (или **MySQL**). Перед запуском убедитесь, что у вас установлен и запущен сервер MariaDB/MySQL.
    
    Строка подключения находится в файле `WaterCarrier.Infrastructure/hibernate.cfg.xml`. По умолчанию она настроена на:
    - **Server**: `localhost`
    - **Port**: `3306`
    - **Database**: `water_carrier_db`
    - **User ID**: `root`
    - **Password**: `Ellenoize2002`

    База данных `water_carrier_db` и таблицы будут созданы/обновлены автоматически при первом запуске приложения благодаря настройке `hbm2ddl.auto`.

4.  **Запустите проект:**
    Нажмите `F5` или кнопку "Start" в Visual Studio, чтобы собрать и запустить приложение. 
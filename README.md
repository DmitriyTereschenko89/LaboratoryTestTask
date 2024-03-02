# Laboratory Test Task
Обработка XML файлов с использованием микросервисного подхода. (C#
RabbitMQ, SQLite)
Задача: Разработать систему для обработки XML файлов с использованием RabbitMQ для
взаимодействия между несколькими сервисами в .NET.
Требования:
1.FileParser Service: Сервис, отвечающий за обработку XML файлов. Читает XML файлы с
диска с периодичностью 1 раз в секунду. Парсит XML данные и создает классы на основе
полученных данных. Случайным образом изменяет поле ModuleState каждого модуля (Online,
Run, NotReady, Offline). Формирует результат в формате JSON (содержащем все модули с
измененным полем). Отправляет сформированный JSON в DataProcessor Service через
RabbitMQ для дальнейшей обработки.
2.DataProcessor Service: Сервис, который принимает сообщения из RabbitMQ и обрабатывает
данные. Сохраняет результаты обработки в базу данных (возможно использование локальной
БД). Сохраняет поля ModuleCategoryID и ModuleState. Если ModuleCategoryID уже
существует в БД, обновляет только ModuleState.
3.RabbitMQ:
Используется для связи между FileParser и DataProcessor сервисами.
4.Многопоточность:
Обеспечить обработку файла в отдельном потоке для оптимизации процесса.
5.Дополнительные задания:
Реализовать логирование для отслеживания процесса обработки файла. Обработка ошибок. 

Подключение.
-Установить докер.
-Установить sqlite
-Открыть два экземпляра проекта, в одном запускаемым проектом сделать HostedServiceXmlParser, во втором - DataProcessor.
-Запустить терминал и выполнить команду docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
-Запустить первым сервис HostedServiceXmlParser
-В докере октрыть ссылку с портом 15672 и авторизоваться user - guest, password - guest для проверки создания очереди в rabittmq
-Запустить второй сервис DataProcessor.
-Открыть sqlite, добавить созданную бд во втором сервисе DataProcessor, после этого можно проверить записи в таблице.

В файла docker-compose, docker-compose.override.yml, appsettings содержатся конфигурируемыу поля. 
В сервисе HostedServiceXmlParser в appsettings содержатся поля FrequencyOfDataChangeInSeconds - периодичность чтения диска, DirectoryPath - путь, который нужно проверять, по умолчанию Files/ (файлы для теста существуют в данной папке)
В сервисе DataProcessor в appsettings содержит поле DataSource - путь и название базы данных, по умолчанию прописана база данных "mydatabase.db" (находиться в папку сервиса DataProcessor) и находиться в папке сервиса.

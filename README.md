# DI и DIP в приложении

1. Применить DI и DIP. Не должно остаться статических классов
2. По понедельникам стоимость всех товаров должна уменьшаться на 30%
3. Создайте абстракцию, назначение которой – предоставлять текущее время.
- [ ] Сделайте реализацию, предоставляющую текущее время во временной зоне UTC.
- [ ] Внедрите получившуюся абстракцию и реализацию в виде зависимости в свой проект.
- [ ] Добавьте эту зависимость в свой сервис и разрешите ее.
4. Сделайте класс отправки сообщений асинхронным.
5. Подумайте и примите решение о выборе оптимального времени жизни для сервиса отправки сообщений.
6. Каждый час отправляйте письмо на email с сообщением о том, что сервер работает исправно.
---

1. Сделайте класс и интерфейс SmtpEmailSender, IEmailSender полностью асинхронными.
2. Реализуйте асинхронное высвобождение ресурсов.
3. Сделайте SmtpEmailSender корректным Scoped сервисом.
---

1. Реализуйте возможность изменять параметры авторизации вашего SmtpEmailSender’а без перезапуска веб-приложения(IOptionsSnapshot, IOptionsMonitor).
2. Реализуйте логирование для класса отправки имейла путем реализации приема перехвата зависимостей.
---
# Логирование(уровни логирования, структурное логирование, Polly)

1. Переместите логин и пароль к SMTP серверу в пользовательские секреты.
2. При неуспешной отправке Email’a, сделайте 2 повторные попытки отправки. Обратите внимание на уровни логирование.
3. Добавьте Serilog в ваш проект по примеру: https://github.com/datalust/dotnet6-serilog-example
---
1. Добавьте возможность задать кол-во попыток через конфигурации.
2. Реализуйте динамическую задержку перед каждым ретраем (WaitAndRetry).
- [ ] Добавьте отправку логов в баг трекер Sentry
- [ ] Добавьте логирование в Seq в ваш проект.


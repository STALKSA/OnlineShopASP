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

- [ ] Подключение Scoped зависимости, приминение антипаттерна Service Locator

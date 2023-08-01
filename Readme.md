# CryptoEat 2.0
Metamask, Brave, Binance Chain, Ronin, Exodus, Atomic wallets checker

Функционал и фичи:
- Поддержка кошельков Metamask, Ronin Wallet, Binance Chain, Brave Wallet, Exodus desktop wallet, Atomic desktop wallet
- Написан на C#, красивый и модульный код
- Везде присутствует логгирование и обработка ошибок
- Брутфорс на GPU для всех кошельков, кроме Atomic Wallet
- Автовывод с кошельков
- Автосвап для bsc, eth
- Лёгкое добавление свапа для других сетей - требуется только адрес роутера и rpc
- Брутфорс с комбинациями, мутациями, получение паролей с Antipublic MYRZ, из FTP, из FileGrabber
- Скaн по debank, cкaн в сети Tron, cкaн BTC и LTC
- Предварительный cкaн валлетов и сортировка от высокого баланса к низкому
- Поиск seed фраз в FileGrabber
- Очень быстрая скорость работы, многопоточность и асинхронные задачи
- Поддержка прокси листов разных форматов
- Кеш сканирования

Программа сделана исключительно в образовательных целях, для тестирования на собственных кошельках

P.S. Нужны прокси с ротацией. Если у вас нет прокси с ротацией - пожалуйста, не пишите мне

[Windows build](https://github.com/kzorin52/CryptoEat/actions/workflows/dotnet.yml)

# Native Console
Нативная консоль позволяет вызывать методы используя lldb отладчик в XCode или с помощью Quick Search в редакторе. 

<img src="https://github.com/0limpik/NConsole/assets/50516863/0be365c0-5b6e-42e2-b60e-b6f9eeab2720" height="200" />

### Quick start:
- Открыть окно поиска
- Ввести nc:help
- Нажать Execute

<img src="https://github.com/0limpik/NConsole/assets/50516863/ad310af9-7edd-4f19-9896-cde9c60edf13" height="200" />

### Регистрация собственных команд
- Добавить регистрацию команд из классов BootstrapEditor и Bootstrap к DI, или оставить Singleton.
- При наличии собственной сборки добавить ее в процесс регистрации.
- Создать статический метод консоли с сигнатурой static string/void CommandMethod (string argument)/()
- Проверить его наличие активировав в верхнем меню Tools/NConsole/CheckCommands
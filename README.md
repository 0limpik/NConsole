# Native Console
Нативная консоль позволяет вызывать методы используя lldb отладчик в XCode или с помощью Quick Search в редакторе. 

<img src="https://github.com/0limpik/NConsole/assets/50516863/bc0fd235-2767-4253-a803-a3b124f4743a" height="200" />

### Quick start:
- Открыть окно поиска
- Ввести nc:help
- Нажать Execute

<img src="https://github.com/0limpik/NConsole/assets/50516863/4141f0b9-fb6c-4c37-8391-feab52d190a4" height="200" />

### Регистрация собственных команд
- Добавить регистрацию команд из классов BootstrapEditor и Bootstrap к DI, или оставить Singleton.
- При наличии собственной сборки добавить ее в процесс регистрации.
- Создать статический метод консоли с сигнатурой static string/void CommandMethod (string argument)/()
- Проверить его наличие активировав в верхнем меню Tools/NConsole/CheckCommands

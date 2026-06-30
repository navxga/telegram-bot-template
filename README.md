# CPASMS — Telegram Bot Architecture Template

<details>
<summary><b>🇧🇷 Português</b></summary>

## CPASMS — Template de Bot para Telegram

Projeto base (template) para bots de Telegram em **.NET**, usando `Microsoft.Extensions.Hosting` e injeção de dependência. A ideia central é manter cada comando do bot isolado em sua própria classe, sem um `switch` central acoplando tudo.

### Como funciona

- **`ICommand`** define o contrato de um comando: um `Trigger` (ex: `/help`) e um método `Execute`.
- Cada comando (`HelpCommand`, `RecargaCommand`, etc.) é uma classe independente que implementa `ICommand`.
- **`CommandDispatcher`** recebe todos os `ICommand` registrados via DI (`IEnumerable<ICommand>`) e roteia a mensagem recebida para o comando certo, com base no `Trigger`.
- **`BotHostedService`** é o `IHostedService` que conecta no Telegram, escuta mensagens e delega pro `CommandDispatcher`.

### Adicionando um novo comando

1. Crie uma classe em `Commands/` implementando `ICommand`:

```csharp
public class MeuComando : ICommand
{
    public string Trigger => "/meucomando";
    public string Description => "Descrição curta do que ele faz";

    public string Execute(string messageText)
    {
        return "Resposta do comando";
    }
}
```

2. Registre no `Program.cs`:

```csharp
builder.Services.AddTransient<ICommand, MeuComando>();
```

Pronto — o comando já aparece no `/help` automaticamente e já é roteado pelo `CommandDispatcher`, sem precisar tocar em nenhum outro arquivo.

### Configuração

O token do bot fica em `appsettings.json` (não versionar com valores reais) ou, preferencialmente, em `user-secrets` durante o desenvolvimento:

```bash
dotnet user-secrets init
dotnet user-secrets set "Telegram:Token" "SEU_TOKEN_AQUI"
```

### Stack

- .NET / `Microsoft.Extensions.Hosting`
- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- Injeção de dependência nativa (`Microsoft.Extensions.DependencyInjection`)

</details>

<details open>
<summary><b>🇺🇸 English</b></summary>

## CPASMS — Telegram Bot Architecture Template

A base project (template) for Telegram bots in **.NET**, using `Microsoft.Extensions.Hosting` and dependency injection. The core idea is to keep each bot command isolated in its own class, instead of coupling everything to a central `switch`.

### How it works

- **`ICommand`** defines a command's contract: a `Trigger` (e.g. `/help`) and an `Execute` method.
- Each command (`HelpCommand`, `RecargaCommand`, etc.) is an independent class implementing `ICommand`.
- **`CommandDispatcher`** receives all `ICommand` instances registered via DI (`IEnumerable<ICommand>`) and routes incoming messages to the right command based on its `Trigger`.
- **`BotHostedService`** is the `IHostedService` that connects to Telegram, listens for messages, and delegates them to the `CommandDispatcher`.

### Adding a new command

1. Create a class under `Commands/` implementing `ICommand`:

```csharp
public class MyCommand : ICommand
{
    public string Trigger => "/mycommand";
    public string Description => "Short description of what it does";

    public string Execute(string messageText)
    {
        return "Command response";
    }
}
```

2. Register it in `Program.cs`:

```csharp
builder.Services.AddTransient<ICommand, MyCommand>();
```

That's it — the command automatically shows up in `/help` and gets routed by `CommandDispatcher`, with no other file to touch.

### Configuration

The bot token lives in `appsettings.json` (don't commit real values) or, preferably, in `user-secrets` during development:

```bash
dotnet user-secrets init
dotnet user-secrets set "Telegram:Token" "YOUR_TOKEN_HERE"
```

### Stack

- .NET / `Microsoft.Extensions.Hosting`
- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- Native dependency injection (`Microsoft.Extensions.DependencyInjection`)

</details>
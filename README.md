# Telegram Bot Architecture Template

<details>
<summary><b>🇧🇷 Português</b></summary>

## Template de Bot para Telegram

Template pronto para construir bots de Telegram em **.NET**, usando `Microsoft.Extensions.Hosting` e injeção de dependência. Cada comando do bot fica isolado em sua própria classe, e novos comandos são adicionados sem precisar editar lógica de roteamento já existente.

### Como funciona

- **`ICommand`** define o contrato de um comando: um `Trigger` (ex: `/start`, `/ajuda`) e um método `Execute` que retorna a resposta do bot.
- **`CommandDispatcher`** recebe todos os comandos registrados via DI e roteia cada mensagem recebida para o comando correspondente, com base no `Trigger`.
- **`BotHostedService`** é o `IHostedService` responsável por conectar no Telegram, escutar mensagens recebidas e delegar o processamento para o `CommandDispatcher`.
- Já vem incluído um **`TestCommand`** (`/test`) de exemplo, só para confirmar que o bot está rodando e respondendo corretamente.

### Como começar a construir o seu bot

1. **Clone ou use este template** para criar seu próprio repositório.
2. **Configure o token do bot** (veja a seção [Configuração](#configuração) abaixo).
3. **Rode o projeto** e teste enviando `/test` para o bot no Telegram — se ele responder, está tudo certo.
4. **Crie seus próprios comandos** seguindo o passo a passo abaixo.
5. **Remova o `TestCommand`** quando não precisar mais dele (ou deixe como comando de diagnóstico).

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

2. Registre no `Program.cs`, dentro da região `Commands`:

```csharp
builder.Services.AddTransient<ICommand, MeuComando>();
```

Pronto — o comando já é reconhecido automaticamente pelo `CommandDispatcher`, sem precisar alterar nenhum outro arquivo.

### Configuração

O token do bot fica em `appsettings.json` (não recomendado versionar com valores reais) ou, preferencialmente, em `user-secrets` durante o desenvolvimento:

```bash
dotnet user-secrets init
dotnet user-secrets set "Telegram:Token" "SEU_TOKEN_AQUI"
```

Você pode gerar um token novo conversando com o [@BotFather](https://t.me/BotFather) no Telegram.

### Stack

- .NET / `Microsoft.Extensions.Hosting`
- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- Injeção de dependência nativa (`Microsoft.Extensions.DependencyInjection`)

</details>

<details open>
<summary><b>🇺🇸 English</b></summary>

## Telegram Bot Architecture Template

A ready-to-use template for building Telegram bots in **.NET**, using `Microsoft.Extensions.Hosting` and dependency injection. Each bot command lives in its own isolated class, and new commands are added without touching any existing routing logic.

### How it works

- **`ICommand`** defines a command's contract: a `Trigger` (e.g. `/start`, `/help`) and an `Execute` method that returns the bot's response.
- **`CommandDispatcher`** receives all commands registered via DI and routes each incoming message to the matching command, based on its `Trigger`.
- **`BotHostedService`** is the `IHostedService` responsible for connecting to Telegram, listening for incoming messages, and delegating processing to the `CommandDispatcher`.
- A sample **`TestCommand`** (`/test`) is included out of the box, just to confirm the bot is running and responding correctly.

### Getting started with your bot

1. **Clone or use this template** to create your own repository.
2. **Set up your bot token** (see the [Configuration](#configuration) section below).
3. **Run the project** and test it by sending `/test` to your bot on Telegram — if it replies, you're good to go.
4. **Create your own commands** following the steps below.
5. **Remove the `TestCommand`** once you no longer need it (or keep it as a diagnostic command).

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

2. Register it in `Program.cs`, inside the `Commands` region:

```csharp
builder.Services.AddTransient<ICommand, MyCommand>();
```

That's it — the command is automatically recognized by the `CommandDispatcher`, with no other file to touch.

### Configuration

The bot token lives in `appsettings.json` (not recommended to commit real values) or, preferably, in `user-secrets` during development:

```bash
dotnet user-secrets init
dotnet user-secrets set "Telegram:Token" "YOUR_TOKEN_HERE"
```

You can generate a new token by talking to [@BotFather](https://t.me/BotFather) on Telegram.

### Stack

- .NET / `Microsoft.Extensions.Hosting`
- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- Native dependency injection (`Microsoft.Extensions.DependencyInjection`)

</details>

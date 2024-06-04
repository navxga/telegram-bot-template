using BotTelegram.Interfaces;
using Newtonsoft.Json.Linq;
using static Program;

namespace BotTelegram.Commands
{
    /// <summary>
    /// 💬/ObterLogins ➡ Obtém todos os logins atuais configurados em produção.
    /// </summary>
    public class ObterLoginsCommand : ICommand
    {
        public string Execute(string message)
        {
            string[] caminhos =
            {
                _caminhoAutenticacaoP3,
                _caminhoAutenticacaoAutorizador,
                _caminhoAutenticacaoFrontEnd
            };

            string mensagemRetorno = "📌 Lista de todos os logins:\n\n";

            foreach (var caminho in caminhos)
            {
                var file = new FileInfo(caminho);
                var jsonLogin = JObject.Parse(File.ReadAllText(caminho));

                string nomeRobo = file.Directory.Name;
                string usuario = jsonLogin["Usuario"].ToString();
                string senha = jsonLogin["Senha"].ToString();
                bool isAtivo = bool.Parse(jsonLogin["IsAtivo"].ToString());

                mensagemRetorno += $"🤖 Robô: {nomeRobo}\n" +
                                   $"- Usuário: {usuario}\n" +
                                   $"- Senha: {senha}\n" +
                                   $"- Ativo: {(isAtivo ? "✅" : "❌")}\n\n";
            }

            return mensagemRetorno;
        }
    }
}

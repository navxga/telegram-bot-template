using BotTelegram.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Program;
using BotTelegram.Enums;

namespace BotTelegram.Commands
{
    /// <summary>
    /// 💬/AtualizarSenha NomeRobo Usuario Senha ➡ Altera a senha do robô conforme os dados informados nos parâmetros.
    /// </summary>
    public class AtualizarSenhaCommand : ICommand
    {
        public string Execute(string message)
        {
            if (message.Split(" ").Count() != 4)
            {
                throw new Exception("❌ O comando foi digitado em um formato incorreto!\n\n" +
                                    "Formato correto: /AtualizarSenha NomeRobo Usuario Senha");
            }

            string nomeRobo = message.Split(" ")[1].Trim();
            string usuario = message.Split(" ")[2].Trim();
            string senha = message.Split(" ")[3].Trim();

            RoboEnum robo = ObterRobo(nomeRobo);

            AtualizarSenha(robo, usuario, senha);

            return "✅ Login alterado com sucesso!\n\n" +
                  $"Novo usuário: {usuario}\n" +
                  $"Nova senha: {senha}\n";
        }

        private void AtualizarSenha(RoboEnum robo, string usuario, string senha)
        {
            string caminhoAutenticacao = ObterCaminho(robo);

            var jsonObj = JObject.Parse(File.ReadAllText(caminhoAutenticacao));

            jsonObj["Usuario"] = usuario;
            jsonObj["Senha"] = senha;
            jsonObj["IsAtivo"] = true;

            if (robo == RoboEnum.P3)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"http://172.16.16.167:3008/p3encrypt/{usuario}/{senha}";

                    var result = JsonConvert.DeserializeObject<JObject>(client.GetAsync(url).Result.Content.ReadAsStringAsync().Result);

                    jsonObj["UsuarioCrypto"] = result["user"].ToString();
                    jsonObj["SenhaCrypto"] = result["passwd"].ToString();
                }
            }

            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

            File.WriteAllText(caminhoAutenticacao, output);
        }

        private RoboEnum ObterRobo(string robo)
        {
            switch (robo.ToUpper())
            {
                case "P3":
                    return RoboEnum.P3;
                case "AUTORIZADOR":
                    return RoboEnum.Autorizador;
                case "FRONTEND":
                    return RoboEnum.FrontEnd;
                default:
                    throw new Exception("❌ Nenhum robô identificado");
            }
        }

        private string ObterCaminho(RoboEnum nomeRobo)
        {
            switch (nomeRobo)
            {
                case RoboEnum.P3:
                    return _caminhoAutenticacaoP3;
                case RoboEnum.Autorizador:
                    return _caminhoAutenticacaoAutorizador;
                case RoboEnum.FrontEnd:
                    return _caminhoAutenticacaoFrontEnd;
                default:
                    throw new Exception("❌ Nenhum robô identificado");
            }
        }
    }
}

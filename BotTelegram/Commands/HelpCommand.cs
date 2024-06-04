using BotTelegram.Interfaces;
using System.Reflection;
using System.Xml;

namespace BotTelegram.Commands
{
    public class HelpCommand : ICommand
    {
        public string Execute(string message)
        {
            string mensagemRetorno = "📌 Central de Ajuda - Bot Cetelem BPO!\n\n" +
                                    $"Lista de comandos:\n\n";

            var classesCommand = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.Name != "HelpCommand");

            foreach (var classe in classesCommand)
            {
                string summary = GetXmlSummary(classe);

                mensagemRetorno += summary + "\n\n";
            }

            return mensagemRetorno;
        }

        private string GetXmlSummary(Type type)
        {
            string assemblyLocation = type.Assembly.Location;
            string xmlPath = Path.ChangeExtension(assemblyLocation, ".xml");

            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("O arquivo de documentação XML não foi encontrado.", xmlPath);
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlPath);

            var memberName = $"T:{type.FullName}";
            var summaryNode = xmlDocument.SelectSingleNode($"/doc/members/member[@name='{memberName}']/summary");

            return summaryNode?.InnerText.Trim();
        }
    }
}

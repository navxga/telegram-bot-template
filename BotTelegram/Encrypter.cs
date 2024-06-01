using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace BotTelegram
{
    public class Encrypter
    {
        public static string Encriptografar(string text)
        {
            // Parâmetros de criptografia
            var key = Encoding.UTF8.GetBytes("AMINHAKEYTEM32NYTES1234567891234");
            var iv = Encoding.UTF8.GetBytes("7061737323313233");

            // Configuração do AES com CBC e PKCS7
            var aes = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()));
            aes.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

            var inputBytes = Encoding.UTF8.GetBytes(text);
            var outputBytes = new byte[aes.GetOutputSize(inputBytes.Length)];

            int length = aes.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
            aes.DoFinal(outputBytes, length);

            // Convertendo para Base64
            return Convert.ToBase64String(outputBytes);
        }
    }
}

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UserCRUDApi.Domain.Security
{
    public sealed class Cryptography
    {
        #region -- IV --
        /// <summary>
        /// Vetor de bytes utilizados para a criptografia (Chave Externa)
        /// </summary>  
        private static byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 }; 
        #endregion

        #region -- cryptoKey --
        /// <summary>
        /// Representação de valor em base 64 (Chave Interna)
        /// </summary>
        private static string cryptoKey = Settings.SecretKeyBase64;
        // O Valor acima representa a transformação para base64 de
        // um conjunto de 32 caracteres (8 * 32 = 256bits)
        // A chave é: "Criptografias com Rijndael / AES" 
        #endregion

        #region -- Encrypt --
        /// <summary>
        /// Metodo de criptografia de valor
        /// </summary>
        /// <param name="text">valor a ser criptografado</param>
        /// <returns>valor criptografado</returns>
        public static string Encrypt(string text)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves
                    byte[] bText, bKey;
                    bKey = Convert.FromBase64String(cryptoKey);
                    bText = new UTF8Encoding().GetBytes(text);

                    // Instancia a classe de criptografia Rijndael
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"
                    // Lembre-se: chaves possíves:
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor criptografado:
                    MemoryStream mStream = new MemoryStream();

                    // Instancia o encriptador 
                    CryptoStream encryptor = new CryptoStream(mStream, rijndael.CreateEncryptor(bKey, IV), CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória
                    encryptor.Write(bText, 0, bText.Length);

                    // Despeja toda a memória.
                    encryptor.FlushFinalBlock();

                    // Pega o vetor de bytes da memória e gera a string criptografada
                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        } 
        #endregion

        #region -- Decrypt --
        /// <summary>
        /// Metodo de descriptografia
        /// </summary>
        /// <param name="text">texto criptografado</param>
        /// <returns>valor descriptografado</returns>
        public static string Decrypt(string text)
        {

            try
            {
                // Se a string não está vazia, executa a criptografia

                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves

                    byte[] bText, bKey;

                    // codificação de 64 bits não funciona bem com os espaços vazio por algum motivo estranho. Eu encontrei a seguinte Solução Temporariamente. "Paulo Roberto"
                    text  = text.Replace(" ", "+");

                    bKey = Convert.FromBase64String(cryptoKey);
                    bText = Convert.FromBase64String(text);

                    // Instancia a classe de criptografia Rijndael
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"
                    // Lembre-se: chaves possíves:
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor DEScriptografado:
                    MemoryStream mStream = new MemoryStream();

                    // Instancia o Decriptador 
                    CryptoStream decryptor = new CryptoStream(mStream, rijndael.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória
                    decryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.
                    decryptor.FlushFinalBlock();

                    // Instancia a classe de codificação para que a string venha de forma correta
                    UTF8Encoding utf8 = new UTF8Encoding();

                    // Com o vetor de bytes da memória, gera a string descritografada em UTF8
                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo
                    return null;
                }
            }

            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        } 
        #endregion

        #region -- Ecrypt Hash MD5 --
        /// <summary>
        /// Criptografa o valor para o tipo de criptografia MD5
        /// </summary>
        /// <param name="text">valor a ser criptografado</param>
        /// <returns>retorna o valor criptografado em hexadecimal</returns>
        public static string EncryptMD5(string text)
        {
            MD5 md5Hash = MD5.Create();

            //valor cryptografando
            byte[] crypt = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sb = new StringBuilder();

            //coverte para hexadecimal
            foreach (var b in crypt)
            {
                sb.Append(b.ToString("x2"));
            }
            
            return sb.ToString();
        }
        #endregion
    }
}

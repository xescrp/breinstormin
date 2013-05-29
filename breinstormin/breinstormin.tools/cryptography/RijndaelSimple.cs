
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace breinstormin.tools.cryptograpthy
{
    public class RijndaelSimple
    {
        internal const string cookiepassphrase = "Coo5ki@se";
        public const string passphrase = "Pas5pr@se";
        public const string saltvalue = "s@1tValue";
        public const string hashalgorithm = "SHA1";
        public const int passworditerations = 2;
        public const string initvector = "@1B2c3D4e5F6g7H8";
        public const int keysize = 256;

        /// <summary>
        /// Encripta texto usando algoritmo Rijndael de clave simetrica. Devuelve resultado codificado en base64
        /// </summary>
        /// <param name="plainText">
        /// Texto a encriptar
        /// </param>
        /// <param name="passPhrase">
        /// Frase de paso (de donde un pseudo-aleatorio password sera derivado.
        /// Este se usara para generararl a clave de encriptacion
        /// </param>
        /// <param name="saltValue">
        /// Se usara con el parametro passphrase para generar password.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Nombre del algoritmo hash a utilizar: "MD5" o "SHA"
        /// </param>
        /// <param name="passwordIterations">
        /// Numero de iteraciones para generar el password.
        /// </param>
        /// <param name="initVector">
        /// Vector de inicializacion. Este valor se requiera para encriptar el 
        /// primer bloque de datos del texto a encriptar. Requiere 16 caracteres ASCII
        /// </param>
        /// <param name="keySize">
        /// Tamaño de la clave de encriptacion en bits. Valores: 128, 192, y 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Valor encriptado como una cadena codificada en base64.
        /// </returns>
        public static string Encrypt(string plainText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convertir cadenas de texto en arrays de bytes.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convertir cadena a encriptar en array de bytes.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            //Primero, crearemos el password, para generar la clave.
            //Este password se genera desde el parametro passPhrase y saltValue y 
            //usando el metodo de hash especificado y el numero de iteraciones

            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);


            //Usamos el password para generar bytes pseudo-aleatorios para la encriptacion
            byte[] keyBytes = password.GetBytes(keySize / 8);


            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;


            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        /// <summary>
        /// Desencripta texto en Base64 usando algoritmo Rijndael de clave simetrica. Devuelve resultado en cadena de texto
        /// </summary>
        /// <param name="cipherText">
        /// Texto cifrado en formato Base64.
        /// </param>
        /// <param name="passPhrase">
        /// Frase de paso (de donde un pseudo-aleatorio password sera derivado.
        /// Este se usara para generararl a clave de encriptacion
        /// </param>
        /// <param name="saltValue">
        /// Se usara con el parametro passphrase para generar password.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Nombre del algoritmo hash a utilizar: "MD5" o "SHA"
        /// </param>
        /// <param name="passwordIterations">
        /// Numero de iteraciones para generar el password.
        /// </param>
        /// <param name="initVector">
        /// Vector de inicializacion. Este valor se requiera para encriptar el 
        /// primer bloque de datos del texto a encriptar. Requiere 16 caracteres ASCII
        /// </param>
        /// <param name="keySize">
        /// Tamaño de la clave de encriptacion en bits. Valores: 128, 192, y 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Valor desencriptado en cadena de texto.
        /// </returns>
        public static string Decrypt(string cipherText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convertir cadenas de texto en arrays de bytes.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // convetir texto cifrado en un byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            //Generar password
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);


            byte[] keyBytes = password.GetBytes(keySize / 8);


            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);
            return plainText;
        }
    }


    class RijndaelSimpleTest
    {


        static void Test1()
        {
            string plainText = "559038395";    // texto original

            string passPhrase = "Pas5pr@se";        // puede ser cualquier cadena de texto
            string saltValue = "s@1tValue";        // puede ser cualquier cadena de texto
            string hashAlgorithm = "SHA1";             // MD5 o SHA1
            int passwordIterations = 2;                  // cualquier numero, con 1 o 2 es suficiente
            string initVector = "@1B2c3D4e5F6g7H8"; // deben ser 16 bits
            int keySize = 256;                // puede ser 256, 192 o 128

            Console.WriteLine(String.Format("Cadena a encriptar : {0}", plainText));

            string cipherText = RijndaelSimple.Encrypt(plainText,
                                                        passPhrase,
                                                        saltValue,
                                                        hashAlgorithm,
                                                        passwordIterations,
                                                        initVector,
                                                        keySize);

            Console.WriteLine(String.Format("Cadena encriptada : {0}", cipherText));

            plainText = RijndaelSimple.Decrypt(cipherText,
                                                        passPhrase,
                                                        saltValue,
                                                        hashAlgorithm,
                                                        passwordIterations,
                                                        initVector,
                                                        keySize);

            Console.WriteLine(String.Format("Cadena desencriptada : {0}", plainText));
        }
    }
}


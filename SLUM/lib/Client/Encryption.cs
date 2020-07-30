using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace SLUM.lib.Client
{
    public static class Encryption
    {
        public static RSA RSAHandle;
        public static byte[] PublicKey;
        //public static byte[] PrivateKey;
        public static RsaKeyParameters KeyParams;
        //public static KeyParameter SecretKeyParams;
        public static Random GetRandom;

        static Encryption()
        {
            //RSAHandle = new RSACryptoServiceProvider(1024);
            // RSAHandle = CipherUtilities.GetCipher("AES/CFB8/NoPadding");

            RSAHandle = RSA.Create(2048); // my rsa to decrypt and encrypt data

            var rsaPArams = RSAHandle.ExportParameters(false);

            KeyParams = new RsaKeyParameters(
                false,
                new BigInteger(1, rsaPArams.Modulus),
                new BigInteger(1, rsaPArams.Exponent));

            /*SecretKeyParams = new KeyParameter(RSAHandle.ExportPkcs8PrivateKey());
              /*  true,
                new BigInteger(1, rsaPArams.Modulus),
                new BigInteger(1, rsaPArams.Exponent));*/

            PublicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(KeyParams).GetDerEncoded(); // my exported public key in DER format
           // PrivateKey = RSAHandle.ExportRSAPrivateKey();

            //RSAHandle = System.Security.Cryptography.RSA.Create();
            //RSAHandle.KeySize = 1024;
            GetRandom = new Random();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalChat
{
    class CrypterRSA
    {
        private System.Security.Cryptography.RSACryptoServiceProvider rsa;
        private System.Security.Cryptography.RSAParameters privatKey;
        private byte[] publicKey;
        public bool isPrivate;

        public CrypterRSA()
        {
            rsa = new System.Security.Cryptography.RSACryptoServiceProvider(1024);
            privatKey = rsa.ExportParameters(true);
            isPrivate = true;
        }

        public CrypterRSA(byte[] pubKey)
        {
            rsa = new System.Security.Cryptography.RSACryptoServiceProvider(1024);
            publicKey = pubKey;
            isPrivate = false;
        }

        public byte[] Coder(string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            return this.Coder(buffer);
        }

        public byte[] Coder(byte[] input)
        {
            byte[] coder = new byte[] { };
            int k = rsa.KeySize / 8;
            byte[] buffer = new byte[k];
            rsa.ImportCspBlob(publicKey);
            
            while (input.Length > 0)
            {
                if (input.Length > k)
                {
                    buffer = Program.SubBytes(input, 0, k);
                    input = Program.SubBytes(input, k + 1);
                }
                else
                {
                    buffer = Program.SubBytes(input, 0);
                    input = new byte[] { };
                }
                coder = Program.ConcatByte(coder, rsa.Encrypt(buffer, false));
            }

            return coder;
        }

        public byte[] Decoder(byte[] input)
        {
            byte[] decoder = new byte[]{};
            int k = rsa.KeySize / 8;
            byte[] buffer = new byte[k];

            rsa.ImportParameters(privatKey);
            while (input.Length > 0)
            {
                if (input.Length > k)
                {
                    buffer = Program.SubBytes(input, 0, k);
                    input = Program.SubBytes(input, k + 1);
                }
                else
                {
                    buffer = Program.SubBytes(input, 0);
                    input = new byte[]{};
                }
                decoder = Program.ConcatByte(decoder, rsa.Decrypt(buffer, false));
            }

            return decoder;
        }

        public byte[] getPublicKey()
        {
            return this.rsa.ExportCspBlob(false);
        }

        
    }
}

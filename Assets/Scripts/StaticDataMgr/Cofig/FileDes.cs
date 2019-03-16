using System.Collections;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace StaticDataTool
{
    //对数据文件进行加密

    public class FileDes
    {
        private static byte[] sKey = { 0x21, 0x4A, 0x95, 0x78, 0xA0, 0xAB, 0xCD, 0xEF };
        private static byte[] sIV = { 0x12, 0x3A, 0x20, 0xA8, 0x9C, 0x83, 0x56, 0xDA };
        private static CryptoStream cStream = null;

        //----------------------------------------------------------------------
        //						将文件加密到文件
        //----------------------------------------------------------------------

        public static void EncryptFile(string inFileName, string outFileName, bool deleteSource)
        {
            //创建输入文件流
            FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);

            if (File.Exists(outFileName))
            {// 如果输出文件已经存在，那么删除掉.
                File.Delete(outFileName);
            }
            //创建输出文件流
            FileStream fout = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
            fout.SetLength(0);

            //控制读写的中间变量
            byte[] bin = new byte[256]; //读取buffer
            long rdlen = 0;              //已读取总数
            long totlen = fin.Length;    //输入文件的总大小
            int len;                     //本次需要写入的长度

            //创建DES加密服务对象
            DES des = new DESCryptoServiceProvider();
            //加密流对象
            CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(sKey, sIV), CryptoStreamMode.Write);

            //从输入文件读取然后写入到加密流
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 256);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }
            //释放流对象
            encStream.Close();
            fout.Close();
            fin.Close();
            //是否需要删除原始文件
            if (deleteSource == true)
                File.Delete(inFileName);
        }

        //----------------------------------------------------------------------
        //						从加密文件创建解密后的流
        //----------------------------------------------------------------------
        public static Stream DecryptFileToStream(string inFile)
        {
            try
            {
                // 打开输入文件（加密）
                FileStream fStream = File.OpenRead(inFile);
                // 如果解密流没有释放，释放解密流
                if (cStream != null)
                {
                    cStream.Close();
                    cStream = null;
                }
                //创建新的解密流对象
                cStream = new CryptoStream(fStream,
                                                        new DESCryptoServiceProvider().CreateDecryptor(sKey, sIV),
                                                        CryptoStreamMode.Read);
                //把加密流读到不加密的流里
                List<byte> byteList = new List<byte>();
                try
                {
                    while (true)
                    {
                        int value = cStream.ReadByte();
                        if (value == -1)
                            break;
                        else
                            byteList.Add((byte)value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                var data = byteList.ToArray();

                MemoryStream outStream = new MemoryStream();

                outStream.Write(data, 0, data.Length);
                outStream.Seek(0, SeekOrigin.Begin); 
                return outStream;
            }
            catch (CryptographicException e)
            {
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                return null;
            }
        }

        //----------------------------------------------------------------------
        //						从加密字节数组中创建解密后的流
        //----------------------------------------------------------------------
        public static Stream DecryptDataToStream(byte[] inData)
        {
            try
            {
                // 如果解密流没有释放，释放解密流
                if (cStream != null)
                {
                    cStream.Close();
                    cStream = null;
                }
                MemoryStream inStream = new MemoryStream(inData);
                //创建新的解密流对象
                cStream = new CryptoStream(inStream,
                                           new DESCryptoServiceProvider().CreateDecryptor(sKey, sIV),
                                           CryptoStreamMode.Read);
                //把加密流读到不加密的流里
                List<byte> byteList = new List<byte>();
                try
                {
                    while (true)
                    {
                        int value = cStream.ReadByte();
                        if (value == -1)
                            break;
                        else
                            byteList.Add((byte)value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                var data = byteList.ToArray();

                MemoryStream outStream = new MemoryStream();

                outStream.Write(data, 0, data.Length);
                outStream.Seek(0, SeekOrigin.Begin); 
                return outStream;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: " + e.ToString());
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: " + e.ToString());
                return null;
            }
        }

        //----------------------------------------------------------------------
        //		释放解密流对象（使用完后需要手动调用）
        //----------------------------------------------------------------------
        public static void CloseStream()
        {
            if (cStream != null)
            {
                cStream.Close();
                cStream = null;
            }
        }
    }
}
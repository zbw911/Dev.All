//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;

//namespace Dev.Comm.Utils
//{
//    public class MD5Helper
//    {

//        static MD5 m_MD5;
//        static MD5Helper()
//        {
//            m_MD5 = MD5.Create();
//        }
//        static readonly int ComputePageSize = 8 * 1024 * 1024;
//        /// <summary>
//        /// 对文件计算MD5 支持大文件 定义为ComputePageSize为一个计算块
//        /// </summary>
//        /// <param name="largebytes"></param>
//        /// <returns></returns>
//        public static byte[] ComputeFileFastMD5(byte[] largebytes)
//        {
//            m_MD5.Initialize();
//            if (largebytes.Length <= ComputePageSize)
//            {
//                return m_MD5.ComputeHash(largebytes);
//            }
//            else
//            {
//                int offset = 0;
//                while (largebytes.Length - offset > 0)
//                {
//                    int len = largebytes.Length - offset;
//                    if (len > ComputePageSize)
//                    {
//                        len = ComputePageSize;
//                        byte[] buf = new byte[len];
//                        System.Buffer.BlockCopy(largebytes, offset, buf, 0, len);
//                        m_MD5.TransformBlock(buf, 0, buf.Length, buf, 0);
//                        //m_MD5.TransformBlock(largebytes, offset, len, largebytes, offset);
//                    }
//                    else
//                    {
//                        m_MD5.TransformFinalBlock(largebytes, offset, len);
//                    }
//                    offset += len;
//                }
//                return m_MD5.Hash;
//            }
//        }
//        public static byte[] ComputeFileFastMD5(Stream sm)
//        {
//            m_MD5.Initialize();
//            return m_MD5.ComputeHash(sm);
//        }
//        /// <summary>
//        /// 快速比较2进制数据是否相同
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="target"></param>
//        /// <returns></returns>
//        public static bool IsEqual(byte[] source, byte[] target)
//        {
//            if (source == null && target == null) return true;
//            if (source == null || target == null) return false;
//            if (source.Length != target.Length) return false;
//            byte[] md5src = ComputeFileFastMD5(source);
//            byte[] md5target = ComputeFileFastMD5(target);
//            for (int i = 0; i < md5src.Length; i++)
//            {
//                if (md5src[i] != target[i])
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//        public static string ByteToHexStr(byte[] md5)
//        {
//            StringBuilder sb = new StringBuilder(32);
//            for (int i = 0; i < md5.Length; i++)
//            {
//                sb.Append(md5[i].ToString("X2"));
//            }
//            return sb.ToString();
//        }
//        internal static void Destory()
//        {
//            m_MD5.Clear();
//            m_MD5 = null;
//        }



//    }


//    internal static class MD5Compute
//    {
//        static MD5 m_MD5;
//        static MD5Compute()
//        {
//            m_MD5 = MD5.Create();
//        }
//        static readonly int ComputePageSize = 8 * 1024 * 1024;
//        /// <summary>
//        /// 对文件计算MD5 支持大文件 定义为ComputePageSize为一个计算块
//        /// </summary>
//        /// <param name="largebytes"></param>
//        /// <returns></returns>
//        internal static byte[] ComputeFileFastMD5(byte[] largebytes)
//        {
//            m_MD5.Initialize();
//            if (largebytes.Length <= ComputePageSize)
//            {
//                return m_MD5.ComputeHash(largebytes);
//            }
//            else
//            {
//                int offset = 0;
//                while (largebytes.Length - offset > 0)
//                {
//                    int len = largebytes.Length - offset;
//                    if (len > ComputePageSize)
//                    {
//                        len = ComputePageSize;
//                        byte[] buf = new byte[len];
//                        Buffer.BlockCopy(largebytes, offset, buf, 0, len);
//                        m_MD5.TransformBlock(buf, 0, buf.Length, buf, 0);
//                        //m_MD5.TransformBlock(largebytes, offset, len, largebytes, offset);
//                    }
//                    else
//                    {
//                        m_MD5.TransformFinalBlock(largebytes, offset, len);
//                    }
//                    offset += len;
//                }
//                return m_MD5.Hash;
//            }
//        }
//        internal static byte[] ComputeFileMD5(string filename)
//        {
//            m_MD5.Initialize();
//            byte[] md5 = null;
//            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
//            {
//                md5 = m_MD5.ComputeHash(fs);
//            }
//            return md5;
//        }
//        internal static byte[] ComputeFileFastMD5(Stream sm)
//        {
//            m_MD5.Initialize();
//            return m_MD5.ComputeHash(sm);
//        }
//        internal static byte[] ComputeFileFastMD5(Stream sm, int start, int count)
//        {
//            m_MD5.Initialize();
//            //来小范围读取计算 增加偏移
//            if (count <= ComputePageSize)
//            {
//                System.Diagnostics.Debug.Assert(start + count <= sm.Length);
//                byte[] buf = null;
//                using (BinaryReader br = new BinaryReader(sm))
//                {
//                    sm.Position = start;
//                    buf = br.ReadBytes(count);
//                    buf = m_MD5.ComputeHash(buf);
//                }
//                return buf;
//            }
//            else
//            {
//                using (BinaryReader br = new BinaryReader(sm))
//                {
//                    sm.Position = start;
//                    int offset = 0;
//                    while (count - offset > 0)
//                    {
//                        int len = count - offset;
//                        if (len > ComputePageSize)
//                        {
//                            len = ComputePageSize;
//                            byte[] buf = br.ReadBytes(len);
//                            //Buffer.BlockCopy(largebytes, offset, buf, 0, len);
//                            m_MD5.TransformBlock(buf, 0, buf.Length, buf, 0);
//                            //m_MD5.TransformBlock(largebytes, offset, len, largebytes, offset);
//                        }
//                        else
//                        {
//                            byte[] buf = br.ReadBytes(len);
//                            m_MD5.TransformFinalBlock(buf, 0, buf.Length);
//                        }
//                        offset += len;
//                    }
//                }
//                return m_MD5.Hash;
//            }
//            //return m_MD5.ComputeHash(sm,start, count);
//        }
//        internal static string ByteToHexStr(byte[] md5)
//        {
//            StringBuilder sb = new StringBuilder(32);
//            for (int i = 0; i < md5.Length; i++)
//            {
//                sb.Append(md5[i].ToString("X2"));
//            }
//            return sb.ToString();
//        }
//        internal static void Destory()
//        {
//            m_MD5.Clear();
//            m_MD5 = null;
//        }
//    }

//}

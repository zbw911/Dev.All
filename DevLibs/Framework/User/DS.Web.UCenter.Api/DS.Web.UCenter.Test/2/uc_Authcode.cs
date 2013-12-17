using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Commons
{
    public class uc_Authcode
    {
        public enum DiscuzAuthcodeMode { Encode, Decode };


        /// <summary>
        /// ���ַ�����ָ��λ�ý�ȡָ�����ȵ����ַ���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <param name="length">���ַ����ĳ���</param>
        /// <returns>���ַ���</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// ���ַ�����ָ��λ�ÿ�ʼ��ȡ���ַ�����β���˷���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <returns>���ַ���</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }


        /// <summary>
        /// �����ļ��Ƿ����
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <returns>�Ƿ����</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="str">ԭʼ�ַ���</param>
        /// <returns>MD5���</returns>
        public static string MD5(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }

        /// <summary>
        /// ���ַ������� base64 ����
        /// </summary>
        /// <param name="str">ԭʼ�ִ�</param>
        /// <returns>���</returns>
        public static string Base64Encode(string str)
        {
            try
            {
                byte[] bytes_1 = System.Text.Encoding.Default.GetBytes(str);
                return System.Convert.ToBase64String(bytes_1);
            }
            catch
            {
                return "";
            }

        }

        public static string Base64Decode(string str)
        {
            try
            {
                byte[] bytes_2 = Convert.FromBase64String(str);
                return System.Text.Encoding.Default.GetString(bytes_2);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// �ֶδ��Ƿ�ΪNull��Ϊ""(��)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            //#if NET1
            if (str == null || str.Trim() == "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ���� RC4 ��������
        /// </summary>
        /// <param name="pass">�����ִ�</param>
        /// <param name="kLen">��Կ���ȣ�һ��Ϊ 256</param>
        /// <returns></returns>
        static private Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            Byte[] mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }

        /// <summary>
        /// ��������ַ�
        /// </summary>
        /// <param name="lens">����ַ�����</param>
        /// <returns>����ַ�</returns>
        public static string RandomString(int lens)
        {
            char[] CharArray = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int clens = CharArray.Length;
            string sCode = "";
            Random random = new Random();
            for (int i = 0; i < lens; i++)
            {
                sCode += CharArray[random.Next(clens)];
            }
            return sCode;
        }

        /// <summary>
        /// ʹ�� Discuz authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <param name="expiry">�����ִ���Чʱ�䣬��λ����</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeEncode(string source, string key, int expiry)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Encode, expiry);

        }

        /// <summary>
        /// ʹ�� Discuz authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeEncode(string source, string key)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Encode, 0);

        }

        /// <summary>
        /// ʹ�� Discuz authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeDecode(string source, string key)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Decode, 0);

        }

        /// <summary>
        /// ʹ�� ���ε� rc4 ���뷽�����ַ������м��ܻ��߽���
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <param name="operation">���� ���ܻ��ǽ���</param>
        /// <param name="expiry">�����ִ�����ʱ��</param>
        /// <returns>���ܻ��߽��ܺ���ַ���</returns>
        private static string DiscuzAuthcode(string source, string key, DiscuzAuthcodeMode operation, int expiry)
        {

            if (source == null || key == null)
            {
                return "";
            }

            int ckey_length = 4;
            string keya, keyb, keyc, cryptkey, result;
            string timestamp = UnixTimestamp();

            key = MD5(key);
            keya = MD5(CutString(key, 0, 16));
            keyb = MD5(CutString(key, 16, 16));
            keyc = ckey_length > 0 ? (operation == DiscuzAuthcodeMode.Decode ? CutString(source, 0, ckey_length) : RandomString(ckey_length)) : "";

            cryptkey = keya + MD5(keya + keyc);

            if (operation == DiscuzAuthcodeMode.Decode)
            {
                byte[] temp;
                try
                {
                    temp = System.Convert.FromBase64String(CutString(source, ckey_length));
                }
                catch
                {
                    try
                    {
                        temp = System.Convert.FromBase64String(CutString(source + "=", ckey_length));
                    }
                    catch
                    {
                        try
                        {
                            temp = System.Convert.FromBase64String(CutString(source + "==", ckey_length));
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }

                result = System.Text.Encoding.Default.GetString(RC4(temp, cryptkey));
                if (CutString(result, 10, 16) == CutString(MD5(CutString(result, 26) + keyb), 0, 16))
                {
                    return CutString(result, 26);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                source = "0000000000" + CutString(MD5(source + keyb), 0, 16) + source;
                byte[] temp = RC4(System.Text.Encoding.Default.GetBytes(source), cryptkey);
                return keyc + System.Convert.ToBase64String(temp);

            }

        }

        /// <summary>
        /// RC4 ԭʼ�㷨
        /// </summary>
        /// <param name="input">ԭʼ�ִ�����</param>
        /// <param name="pass">��Կ</param>
        /// <returns>�������ִ�����</returns>
        private static Byte[] RC4(Byte[] input, String pass)
        {
            if (input == null || pass == null) return null;

            Encoding enc_default = System.Text.Encoding.Default;

            byte[] output = new Byte[input.Length];
            byte[] mBox = GetKey(enc_default.GetBytes(pass), 256);

            // ����
            Int64 i = 0;
            Int64 j = 0;
            for (Int64 offset = 0; offset < input.Length; offset++)
            {
                i = (i + 1) % mBox.Length;
                j = (j + mBox[i]) % mBox.Length;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                Byte a = input[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] һ���� mBox.Length С������Ҫ��ȡģ
                Byte b = mBox[(mBox[i] + mBox[j]) % mBox.Length];
                output[offset] = (Byte)((Int32)a ^ (Int32)b);
            }

            return output;
        }


        public static string AscArr2Str(byte[] b)
        {
            return System.Text.UnicodeEncoding.Unicode.GetString(
             System.Text.ASCIIEncoding.Convert(System.Text.Encoding.ASCII,
             System.Text.Encoding.Unicode, b)
             );
        }

        public static string UnixTimestamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }
    }
}

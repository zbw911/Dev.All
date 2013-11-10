// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��06��07�� 14:25
//  
//  �޸��ڣ�2013��09��17�� 11:32
//  �ļ�����Dev.Libs/Dev.Comm.Core/StringUtil.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dev.Comm
{
    public class StringUtil
    {
        #region ���ַ���������Ч��HTMLת��

        /// <summary>
        ///   ���ַ���������Ч��HTMLת��
        /// </summary>
        /// <param name="inputString"> �ַ��� </param>
        /// <param name="maxLength"> ��೤�� </param>
        /// <returns> </returns>
        public static string InputText(string inputString, int maxLength)
        {
            // ����һ���ɱ��ַ��ַ���
            var retVal = new StringBuilder();
            // ����ַ����Ƿ�Ϊ��
            if ((inputString != null) && (inputString != String.Empty))
            {
                // ȥ���ո�
                inputString = inputString.Trim();

                // ȡ��󳤶ȣ�����Ľ�ȡ��
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                // ���ַ�ת��ΪHTML�ַ�
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                retVal.Replace("'", " ");
            }

            return retVal.ToString();
        }

        #endregion

        public static string GetJsonValue(string str, string key)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(key))
            {
                return "";
            }

            int pos = str.IndexOf(key);
            if (pos < 0)
            {
                return "";
            }

            string temp = str.Substring(pos + 1, str.Length - pos - 1);

            pos = temp.IndexOf(":");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            string value = temp.Substring(0, pos);

            return value;
        }

        public static string GetDoMain(string url, string key)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            if (string.IsNullOrEmpty(key))
            {
                return url;
            }

            int pos = url.IndexOf(key);

            if (pos < 0)
            {
                return url;
            }
            else
            {
                string rtn = url.Substring(0, pos - 1);
                return rtn;
            }
        }

        public static string inputHtml(string inputString)
        {
            string str = inputString;
            //����ʵ���е�ָ�� Unicode �ַ�������ƥ�����滻Ϊ����ָ���� Unicode �ַ���
            str = str.Replace("\r", "<br>");
            str = str.Replace(" ", "&nbsp;");
            return str;
        }

        public static string HtmlInputTex(string inputHtml)
        {
            string str = inputHtml;
            str = str.Replace("<br>", "\r");
            str = str.Replace("&nbsp;", " ");
            return str;
        }

        public static string GetPlainText(string inputText, int outNum)
        {
            try
            {
                string tempStr = inputText;
                int num1 = GetStringCount(inputText, "&nbsp;");
                num1 = num1 * 6;
                int num2 = GetStringCount(inputText, "<br>");
                num2 = num2 * 4;
                int numCount = inputText.Length - num1 - num2;

                int forNum = outNum;
                if (outNum > inputText.Length)
                    forNum = inputText.Length;
                if (numCount > forNum)
                {
                    string tempDescr = "";
                    for (int i = 0; i < forNum; i++)
                    {
                        inputText = tempStr.Substring(0, i + 1);
                        tempDescr = inputText.Substring(i, 1);
                        if (tempDescr == "&" || tempDescr == "n" || tempDescr == "b" || tempDescr == "s" ||
                            tempDescr == "p"
                            || tempDescr == ";" || tempDescr == "<" || tempDescr == "b" || tempDescr == "r" ||
                            tempDescr == ">")
                        {
                            forNum = forNum + 1;
                        }
                    }
                }
                return inputText + "...";
            }
            catch
            {
                return inputText = inputText + "..."; // "��ʽ���⣬�޷���ʾ";
            }
        }

        public static string GetJsonAndReturn(ref string temp, string str, string key)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(key))
            {
                return "";
            }

            int pos = str.IndexOf(key);
            if (pos < 0)
            {
                return "";
            }

            temp = str.Substring(pos + 1, str.Length - pos - 1);

            pos = temp.IndexOf(":");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            string value = temp.Substring(0, pos);

            return value;
        }


        /// <summary>
        ///   Returns a string with backslashes before characters that need to be quoted
        /// </summary>
        /// <param name="InputTxt"> Text string need to be escape with slashes </param>
        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            //try
            //{
            Result = Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            //}
            //catch (Exception Ex)
            //{
            //    // handle any exception here
            //    Console.WriteLine(Ex.Message);
            //}

            return Result;
        }

        /// <summary>
        ///   Un-quotes a quoted string
        /// </summary>
        /// <param name="InputTxt"> Text string need to be escape with slashes </param>
        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            //try
            //{
            Result = Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            //}
            //catch (Exception Ex)
            //{
            //    // handle any exception here
            //    Console.WriteLine(Ex.Message);
            //}

            return Result;
        }


        /// <summary>
        ///   ���������滻
        /// </summary>
        /// <param name="oldvalue"> </param>
        /// <param name="newvalue"> </param>
        /// <param name="content"> </param>
        /// <returns> </returns>
        public static string ReplaceBat(IList<string> oldvalue, IList<string> newvalue, string content)
        {
            //StringBuilder sb = new StringBuilder(content);

            for (int i = 0; i < oldvalue.Count; i++)
            {
                content = content.Replace(oldvalue[i], newvalue[i]);
                //sb = sb.Replace(dest[i], source[i]);
            }

            return content;
            //return sb.ToString();
        }


        /// <summary>
        ///   �÷ָ������Ӷ����
        /// </summary>
        /// <param name="strs"> </param>
        /// <param name="spliter"> </param>
        /// <returns> </returns>
        public static string ConcatStrs(string[] strs, string spliter = "")
        {
            var sb = new StringBuilder();

            foreach (var str in strs)
            {
                if (sb.Length > 0)
                {
                    sb.Append(spliter);
                }
                sb.Append(str);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   ���ֽ�����ȡ,��ȥ���������
        /// </summary>
        /// <param name="str"> �ַ��� </param>
        /// <param name="len"> ��ȡ���� </param>
        /// <returns> </returns>
        public static string CutGBStr(string str, int len, string dot = "")
        {
            if (string.IsNullOrEmpty(str)) return "";


            if (len >= Encoding.Default.GetByteCount(str))
                return str;


            string strRe = "";
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                strRe += str.Substring(i, 1);
                count += Encoding.Default.GetByteCount(str.Substring(i, 1));
                if (count >= len)
                    break;
            }
            //if(count>len)//��ȡ�ַ������������ǰ�����ģ�������İ��
            //strRe = strRe.Substring(0,strRe.Length - 1);
            return strRe + dot;
        }


        public static int GetGBStrLen(string str)
        {
            return Encoding.Default.GetByteCount(str);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        ///   ȥ��HTML���
        /// </summary>
        /// <param name="AHtml"> </param>
        /// <returns> </returns>
        public static string Strip_Tags(string AHtml)
        {
            if (AHtml == null)
                return null;
            var regex = new Regex(@"<[^>]*>");
            AHtml = regex.Replace(AHtml, "");

            AHtml = ReplaceMarkupChar(AHtml);

            return AHtml;
        }


        /// <summary>
        ///   �����ı����ȣ�������Ӣ���ַ����������������ȣ�Ӣ����һ������
        /// </summary>
        /// <param name="Text"> ����㳤�ȵ��ַ��� </param>
        /// <returns> int </returns>
        public static int GbStrLength(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2; //������ȴ���1�������ģ�ռ�����ֽڣ�+2
                else
                    len += 1; //������ȵ���1����Ӣ�ģ�ռһ���ֽڣ�+1
            }

            return len;
        }


        public static int[] SplitIntString(string strContent, string strSplit, bool RemoveEmpty = true)
        {
            string[] list = SplitString(strContent, strSplit, RemoveEmpty);
            if (list.Length == 0) return new int[0] { };

            var intlist = new List<int>();
            for (int i = 0; i < list.Length; i++)
            {
                if (RemoveEmpty && !string.IsNullOrWhiteSpace(list[i]))
                    intlist.Add(int.Parse(list[i]));
                else
                {
                    int x;
                    int.TryParse(list[i], out x);

                    intlist.Add(x);
                }
            }
            return intlist.ToArray();
        }


        /// <summary>
        ///   �ָ��ַ���
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit, bool RemoveEmpty = true)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new[] { strContent };

                if (RemoveEmpty)
                    return strContent.Split(strSplit.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                else
                    return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        ///   �ָ��ַ���
        /// </summary>
        /// <returns> </returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            var result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        ///   �ֶδ��Ƿ�ΪNull��Ϊ""(��)
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return true;

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////


        public static string GetPrecetStr(decimal x1, decimal all, int point = 2)
        {
            all = all == 0 ? 1 : all;

            decimal result = x1 / all;

            return result.ToString(string.Format("p{0}", point));
        }


        public static string GetPrecetStr(double x1, double all, int point = 2)
        {
            all = all == 0 ? 1 : all;

            double result = x1 / all;

            return result.ToString(string.Format("p{0}", point));
        }


        //public static string GetPrecetStr(int x1, int x2, int point = 2)
        //{
        //    x2 = x2 == 0 ? 1 : x2;

        //    var result = x1 / x2;

        //    return result.ToString(string.Format("p{0}", point));
        //}


        public static string GetPrecetStr(decimal strPercent, int point)
        {
            return strPercent.ToString(string.Format("p{0}", point));
        }

        public static int strpos(string str, string find)
        {
            return str.IndexOf(find);
        }

        #region �ַ��Ƿ�Сд

        /// <summary>
        ///   �ַ��Ƿ�Сд
        /// </summary>
        /// <param name="ch"> �ַ� </param>
        /// <returns> bool </returns>
        public static bool isLower(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else
                return false;
        }

        #endregion

        #region �ַ��Ƿ��д

        /// <summary>
        ///   �ַ��Ƿ��д
        /// </summary>
        /// <param name="ch"> �ַ� </param>
        /// <returns> bool </returns>
        public static bool isUpper(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;
        }

        #endregion

        #region ������ַ��Ƿ�������

        /// <summary>
        ///   ������ַ��Ƿ�������
        /// </summary>
        /// <param name="ch"> һ���ַ� </param>
        /// <returns> bool </returns>
        public static bool isNumberic(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            else
                return false;
        }

        #endregion

        #region �����ַ�����

        /// <summary>
        ///   �����ַ�����
        /// </summary>
        /// <param name="ch"> �����ַ� </param>
        /// <returns> </returns>
        public static bool isSpecialCharacter(char ch)
        {
            if (ch == '!')
                return true;
            else if (ch == '@')
                return true;
            else if (ch == '#')
                return true;
            else if (ch == '$')
                return true;
            else if (ch == '^')
                return true;
            else if (ch == '&')
                return true;
            else if (ch == '*')
                return true;
            else if (ch == '?')
                return true;
            else if (ch == '/')
                return true;
            else if (ch == '\\')
                return true;
            else
                return false;
        }

        #endregion

        #region ���ַ����е�β��ɾ��ָ�����ַ���

        /// <summary>
        ///   ���ַ����е�β��ɾ��ָ�����ַ���
        /// </summary>
        /// <param name="sourceString"> </param>
        /// <param name="removedString"> </param>
        /// <returns> </returns>
        public static string Remove(string sourceString, string removedString)
        {
            try
            {
                if (sourceString.IndexOf(removedString) < 0) //�ж�ɾ�����ַ����Ƿ����
                    throw new Exception("ԭ�ַ����в������Ƴ��ַ�����");
                string result = sourceString;
                int lengthOfSourceString = sourceString.Length;
                int lengthOfRemovedString = removedString.Length;
                int startIndex = lengthOfSourceString - lengthOfRemovedString;
                string tempSubString = sourceString.Substring(startIndex);
                if (tempSubString.ToUpper() == removedString.ToUpper())
                {
                    result = sourceString.Remove(startIndex, lengthOfRemovedString);
                }
                return result;
            }
            catch
            {
                return sourceString;
            }
        }

        #endregion

        #region ��ȡ��ַ��ұߵ��ַ���

        /// <summary>
        ///   ��ȡ��ַ��ұߵ��ַ���
        /// </summary>
        /// <param name="sourceString"> </param>
        /// <param name="splitChar"> </param>
        /// <returns> </returns>
        public static string RightSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[tempString.Length - 1];
            }
            return result;
        }

        #endregion

        #region ��ȡ��ַ���ߵ��ַ���

        /// <summary>
        ///   ��ȡ��ַ���ߵ��ַ���
        /// </summary>
        /// <param name="sourceString"> </param>
        /// <param name="splitChar"> </param>
        /// <returns> </returns>
        public static string LeftSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[0];
            }
            return result;
        }

        #endregion

        #region ȥ�����һ������

        /// <summary>
        ///   ȥ�����һ������
        /// </summary>
        /// <param name="origin"> </param>
        /// <returns> </returns>
        public static string DelLastComma(string origin)
        {
            if (origin.IndexOf(",") == -1)
            {
                return origin;
            }
            return origin.Substring(0, origin.LastIndexOf(","));
        }

        #endregion

        #region ɾ�����ɼ��ַ�

        /// <summary>
        ///   ɾ�����ɼ��ַ�
        /// </summary>
        /// <param name="sourceString"> </param>
        /// <returns> </returns>
        public static string DeleteUnVisibleChar(string sourceString)
        {
            var sBuilder = new StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        #endregion

        #region ��ȡ����Ԫ�صĺϲ��ַ���

        /// <summary>
        ///   ��ȡ����Ԫ�صĺϲ��ַ���
        /// </summary>
        /// <param name="stringArray"> </param>
        /// <returns> </returns>
        public static string GetArrayString(string[] stringArray)
        {
            string totalString = null;
            for (int i = 0; i < stringArray.Length; i++)
            {
                totalString = totalString + stringArray[i];
            }
            return totalString;
        }

        #endregion

        #region ��ȡĳһ�ַ������ַ��������г��ֵĴ���

        /// <summary>
        ///   ��ȡĳһ�ַ������ַ��������г��ֵĴ���
        /// </summary>
        /// <param name="stringArray"> �ַ����� </param>
        /// <param name="findString"> Ѱ�ҵ��ַ��� </param>
        /// <returns> INT </returns>
        public static int GetStringCount(string[] stringArray, string findString)
        {
            int count = -1;
            string totalString = GetArrayString(stringArray); //��ȡ����Ԫ�صĺϲ��ַ���	
            string subString = totalString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = totalString.Substring(subString.IndexOf(findString));
                count += 1;
            }
            return count;
        }

        #endregion

        #region ��ȡĳһ�ַ������ַ����г��ֵĴ���

        /// <summary>
        ///   ��ȡĳһ�ַ������ַ����г��ֵĴ���
        /// </summary>
        /// <param name="stringArray" type="string">
        ///   <para> ԭ�ַ��� </para>
        /// </param>
        /// <param name="findString" type="string">
        ///   <para> ƥ���ַ��� </para>
        /// </param>
        /// <returns> ƥ���ַ������� </returns>
        public static int GetStringCount(string sourceString, string findString)
        {
            int count = 0;
            int findStringLength = findString.Length;
            string subString = sourceString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
                count += 1;
            }
            return count;
        }

        #endregion

        #region ��ȡ��startString��ʼ��ԭ�ַ�����β�������ַ�

        /// <summary>
        ///   ��ȡ��startString��ʼ��ԭ�ַ�����β�������ַ�
        /// </summary>
        /// <param name="sourceString" type="string">
        ///   <para> </para>
        /// </param>
        /// <param name="startString" type="string">
        ///   <para> </para>
        /// </param>
        /// <returns> A string value... </returns>
        public static string GetSubString(string sourceString, string startString)
        {
            try
            {
                int index = sourceString.ToUpper().IndexOf(startString);
                if (index > 0)
                {
                    return sourceString.Substring(index);
                }
                return sourceString;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region ????

        /// <summary>
        /// </summary>
        /// <param name="sourceString"> </param>
        /// <param name="beginRemovedString"> </param>
        /// <param name="endRemovedString"> </param>
        /// <returns> </returns>
        public static string GetSubString(string sourceString, string beginRemovedString, string endRemovedString)
        {
            try
            {
                if (sourceString.IndexOf(beginRemovedString) != 0)
                    beginRemovedString = "";

                if (sourceString.LastIndexOf(endRemovedString, sourceString.Length - endRemovedString.Length) < 0)
                    endRemovedString = "";

                int startIndex = beginRemovedString.Length;
                int length = sourceString.Length - beginRemovedString.Length - endRemovedString.Length;
                if (length > 0)
                {
                    return sourceString.Substring(startIndex, length);
                }
                return sourceString;
            }
            catch
            {
                return sourceString;
                ;
            }
        }

        #endregion

        #region ���ֽ���ȡ���ַ����ĳ���

        /// <summary>
        ///   ���ֽ���ȡ���ַ����ĳ���
        /// </summary>
        /// <param name="strTmp"> Ҫ������ַ��� </param>
        /// <returns> �ַ������ֽ��� </returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }

        #endregion

        #region ���ֽ���Ҫ���ַ�����λ��

        /// <summary>
        ///   ���ֽ���Ҫ���ַ�����λ��
        /// </summary>
        /// <param name="intIns"> �ַ�����λ�� </param>
        /// <param name="strTmp"> Ҫ������ַ��� </param>
        /// <returns> �ֽڵ�λ�� </returns>
        public static int GetByteIndex(int intIns, string strTmp)
        {
            int intReIns = 0;
            if (strTmp.Trim() == "")
            {
                return intIns;
            }
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intReIns = intReIns + 2;
                }
                else
                {
                    intReIns = intReIns + 1;
                }
                if (intReIns >= intIns)
                {
                    intReIns = i + 1;
                    break;
                }
            }
            return intReIns;
        }

        #endregion

        #region ȥ���ַ��еĿո�

        /// <summary>
        ///   ȥ���ַ��еĿո�
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static string RemoveMiddleSpace(string str)
        {
            char[] ch = str.ToCharArray();
            var sb = new StringBuilder();

            foreach (var c in ch)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                {
                    sb.Append(c.ToString());
                }
            }
            return sb.ToString();
        }

        #endregion

        #region ������ˮ��

        /// <summary>
        ///   ������ˮ��
        /// </summary>
        /// <param name="strId"> ��ʼ�ַ��� </param>
        /// <param name="i"> ��ˮλ�� </param>
        /// <returns> �ַ��� </returns>
        private static string DoInc(string strId, int i)
        {
            string chrId;
            if (i > 0)
            {
                chrId = strId.Substring(i - 1, 1);
                switch (chrId)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        return strId.Substring(0, i - 1) + (int.Parse(chrId) + 1).ToString() +
                               strId.Substring(i, strId.Length - i);
                    case "9":
                        if (i == 1)
                        {
                            return "10" + strId.Substring(1, strId.Length - 1);
                            ;
                        }
                        else
                        {
                            return DoInc(strId.Substring(0, i - 1) + "0" + strId.Substring(i, strId.Length - i), i - 1);
                        }
                    default:
                        return DoInc(strId, i - 1);
                }
            }
            else
            {
                return strId;
            }
        }

        #endregion

        #region ������ˮ��

        /// <summary>
        ///   ������ˮ��
        /// </summary>
        /// <param name="strId"> ������ַ� </param>
        /// <returns> </returns>
        public static string IncStr(string strId)
        {
            return DoInc(strId, strId.Length);
        }

        #endregion

        #region ����ʽȡ���ַ����б���ֵ-����

        public static string GetDataArea(string StrDataArea, string strData)
        {
            int i_PositionBof = 0;
            int i_PositionEof = 0;
            int i_length = 0;
            string StrRet = "";
            //			i_PositionBof = InStr(LCase(strData), LCase(StrDataArea)); //'���������ʼλ��
            i_PositionBof = strData.IndexOf(StrDataArea);
            if (i_PositionBof == -1)
            {
                return "";
            }
            i_PositionBof = i_PositionBof + StrDataArea.Length + 1;
            i_PositionEof = strData.IndexOf("&", i_PositionBof); //  InStr(i_PositionBof, strData, "&");      
            i_length = i_PositionEof - i_PositionBof;
            if (i_PositionEof == -1) //'���û���ҵ��ֺ�;��˵�������һ����		
            {
                StrRet = strData.Substring(i_PositionBof); //Mid(strData, m_PositionBof);		
            }
            else
            {
                StrRet = strData.Substring(i_PositionBof, i_length); //Mid(strData, m_PositionBof, m_length);		
            }
            return StrRet;
        }

        #endregion

        #region ��ȡ��������ip��ip���ĵ�һ��IP

        public static string GetFirstIp(string ips)
        {
            if ((ips == null) || (ips.Length <= 0))
            {
                return "";
            }

            string[] ip = ips.Split(',');

            if (ip.Length > 0)
            {
                return ip[0].Trim();
            }
            else
            {
                return ips;
            }
        }

        #endregion

        #region �ַ����ֽ����� lianyee

        public static string StrSorts(string text, string splitStr)
        {
            char[] splitChar = splitStr.ToCharArray();
            string[] keywords = text.Split(splitChar);
            string newtext = "";
            Array.Sort(keywords);
            var list = new ArrayList();
            for (int i = 0; i < keywords.Length; i++)
            {
                newtext = newtext + keywords[i];
            }
            return newtext;
        }

        #endregion

        #region ת��ArrayListΪ�ַ���

        public static string ConvertArrayList2String(ArrayList list)
        {
            return ConvertArrayList2String(list, ',');
        }

        public static string ConvertArrayList2String(ArrayList list, char separator)
        {
            var sb = new StringBuilder();
            if (list == null) return string.Empty;
            foreach (var o in list)
            {
                if (sb.Length != 0)
                    sb.Append(separator);
                sb.Append(o);
            }
            return sb.ToString();
        }

        #endregion

        #region ȡ�������

        private static Object thisLock = new Object();

        #endregion

        #region �滻html�ַ�

        private static readonly char[] _markupChar = { ' ', ' ', ' ', '<', '>', '&', '"', '*', '/' };

        private static readonly string[] _replaceString =
            {
                "&ensp;", "&emsp;", "&nbsp;", "&lt;", "&gt;", "&amp;",
                "&quot;", "&times;", "&divide;"
            };

        public static string ReplaceMarkupChar(string source)
        {
            for (int i = 0; i < _replaceString.Length; i++)
                source = source.Replace(_replaceString[i], _markupChar[i].ToString());

            return source;
        }

        #endregion

        #region ��ȡhtml�ַ�����innertext

        public static string FormatHtmlInnerText(string AHtml)
        {
            var regex = new Regex(@"<[^>]*>");
            AHtml = regex.Replace(AHtml, "");

            AHtml = ReplaceMarkupChar(AHtml);

            return AHtml;
        }

        #endregion

        #region �õ�С��������λ��

        public static int GetDotNumCount(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            int pos = value.IndexOf(".");

            if (pos <= 0)
            {
                return 0;
            }

            string nextdot = value.Substring(pos + 1, value.Length - pos - 1);

            return nextdot.Length;
        }

        #endregion

        #region С���������λ

        public static string FormatDot(string value, int num)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            int pos = value.IndexOf(".");

            if (pos <= 0)
            {
                return value;
            }

            string predot = value.Substring(0, pos);

            string nextdot = value.Substring(pos + 1, value.Length - pos - 1);

            if (nextdot.Length <= num)
            {
                return predot + "." + nextdot;
            }
            else
            {
                return predot + "." + nextdot.Substring(0, num);
            }
        }


        public static string DelLast0(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            int pos = value.IndexOf(".");
            while (pos > 0)
            {
                if (value.Substring(value.Length - 1, 1) == "0")
                {
                    value = value.Substring(0, value.Length - 1);
                    pos = 1;
                }
                else if (value.Substring(value.Length - 1, 1) == ".")
                {
                    value = value.Substring(0, value.Length - 1);
                    pos = -1;
                }
                else
                {
                    pos = -1;
                }
            }

            return value;
        }

        #endregion

        #region ����ƴ������ĸ

        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        public static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = arrCN[0];
                int pos = arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode =
                    {
                        45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324,
                        49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980,
                        53689, 54481
                    };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }

        #endregion

        /// <summary>
        ///   ���ַ�����ĳЩ�ַ������滻
        /// </summary>
        /// <param name="str"> </param>
        /// <param name="from"> </param>
        /// <param name="len"> </param>
        /// <param name="mask"> </param>
        /// <returns> </returns>
        public static string Mask(string str, int from, int len, char mask = '*')
        {
            if (str.Length <= from + 1)
                return str;

            string firstpart = str.Substring(0, from + 1);

            if (str.Length <= from + 1 + len)
            {
                return firstpart.PadRight(str.Length, mask);
            }
            string secondpart = str.Substring(from + len + 1);

            var retstr = firstpart.PadRight(from + 1 + len, mask) + secondpart;
            return retstr;
        }

        public static string Mask(string str, int from, char mask = '*')
        {
            return Mask(str, from, int.MaxValue, mask);
        }

        public static string MaskRight(string str, int len, char mask = '*')
        {
            if (str.Length < len)
                return "".PadRight(str.Length, mask);

            return str.Substring(0, str.Length - len).PadRight(str.Length, mask);
        }


        public static string RMB(decimal? rmb)
        {
            if (rmb == null)
                return "0";
            if (Convert.ToInt32(rmb) == rmb)
                return Convert.ToInt32(rmb).ToString();

            return decimal.Round(rmb.Value, 2).ToString();
        }

        /// <summary>
        ///   ���������ɫ
        /// </summary>
        /// <returns> </returns>
        public static string GetColor()
        {
            var nums = new List<string> { "0", "3", "6", "9", "C", "F" };
            var clr = "#";
            for (var i = 0; i < 6; i++)
            {
                var n = Dev.Comm.Randoms.CreateRandomNumber(6);
                clr = clr + nums[n];
            }
            return clr;
        }
    }
}
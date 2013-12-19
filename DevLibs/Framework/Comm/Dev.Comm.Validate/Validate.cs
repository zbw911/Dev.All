// ***********************************************************************************
// Created by zbw911 
// �����ڣ�2012��12��18�� 10:44
// 
// �޸��ڣ�2013��02��18�� 18:24
// �ļ�����Validate.cs
// 
// ����и��õĽ����������ʼ���zbw911#gmail.com
// ***********************************************************************************
using System;
using System.Collections;
using System.Text.RegularExpressions;
using Dev.Comm.Utils;

namespace Dev.Comm.Validate
{
    public class Validate
    {
        private static readonly Regex RegCHZN = new Regex("[һ-��]");
        private static readonly Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static readonly Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");

        private static readonly Regex RegEmail =
            new Regex(
                "^(([0-9a-zA-Z]+)|([0-9a-zA-Z]+[_.0-9a-zA-Z-]*[0-9a-zA-Z]+))@([a-zA-Z0-9-]+[.])+(net|NET|com|COM|gov|GOV|mil|MIL|org|ORG|edu|EDU|int|INT|cn|CN)$");

        private static readonly Regex RegMobileTel = new Regex("^[1][3|5][0-9]{9}$");
        private static readonly Regex RegNumber = new Regex("^[0-9]+$");
        private static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");

        private static readonly Regex RegUrl = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        private static readonly Regex RegID = new Regex("^[0-9a-zA-Z]*$");

        private static readonly Regex RegData =
            new Regex(@"^[1-2]\d{3}-((0?[1-9])|(1[0-2]))-((0[1-9])|([1-2]?\d)|(3[0-1]))$");

        private static readonly Regex RegDataTime =
            new Regex(
                @"^[1-9]\d{3}-(0?[1-9]|1[0|1|2])-(0?[1-9]|[1|2][0-9]|3[0|1])\s(0?[0-9]|1[0-9]|2[0-3]):(0?[0-9]|[1|2|3|4|5][0-9]):(0?[0-9]|[1|2|3|4|5][0-9])$");


        public static bool isBlank(string strInput)
        {
            return ((strInput == null) || (strInput.Trim() == ""));
        }

        public static bool IsDecimal(string inputData)
        {
            return RegDecimal.Match(inputData).Success;
        }

        public static bool IsDecimalSign(string inputData)
        {
            return RegDecimalSign.Match(inputData).Success;
        }

        public static bool IsDouble(string strInput)
        {
            if (strInput.IndexOf("-") == 0)
            {
                strInput = strInput.Substring(1);
            }
            char[] chArray = strInput.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (((chArray[i] < '0') || (chArray[i] > '9')) && (chArray[i] != '.'))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsEmail(string inputData)
        {
            return RegEmail.Match(inputData.ToLower()).Success;
        }

        public static bool IsHasCHZN(string inputData)
        {
            return RegCHZN.Match(inputData).Success;
        }

        public static bool IsInt(string strInput)
        {
            if (strInput.IndexOf("-") == 0)
            {
                strInput = strInput.Substring(1);
            }
            char[] chArray = strInput.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if ((chArray[i] < '0') || (chArray[i] > '9'))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsMobileTel(string inputData)
        {
            return RegMobileTel.Match(inputData).Success;
        }

        public static bool IsNumber(string inputData)
        {
            return RegNumber.Match(inputData).Success;
        }

        public static bool IsNumberSign(string inputData)
        {
            return RegNumberSign.Match(inputData).Success;
        }

        public static bool IsNumeric(string strInput)
        {
            if (strInput.IndexOf("-") == 0)
            {
                strInput = strInput.Substring(1);
            }
            char[] chArray = strInput.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (((chArray[i] < '0') || (chArray[i] > '9')) && (chArray[i] != '.'))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>�Ƿ�绰,�����ֻ� </summary>
        /// <param name="strInput">�����ַ���</param>
        /// <returns>true/false</returns>
        public static bool isPhone(string strInput)
        {
            if ((strInput == null) || (strInput == ""))
            {
                return false;
            }
            else
            {
                /*
                char[] ca =strInput.ToCharArray();
                for (int i=0;i<ca.Length;i++)
                {
                    if ((ca[i]<'0' || ca[i]>'9') && ca[i]!='-' && ca[i]!='(' && ca[i]!=')' && ca[i]!='+')
                    {					 
                        found=false;
                        break;
                    };
                };
                if (strInput.Substring(strInput.Length-1,1) == "-") found = false;
                */
                Match tt = Regex.Match(strInput, @"^((\(?\d{2,3})\)?)?-?(\(\d{3,4}\)|\d{3,4}-)?((\d{7,8})|(\d{11}))$");
                return tt.Success;
            }
        }

        /// <summary>
        /// �Ƿ���URL
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool isUrl(string strInput)
        {
            Match m = RegUrl.Match(strInput);
            return m.Success;
        }

        public static bool isnum(string strid)
        {
            Match m = RegID.Match(strid);
            return m.Success;
        }

        /// <summary>
        /// �ǲ�������+ʱ���ʽ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDateTime(string p)
        {
            Match m = RegDataTime.Match(p);
            return m.Success;
        }

        /// <summary>
        /// �ǲ������ڸ�ʽ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDate(string p)
        {
            Match m = RegData.Match(p);
            return m.Success;
        }

        /// <summary>
        /// �ж��ǲ���ʱ���ʽ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDateOrDateTime(string p)
        {
            DateTime dt;
            return DateTime.TryParse(p, out dt);
        }

        #region ��֤�����е��ַ��Ƿ�������

        /// <summary>
        /// ��֤�����е��ַ��Ƿ�������
        /// </summary>
        /// <param name="strPwd">�ַ���</param>
        /// <returns>BOOL</returns>
        public static bool validateNumberCase(string strPwd)
        {
            bool foundNumber = false;
            for (int i = 0; i < strPwd.Length; i++)
            {
                if (foundNumber == false)
                    foundNumber = StringUtil.isNumberic(strPwd[i]);
            }
            if (foundNumber)
                return true;
            else
                return false;
        }

        #endregion

        #region ��֤�����е��ַ��Ƿ��������ַ�

        public static bool validateSpecialCase(string strPwd)
        {
            bool foundSpecial = false;
            for (int i = 0; i < strPwd.Length; i++)
            {
                if (foundSpecial == false)
                    foundSpecial = StringUtil.isSpecialCharacter(strPwd[i]);
            }
            if (foundSpecial)
                return true;
            else
                return false;
        }

        #endregion

        #region ��֤�����е��ַ��Ƿ��Сд���

        /// <summary>
        /// ��֤�����е��ַ��Ƿ��Сд���
        /// </summary>
        /// <param name="strPwd">�����ַ���</param>
        /// <returns>bool</returns>
        public static bool validateMixedCase(string strPwd)
        {
            bool foundLower = false, foundUper = false;
            for (int i = 0; i < strPwd.Length; i++)
            {
                if (foundLower == false)
                    foundLower = StringUtil.isLower(strPwd[i]);
                if (foundUper == false)
                    foundUper = StringUtil.isUpper(strPwd[i]);
            }
            if (foundLower && foundUper)
                return true;
            else
                return false;
        }

        #endregion

        #region ������ĳ��Ƚ��м���

        /// <summary>
        /// ������ĳ��Ƚ��м���
        /// </summary>
        /// <param name="strPwd">�����ַ���</param>
        /// <param name="intLen">���볤��</param>
        /// <returns>BOOL,false�����Ȳ���</returns>
        public static bool validatePasswordLength(string strPwd, int intLen)
        {
            if (strPwd.Length < intLen)
                return false;
            else
                return true;
        }

        #endregion

        #region ����ڿ�SQLע�뺯�� [='/<>-*]

        public static bool CheckSqlImmitParams(params object[] args)
        {
            //string[] Lawlesses ={ "=", "'", "<", ">" ,"%"};

            string[] Lawlesses =
                {
                    "=", "'", "<", ">", "%", "exec", "insert", "select", "from", "join", "delete",
                    "update", "master", "truncate", "declare", "sp_executesql", "drop", "table"
                };

            if (Lawlesses == null || Lawlesses.Length <= 0)
                return true;
            // ����������ʽ,��:Lawlesses��=�ź�'��,��������ʽΪ .*[=}'].*  
            //string str_Regex = ".*[";

            string str_Regex = "";
            for (int i = 0; i < Lawlesses.Length - 1; i++)
                str_Regex += Lawlesses[i] + "|";

            //str_Regex += Lawlesses[Lawlesses.Length - 1] + "].*";

            str_Regex = str_Regex.Substring(0, str_Regex.Length - 1);

            foreach (var arg in args)
            {
                if (arg is string) //������ַ���,ֱ�Ӽ��        
                {
                    if (Regex.Matches(arg.ToString().ToLower(), str_Regex).Count > 0)
                        return false;
                }
                else if (arg is ICollection) //�����һ������,���鼯����Ԫ���Ƿ��ַ���,���ַ���,�ͽ��м��       
                {
                    foreach (var obj in (ICollection) arg)
                    {
                        if (obj is string)
                        {
                            if (Regex.Matches(obj.ToString().ToLower(), str_Regex).Count > 0)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��06��07�� 14:25
//  
//  �޸��ڣ�2013��09��17�� 11:32
//  �ļ�����Dev.Libs/Dev.Comm.Core/AsmUtil.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Dev.Comm.Utils
{
    /// <summary>
    /// ���򼯷���
    /// </summary>
    public class AsmUtil
    {
        public static Assembly GetAssemblyFromCurrentDomain(string AName, bool IsLoadAsm)
        {
            Assembly[] asm = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in asm)
            {
                if (a.GetName().Name.Equals(AName)) return a;
            }

            if (IsLoadAsm /*&& File.Exists(AName)*/)
            {
                return Assembly.LoadFile(AName);
            }
            else
            {
                return null;
            }
        }


        public static Assembly GetAssemblyFromCurrentDomain(string AName)
        {
            return GetAssemblyFromCurrentDomain(AName, true);
        }

        public static object InvokeMethod(string AName, object[] AParam, object AInstance)
        {
            if (AInstance == null) return null;
            Type type = AInstance.GetType();
            MethodInfo mi = type.GetMethod(AName);
            if (null == mi)
            {
                throw new Exception(String.Format("û�ҵ�����", AName));
            }
            //else
            //{
            //    return mi;
            //}

            ParameterInfo[] param = mi.GetParameters();
            if (AParam != null && !param.Length.Equals(AParam.Length))
            {
                throw new Exception(String.Format("û�ҵ�����", AName));
            }
            //else
            //{
            //    return "noParam";
            //}

            return mi.Invoke(AInstance, AParam);
        }

        /// <summary>
        ///   ִ��ĳ������
        /// </summary>
        /// <param name="AAsmName"> </param>
        /// <param name="AClassName"> </param>
        /// <param name="AMethodName"> </param>
        /// <param name="AConstructorParam"> </param>
        /// <param name="AMethodParam"> </param>
        /// <param name="AInstance"> </param>
        /// <returns> </returns>
        /// <exception cref="Exception"></exception>
        public static object InvokeMethod(string AAsmName, string AClassName, string AMethodName,
                                          object[] AConstructorParam, object[] AMethodParam, ref object AInstance)
        {
            Assembly asm = GetAssemblyFromCurrentDomain(AAsmName);
            if (null == (asm))
            {
                throw new Exception(String.Format("������������", AAsmName));
            }
            else
            {
            }

            Type type = asm.GetType(AClassName, false, true);
            if (null == (type))
            {
                throw new Exception(String.Format("�಻����", AClassName));
            }
            else
            {
                MethodInfo mi = type.GetMethod(AMethodName);
                if (null == (mi))
                {
                    throw new Exception(String.Format("����������", AMethodName));
                }
                else
                {
                }

                ParameterInfo[] param = mi.GetParameters();
                if (!param.Length.Equals(AMethodParam.Length))
                {
                    throw new Exception(String.Format("������������", AMethodName));
                }
                else
                {
                }

                if (!mi.IsStatic && null == (AInstance))
                {
                    AInstance = Activator.CreateInstance(type, AConstructorParam);
                }
                else
                {
                }

                return mi.Invoke(AInstance, AMethodParam);
            }
        }

        /// <summary>
        ///   ȡ������ֵ
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="PropertyName"> </param>
        /// <param name="Index"> </param>
        /// <returns> </returns>
        public static object GetPropertyValue(object obj, string PropertyName, object[] Index)
        {
            PropertyInfo t = obj.GetType().GetProperty(PropertyName);

            if (null == (t)) return null;


            return t.GetValue(obj, Index);
        }

        /// <summary>
        ///   �����Ƿ����
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="PropertyName"> </param>
        /// <returns> </returns>
        public static bool ExistPropertyName(object obj, string PropertyName)
        {
            PropertyInfo t = obj.GetType().GetProperty(PropertyName);
            return t != null;
        }

        /// <summary>
        ///   ��������ֵ
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <param name="value"> </param>
        /// <param name="index"> </param>
        public static void SetPropertyValue(object obj, string propertyName, object value, object[] index)
        {
            PropertyInfo t = obj.GetType().GetProperty(propertyName);

            if (null == (t)) throw new ArgumentNullException("t");

            //ɾ��������ڽ�������ת����ʱ��ᷢ������
            //object tmp = Convert.ChangeType(Value, t.PropertyType);
            //t.SetValue(obj, tmp, Index);

            t.SetValue(obj, value, index);

        }

        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj, propertyName, value, null);
        }



        /// <summary>
        ///   ȡ�ò�����Ϣ
        /// </summary>
        /// <returns> </returns>
        public static ParameterInfo[] GetMethodParameterInfo()
        {
            return (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
        }

        //�������б�
        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static string[] GetMethodParamNames()
        {
            ParameterInfo[] pis = (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
            Array a = Array.CreateInstance(typeof(string), pis.Length);
            for (int i = 0; i < pis.Length; i++)
            {
                a.SetValue(pis[i].Name, i);
            }
            return (string[])a;
        }

        /// <summary>
        ///   ȡ�ø�ʽ����Ĳ����б�
        /// </summary>
        /// <param name="methodNamefromat"> </param>
        /// <param name="paramFormat"> </param>
        /// <returns> </returns>
        public static string GetMethodParamNamesByFormat(string methodNamefromat = "{0}|",
                                                         string paramFormat = "{0}={{{1}}}")
        {
            string methodName = (new StackTrace()).GetFrame(1).GetMethod().Name;
            ParameterInfo[] pis = (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
            var result = new StringBuilder();
            result.AppendFormat(methodNamefromat, methodName);
            for (int i = 0; i < pis.Length; i++)
            {
                result.AppendFormat(paramFormat, pis[i].Name, pis[i].Position);
            }
            return result.ToString();
        }

        #region Nested type: ObjectUtil

        //private class ObjectUtil
        //{
        //    //public static bool IsNull(object AObject)
        //    //{
        //    //    return AObject == null;
        //    //}
        //}

        #endregion
    }
}
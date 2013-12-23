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
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dev.Comm.Utils
{
    /// <summary>
    /// ���򼯷���
    /// </summary>
    public class AsmUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyNameOrPath"></param>
        /// <param name="isLoadAsmPath">�Ƿ�Ӿ���·����ȡ</param>
        /// <returns></returns>
        public static Assembly GetAssemblyFromCurrentDomain(string assemblyNameOrPath, bool isLoadAsmPath)
        {
            Assembly[] asm = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in asm)
            {
                if (a.GetName().Name.Equals(assemblyNameOrPath)) return a;
            }

            if (isLoadAsmPath /*&& File.Exists(AName)*/)
            {
                return Assembly.LoadFile(assemblyNameOrPath);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// ���� AssemblyName ȡ��Ӧ�ó��� ��
        /// </summary>
        /// <param name="assemblyNameOrPath">��������·��</param>
        /// <returns></returns>
        public static Assembly GetAssemblyFromCurrentDomain(string assemblyNameOrPath)
        {
            return GetAssemblyFromCurrentDomain(assemblyNameOrPath, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="aParam"></param>
        /// <param name="aInstance"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object InvokeMethod(Func<Type, MethodInfo> filter, object[] aParam, object aInstance)
        {
            if (aInstance == null) return null;
            Type type = aInstance.GetType();

            var mi = filter.Invoke(type);



            if (null == mi)
            {
                throw new Exception(String.Format("û�ҵ�����"));
            }

            ParameterInfo[] param = mi.GetParameters();
            if (aParam != null && !param.Length.Equals(aParam.Length))
            {
                throw new Exception(String.Format("û�ҵ�����"));
            }

            return mi.Invoke(aInstance, aParam);

        }



        /// <summary>
        /// ִ�з��� 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="aParam"></param>
        /// <param name="aInstance"></param>
        /// <returns></returns>
        public static object InvokeMethod(string aName, object[] aParam, object aInstance)
        {

            return InvokeMethod(x => x.GetMethods().FirstOrDefault(y => y.Name == aName), aParam, aInstance);
        }



        /// <summary>
        ///   ִ��ĳ������
        /// </summary>
        /// <param name="aAsmName"> </param>
        /// <param name="aClassName"> </param>
        /// <param name="aMethodName"> </param>
        /// <param name="aConstructorParam"> </param>
        /// <param name="aMethodParam"> </param>
        /// <param name="aInstance"> </param>
        /// <returns> </returns>
        /// <exception cref="Exception"></exception>
        public static object InvokeMethod(string aAsmName, string aClassName, string aMethodName,
                                          object[] aConstructorParam, object[] aMethodParam, ref object aInstance)
        {
            Assembly asm = GetAssemblyFromCurrentDomain(aAsmName);
            if (null == (asm))
            {
                throw new Exception(String.Format("������������", aAsmName));
            }
            else
            {
            }

            Type type = asm.GetType(aClassName, false, true);
            if (null == (type))
            {
                throw new Exception(String.Format("�಻����", aClassName));
            }
            else
            {
                MethodInfo mi = type.GetMethod(aMethodName);
                if (null == (mi))
                {
                    throw new Exception(String.Format("����������", aMethodName));
                }
                else
                {
                }

                ParameterInfo[] param = mi.GetParameters();
                if (!param.Length.Equals(aMethodParam.Length))
                {
                    throw new Exception(String.Format("������������", aMethodName));
                }
                else
                {
                }

                if (!mi.IsStatic && null == (aInstance))
                {
                    aInstance = Activator.CreateInstance(type, aConstructorParam);
                }
                else
                {
                }

                return mi.Invoke(aInstance, aMethodParam);
            }
        }

        /// <summary>
        ///   ȡ������ֵ
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <param name="index"> </param>
        /// <returns> </returns>
        public static object GetPropertyValue(object obj, string propertyName, object[] index)
        {
            PropertyInfo t = obj.GetType().GetProperty(propertyName);

            if (null == (t)) return null;


            return t.GetValue(obj, index);
        }

        /// <summary>
        ///   �����Ƿ����
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        public static bool ExistPropertyName(object obj, string propertyName)
        {
            PropertyInfo t = obj.GetType().GetProperty(propertyName);
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
            var a = new string[pis.Length];
            for (int i = 0; i < pis.Length; i++)
            {
                a.SetValue(pis[i].Name, i);
            }
            return a;
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


    }
}
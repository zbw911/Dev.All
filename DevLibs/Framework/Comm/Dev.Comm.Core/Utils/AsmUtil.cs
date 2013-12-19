// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月07日 14:25
//  
//  修改于：2013年09月17日 11:32
//  文件名：Dev.Libs/Dev.Comm.Core/AsmUtil.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Dev.Comm.Utils
{
    /// <summary>
    /// 程序集方法
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
                throw new Exception(String.Format("没找到方法", AName));
            }
            //else
            //{
            //    return mi;
            //}

            ParameterInfo[] param = mi.GetParameters();
            if (AParam != null && !param.Length.Equals(AParam.Length))
            {
                throw new Exception(String.Format("没找到参数", AName));
            }
            //else
            //{
            //    return "noParam";
            //}

            return mi.Invoke(AInstance, AParam);
        }

        /// <summary>
        ///   执行某个方法
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
                throw new Exception(String.Format("错误的载入程序集", AAsmName));
            }
            else
            {
            }

            Type type = asm.GetType(AClassName, false, true);
            if (null == (type))
            {
                throw new Exception(String.Format("类不存在", AClassName));
            }
            else
            {
                MethodInfo mi = type.GetMethod(AMethodName);
                if (null == (mi))
                {
                    throw new Exception(String.Format("方法不存在", AMethodName));
                }
                else
                {
                }

                ParameterInfo[] param = mi.GetParameters();
                if (!param.Length.Equals(AMethodParam.Length))
                {
                    throw new Exception(String.Format("方法参数错误", AMethodName));
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
        ///   取得属性值
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
        ///   属性是否存在
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
        ///   设置属性值
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <param name="value"> </param>
        /// <param name="index"> </param>
        public static void SetPropertyValue(object obj, string propertyName, object value, object[] index)
        {
            PropertyInfo t = obj.GetType().GetProperty(propertyName);

            if (null == (t)) throw new ArgumentNullException("t");

            //删除这个，在进行类型转换的时候会发生错误
            //object tmp = Convert.ChangeType(Value, t.PropertyType);
            //t.SetValue(obj, tmp, Index);

            t.SetValue(obj, value, index);

        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj, propertyName, value, null);
        }



        /// <summary>
        ///   取得参数信息
        /// </summary>
        /// <returns> </returns>
        public static ParameterInfo[] GetMethodParameterInfo()
        {
            return (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
        }

        //参数名列表
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
        ///   取得格式化后的参数列表
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
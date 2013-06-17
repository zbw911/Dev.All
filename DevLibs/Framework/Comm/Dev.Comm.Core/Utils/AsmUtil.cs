// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：AsmUtil.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Dev.Comm
{
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
            if (ObjectUtil.IsNull(AInstance)) return null;
            Type type = AInstance.GetType();
            MethodInfo mi = type.GetMethod(AName);
            if (ObjectUtil.IsNull(mi))
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

        public static object InvokeMethod(string AAsmName, string AClassName, string AMethodName,
                                          object[] AConstructorParam, object[] AMethodParam, ref object AInstance)
        {
            Assembly asm = GetAssemblyFromCurrentDomain(AAsmName);
            if (ObjectUtil.IsNull(asm))
            {
                throw new Exception(String.Format("错误的载入程序集", AAsmName));
            }
            else
            {
            }

            Type type = asm.GetType(AClassName, false, true);
            if (ObjectUtil.IsNull(type))
            {
                throw new Exception(String.Format("类不存在", AClassName));
            }
            else
            {
                MethodInfo mi = type.GetMethod(AMethodName);
                if (ObjectUtil.IsNull(mi))
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

                if (!mi.IsStatic && ObjectUtil.IsNull(AInstance))
                {
                    AInstance = Activator.CreateInstance(type, AConstructorParam);
                }
                else
                {
                }

                return mi.Invoke(AInstance, AMethodParam);
            }
        }

        public static object GetPropertyValue(object obj, string PropertyName, object[] Index)
        {
            PropertyInfo t = obj.GetType().GetProperty(PropertyName);

            if (ObjectUtil.IsNull(t)) return null;


            return t.GetValue(obj, Index);
        }

        public static bool ExistPropertyName(object obj, string PropertyName)
        {
            PropertyInfo t = obj.GetType().GetProperty(PropertyName);
            return t != null;
        }

        public static void SetPropertyValue(object obj, string PropertyName, object Value, object[] Index)
        {
            PropertyInfo t = obj.GetType().GetProperty(PropertyName);

            if (ObjectUtil.IsNull(t)) throw new ArgumentNullException("t");

            object tmp = Convert.ChangeType(Value, t.PropertyType);
            t.SetValue(obj, tmp, Index);
        }

        public static ParameterInfo[] GetMethodParameterInfo()
        {
            return (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
        }

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

        public static string GetMethodParamNamesByFormat()
        {
            string MethodName = (new StackTrace()).GetFrame(1).GetMethod().Name;
            ParameterInfo[] pis = (new StackTrace()).GetFrame(1).GetMethod().GetParameters();
            var result = new StringBuilder();
            result.AppendFormat("{0}|", MethodName);
            for (int i = 0; i < pis.Length; i++)
            {
                result.AppendFormat("{0}={{{1}}}", pis[i].Name, pis[i].Position);
            }
            return result.ToString();
        }

        #region Nested type: ObjectUtil

        public class ObjectUtil
        {
            public static bool IsNull(object AObject)
            {
                return AObject == null;
            }
        }

        #endregion
    }
}
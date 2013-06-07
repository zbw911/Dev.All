// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：ObjUtil.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dev.Comm
{
    public class ObjUtil
    {
        public static bool IsNull(object AObject)
        {
            return AObject == null;
        }

        public static bool IsDbNull(object AObject)
        {
            if (IsNull(AObject)) return true;

            return AObject.Equals(DBNull.Value);
        }

        public static object FormatValue(object AValue)
        {
            switch (AValue.GetType().FullName.ToLower())
            {
                case "system.string":
                    return StrUtil.FormatValue(AValue);
                case "system.datetime":
                    return FormatDateTimeValue(AValue);
                default:
                    if (IsDbNull(AValue))
                    {
                        return 0;
                    }
                    else
                    {
                        return AValue;
                    }
            }
        }

        public static object FormatDateTimeValue(object AValue)
        {
            return Convert.ToDateTime(AValue);
        }


        public static object CloneObject(object obj)
        {
            // 创建内存流   
            using (var ms = new MemoryStream(1000))
            {
                object CloneObject;

                // 创建序列化器（有的书称为串行器）   

                // 创建一个新的序列化器对象总是比较慢。
                var bf =
                    new BinaryFormatter(
                        null, new StreamingContext(StreamingContextStates.Clone));
                // 将对象序列化至流   

                bf.Serialize(ms, obj);

                // 将流指针指向第一个字符   

                ms.Seek(0, SeekOrigin.Begin);


                // 反序列化至另一个对象（即创建了一个原对象的深表副本）   

                CloneObject = bf.Deserialize(ms);

                // 关闭流    
                ms.Close();
                return CloneObject;
            }
        }

        #region 序列化反序列化

        public static string Serialize(object data)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                using (var compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    formatter.Serialize(compressedzipStream, data);
                    compressedzipStream.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static object Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            object ret;
            byte[] bytes = Convert.FromBase64String(data);

            using (var ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                using (var zipStream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    var formatter = new BinaryFormatter();
                    //try
                    //{
                    ret = formatter.Deserialize(zipStream);
                    //}
                    //catch { return null; }
                }
            }
            return ret;
        }

        #endregion
    }
}
// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：server.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Xml.Serialization;

namespace Dev.Framework.FileServer.Config
{
    /// <summary>
    /// Server Struct Map to Config
    /// added by zbw911
    /// </summary>
    public class Server
    {
        [XmlAttribute]
        public int id { get; set; }

        [XmlAttribute]
        public bool used { get; set; }

        [XmlAttribute]
        public string hostip { get; set; }

        [XmlAttribute]
        public string startdirname { get; set; }

        [XmlAttribute]
        public string username { get; set; }

        [XmlAttribute]
        public string password { get; set; }

        [XmlAttribute]
        public string serverurl { get; set; }
    }
}
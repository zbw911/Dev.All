// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��06��07�� 14:25
//  
//  �޸��ڣ�2013��09��17�� 11:32
//  �ļ�����Dev.Libs/Dev.Comm.Core/XmlCompent.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System.Collections;
using System.Xml;

namespace Dev.Comm.XML
{
    public class XmlCompent
    {
        protected static Hashtable GetChildTable(XmlNode xn) //��֪�����ӽӵ�
        {
            var ht = new Hashtable();
            foreach (XmlNode nxn in xn.ChildNodes)
            {
                if (nxn.ChildNodes.Count <= 0)
                {
                    ht.Add(nxn.Name, nxn.InnerText);
                }
                else if (nxn.ChildNodes.Count == 1)
                {
                    XmlNode nxn1 = nxn.ChildNodes[0];
                    if (nxn1.NodeType == XmlNodeType.CDATA)
                    {
                        ht.Add(nxn.Name, nxn.InnerText);
                    }
                    else
                    {
                        ht.Add(nxn.Name, GetChildTable(nxn));
                    }
                }
                else
                {
                    ht.Add(nxn.Name, GetChildTable(nxn));
                }
            }
            return ht;
        }

        /// <summary>
        ///   �ַ�����ʽXMLת����HASHTABLE
        /// </summary>
        /// <param name="XmlFile"> </param>
        /// <returns> </returns>
        public static Hashtable GetTable(string XmlFile)
        {
            var ht = new Hashtable();
            var XMLDom = new XmlDocument();
            XMLDom.LoadXml(XmlFile);
            XmlNode newXMLNode = XMLDom.SelectSingleNode("root");
            foreach (XmlNode xn in newXMLNode.ChildNodes)
            {
                if (xn.ChildNodes.Count <= 0)
                {
                    ht.Add(xn.Name, xn.InnerText);
                }
                else if (xn.ChildNodes.Count == 1) //������Ҫ���ж��ӽӵ����Ƿ���<![CDATA[0]]>����
                {
                    XmlNode nxn = xn.ChildNodes[0];
                    if (nxn.NodeType == XmlNodeType.CDATA)
                    {
                        ht.Add(xn.Name, xn.InnerText);
                    }
                    else
                    {
                        ht.Add(xn.Name, GetChildTable(xn));
                    }
                }
                else
                {
                    ht.Add(xn.Name, GetChildTable(xn));
                }
            }

            return ht;
        }
    }

    //�̶�˳���HASHTABLE
    public class NoSortHashTable : Hashtable
    {
        private readonly ArrayList list = new ArrayList();

        public override ICollection Keys
        {
            get { return list; }
        }

        public override void Add(object key, object value)
        {
            base.Add(key, value);
            list.Add(key);
        }

        public override void Clear()
        {
            base.Clear();
            list.Clear();
        }

        public override void Remove(object key)
        {
            base.Remove(key);
            list.Remove(key);
        }
    }
}
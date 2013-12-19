// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月26日 11:33
// 
// 修改于：2013年02月19日 15:10
// 文件名：CasClient.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.Web;
using Dev.CasClient.Configuration;
using Dev.CasClient.UserAuthenticate;
using Dev.Comm;
using Dev.Comm.XML;

namespace Dev.CasClient
{
    /// <summary>
    ///   Static CAS client class that consumes the basic Jasig CAS functionality.
    /// </summary>
    public class CasClient
    {
        #region Readonly & Static Fields

        public static string StrCasServerUrl = CasClientConfiguration.Config.CasServerUrl +
                                               CasClientConfiguration.Config.CasPath;

        #endregion

        public static Action LoginSucess;

        //private readonly IUserAuthenticate userAuthenticate;

        #region C'tors

        /// <summary>
        /// </summary>
        public CasClient()
        {
            //userAuthenticate = UserAuthenticateManager.Provider;
        }

        /// <summary>
        /// </summary>
        /// <param name="userAuthenticate"> </param>
        /// <exception cref="ArgumentNullException"></exception>
        public CasClient(IUserAuthenticate userAuthenticate)
        {
            if (userAuthenticate == null)
            {
                throw new ArgumentNullException("userAuthenticate");
            }
            UserAuthenticateManager.Provider = userAuthenticate;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   登录,如果这是一个登录的方法,那就直接去调用登录了
        /// </summary>
        /// <param name="strTicket"> </param>
        /// <param name="strService"> </param>
        /// <param name="strRedirectUrl"> </param>
        /// <param name="strUserName"> </param>
        /// <param name="strErrorText"> </param>
        /// <returns> </returns>
        public bool Login(
            string strTicket,
            string strService,
            out string strRedirectUrl,
            out string strUserName,
            out string strErrorText)
        {
            strRedirectUrl = "";
            strUserName = "";
            strErrorText = "";

            //如果没有登录成功就跳转到
            if (String.IsNullOrEmpty(strTicket))
            {
                strRedirectUrl = BuildLoginRequest(StrCasServerUrl, strService);

                return true;
            }

            // when we have a ticket, then validate it
            var strValidateUrl = BuildServiceValidateRequest(StrCasServerUrl, strService, strTicket);

            var isOK = false;
            try
            {
                //var xml = Dev.Comm.Net.Http.GetUrl(strValidateUrl);

                var xmlh = new XmlHelper();
                xmlh.LoadXML(strValidateUrl, XmlHelper.LoadType.FromURL);

                if (xmlh.RootNode.FirstChild.LocalName == "authenticationFailure")
                {
                    strErrorText = xmlh.RootNode.FirstChild.InnerText;
                }
                else if (xmlh.RootNode.FirstChild.LocalName == "authenticationSuccess")
                {
                    strUserName = xmlh.GetChildElementValue(xmlh.RootNode.FirstChild, "cas:user");

                    //ext Infos
                    var exts = xmlh.GetFirstChildXmlNode(xmlh.RootNode.FirstChild, "cas:ext");
                    var dic = new Dictionary<string, string>();

                    if (exts != null)
                    {
                        for (var i = 0; i < exts.ChildNodes.Count; i++)
                        {
                            var ext = exts.ChildNodes.Item(i);

                            dic.Add(ext.LocalName, ext.InnerText);
                        }
                    }

                    //hand User
                    UserAuthenticateManager.Provider.SignUserLogin(strUserName, extDatas: dic);

                    User.UserInfo.SetCurrentUserName(strUserName);

                    isOK = true;
                }
            }
            catch (Exception e)
            {
                strErrorText = e.Message;

                Dev.Log.Loger.Error(e);
            }

            return isOK;
        }

        /// <summary>
        ///   登出的方法
        /// </summary>
        /// <param name="strRedirectUrl"> </param>
        /// <returns> </returns>
        public bool LoginOut(out string strRedirectUrl)
        {
            // if the logout parameter is given, redirect to CAS logout page

            strRedirectUrl = BuildLogoutRequest(StrCasServerUrl);

            UserAuthenticateManager.Provider.CurUserLoginOut();

            return true;
        }

        #endregion

        #region Class Methods

        /// <summary>
        /// 登录请求URL
        /// </summary>
        /// <param name="strCasServerUrl"></param>
        /// <param name="strService"></param>
        /// <returns></returns>
        private static string BuildLoginRequest(string strCasServerUrl, string strService)
        {
            return strCasServerUrl.TrimEnd('/') + "/login?service=" + HttpUtility.UrlEncode(strService);
        }
        /// <summary>
        /// 退出主求Url
        /// </summary>
        /// <param name="strCasServerUrl"></param>
        /// <returns></returns>
        private static string BuildLogoutRequest(string strCasServerUrl)
        {
            return strCasServerUrl.TrimEnd('/') + "/logout";
        }

        /// <summary>
        /// Validate Service Url
        /// </summary>
        /// <param name="strCasServerUrl"></param>
        /// <param name="strService"></param>
        /// <param name="strTicket"></param>
        /// <returns></returns>
        private static string BuildServiceValidateRequest(string strCasServerUrl, string strService, string strTicket)
        {
            return strCasServerUrl.TrimEnd('/') + "/serviceValidate?service=" + HttpUtility.UrlEncode(strService) +
                   "&ticket="
                   + strTicket;
        }

        #endregion
    }
}
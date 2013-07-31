// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 15:01
// 
// 修改于：2013年02月19日 15:10
// 文件名：CasClientConfiguration.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace Dev.CasClient.Configuration
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class CasClientConfiguration
    {
        #region Readonly & Static Fields

        private static CasClientConfiguration _config;

        private static ICasClientConfigurationStorage _configProvider;

        private static readonly object Lockobj = new object();

        #endregion

        #region Fields

        private string _localCheckPath;

        private string _localLogOffPath;
        private string _localLoginPath;
        private string _localParam;
        private string _returnUrlParm;
        private string _ticketName;

        #endregion

        #region Instance Properties

        /// <summary>
        /// </summary>
        [DataMember]
        public string CasPath { get; set; }

        /// <summary>
        ///   CAS的服务地址
        /// </summary>
        [DataMember]
        public string CasServerUrl { get; set; }

        ///<summary>
        ///  检测本地用户是否已经登录径
        ///</summary>
        [DataMember]
        public string LocalCheckPath
        {
            get
            {
                if (string.IsNullOrEmpty(this._localCheckPath))
                {
                    this._localCheckPath = "~/Account/LocalUserCheck";

                    this._localCheckPath = VirtualPathUtility.ToAbsolute(this._localCheckPath);
                }

                return this._localCheckPath;
            }
            set { this._localCheckPath = value; }
        }

        ///<summary>
        ///  本地退出请求路径
        ///</summary>
        [DataMember]
        public string LocalLogOffPath
        {
            get
            {
                if (string.IsNullOrEmpty(this._localLogOffPath))
                {
                    this._localLogOffPath = "~/Account/LogOff";
                    this._localLogOffPath = VirtualPathUtility.ToAbsolute(this._localLogOffPath);
                }
                return this._localLogOffPath;
            }
            set { this._localLogOffPath = value; }
        }

        ///<summary>
        ///  本地登录路径
        ///</summary>
        [DataMember]
        public string LocalLoginPath
        {
            get
            {
                if (string.IsNullOrEmpty(this._localLoginPath))
                {
                    this._localLoginPath = Utils.WebConfigUtils.FormsLoginUrl();

                    if (string.IsNullOrEmpty(this._localLoginPath))
                        this._localLoginPath = "~/Account/Login";

                    this._localLoginPath = VirtualPathUtility.ToAbsolute(this._localLoginPath);
                }
                return this._localLoginPath;
            }
            set { this._localLoginPath = value; }
        }

        /// <summary>
        ///   本地检测使用参数名
        /// </summary>
        [DataMember]
        public string LocalParam
        {
            get
            {
                if (string.IsNullOrEmpty(this._localParam))
                {
                    this._localParam = "local";
                }
                return this._localParam;
            }
            set { this._localParam = value; }
        }

        /// <summary>
        ///   系统中的返回地址 默认 "returnUrl"
        /// </summary>
        [DataMember]
        public string ReturnUrlParm
        {
            get
            {
                if (string.IsNullOrEmpty(this._returnUrlParm))
                {
                    this._returnUrlParm = "returnUrl";
                }
                return this._returnUrlParm;
            }
            set { this._returnUrlParm = value; }
        }

        /// <summary>
        ///   Ticket Name
        /// </summary>
        [DataMember]
        public string TicketName
        {
            get
            {
                if (string.IsNullOrEmpty(this._ticketName))
                {
                    this._ticketName = "ticket";
                }

                return this._ticketName;
            }
            set { this._ticketName = value; }
        }

        #endregion

        #region Class Properties

        /// <summary>
        ///   配置项
        /// </summary>
        public static CasClientConfiguration Config
        {
            get
            {
                lock (Lockobj)
                {
                    if (_config == null)
                    {
                        _config = ConfigProvider.Get();
                    }
                    return _config;
                }
            }
            set
            {
                _config = value;
                ConfigProvider.Save(_config);
            }
        }

        /// <summary>
        ///   提供者
        /// </summary>
        [XmlIgnore]
        public static ICasClientConfigurationStorage ConfigProvider
        {
            get
            {
                if (_configProvider == null)
                {
                    _configProvider = new XmlCasClientConfigurationStorage();
                    _configProvider.ConfigChangedEvent += ConfigProviderConfigChangedEvent;
                }
                return _configProvider;
            }
            set { _configProvider = value; }
        }

        #endregion

        #region Class Methods

        private static void ConfigProviderConfigChangedEvent(object sender, System.EventArgs e)
        {
            _config = ConfigProvider.Get();
        }

        #endregion
    }
}
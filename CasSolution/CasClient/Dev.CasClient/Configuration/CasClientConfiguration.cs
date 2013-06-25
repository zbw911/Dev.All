// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 15:01
// 
// 修改于：2013年02月19日 15:10
// 文件名：CasClientConfiguration.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasClient.Configuration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CasClientConfiguration
    {
        #region Static Fields

        private static CasClientConfiguration _config;

        private static ICasClientConfigurationStorage _configProvider;

        private static object lockobj = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// 配置项
        /// </summary>
        public static CasClientConfiguration Config
        {
            get
            {
                lock (lockobj)
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
            set
            {
                _configProvider = value;
            }
        }

        static void ConfigProviderConfigChangedEvent(object sender, System.EventArgs e)
        {
            _config = ConfigProvider.Get();
        }

        /// <summary>
        /// CAS的服务地址
        /// </summary>
        [DataMember]
        public string CasServerUrl { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CasPath { get; set; }

        #endregion
    }


}
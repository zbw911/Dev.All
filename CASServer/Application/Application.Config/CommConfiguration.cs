using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.Config
{
    [DataContract]
    public class CommConfiguration
    {
        private static CommConfiguration _config;
        private static IConfigurationStorage _configProvider;

        [XmlIgnore]
        public static IConfigurationStorage ConfigProvider
        {
            get
            {
                if (_configProvider == null)
                    _configProvider = new XmlConfigurationStorage();
                return _configProvider;
            }
            set { _configProvider = value; }
        }

        public static CommConfiguration Config
        {
            get
            {

                if (_config == null)
                    _config = ConfigProvider.Get();
                return _config;
            }
            set
            {
                _config = value;
                ConfigProvider.Save(_config);
            }
        }


        /// <summary>
        /// 默认的主页面
        /// </summary>
        [DataMember]
        public string CurrentUrl { get; set; }
        /// <summary>
        /// 短信接口地址
        /// </summary>
        [DataMember]
        public string SmsApi;
        /// <summary>
        /// 游戏团提供的API
        /// </summary>
        [DataMember]
        public string TuanApibase { get; set; }
    }
}
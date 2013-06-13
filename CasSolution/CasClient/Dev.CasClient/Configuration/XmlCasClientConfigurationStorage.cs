// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 15:01
// 
// 修改于：2013年02月19日 15:10
// 文件名：XmlCasClientConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasClient.Configuration
{
    using System;
    using System.IO;

    using Dev.Comm.Core.Runtime.Serialization;

    /// <summary>
    ///     xml保存，911
    /// </summary>
    internal class XmlCasClientConfigurationStorage : ICasClientConfigurationStorage
    {
        #region Public Methods and Operators

        public CasClientConfiguration Get()
        {
            return this.Get(null);
        }

        public CasClientConfiguration Get(string configname)
        {
            CasClientConfiguration instance;
            string xmlFile = GetSettingFile(configname);
            if (File.Exists(xmlFile))
            {
                instance = DataContractSerializationHelper.Deserialize<CasClientConfiguration>(xmlFile);
            }
            else
            {
                instance = new CasClientConfiguration
                {
                    CasServerUrl = "http://localhost",
                    CasPath = "/CAS"
                };
                this.Save(instance, configname);
            }

            return instance;
        }

        public void Save(CasClientConfiguration config)
        {
            this.Save(config, null);
        }

        public void Save(CasClientConfiguration config, string configname)
        {
            string settingFile = GetSettingFile(configname);
            DataContractSerializationHelper.Serialize(config, settingFile);
        }

        #endregion

        #region Methods

        private static string GetSettingFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = "CasClient.config";
            }
            string applicationBaseDirectory = null;
            applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(applicationBaseDirectory, filename);
        }

        #endregion
    }
}
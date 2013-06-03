// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月29日 11:06
// 
// 修改于：2013年02月18日 13:52
// 文件名：XmlCasServerConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm.Core.Runtime.Serialization;

namespace Application.Config
{
    /// <summary>
    /// xml保存，911
    /// </summary>
    class XmlConfigurationStorage : IConfigurationStorage
    {
        public CommConfiguration Get()
        {
            return Get(null);
        }

        public CommConfiguration Get(string configname)
        {
            CommConfiguration instance;
            var xmlFile = GetSettingFile(configname);
            if (File.Exists(xmlFile))
            {
                instance = DataContractSerializationHelper.Deserialize<CommConfiguration>(xmlFile);
            }
            else
            {
                instance = new CommConfiguration
                               {
                                   CurrentUrl = "http://caslocal.youxituan.com",
                                   SmsApi = "http://192.168.0.188:9595",
                                   TuanApibase = "http://apilocal.youxituan.com"
                               };
                this.Save(instance, configname);
            }

            return instance;
        }

        public void Save(CommConfiguration config)
        {
            Save(config, null);
        }


        public void Save(CommConfiguration config, string configname)
        {
            string settingFile = GetSettingFile(configname);
            DataContractSerializationHelper.Serialize<CommConfiguration>(config, settingFile);
        }



        private static string GetSettingFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = "Comm.config";
            }
            string applicationBaseDirectory = null;
            applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(applicationBaseDirectory, filename);
        }
    }
}
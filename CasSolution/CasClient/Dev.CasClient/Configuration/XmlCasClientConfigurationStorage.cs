// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 15:01
// 
// 修改于：2013年02月19日 15:10
// 文件名：XmlCasClientConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm.Runtime.Serialization;


namespace Dev.CasClient.Configuration
{
    /// <summary>
    ///   xml保存，911
    /// </summary>
    internal class XmlCasClientConfigurationStorage : ICasClientConfigurationStorage
    {
        #region Readonly & Static Fields

        private static System.IO.FileSystemWatcher filewatcher;

        #endregion

        #region Event Handling

        private void FlilewatcherChanged(object sender, FileSystemEventArgs e)
        {
            var type = e.ChangeType;
            this.ConfigChangedEvent(sender, e);
            //Log.Loger.Error("changed");
            //System.Diagnostics.Debug.WriteLine("changed");
            //throw new NotImplementedException();
        }

        #endregion

        #region ICasClientConfigurationStorage Members

        public CasClientConfiguration Get()
        {
            return this.Get(null);
        }

        public CasClientConfiguration Get(string configname)
        {
            CasClientConfiguration instance;
            var xmlFile = GetSettingFile(configname);
            if (File.Exists(xmlFile))
            {
                instance = DataContractSerializationHelper.Deserialize<CasClientConfiguration>(xmlFile);
            }
            else
            {
                instance = new CasClientConfiguration
                               {
                                   CasServerUrl = "http://passport.xxxxxx.com",
                                   CasPath = "/CAS",
                                   //LocalLoginPath = Utils.WebConfigUtils.FormsLoginUrl()
                               };
                this.Save(instance, configname);
            }

            if (filewatcher == null)
            {
                filewatcher = new System.IO.FileSystemWatcher();
                //filewatcher.
                filewatcher.Filter = Path.GetFileName(xmlFile);
                //filewatcher.NotifyFilter = NotifyFilters.LastWrite;
                filewatcher.Path = Path.GetDirectoryName(xmlFile).TrimEnd('\\') + "\\";
                filewatcher.Changed += this.FlilewatcherChanged;
                filewatcher.EnableRaisingEvents = true;
            }
            return instance;
        }

        public void Save(CasClientConfiguration config)
        {
            this.Save(config, null);
        }

        public void Save(CasClientConfiguration config, string configname)
        {
            var settingFile = GetSettingFile(configname);
            DataContractSerializationHelper.Serialize(config, settingFile);
        }

        public event EventHandler<EventArgs> ConfigChangedEvent;

        #endregion

        #region Class Methods

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
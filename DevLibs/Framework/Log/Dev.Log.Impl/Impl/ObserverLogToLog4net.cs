// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：ObserverLogToLog4net.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using log4net;

namespace Dev.Log.Impl
{
    public class ObserverLogToLog4net : ILog
    {
        #region ILog Members

        public void Log(object sender, LogEventArgs e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    getLog().Debug(e.Message, e.Exception);
                    break;
                case LogSeverity.Error:
                    getLog().Error(e.Message, e.Exception);
                    break;
                case LogSeverity.Fatal:
                    getLog().Fatal(e.Message, e.Exception);
                    break;
                case LogSeverity.Info:
                    getLog().Info(e.Message, e.Exception);
                    break;
                case LogSeverity.Warning:
                    getLog().Warn(e.Message, e.Exception);
                    break;
            }
        }

        #endregion

        private static log4net.ILog getLog()
        {
            //StackFrame frame = new StackFrame(4);

            //MethodBase method = frame.GetMethod();      //取得调用函数
            log4net.ILog log = LogManager.GetLogger("");
            return log;
        }
    }
}
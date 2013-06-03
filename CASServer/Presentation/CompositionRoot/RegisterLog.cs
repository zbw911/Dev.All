using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dev.Log;
using Dev.Log.Config;
using Dev.Web.CompositionRootBase;

namespace CompositionRoot
{
    class RegisterLog : IRegister
    {
        public Ninject.IKernel Kernel
        {
            get;
            set;
        }

        public void Register()
        {
            Setting.SetLogSeverity(LogSeverity.Debug);
            Setting.AttachLog(new Dev.Log.Impl.ObserverLogToLog4net());
        }
    }
}

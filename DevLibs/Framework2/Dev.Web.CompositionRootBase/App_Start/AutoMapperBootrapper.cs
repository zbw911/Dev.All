using System.Web;
using Dev.Web.CompositionRootBase.App_Start;

//[assembly: PreApplicationStartMethod(typeof(AutoMapperBootrapper), "Start")]
//[assembly: ApplicationShutdownMethod(typeof(AutoMapperBootrapper), "Stop")]

namespace Dev.Web.CompositionRootBase.App_Start
{
    using Dev.Crosscutting.Adapter.Adapter;
    using Dev.Crosscutting.Adapter.NetFramework.Adapter;

    internal class AutoMapperBootrapper
    {

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            //启动automapper
            var typeAdapterFactory = new AutomapperTypeAdapterFactory();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {

        }
    }
}
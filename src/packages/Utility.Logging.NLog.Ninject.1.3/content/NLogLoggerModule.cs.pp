
using Utility.Logging;
using Utility.Logging.NLog;
using Ninject.Modules;

namespace $rootnamespace$
{
  /// <summary>
  /// Ninject module to load factory and logger for NLog.
  /// </summary>
  public class NLogLoggerModule : NinjectModule
  {
    public override void Load()
    {
      Bind<ILoggerFactory>().To<NLogLoggerFactory>().InSingletonScope();
      Bind<ILogger>().ToMethod(
        context =>
          {
            if (context.Request.Target != null)
            {
              return ((ILoggerFactory) context.Kernel.GetService(typeof (ILoggerFactory))).GetLogger(context.Request.Target.Member.DeclaringType);
            }
            return ((ILoggerFactory) context.Kernel.GetService(typeof(ILoggerFactory))).GetLogger("UnknownTarget");
          });
    }
  }
}

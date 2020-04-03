using Microsoft.AspNet.SignalR;
using PMIS.Hubs;
using PMIS.Model;
using PMIS.ServiceLayer;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PMIS.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PMIS.App_Start.NinjectWebCommon), "Stop")]

namespace PMIS.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            return Container;
        }

        public static T GetConcreteInstance<T>()
        {
            object instance = Container.TryGet<T>();
            if (instance != null)
                return (T)instance;
            throw new InvalidOperationException(string.Format("Unable to create an instance of {0}", typeof(T).FullName));
        }

        public static IKernel _container;

        private static IKernel Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new StandardKernel();
                    _container.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                    _container.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                   
                    RegisterServices(_container);
                }
                return _container;
            }
        }


        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
          
            kernel.Bind<PMISEntities>().ToSelf().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IAppointmentServices>().To<AppointmentServices>();
            kernel.Bind<IPatientServices>().To<PatientServices>();
            kernel.Bind<IPatientRecordServices>().To<PatientRecordServices>();
            kernel.Bind<IPhycisianServices>().To<PhycisianServices>();
            kernel.Bind<IUserPhysicianService>().To<UserPhysicianService>();
           

        }        
    }
}
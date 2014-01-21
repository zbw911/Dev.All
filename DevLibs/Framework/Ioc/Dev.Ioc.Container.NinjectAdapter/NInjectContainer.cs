// ***********************************************************************************
// Created by zbw911 
// �����ڣ�2012��12��18�� 13:28
// 
// �޸��ڣ�2013��02��18�� 18:24
// �ļ�����NInjectContainerAdapter.cs
// 
// ����и��õĽ����������ʼ���zbw911#gmail.com
// ***********************************************************************************


using System;
using Ninject;

namespace Dev.Ioc.Container.NinjectAdapter
{
    /// <summary>
    /// <see cref="IContainer"/> implementation for NInject.
    /// </summary>
    public class NInjectContainer : ContainerImplBase
    {
        readonly IKernel _kernel;

        /// <summary>
        /// Default Constructor.
        /// Creates an instance of <see cref="NInjectContainer"/> class.
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/> instance used by the NInjectContainerAdapter
        /// to register components.</param>
        public NInjectContainer(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override void Register(Type service, Type implementation)
        {
            _kernel.Bind(service).To(implementation);
        }
 

        public override void RegisterWithConstructor(Type service, Type implementation, string named, params Parameter[] parameter)
        {
            var bind = _kernel.Bind(service).To(implementation);

            if (!string.IsNullOrEmpty(named))
                bind.Named(named);

            foreach (var parameter1 in parameter)
            {
                var x = bind.WithConstructorArgument(parameter1.Name, parameter1.Value);
            }
        }

        public override void Register(Type service, Type implementation, string named)
        {
            _kernel.Bind(service).To(implementation).Named(named);
        }

        public override void RegisterSingleton(Type service, Type implementation)
        {
            _kernel.Bind(service).To(implementation).InSingletonScope();
        }

        public override void RegisterSingleton(Type service, Type implementation, string named)
        {
            _kernel.Bind(service).To(implementation).InSingletonScope().Named(named);
        }

        public override void RegisterInstance(Type service, object instance)
        {
            _kernel.Bind(service).ToConstant(instance);
        }

        public override void RegisterInstance(Type service, object instance, string named)
        {
            _kernel.Bind(service).ToConstant(instance).Named(named);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;

namespace ChallengeMeLiServices.Web.Unity
{
    /// <summary>
    /// Resolves unity containers.
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        /// <summary>
        /// The Unity Container.
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolver"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when container is null.</exception>
        /// <param name="container">the container</param>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        /// <summary>
        /// Retrieve service for given service type.
        /// </summary>
        /// <param name="serviceType">the service type</param>
        /// <returns>the service</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve collection of services for given service type.
        /// </summary>
        /// <param name="serviceType">the service type</param>
        /// <returns>a list of services</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>the started scope</returns>
        public IDependencyScope BeginScope()
        {
            IUnityContainer child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Dependencies;
using Unity;

namespace ChallengeMeLiServices.Web.Unity
{
    /// <summary>
    /// Resolves unity containers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityResolver : IDependencyResolver
    {
        /// <summary>
        /// The Unity Container.
        /// </summary>
        private IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolver"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when container is null.</exception>
        /// <param name="container">the container</param>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            _container = container;
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
                return _container.Resolve(serviceType);
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
                return _container.ResolveAll(serviceType);
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
            IUnityContainer child = _container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        /// <param name="disposing">true for clean up managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }

            // free native resources if there are any.
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.DistributedTask.ServiceEndpoints;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.TeamServices.Samples.Client.ServiceEndpoints
{
    /// <summary>
    /// The service endpoint types sample
    /// </summary>
    [ClientSample(ServiceEndpointResourceIds.AreaName, ServiceEndpointResourceIds.EndpointTypeResource.Name)]
    class ServiceEndpointTypesSample : ClientSample
    {
        /// <summary>
        /// The get service endpoint types.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpointType}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpointType> GetServiceEndpointTypes()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get service endpoint types
            var serviceEndpointTypes = serviceEndpointClient.GetServiceEndpointTypesAsync().Result;

            // Show the received service endpoint types
            serviceEndpointTypes.ForEach((ServiceEndpointType se) =>
            {
                Console.WriteLine(se.DisplayName);
            });
            return serviceEndpointTypes;
        }

        /// <summary>
        /// The get service endpoint types.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpointType}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpointType> GetServiceEndpointTypesByScheme()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            string scheme = "Token";

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get service endpoint types by scheme
            var serviceEndpointTypes = serviceEndpointClient.GetServiceEndpointTypesAsync(scheme: scheme).Result;

            // Show the received service endpoint types
            serviceEndpointTypes.ForEach((ServiceEndpointType se) =>
            {
                Console.WriteLine(se.DisplayName);
            });

            return serviceEndpointTypes;
        }
    }
}
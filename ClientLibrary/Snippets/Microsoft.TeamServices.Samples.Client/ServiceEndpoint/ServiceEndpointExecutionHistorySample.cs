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
    /// The service endpoint execution history sample
    /// </summary>
    [ClientSample(ServiceEndpointResourceIds.AreaName, ServiceEndpointResourceIds.ExectionHistoryResource.Name)]
    class ServiceEndpointExecutionHistorySample : ClientSample
    {
        /// <summary>
        /// The get service endpoint execution records.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpointExecutionRecord}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpointExecutionRecord> GetServiceEndpointExecutionRecords()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            Guid endpointId = new Guid("18807d4f-3345-4888-b2dc-f2884e4eb235");

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get service endpoint execution records
            var serviceEndpointExecutionRecords = serviceEndpointClient.GetServiceEndpointExecutionRecordsAsync(project: projectName, endpointId: endpointId).Result;

            // Show the received service endpoint execution records
            serviceEndpointExecutionRecords.ForEach((ServiceEndpointExecutionRecord record) =>
            {
                Console.WriteLine("{0} {1}", record.EndpointId.ToString(), record.Data.PlanType);
            });

            return serviceEndpointExecutionRecords;
        }
    }
}

using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TeamServices.Samples.Client.Services
{
    /// <summary>
    /// The service endpoints sample
    /// </summary>
    [ClientSample(TaskResourceIds.Area2Name, TaskResourceIds.ServiceEndpointsResource)]
    class ServiceEndpointSample : ClientSample
    {
        /// <summary>
        /// The added service endpoint id.
        /// </summary>
        private Guid addedServiceEndpointId;

        /// <summary>
        /// The added service endpoint name.
        /// </summary>
        private string addedServiceEndpointName;

        /// <summary>
        /// The create a service endpoint.
        /// </summary>
        /// <returns>
        /// The see <cref="ServiceEndpoint">.
        /// </returns>
        [ClientSampleMethod]
        public ServiceEndpoint CreateServiceEndpoint()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            var serviceEndpoint = new ServiceEndpoint
            {
                Id = Guid.NewGuid(),
                Name = "TestEndpoint",
                Type = "Azure",
                Authorization =  new EndpointAuthorization()
                                 {
                                    Scheme = EndpointAuthorizationSchemes.Certificate,
                                    Parameters =
                                        {
                                            {
                                                EndpointAuthorizationParameters.Certificate,
                                                "dummyCertificate"
                                            }
                                        }
                                 }
            };

            serviceEndpoint.Data.AddRange(
                new Dictionary<string, string>()
                    {
                        { "SubscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "SubscriptionName", "TestSubscriptionName" }
                    }
            );

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create variable group
            ServiceEndpoint addedServiceEndpoint = serviceEndpointClient.CreateServiceEndpointAsync(project: projectName, endpoint: serviceEndpoint).Result;
            this.addedServiceEndpointId = addedServiceEndpoint.Id;
            this.addedServiceEndpointName = addedServiceEndpoint.Name;

            // Show the added variable group
            Console.WriteLine("{0} {1}", addedServiceEndpoint.Id.ToString(), addedServiceEndpoint.Name);
            
            return addedServiceEndpoint;
        }

        /// <summary>
        /// The update a service endpoint.
        /// </summary>
        /// <returns>
        /// The see <cref="ServiceEndpoint">.
        /// </returns>
        [ClientSampleMethod]
        public ServiceEndpoint UpdateServiceEndpoint()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            List<string> endpointNames = new List<string>() { this.addedServiceEndpointName };

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get service endpoint to update
            ServiceEndpoint serviceEndpoint = serviceEndpointClient.GetServiceEndpointsByNamesAsync(project: projectName, endpointNames: endpointNames).Result.First();
            
            // Updated name in service endpoint
            serviceEndpoint.Name = "Updated TestEndpoint";

            // Update the service endpoint
            ServiceEndpoint updatedServiceEndpoint = serviceEndpointClient.UpdateServiceEndpointAsync(project: projectName, endpointId: this.addedServiceEndpointId, endpoint: serviceEndpoint).Result;
            this.addedServiceEndpointName = updatedServiceEndpoint.Name;

            // Show the updated service endpoint
            Console.WriteLine("{0} {1}", updatedServiceEndpoint.Id.ToString(), updatedServiceEndpoint.Name);
            
            return updatedServiceEndpoint;
        }

        /// <summary>
        /// The get service endpoint by Id.
        /// </summary>
        /// <returns>
        /// The <see cref="ServiceEndpoint">.
        /// </returns>
        [ClientSampleMethod]
        public ServiceEndpoint GetServiceEndpointDetails()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create second service endpoint
            ServiceEndpoint serviceEndpoint = new ServiceEndpoint
            {
                Id = Guid.NewGuid(),
                Name = "TestEndpoint2",
                Type = "Generic",
                Url = new Uri("http://testUrl"),
                Authorization = new EndpointAuthorization()
                {
                    Scheme = EndpointAuthorizationSchemes.UsernamePassword,
                    Parameters =
                        {
                            { "Username", "username" },
                            { "Password", "password" }
                        }
                }
            };

            ServiceEndpoint addedServiceEndpoint = serviceEndpointClient.CreateServiceEndpointAsync(project: projectName, endpoint: serviceEndpoint).Result;
            Guid addedServiceEndpointId = addedServiceEndpoint.Id;

            // Get service endpoint by Id
            serviceEndpoint = serviceEndpointClient.GetServiceEndpointDetailsAsync(project: projectName, endpointId: addedServiceEndpointId).Result;

            // Show the received service endpoint
            Console.WriteLine("{0} {1}", serviceEndpoint.Id.ToString(), serviceEndpoint.Name);

            return serviceEndpoint;
        }

        /// <summary>
        /// The get service endpoints by names.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpoint}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpoint> GetServiceEndpointsByNames()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create third service endpoint
            var serviceEndpoint = new ServiceEndpoint
            {
                Id = Guid.NewGuid(),
                Name = "TestEndpoint3",
                Type = "Azure",
                Authorization = new EndpointAuthorization()
                {
                    Scheme = EndpointAuthorizationSchemes.UsernamePassword,
                    Parameters =
                        {
                            { "Username", "username" },
                            { "Password", "password" }
                        }
                }
            };

            serviceEndpoint.Data.AddRange(
                new Dictionary<string, string>()
                    {
                        { "SubscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "SubscriptionName", "TestSubscriptionName" }
                    }
            );

            ServiceEndpoint addedServiceEndpoint = serviceEndpointClient.CreateServiceEndpointAsync(project: projectName, endpoint: serviceEndpoint).Result;
            
            // Create list of names
            string serviceEndpoint1Name = this.addedServiceEndpointName;
            string serviceEndpoint2Name = addedServiceEndpoint.Name;
            List<string> endpointNames = new List<string>() { serviceEndpoint1Name, serviceEndpoint2Name };
            
            // Get service endpoints by name
            List<ServiceEndpoint> serviceEndpoints = serviceEndpointClient.GetServiceEndpointsByNamesAsync(project: projectName, endpointNames: endpointNames).Result;

            // Show the received service endpoints
            serviceEndpoints.ForEach((ServiceEndpoint se) =>
            {
                Console.WriteLine("{0} {1}", se.Id.ToString(), se.Name);
            });
            
            return serviceEndpoints;
        }

        /// <summary>
        /// The get service endpoints.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpoint}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpoint> GetServiceEndpoints()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create fourth service endpoint
            var serviceEndpoint = new ServiceEndpoint
            {
                Id = Guid.NewGuid(),
                Name = "TestEndpoint3",
                Type = "Azure",
                Authorization = new EndpointAuthorization()
                {
                    Scheme = EndpointAuthorizationSchemes.None,
                    Parameters =
                        {
                            { "nugetkey", "dummykey" }
                        }
                }
            };

            ServiceEndpoint addedServiceEndpoint = serviceEndpointClient.CreateServiceEndpointAsync(project: projectName, endpoint: serviceEndpoint).Result;

            // Create get params
            string type = "Azure";
            List<string> authSchemes = new List<string>() { EndpointAuthorizationSchemes.Certificate, EndpointAuthorizationSchemes.None };

            // Get service endpoints by name
            List<ServiceEndpoint> serviceEndpoints = serviceEndpointClient.GetServiceEndpointsAsync(project: projectName, type: type, authSchemes: authSchemes).Result;

            // Show the received service endpoints
            serviceEndpoints.ForEach((ServiceEndpoint se) =>
            {
                Console.WriteLine("{0} {1}", se.Id.ToString(), se.Name);
            });

            return serviceEndpoints;
        }

        /// <summary>
        /// The update service endpoints.
        /// </summary>
        /// <returns>
        /// The <see cref="List{ServiceEndpoint}">.
        /// </returns>
        [ClientSampleMethod]
        public List<ServiceEndpoint> UpdateServiceEndpoints()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            string type = "Azure";

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get service endpoints to update
            List<ServiceEndpoint> serviceEndpoints = serviceEndpointClient.GetServiceEndpointsAsync(project: projectName, type: type).Result;

            // Updated url in service endpoints
            serviceEndpoints.ForEach((ServiceEndpoint se) =>
            {
                se.Url = new Uri("http://testUrl//" + se.Name);
            });

            // Update the service endpoints
            List<ServiceEndpoint> updatedServiceEndpoints = serviceEndpointClient.UpdateServiceEndpointsAsync(project: projectName, endpoints: serviceEndpoints).Result;

            // Show the updated service endpoints
            serviceEndpoints.ForEach((ServiceEndpoint se) =>
            {
                Console.WriteLine("{0} {1} {2}", se.Id.ToString(), se.Name, se.Url);
            });

            return updatedServiceEndpoints;
        }

        /// <summary>
        /// The delete a service endpoint.
        /// </summary>
        [ClientSampleMethod]
        public void DeleteServiceEndpoint()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Delete the already created task group
            serviceEndpointClient.DeleteServiceEndpointAsync(project: projectName, endpointId: this.addedServiceEndpointId).SyncResult();
        }
    }
}

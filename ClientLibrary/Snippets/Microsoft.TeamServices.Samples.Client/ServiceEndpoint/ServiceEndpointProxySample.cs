using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.DistributedTask.ServiceEndpoints;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.TeamServices.Samples.Client.ServiceEndpoints
{
    /// <summary>
    /// The service endpoint proxy sample
    /// </summary>
    [ClientSample(ServiceEndpointResourceIds.AreaName, ServiceEndpointResourceIds.EndpointProxyResource.Name)]
    class ServiceEndpointProxySample : ClientSample
    {
        /// <summary>
        /// The added service endpoint id.
        /// </summary>
        private Guid addedServiceEndpointId;

        private ServiceEndpoint CreateServiceEndpoint()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            var serviceEndpoint = new ServiceEndpoint
            {
                Name = "JenkinsEndpoint",
                Id = Guid.NewGuid(),
                Type = "jenkins",
                Url = new Uri("http://redvstt-lab43:8080"),
                Authorization = new EndpointAuthorization()
                {
                    Scheme = "UsernamePassword",
                    Parameters =
                        {
                            { "Username", "admin" },
                            { "Password", "jenkins2017" }
                        }
                }
            };

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create service endpoint
            ServiceEndpoint addedServiceEndpoint = serviceEndpointClient.CreateServiceEndpointAsync(project: projectName, endpoint: serviceEndpoint).Result;
            this.addedServiceEndpointId = addedServiceEndpoint.Id;

            // Show the added service endpoint
            Console.WriteLine("{0} {1}", addedServiceEndpoint.Id.ToString(), addedServiceEndpoint.Name);

            return addedServiceEndpoint;
        }

        /// <summary>
        /// The execute service endpoint request.
        /// </summary>
        /// <returns>
        /// The <see cref="ServiceEndpointRequestResult">.
        /// </returns>
        [ClientSampleMethod]
        public ServiceEndpointRequestResult ExecuteServiceEndpointRequestAsync()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Create service endpoint
            CreateServiceEndpoint();

            var dataSourceBinding = new DataSourceBinding
            {
                EndpointId = null
            };

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient serviceEndpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Create service endpoint request
            ServiceEndpointRequest serviceEndpointRequest = new ServiceEndpointRequest()
            {
                DataSourceDetails = new DataSourceDetails()
                {
                    DataSourceName = "Jobs"
                },
                ResultTransformationDetails = new ResultTransformationDetails()
                {
                    ResultTemplate = "{{#addField jobs 'parentPath' 'name' '/'}}{{#recursiveSelect jobs}}{{#notEquals _class 'com.cloudbees.hudson.plugins.folder.Folder'}}{{#notEquals _class 'org.jenkinsci.plugins.workflow.job.WorkflowJob'}}{ \"Value\" : \"{{#if parentPath}}{{parentPath}}/{{/if}}{{name}}\", \"DisplayValue\" : \"{{#if parentPath}}{{parentPath}}/{{/if}}{{{displayName}}}\" }{{/notEquals}}{{/notEquals}}{{/recursiveSelect}}{{/addField}}"
                }
            };

            ServiceEndpointRequestResult serviceEndpointRequestResult = serviceEndpointClient.ExecuteServiceEndpointRequestAsync(project: projectName, endpointId: this.addedServiceEndpointId.ToString(), serviceEndpointRequest: serviceEndpointRequest).Result;

            // Show the request result
            Console.WriteLine("{0}", serviceEndpointRequestResult.Result);
            return serviceEndpointRequestResult;
        }
    }
}

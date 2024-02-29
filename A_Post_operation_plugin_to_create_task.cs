using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace  A_Post_operation_plugin_to_create_task
{

    /// <summary>
    /// Post-Operation Plugin.
    /// A plug-in that receives data from another plug-in through the SharedVariables property of IPluginExecutionContext.
    /// This plugin verifies and retrieves a SharedVariable named PrimaryContact from context.SharedVariables
    /// and create a follow up task.
    /// </summary>
    /// <remarks>
    /// Register the PostEventPluginC plug-in 
    /// Message: Create
    /// Primary Entity: account
    /// Event Pipeline Stage: Post-Operation
    /// </remarks>
    public class CreateTaskforContact : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            // Obtain the execution context from the service provider.
            var context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)
                serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));

            // Obtain the contact from the execution context shared variables.
            if (context.SharedVariables.Contains("PrimaryContact"))
            {
                //receiving primary contact ID from preoperation Plugin as per below code
                var contactId = new Guid((string)context.SharedVariables["PrimaryContact"]);
                if (contactId != Guid.Empty)
                {
                    //Create a FollowUp task
                    var followup = new Entity("task");

                    followup["subject"] = "Shared Variables examples";
                    followup["description"] =
                        "Follow up with the customer. Check if there are any new issues that need resolution.";
                    followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    followup["scheduledend"] = DateTime.Now.AddDays(7);
                    string regardingobjectidType = "contact";
                    followup["regardingobjectid"] = new EntityReference(regardingobjectidType, contactId);



                    // Obtain the organization service reference.
                    var serviceFactory =
                        (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    var service = serviceFactory.CreateOrganizationService(context.UserId);

                    // Create the task in Microsoft Dynamics CRM.
                    service.Create(followup);
                }

            }
        }
    }
}



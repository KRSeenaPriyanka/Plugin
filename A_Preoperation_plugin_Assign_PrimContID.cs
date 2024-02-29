using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace A_Preoperation_plugin_Assign_PrimContID
{

    /// <summary>
    /// Pre-Operation Plugin.
    /// A plug-in that sends data to another plug-in through the SharedVariables property of IPluginExecutionContext.
    /// This plugin assigns a valid ContactId value to a SharedVariable named PrimaryContact.
    /// </summary>
    /// <remarks>
    /// Register the PreEventPlugin for a Pre-Operation event or stage
    /// Message: Create
    /// Primary Entity: account
    /// Event Pipeline Stage: Pre-Operation
    /// </remarks>
    public class PreoperationPluginpassingPrimaryContactID : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));


            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference.
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {
                if (context.InputParameters.Contains("Target") &&
                               context.InputParameters["Target"] is Entity)
                {


                    // Obtain the target entity from the input parameters.
                    Entity entity = (Entity)context.InputParameters["Target"];
                    // Verify that the target entity represents an account.
                    // If not, this plug-in was not registered correctly.
                    if (entity.LogicalName != "account")
                        return;

                    EntityReference primaryConatcID = (EntityReference)entity.Attributes["primarycontactid"];

                    Guid contactId = primaryConatcID.Id;//Narasimha

                    context.SharedVariables.Add("PrimaryContact", (Object)contactId.ToString());

                    //This plugin is doing what
                    //it will get primary contact ID and stored in to variable called
                    //PrimaryContact


                }

            }
            catch (Exception ex)
            {
            }
        }

    }
}


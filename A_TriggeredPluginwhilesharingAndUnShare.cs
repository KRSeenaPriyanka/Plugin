
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using System.Xml;
using Microsoft.Xrm.Sdk.Query;

namespace A_TriggeredPluginwhilesharingAndUnShare
{
    public class TriggeredpluginonshareandUnShare:IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            //grant access == share
            //Revoke Access = Unshare
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

                //Obtain the target entity from the input parameter.
                EntityReference EntityRef = (EntityReference)context.InputParameters["Target"];
                if (context.MessageName == "GrantAccess") // Share 
                {
                    Guid createdRecordId = createEntityRecord("task", service);
                }


                if (context.MessageName == "RevokeAccess") //Unshare
                {
                    Guid createdRecordId = createEntityRecordifYouUnShare("task", service);
                }

            }
            catch (Exception ex)
            {
            }

        }

        public Guid createEntityRecord(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "task created after shared the Record.";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }

        public Guid createEntityRecordifYouUnShare(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "task created after UnShared the Record.";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }
    }
}
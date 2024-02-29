
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
namespace A_triggeredpluginonQuote
{
    public class TriggeredPluginonQuote:IPlugin
    {
        //messae : setstateDynamics
        //Entity : quote
        //stae : preoperation or post operation


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

            bool isCorrectEvent = context.MessageName == "SetStateDynamicEntity" || context.MessageName == "SetState" || context.MessageName == "Win" || context.MessageName == "Close";
            bool hasEnityMoniker = context.InputParameters.Contains("EntityMoniker");


            // get the reference to the quote entity
            EntityReference quoteEntityReference = (EntityReference)context.InputParameters["EntityMoniker"];
            // ensure we are handling the correct event and we were passed an entity from the context
            if (!isCorrectEvent || !hasEnityMoniker) return;
            // ensure that we have the correct entity
            if (quoteEntityReference.LogicalName != "quote") return;


            try
            {
                Guid createdRecordId = createEntityRecord("task", service);

            }
            catch (Exception ex)
            {
            }

        }


        public Guid createEntityRecord(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "Quote status Changd ";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }
    }
}



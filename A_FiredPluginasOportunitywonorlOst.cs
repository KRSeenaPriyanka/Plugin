using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace A_FiredPluginasOportunitywonorlOst
{
    public class A_FiredPluginasOportunitywonorlOst:IPlugin
    {
        //messae -- Win
        //Entity ---Opportunity -- 
        //stage --- Post operation

        //or IF below scenario

        //messae -- Lose
        //Entity ---Opportunity -- 
        //stage --- Post operation
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
                //if (context.InputParameters.Contains("Target") &&
                //              context.InputParameters["Target"] is Entity)
                //{


                if (context.InputParameters.Contains("OpportunityClose") && context.InputParameters["OpportunityClose"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["OpportunityClose"];
                    Guid createdRecordId = createEntityRecord("task", service);
                }





                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                //throw new InvalidPluginExecutionException("fcjfjfjxf");
                //}
            }
            catch (Exception ex)
            {
            }
        }

        public Guid createEntityRecord(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "WinOportunity.";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }
    }
}
    
    


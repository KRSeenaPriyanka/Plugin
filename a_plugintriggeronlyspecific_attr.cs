using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;

namespace A_plugintriggeronlyspecific_Attr
{
    public class A_plugintriggeronlyspecific_Attr:IPlugin
    {

        public void Execute(IServiceProvider IserviceProvider)
        {
            //here it will goes to write the business logic

            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
            (ITracingService)IserviceProvider.GetService(typeof(ITracingService));


            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
            IserviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference.
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)IserviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            Guid UserID = (Guid)context.InitiatingUserId;

            //cd3e4a7b-9019-ec11-b6e6-000d3ac9f64c   --> Nagalatha 2
            //d2e059dd-041d-ec11-b6e7-000d3ac9fbde -->Ramana

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
                    // Create a task activity to follow up with the account customer in 7 days. 
                    Entity Task = new Entity("task");//task

                    Task["subject"] = "Send e-mail to the new customer.";
                    Task["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
                    Task["scheduledstart"] = DateTime.Now.AddDays(7);
                    Task["scheduledend"] = DateTime.Now.AddDays(7);


                    service.Create(Task);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
    

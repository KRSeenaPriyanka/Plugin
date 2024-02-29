using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;

namespace A_Create_Task_After_Account
{
    public class A_Create_Task_After_Account:IPlugin
    
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            
            //here it will goes to write the business logic

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

                    EntityReference taskRegarding = null;

                    if (context.OutputParameters.Contains("id")) //Account Record GUID
                    {
                        //f7afa88c-6722-ec11-b6e6-000d3af2aa67  //account ID
                        Guid regardingobjectId = new Guid(context.OutputParameters["id"].ToString());//GUID

                        string regardingobjectidType = "account";

                        taskRegarding = new EntityReference(regardingobjectidType, regardingobjectId);

                        Guid createdRecordId = createEntityRecord("task", taskRegarding, service);
                    }
                }
            }
            catch (Exception ex)
            {
            }


        }


        public Guid createEntityRecord(string entityName, EntityReference regardingObject, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity Task = new Entity(entityName);//task

            Task["subject"] = "Send e-mail to the new customer.";
            Task["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            Task["scheduledstart"] = DateTime.Now.AddDays(7);
            Task["scheduledend"] = DateTime.Now.AddDays(7);
            Task["category"] = regardingObject.LogicalName;//account
            Task["regardingobjectid"] = regardingObject; // account gUID

            return crmService.Create(Task);
        }
        

    }
}




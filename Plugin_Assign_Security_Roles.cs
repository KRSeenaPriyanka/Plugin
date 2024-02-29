using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace Plugin_Assign_Security_Roles
{
    public class Plugin_Assign_Security_Roles :IPlugin
    {

        //step 1: Register a plugin  on message Associate , primary and secondary entity as none
        //step2:   AssignUserRoles as a message and primary entity as a Role

        //stage : post operation

        // Or if you want trigger a plugin  - - when ever remove security roles

        //step 1: Register a plugin  on message DeAssociate , primary and secondary entity as none
        //step2:   RemoveUserRoles as a message and primary entity as a Role

        //stage : post operation


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

            if (context.MessageName == "Associate" || context.MessageName == "AssignUserRole")
            {
                // Obtain the target entity from the input parameters.

                EntityReference entity = (EntityReference)context.InputParameters["Target"];

                // Verify that the target entity represents an entity type you are expecting.
                // For example, an user entity If not, the plug-in was not registered correctly.
                if (entity.LogicalName.ToUpper() != "SYSTEMUSER")
                    return;

                Guid createdRecordId = createEntityRecord("task", service);
            }

            if (context.MessageName == "Disassociate" || context.MessageName == "RemoveUserRoles")
            {
                // Obtain the target entity from the input parameters.

                EntityReference entity = (EntityReference)context.InputParameters["Target"];

                // Verify that the target entity represents an entity type you are expecting.
                // For example, an user entity If not, the plug-in was not registered correctly.
                if (entity.LogicalName.ToUpper() != "SYSTEMUSER")
                    return;

                Guid createdRecordId = createEntityRecordforDeassociate("task", service);
            }
        }

        public Guid createEntityRecord(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "Assign Security Roles.";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }

        public Guid createEntityRecordforDeassociate(string entityName, IOrganizationService crmService)
        {
            // Create a task activity to follow up with the account customer in 7 days. 
            Entity followup = new Entity(entityName);//task

            followup["subject"] = "Removed Security Roles.";
            followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);


            return crmService.Create(followup);
        }
    }
}
   
    


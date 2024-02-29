using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
namespace Account_Images
{
    /// <summary>
    /// Register this plugin - preoperTION  - account entity--update
    /// </summary>
    public class PluginImages:IPlugin
    {
        Entity preMessageImage;
        Entity postMessageImage;
        string oldcountry;
        public void Execute(IServiceProvider serviceProvider)
        {

            //throw new InvalidPluginExecutionException("Hi");
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference.
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService service = serviceFactory.CreateOrganizationService(null);




            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];
                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "account")
                    return;

                Guid AccountID = context.PrimaryEntityId;

          
                // Guid AccountID = context.PrimaryEntityId;

                //Guid contactId = primaryConatcID.Name;

                //Entity contact = service.Retrieve("account", AccountID, new ColumnSet("address1_county"));

                if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity)
                {

                    preMessageImage = (Entity)context.PreEntityImages["PreImage"];

                    oldcountry = (String)preMessageImage.Attributes["address1_county"];
                    //Entity account = service.Retrieve("account", AccountID, new ColumnSet("new_primarycontactjobtitle"));

                    //string OldCounty = account.GetAttributeValue<string>("address1_county");
                    if (entity.Attributes.Contains("address1_county") == true)
                    {
                        string currentPrimarycontactJobTitle = entity.Attributes["address1_county"].ToString();
                    }


                    if (oldcountry == "India")
                    {
                        Guid createdRecordId = createEntityRecord("task", service, oldcountry);
                    }

                    //if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity)
                    //{

                    //    postMessageImage = (Entity)context.PostEntityImages["PostImage"];

                    //    string oldValuecity = (String)postMessageImage.Attributes["new_primarycontactjobtitle"];

                    //    string currentValueCity = entity.Attributes.Contains("new_primarycontactjobtitle").ToString();
                    //}

                }
            }
        }

             
            

            public Guid createEntityRecord(string entityName, IOrganizationService crmService, string oldPrimarycontactJobTitle)
            {
                // Create a task activity to follow up with the account customer in 7 days. 
                Entity followup = new Entity(entityName);//task

                followup["subject"] = "your previous Job Title is." + oldPrimarycontactJobTitle;
                followup["description"] = "Follow up with the customer. Check if there are any new issues that need resolution.";
                followup["scheduledstart"] = DateTime.Now.AddDays(7);
                followup["scheduledend"] = DateTime.Now.AddDays(7);


                return crmService.Create(followup);
            }
    
        
    }
}

 






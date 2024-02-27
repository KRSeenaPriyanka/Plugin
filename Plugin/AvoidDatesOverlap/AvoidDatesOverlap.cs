using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidDatesOverlap
{
    public class AvoidDatesOverlap : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {

                Entity entity = (Entity)context.InputParameters["Target"];

                DateTime startDate = entity.GetAttributeValue<DateTime>("cc_startdate");
                DateTime endDate = entity.GetAttributeValue<DateTime>("cc_enddate");


                if (entity.LogicalName.ToLower() != "cc_travel")
                    return;
                if (context.MessageName.ToLower() == "create")
                {
                    tracingService.Trace("start create");
                    if (entity.Attributes.Contains("cc_startdate") && entity.Attributes.Contains("cc_enddate"))
                    {
                        tracingService.Trace("start create1");
                        FetchExpression fetchXml = FetchTravelByStartAndEndDate(startDate, endDate);
                        DataCollection<Entity> TravelsResult = service.RetrieveMultiple(fetchXml).Entities;

                        if (TravelsResult != null && TravelsResult.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("This travel is overlapping with other planned travels");
                        }
                    }
                }
            }

        }

        public static FetchExpression FetchTravelByStartAndEndDate(DateTime startDate, DateTime endDate)
        {
            var fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='cc_travel'>
                                <attribute name='cc_travelid' />
                                <attribute name='cc_name' />
                                <attribute name='createdon' />
                                <order attribute='cc_name' descending='false' />
                                <filter type='or'>
                                <filter type='and'>
                                  <condition attribute='cc_startdate' operator='on-or-after' value='{startDate}' />
                                  <condition attribute='cc_startdate' operator='on-or-before' value='{endDate}' />
                                </filter>
                                <filter type='and'>
                                  <condition attribute='cc_enddate' operator='on-or-before' value='{endDate}' />
                                  <condition attribute='cc_enddate' operator='on-or-after' value='{startDate}' />
                                </filter>
                                </filter>
                              </entity>
                            </fetch> ";
            return new FetchExpression(fetchXml);

        }
    }
}

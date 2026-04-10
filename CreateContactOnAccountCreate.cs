using System;
using Microsoft.Xrm.Sdk;

public class CreateContactOnAccountCreate : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));

        var tracingService = (ITracingService)
            serviceProvider.GetService(typeof(ITracingService));

        var factory = (IOrganizationServiceFactory)
            serviceProvider.GetService(typeof(IOrganizationServiceFactory));

        var service = factory.CreateOrganizationService(context.UserId);

        tracingService.Trace("Plugin Started");

        if (context.InputParameters.Contains("Target") &&
            context.InputParameters["Target"] is Entity entity)
        {
            if (entity.LogicalName != "account")
                return;

            tracingService.Trace("Account detected");

            string accountName = entity.GetAttributeValue<string>("name");

            Entity contact = new Entity("contact");

            contact["firstname"] = accountName; 
            contact["parentcustomerid"] = new EntityReference("account", entity.Id);

            service.Create(contact);

            tracingService.Trace("Contact Created Successfully");
        }

        tracingService.Trace("Plugin Completed");
    }
}

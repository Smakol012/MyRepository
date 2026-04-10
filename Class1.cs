using System;
using Microsoft.Xrm.Sdk;

public class AccCreateplugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));

        var tracingService = (ITracingService)
            serviceProvider.GetService(typeof(ITracingService));

        tracingService.Trace("START");

        if (context.InputParameters.Contains("Target") &&
            context.InputParameters["Target"] is Entity entity)
        {
            tracingService.Trace("Target found");

            if (entity.LogicalName == "account")
            {
                tracingService.Trace("Account detected");

                entity["description"] = "Created via Plugin";

                tracingService.Trace("Description set");
            }
        }

        tracingService.Trace("END");
    }
}
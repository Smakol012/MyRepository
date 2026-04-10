using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;

public class AccountJsonPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        ITracingService tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        IOrganizationService service = factory.CreateOrganizationService(context.UserId);

        try
        {
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity target = (Entity)context.InputParameters["Target"];

                string city = target.GetAttributeValue<string>("address1_city");

                QueryExpression query = new QueryExpression("account")
                {
                    ColumnSet = new ColumnSet("name", "address1_city")
                };

                EntityCollection result = service.RetrieveMultiple(query);

                var sameCityAccounts = result.Entities
                    .Where(e => e.GetAttributeValue<string>("address1_city") == city)
                    .Select(e => new
                    {
                        Name = e.GetAttributeValue<string>("name"),
                        City = e.GetAttributeValue<string>("address1_city")
                    })
                    .ToList();

                string json = JsonConvert.SerializeObject(sameCityAccounts, Formatting.Indented);

                tracing.Trace("Accounts in same city:\n" + json);
            }
        }
        catch (Exception ex)
        {
            tracing.Trace("Error: " + ex.ToString());
            throw new InvalidPluginExecutionException("Plugin failed.");
        }
    }
}

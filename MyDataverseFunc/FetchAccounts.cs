using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class FetchAccounts
{
    [Function("FetchAccounts")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        string url = "https://orgd728fd72.api.crm.dynamics.com/api/data/v9.2";

        string conn = $"AuthType=OAuth;Url={url};RedirectUri=http://localhost;LoginPrompt=Always";

        using var client = new ServiceClient(conn);

        if (!client.IsReady) return new BadRequestObjectResult("Login failed.");

        var query = new QueryExpression("account")
        {
            ColumnSet = new ColumnSet("name", "address1_city")
        };

        var result = await client.RetrieveMultipleAsync(query);

        var accounts = result.Entities.Select(e => new {
            Name = e.GetAttributeValue<string>("name"),
            address1_city = e.GetAttributeValue<string>("address1_city")
        });

        return new OkObjectResult(accounts);
    }
}

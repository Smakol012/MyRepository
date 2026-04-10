using System;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

class Program
{
    static void Main()
    {
        string connectionString =
        "AuthType=;" +
        "Url=https://make.powerapps.com/environments/06a1d92d-12dd-e193-b379-b0590ba7da09/entities;" +
        "ClientId=;" +
        "ClientSecret=;" +
        "TenantId=;";

        using (ServiceClient service = new ServiceClient(connectionString))
        {
            if (!service.IsReady)
            {
                Console.WriteLine("Connection failed");
                return;
            }

            QueryExpression query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name", "address1_city")
            };

            var result = service.RetrieveMultiple(query);

            foreach (var acc in result.Entities)
            {
                string name = acc.Contains("name") ? acc["name"].ToString() : "N/A";
                string city = acc.Contains("address1_city") ? acc["address1_city"].ToString() : "N/A";

                Console.WriteLine($"Name: {name} | City: {city}");
            }
        }
    }
}
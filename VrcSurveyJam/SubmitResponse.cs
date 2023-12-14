using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using static VrcSurveyJam.EnvironmentVariables;

namespace VrcSurveyJam;

public static class SubmitResponse
{
    [Function(nameof(SubmitResponse))]
    public static async Task<HttpResponseData> Run
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, GET)]
        HttpRequestData req
    )
    {
        // Make the table if it doesn't exist
        TableClient tableClient = new(AzureWebJobsStorage, TABLE_NAME);
        await tableClient.CreateIfNotExistsAsync();

        // Track the query parameters
        Dictionary<string, object> queryParams = req.Query.AllKeys
            .ToDictionary(k => k, k => (object)req.Query[k]);

        // Build a TableEntity from the query parameters
        TableEntity data = new(queryParams)
        {
            PartitionKey = "responses",
            RowKey = Guid.NewGuid().ToString()
        };

        // Add the TableEntity to the table
        await tableClient.AddEntityAsync(data);

        // Return Success
        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString("Success!");
        return response;
    }
}

using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using static VrcSurveyJam.EnvironmentVariables;

namespace VrcSurveyJam;

public static class GetResponses
{
    [Function(nameof(GetResponses))]
    public static async Task<HttpResponseData> Run
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, GET)]
        HttpRequestData req
    )
    {
        // Make the table if it doesn't exist
        TableClient tableClient = new(AzureWebJobsStorage, TABLE_NAME);
        await tableClient.CreateIfNotExistsAsync();

        // Get all Entites from the table
        TableEntity[] allEntities = await tableClient.QueryAsync<TableEntity>().ToArrayAsync();

        // Return the Entities
        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(allEntities);
        return response;
    }
}

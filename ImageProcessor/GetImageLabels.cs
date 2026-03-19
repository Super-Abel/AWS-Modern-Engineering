using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Text.Json;

[assembly: Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace ImageProcessor;

public class GetImageLabels
{
    private IAmazonDynamoDB DynamoDBClient { get; }
    private const string TableName = "ImageAnalysisResults";

    public GetImageLabels()
    {
        DynamoDBClient = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var scanRequest = new ScanRequest { TableName = TableName };
            var scanResponse = await DynamoDBClient.ScanAsync(scanRequest);

            var results = scanResponse.Items.Select(item => new
            {
                ImageName = item.ContainsKey("ImageName") ? item["ImageName"].S : "Unknown",
                AnalysisDate = item.ContainsKey("AnalysisDate") ? item["AnalysisDate"].S : "Unknown",
                Labels = item.ContainsKey("Labels") ? item["Labels"].SS : new List<string>()
            }).ToList();

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                Body = JsonSerializer.Serialize(results)
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Erreur lecture DynamoDB: {ex.Message}");
            return new APIGatewayProxyResponse { StatusCode = 500, Body = ex.Message };
        }
    }
}
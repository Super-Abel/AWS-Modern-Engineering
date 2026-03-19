using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ImageProcessor;

public class FunctionTP5
{
    private IAmazonS3 S3Client { get; }
    private IAmazonRekognition RekognitionClient { get; }
    private IAmazonDynamoDB DynamoDBClient { get; }
    private const string TableName = "ImageAnalysisResults";

    public FunctionTP5()
    {
        // Forçage de la région sur Virginie (us-east-1)
        var region = Amazon.RegionEndpoint.USEast1;
        S3Client = new AmazonS3Client(region);
        RekognitionClient = new AmazonRekognitionClient(region);
        DynamoDBClient = new AmazonDynamoDBClient(region);
    }

    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();
        foreach (var record in eventRecords)
        {
            var bucket = record.S3.Bucket.Name;
            var key = record.S3.Object.Key;

            try
            {
                context.Logger.LogInformation($"Analyse de l'image : {key} dans le bucket : {bucket}");

                // 1. Appel Rekognition
                var rekRequest = new DetectLabelsRequest
                {
                    Image = new Image { S3Object = new Amazon.Rekognition.Model.S3Object { Bucket = bucket, Name = key } },
                    MaxLabels = 3,
                    MinConfidence = 75F
                };
                var rekResponse = await RekognitionClient.DetectLabelsAsync(rekRequest);
                var labels = rekResponse.Labels.Select(l => l.Name).ToList();

                // 2. Enregistrement DynamoDB
                var item = new Dictionary<string, AttributeValue>
                {
                    ["ImageName"] = new AttributeValue { S = key },
                    ["AnalysisDate"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") },
                    ["Labels"] = new AttributeValue { SS = labels.Count > 0 ? labels : new List<string> { "None" } }
                };

                await DynamoDBClient.PutItemAsync(new PutItemRequest { TableName = TableName, Item = item });

                context.Logger.LogInformation($"Résultat pour {key} : {string.Join(", ", labels)}");
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Erreur lors de l'analyse de {key}: {ex.Message}");
            }
        }
    }
}

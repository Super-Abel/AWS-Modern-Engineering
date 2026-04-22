
// Prototype initial S3
// using Amazon.S3;
// using Amazon.S3.Model;

// var client = new AmazonS3Client(); 
// var bucketName = "project-s3-" + Guid.NewGuid().ToString();

//     await client.PutBucketAsync(bucketName);
//     Console.WriteLine($"Bucket {bucketName} cree!");

//     var request = new PutObjectRequest
//     {
//         BucketName = bucketName,
//         Key = "test.txt",
//         ContentBody = "Ceci est un test depuis le SDK C#"
//     };
//         await client.PutObjectAsync(request);

// Gestion DynamoDB
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

var client = new AmazonDynamoDBClient();
var context = new DynamoDBContext(client);

// 1. Insertion d'un nouvel appareil
var device = new Device
{
    DeviceId = "device-123",
    DeviceName = "Capteur Salon",
    BatteryLevel = 85,
    LastReport = DateTime.UtcNow
};

await context.SaveAsync(device);
Console.WriteLine($"Appareil {device.DeviceId} inséré.");

// 2. Récupération d'un appareil par son ID
var retrievedDevice = await context.LoadAsync<Device>("device-123");
if (retrievedDevice != null)
{
    Console.WriteLine($"Appareil récupéré : {retrievedDevice.DeviceName}, Batterie : {retrievedDevice.BatteryLevel}%");

    // 3. Mise à jour du niveau de batterie
    retrievedDevice.BatteryLevel = 42;
    await context.SaveAsync(retrievedDevice);
    Console.WriteLine("Niveau de batterie mis à jour.");

    var updatedDevice = await context.LoadAsync<Device>("device-123");
    if (updatedDevice != null)
    {
        Console.WriteLine($"Appareil FINAL : ID={updatedDevice.DeviceId}, Nom={updatedDevice.DeviceName}, Batterie={updatedDevice.BatteryLevel}%, Rapport={updatedDevice.LastReport}");
    }
}
else
{
    Console.WriteLine("Appareil non trouvé.");
}
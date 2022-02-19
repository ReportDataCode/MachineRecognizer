using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

ComputerVisionClient Authenticate(string endpoint, string subscriptionKey)
{
    ComputerVisionClient client = new(new ApiKeyServiceClientCredentials(subscriptionKey))
    {
        Endpoint = endpoint
    };
    return client;
}

async Task AnalyzeImageUrlTask(ComputerVisionClient client, string imageUrl)
{
    Console.WriteLine("----------------------------------------------------------");
    Console.WriteLine("ANALYZE IMAGE - URL");
    Console.WriteLine();

    // Creating a list that defines the features to be extracted from the image. 

    List<VisualFeatureTypes?> features = new()
    {
        VisualFeatureTypes.Categories,
        VisualFeatureTypes.Description,
        VisualFeatureTypes.Faces,
        VisualFeatureTypes.ImageType,
        VisualFeatureTypes.Tags,
        VisualFeatureTypes.Adult,
        VisualFeatureTypes.Color,
        VisualFeatureTypes.Brands,
        VisualFeatureTypes.Objects
    };

    Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
    Console.WriteLine();
    // Analyze the URL image .
    ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

    // Summarizes the image content.
    Console.WriteLine("Summary:");
    foreach (var caption in results.Description.Captions)
    {
        Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
    }

    Console.WriteLine();

    // Display categories the image is divided into.
    Console.WriteLine("Categories:");
    foreach (var category in results.Categories)
    {
        Console.WriteLine($"{category.Name} with confidence {category.Score}");
    }

    Console.WriteLine();

    // Objects
    Console.WriteLine("Objects:");
    foreach (var obj in results.Objects)
    {
        Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                          $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
    }

    Console.WriteLine();

    // Well-known (or custom, if set) brands.
    Console.WriteLine("Brands:");
    foreach (var brand in results.Brands)
    {
        Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                          $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
    }

    Console.WriteLine();

    // Faces
    Console.WriteLine("Faces:");
    foreach (var face in results.Faces)
    {
        Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                          $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                          $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
    }

    Console.WriteLine();

    // Adult or racy content, if any.
    Console.WriteLine("Adult:");
    Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
    Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
    Console.WriteLine($"Has gory content: {results.Adult.IsGoryContent} with confidence {results.Adult.GoreScore}");
    Console.WriteLine();

    // Detects the image types.
    Console.WriteLine("Image Type:");
    Console.WriteLine("Clip Art Type: " + results.ImageType.ClipArtType);
    Console.WriteLine("Line Drawing Type: " + results.ImageType.LineDrawingType);
    Console.WriteLine();
}


// Add your Computer Vision subscription key and endpoint.
string subscriptionKey = "<put-your-key-here>";
string endpoint = "<put-your-endpoint-here>";

string ANALYZE_URL_IMAGE = "https://media.spokesman.com/photos/2018/10/19/India_Train_Accident.JPG.jpg";

// Add a client.
ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

// Analyze an image to get features and others properties.
AnalyzeImageUrlTask(client, ANALYZE_URL_IMAGE).Wait();
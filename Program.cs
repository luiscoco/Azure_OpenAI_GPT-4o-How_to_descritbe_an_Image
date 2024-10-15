using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    static async Task Main(string[] args)
    {
        // Corrected endpoint URL: Do not append additional paths or API versions
        var endpoint = new Uri("https://cocoe-m2ae1t7j-swedencentral.openai.azure.com/");

        var credentials = new AzureKeyCredential("1c1ad980b9ae425eb7ae14581fea4fe4");
        // var credentials = new DefaultAzureCredential(); // Use this line for Passwordless auth

        var deploymentName = "gpt-4o"; // Ensure this matches your Azure OpenAI deployment name

        var openAIClient = new AzureOpenAIClient(endpoint, credentials);
        var chatClient = openAIClient.GetChatClient(deploymentName);

        // Updated image path to the uploaded file location
        var imagePath = "C:\\29. Azure OpenAI and the Semantic Kernel SDK\\Azure OpenAI\\GPT-4 Turbo with Vision\\bmwcar.png"; // Replace with your correct local path or uploaded image path

        // Read the image from the local file system and convert it to a byte array
        byte[] imageBytes;
        try
        {
            imageBytes = File.ReadAllBytes(imagePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the image file: {ex.Message}");
            return;
        }

        // Convert the byte[] to BinaryData
        BinaryData imageData = new BinaryData(imageBytes);

        // Create the chat message list
        List<ChatMessage> messages = new List<ChatMessage>
        {
            new UserChatMessage(
                ChatMessageContentPart.CreateTextPart("Please describe the following image:"),
                ChatMessageContentPart.CreateImagePart(imageData, "image/png")) // Assuming PNG format, adjust as needed
        };

        // Send the chat completion request
        ChatCompletion chatCompletion;
        try
        {
            chatCompletion = await chatClient.CompleteChatAsync(messages);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in chat completion: {ex.Message}");
            return;
        }

        // Output the assistant's response
        Console.WriteLine($"[ASSISTANT]:");
        Console.WriteLine($"{chatCompletion.Content[0].Text}");
    }
}

# How to describe an image with Azure OpenAI service and .NET 9 C# console application

This code is a **C#** application that integrates with the **Azure OpenAI** service using the **Azure SDK for .NET**

The main purpose of the code is to send an image to an **OpenAI GPT** model (deployed in Azure) and ask the model to describe the image

## 1. Setting up the Azure OpenAI service with a ChatGPT-4o model

We login in Azure Portal and select the Azure OpenAI service

![image](https://github.com/user-attachments/assets/0d6b477a-1b8b-4426-ab69-0e12971ddc63)

We press in the Create button

![image](https://github.com/user-attachments/assets/04102886-7a19-4f83-b3db-b27f035b4cfd)

We input the Azure Open AI service definition data and press the Next button and finally the Create button

![image](https://github.com/user-attachments/assets/88356a49-7940-4686-a08e-f2df0d70ec92)

![image](https://github.com/user-attachments/assets/f5cd1878-5572-480e-bd2e-9ee09094f0f1)

![image](https://github.com/user-attachments/assets/f7ded4ff-8a04-49a1-934e-c11b6e14fe4c)

![image](https://github.com/user-attachments/assets/d5d60d9d-543c-4ad7-8b10-c44ef7fabd3b)

We verify the service was created

![image](https://github.com/user-attachments/assets/9668483e-d43e-42b5-b6fc-eb5bbd26d96f)

![image](https://github.com/user-attachments/assets/3a523e85-5ce4-4c0f-8e8c-55a735c3e076)

Now we create a AI model deployment

![image](https://github.com/user-attachments/assets/cfb50ba4-27b7-4b49-b5db-21b3c9ff947d)

![image](https://github.com/user-attachments/assets/4a7ff54f-fa43-4cba-b4fd-8a68270243b8)

![image](https://github.com/user-attachments/assets/84fe0b61-5a1d-4a11-bc28-95721889ce9b)

![image](https://github.com/user-attachments/assets/83f84191-558b-466a-8710-5b10a14143cd)

Select the Region where you deployed your Azure OpenAI, only the regions with quota are availe 

![image](https://github.com/user-attachments/assets/0f80adc3-fe88-46fa-bf43-1f997586ab83)

## 2. Create a C# console (.NET9) application with Visual Studio 2022 Community Edition

We run Visual Studio 2022 and we create a new project

![image](https://github.com/user-attachments/assets/0a1a3adf-2ecf-47da-8f73-182cf71c15c4)

We select the C# console application as the project template and press the next button

![image](https://github.com/user-attachments/assets/e86e936a-2c7f-48c9-938d-63f9135a5e6e)

We input the project name and location in the hard disk

![image](https://github.com/user-attachments/assets/e4a462d9-8d21-436c-9bdb-8e8816d3e212)

We select the .NET 9 framework and press the Create button

![image](https://github.com/user-attachments/assets/13705d3f-f356-42bb-96aa-93a0cb5c0667)

## 3. We load the Azure Open AI Nuget pakcakes

![image](https://github.com/user-attachments/assets/31235fb5-f4e6-41b8-8570-d2622dcc8342)

## 4. We enter the application source code

Here’s a breakdown of the key parts of the code:

**Setting up the Azure OpenAI client**:

The **endpoint** is the URI of the Azure OpenAI service where the GPT model is deployed

**Credentials** represent the authentication method using an API key (AzureKeyCredential)

An alternative method using passwordless authentication (DefaultAzureCredential) is commented out deploymentName specifies the name of the Azure OpenAI model that will be used (in this case, "gpt-4o")

**Initializing the OpenAI client**:

An instance of **AzureOpenAIClient** is created with the endpoint and credentials

The client retrieves a **ChatClient** using the model's deployment name, which is used to interact with the OpenAI chat functionality

**Reading an image**:

The image file is read from a local file path (**imagePath**). The file is then converted into a byte array (**imageBytes**)

The byte array is wrapped into a BinaryData object (**imageData**), which is the format expected by the chat client

**Preparing the chat message**:

A list of **ChatMessage** objects is created. The message contains two parts:

A text part asking the assistant to "describe the following image."

An image part containing the image data (in PNG format)

**Sending the request to the model**:

The chat message list is sent to the model using the CompleteChatAsync() method of the ChatClient

The response is stored in a ChatCompletion object

**Error handling**:

The code includes try-catch blocks for reading the image file and making the API call to ensure any issues (e.g., file not found or API error) are captured and handled gracefully

**Displaying the response**:

The assistant's response, which is expected to be a description of the image, is printed to the console using Console.WriteLine()

**Purpose of the Code**:

The application uses Azure OpenAI services to send an image for analysis by a GPT model

The model is asked to describe the image, and the response is printed

This code could be part of a system where an AI model is tasked with interpreting visual content, such as object recognition or scene description

**Potential Use Cases**:

Automated image description for accessibility

Content moderation (analyzing images for specific content)

Assisting visually impaired users by describing images

```csharp
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    static async Task Main(string[] args)
    {
        var endpoint = new Uri("https://cocoe-m2ae1t7j-swedencentral.openai.azure.com/");
        var credentials = new AzureKeyCredential("1c1ad980b9ae425eb7ae14581fea4fe4");
        var deploymentName = "gpt-4o"; // Ensure this matches your Azure OpenAI deployment name

        var openAIClient = new AzureOpenAIClient(endpoint, credentials);
        var chatClient = openAIClient.GetChatClient(deploymentName);

        // Updated image path to the uploaded file location
        var imagePath = "C:\\29. Azure OpenAI and the Semantic Kernel SDK\\Azure OpenAI\\GPT-4 Turbo with Vision\\bmwcar.png";

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
                ChatMessageContentPart.CreateImagePart(imageData, "image/png"))
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
```

## 5. We run the application

We store an image in our hard disk

![image](https://github.com/user-attachments/assets/6d30d9a1-8b9b-476c-b437-f96b91330299)

We run the application and see the results

![image](https://github.com/user-attachments/assets/1599666d-723c-44e4-8769-d4df3db9e7a6)

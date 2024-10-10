using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class SoapEnvelope<TBody>
{
    [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public SoapHeader Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public TBody Body { get; set; }
}

public class SoapHeader
{
    [XmlElement(ElementName = "AuthToken")]
    public string AuthToken { get; set; }

    [XmlElement(ElementName = "RequestId")]
    public string RequestId { get; set; }

    public SoapHeader() { }

    public SoapHeader(string authToken, string requestId)
    {
        AuthToken = authToken;
        RequestId = requestId;
    }
}






[XmlRoot(ElementName = "UpdateOrderRequest")]
public class UpdateOrderRequest
{
    [XmlElement(ElementName = "OrderId")]
    public string OrderId { get; set; }

    [XmlElement(ElementName = "OrderStatus")]
    public string OrderStatus { get; set; }
}
[XmlRoot(ElementName = "AddCustomerRequest")]
public class AddCustomerRequest
{
    [XmlElement(ElementName = "CustomerName")]
    public string CustomerName { get; set; }

    [XmlElement(ElementName = "CustomerEmail")]
    public string CustomerEmail { get; set; }
}



public class Program
{
    public static void Main(string[] args)
    {
        //Retrieve value from file
        string value = RetrieveFromFile();
        Console.WriteLine("value:" + value);


        /*
        // Create a common header
        var header = new SoapHeader("yourAuthToken", "yourRequestId");

        // Create a request with AddCustomer body
        var addCustomerBody = new AddCustomerRequest
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com"
        };

        var addCustomerEnvelope = new SoapEnvelope<AddCustomerRequest>
        {
            Header = header,
            Body = addCustomerBody
        };

        string addCustomerSoapRequest = SerializeSoapRequest(addCustomerEnvelope);
        Console.WriteLine("Add Customer Request:");
        Console.WriteLine(addCustomerSoapRequest);

        // Create a request with UpdateOrder body
        var updateOrderBody = new UpdateOrderRequest
        {
            OrderId = "12345",
            OrderStatus = "Shipped"
        };

        var updateOrderEnvelope = new SoapEnvelope<UpdateOrderRequest>
        {
            Header = header,
            Body = updateOrderBody
        };

        string updateOrderSoapRequest = SerializeSoapRequest(updateOrderEnvelope);
        Console.WriteLine("\nUpdate Order Request:");
        Console.WriteLine(updateOrderSoapRequest);*/
    }

    private static string RetrieveFromFile()
    {
        try
        {
            // Get the current directory (where the executable is running)
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Build the path to the JSON file in the Settings folder
            string jsonFilePath = Path.Combine(currentDirectory, "Settings", "settings.json");

            // Read the JSON file content
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Parse the JSON content into a JsonNode object
            JsonNode jsonNode = JsonNode.Parse(jsonContent);

            // Access the specific value from the JSON
            string appName = jsonNode?["item"]?.ToString();

            if (appName != null)
            {
                Console.WriteLine($"AppName: {appName}");
                return appName;
            }
            else
            {
                Console.WriteLine("Error: 'AppName' not found in the JSON.");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: JSON file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");


        }
        return "bad";

    }
    public static string SerializeSoapRequest<TBody>(SoapEnvelope<TBody> soapEnvelope)
    {
        var xmlSerializer = new XmlSerializer(typeof(SoapEnvelope<TBody>));
        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                xmlSerializer.Serialize(xmlWriter, soapEnvelope);
                return stringWriter.ToString();
            }
        }
    }

}


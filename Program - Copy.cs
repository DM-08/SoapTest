using System.Xml;
using System.Xml.Serialization;

namespace SoapSerializationExample
{
    // Define the SoapEnvelope with namespaces
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapHeader Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapBody Body { get; set; }
    }

    // Define the SoapHeader with a Security element
    public class SoapHeader
    {
        [XmlElement(ElementName = "Security")]
        public Security Security { get; set; }
    }

    // Define the Security class with a custom namespace
    [XmlType(Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
    public class Security
    {
        [XmlElement(ElementName = "UsernameToken")]
        public UsernameToken UsernameToken { get; set; }
    }

    // Define the UsernameToken class with Username and Password
    [XmlType(Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
    public class UsernameToken
    {
        [XmlElement(ElementName = "Username")]
        public string Username { get; set; }

        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }
    }

    // Define the SoapBody with your custom operation data
    public class SoapBody
    {
        [XmlElement(ElementName = "YourOperation")]
        public YourOperationData YourOperation { get; set; }
    }

    // Define your operation data
    [XmlType(Namespace = "http://yourcustomnamespace.com/operation")]
    public class YourOperationData
    {
        [XmlElement(ElementName = "Param1")]
        public string Param1 { get; set; }

        [XmlElement(ElementName = "Param2")]
        public string Param2 { get; set; }
    }

    class Program
    {
        // Method to serialize the SoapEnvelope with Security to XML
        public static string SerializeSoapEnvelopeWithSecurity(SoapEnvelope envelope)
        {
            // Create XmlSerializer for SoapEnvelope type
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SoapEnvelope));

            // Use an XmlWriterSettings to customize the output
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true // Omit the XML declaration "<?xml version="1.0"?>"
            };

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    // Create custom namespaces for different parts of the SOAP envelope
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

                    // Set namespace to different sections only
                    namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                    namespaces.Add("wsse", "http://schemas.xmlsoap.org/ws/2002/12/secext");
                    namespaces.Add("ns1", "http://yourcustomnamespace.com/operation");

                    // Serialize the SoapEnvelope with namespaces
                    xmlSerializer.Serialize(xmlWriter, envelope, namespaces);
                }

                return textWriter.ToString();
            }
        }

        static void Main(string[] args)
        {
            // Create the SoapEnvelope object with Security header and custom operation data
            var soapEnvelope = new SoapEnvelope
            {
                Header = new SoapHeader
                {
                    Security = new Security
                    {
                        UsernameToken = new UsernameToken
                        {
                            Username = "myUsername",
                            Password = "myPassword"
                        }
                    }
                },
                Body = new SoapBody
                {
                    YourOperation = new YourOperationData
                    {
                        Param1 = "Value1",
                        Param2 = "Value2"
                    }
                }
            };

            // Serialize the SOAP envelope to XML
            string serializedSoap = SerializeSoapEnvelopeWithSecurity(soapEnvelope);

            // Output the serialized SOAP message
            Console.WriteLine(serializedSoap);
        }
    }
}

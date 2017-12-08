using System.Xml.Serialization;

namespace Instagraph.DataProcessor.DTOs.Export
{
    [XmlType("user")]
    public class UserWithMostCommentsDto
    {
        public string Username { get; set; }

        public int MostComments { get; set; }
    }
}
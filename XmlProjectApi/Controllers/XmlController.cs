using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmlProjectApi.Models;

namespace XmlProjectApi.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]")]
    [ApiController]
    public class XmlController : ControllerBase
    {
        private readonly string xmlFilePath = "D:\\KSI\\Kurs4\\Semestar 7\\PRS\\KursovaRabota\\XmlProjectApi\\XmlProjectApi\\data.xml";


        [HttpPost("[action]")]
        public IActionResult ProcessXml()
        {
            try
            {
                string xmlData = ReadXmlFromFile(xmlFilePath);
                List<Person> people = ParseXmlData(xmlData);
                return Ok(people);
            }
            catch
            {
                return BadRequest("Error processing XML data.");
            }
        }

        [HttpPost("[action]")]
        public IActionResult InsertPerson([FromBody] Person newPerson)
        {
            try
            {
                string xmlData = ReadXmlFromFile(xmlFilePath);
                XDocument xmlDoc = XDocument.Parse(xmlData);

                // Convert the newPerson object to an XElement
                XElement newPersonElement = newPerson.ToXml();

                // Add the new person element to the root of the XML document
                xmlDoc.Root.Add(newPersonElement);

                // Save the modified XML document back to the file
                xmlDoc.Save(xmlFilePath);

                return Ok("Person inserted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error inserting person into XML: {ex.Message}");
            }
        }

        private string ReadXmlFromFile(string filePath)
        {
            try
            {
                return System.IO.File.ReadAllText(filePath);
            }
            catch
            {
                throw new IOException($"Error reading XML file from path: {filePath}");
            }
        }

        private List<Person> ParseXmlData(string xml)
        {
            var doc = XDocument.Parse(xml);
            var people = new List<Person>();

            foreach (var personElement in doc.Descendants("person"))
            {
                var person = new Person
                {
                    Name = personElement.Element("name")?.Value,
                    Description = personElement.Element("description")?.Value,
                    Contact = new Contact
                    {
                        Email = personElement.Element("contact")?.Element("email")?.Value,
                        Phone = new Phone
                        {
                            Home = personElement.Element("contact")?.Element("phone")?.Element("home")?.Value,
                            Work = personElement.Element("contact")?.Element("phone")?.Element("work")?.Value
                        }
                    },
                    Address = new Address
                    {
                        Street = personElement.Element("address")?.Element("street")?.Value,
                        City = personElement.Element("address")?.Element("city")?.Value
                    }
                };

                people.Add(person);
            }

            return people;
        }
    }
}

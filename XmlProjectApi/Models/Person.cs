using System.Xml.Linq;

namespace XmlProjectApi.Models
{
    public class Person
    {
        public string Name { get; set; }
        public Contact Contact { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }

        public XElement ToXml()
        {
            return new XElement("person",
                new XElement("name", Name),
                new XElement("contact",
                    new XElement("email", Contact.Email),
                    new XElement("phone",
                        new XElement("home", Contact.Phone.Home),
                        new XElement("work", Contact.Phone.Work)
                    )
                ),
                new XElement("address",
                    new XElement("street", Address.Street),
                    new XElement("city", Address.City)
                ),
                new XElement("description", new XCData(Description))
            );
        }
    }
}

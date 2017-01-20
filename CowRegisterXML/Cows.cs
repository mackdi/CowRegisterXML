using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CowRegisterXML
{
    [XmlRoot]
    public class Cows
    {
        [XmlElement]
        public List<Cow> Cow { get; set; }
    }
    public class Cow
    {
        [XmlElement]
        public int CowNumber { get; set; }
        [XmlElement]
        public int CowAge { get; set; }
        [XmlElement]
        public DateTime CowImmunizationDate { get; set; }

    }
}




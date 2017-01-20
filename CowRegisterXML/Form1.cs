using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;


namespace CowRegisterXML
{
    public partial class Form1 : Form
    {
        // името на xml файла
        const string fileName = "register.xml";
        
        // създаване на модела
        private static Cows cows = new Cows();

        public Form1()
        {
            InitializeComponent();
            cows.Cow = new List<Cow>();
        }

        // CRUD
        private void Create_Click(object sender, EventArgs e)
        {
            // Създава се помощен обект който се добвя в списъка и в DataGridView
            Cow cow = new Cow();

            int number;
            int.TryParse(numberBox.Text,out number);
            cow.CowNumber = number;
            int age;
            int.TryParse(ageBox.Text, out age);
            cow.CowAge = age;
            DateTime date;
            DateTime.TryParse(dateBox.Text, out date);
            cow.CowImmunizationDate = date;
            if (date != default(DateTime) && age != default(int))
            {
            cows.Cow.Add(cow);
            XDocument xdoc = XDocument.Load(fileName);
            var check = xdoc.Descendants("Cow").FirstOrDefault(n => n.Element("CowNumber").Value.Equals(number.ToString()));
                if (check == null)
                {
                    XElement element = new XElement("Cow",
                    new XElement("CowNumber", cow.CowNumber),
                    new XElement("CowAge", cow.CowAge),
                    new XElement("CowImmunizationDate", cow.CowImmunizationDate)
                    );
                    xdoc.Root.Add(element);
                    xdoc.Save(fileName);
                    Reload();
                }
                    else
                    {
                        MessageBox.Show("Повторение на номера");
                    }
                }
            else
            {
                MessageBox.Show("Попълнете правилно полетата");
            }
        }
        private void Read_Click(object sender, EventArgs e)
        {
            Reload();
        }
        private void Update_Click(object sender, EventArgs e)
        {
            int age;
            int.TryParse(ageBox.Text, out age);
            DateTime date;
            DateTime.TryParse(dateBox.Text, out date);
            
            XDocument xdoc = XDocument.Load(fileName);
            var element = xdoc.Descendants("Cow").FirstOrDefault(n => n.Element("CowNumber").Value.Equals(numberBox.Text));
            element.SetElementValue("CowAge", age);
            element.SetElementValue("CowImmunizationDate", date);
            xdoc.Save(fileName);
            Reload();
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(fileName);
            var element = xdoc.Descendants("Cow").FirstOrDefault(n => n.Element("CowNumber").Value.Equals(numberBox.Text));
            element.Remove();
            xdoc.Save(fileName);
            Reload();
        }

        // презареждане на DataGridView
        private void Reload()
        {
            cowBindingSource.Clear();
            cows.Cow.Clear();
            foreach (var c in GetFromXML(fileName).Cow)
            {
                cows.Cow.Add(c);
                cowBindingSource.Add(c);
            }
            dataGridView1.DataSource = cowBindingSource;
        }
        // създава XML от обект
        private void AddToXML(string fileName, Cows cowObj)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(Cows));
                XML.Serialize(stream, cowObj);
            }
        }
        // създава обект от XML
        private Cows GetFromXML(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(Cows));
                return (Cows)XML.Deserialize(stream);
            }
        }
        // търсене по номер и дата
        private void searchBtn_Click(object sender, EventArgs e)
        {
            cowBindingSource.Clear();
            string pattern = searchBox.Text;
            int numRes;
            int.TryParse(pattern, out numRes);
            DateTime dtRes;
            DateTime.TryParse(pattern,out dtRes);
            foreach (var c in cows.Cow)
            {
                if (c.CowNumber == numRes || c.CowImmunizationDate == dtRes)
                {
                    cowBindingSource.Add(c);
                    dataGridView1.DataSource = cowBindingSource;
                }
            }
        }
    }
}


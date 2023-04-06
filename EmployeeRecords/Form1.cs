using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EmployeeRecords
{
    public partial class mainForm : Form
    {
        List<Employee> employeeDB = new List<Employee>();

        public mainForm()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        { 
            string id, firstName, lastName, date, salary;

            id = idInput.Text;
            firstName = fnInput.Text;
            lastName = lnInput.Text;
            date = dateInput.Text;
            salary = salaryInput.Text;

            Employee newEmp = new Employee(id, firstName, lastName, date, salary);
            employeeDB.Add(newEmp);

            ClearLabels();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            //use a lambda expression to search for a list item to remove
            string searchID = idInput.Text;

            int index = employeeDB.FindIndex(emp => emp.id == searchID); 
            // above brackets contents (lambda expression) is basically another way of writing a foreach loop
            if (index >= 0) //  if the mathching id is found:
            {
                employeeDB.RemoveAt(index);
                outputLabel.Text = $"Employee {searchID} deleted";
            }
            else
            {
                outputLabel.Text = $"Employee not found";
            }
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";
            foreach (Employee emp in employeeDB)
            {
                outputLabel.Text += $"{emp.id} {emp.firstName} {emp.lastName} {emp.date} {emp.salary} \n";
            }
        }

        private void ClearLabels()
        {
            idInput.Text = "";
            fnInput.Text = "";
            lnInput.Text = "";
            dateInput.Text = "";
            salaryInput.Text = "";
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveDB();
        }

        public void loadDB()
        {
            //almost the exact same process as getting data from user

            string id, firstName, lastName, date, salary;

            XmlReader reader = XmlReader.Create("Resources/employeeXML.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text) // searches for text/"data" (as opposed to xml tags). IF <id> 1234 </id> it will return 1234
                {
                    id = reader.ReadString();

                    reader.ReadToNextSibling("firstName"); // firstName, etc are siblings of <id>
                    firstName = reader.ReadString();

                    reader.ReadToNextSibling("lastName");
                    lastName = reader.ReadString();


                    reader.ReadToNextSibling("date");
                    date = reader.ReadString();

                    reader.ReadToNextSibling("salary");
                    salary = reader.ReadString();

                    /// you could make a for loop with an array: and collect data and put it into the array for each employee

                    Employee newEmp = new Employee(id, firstName, lastName, date, salary);
                    employeeDB.Add(newEmp);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {

            XmlWriter writer = XmlWriter.Create("Resources/employeeXML.xml", null);
            writer.WriteStartElement("Employees");


            foreach (Employee emp in employeeDB)
            {
                writer.WriteStartElement("Employee");

                writer.WriteElementString("id", emp.id);
                writer.WriteElementString("firstName", emp.firstName);
                writer.WriteElementString("lastName", emp.lastName);
                writer.WriteElementString("date", emp.date);
                writer.WriteElementString("salary", emp.salary);

                writer.WriteEndElement();
            }


            writer.WriteEndElement();
            writer.Close();
        }
    }
}

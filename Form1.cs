using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using Newtonsoft.Json;

namespace FireBaseDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "fpTA5NPQweXuyliaaLtGdxJePx4UGGQqKzaeb8Qg",
            BasePath = "https://mysharpproject-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(fcon);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private void InsertBtn_Click(object sender, EventArgs e)
        {
            var std = new Student()
            {
                Fullname = nameTbox.Text,
                RollNo = rollTbox.Text,
                Grade = gradeTbox.Text,
                Section = secTbox.Text
            };
            var setter = client.Set("StudentList/" + rollTbox.Text, std);
            MessageBox.Show("Data has been saved.");
            PopulateDataGrid();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            var result = client.Get("StudentList/" + rollTbox.Text);
            var std = result.ResultAs<Student>();
            nameTbox.Text = std.Fullname;
            gradeTbox.Text = std.Grade;
            secTbox.Text = std.Section;
            MessageBox.Show("Data has been loaded.");
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            var std = new Student()
            {
                Fullname = nameTbox.Text,
                RollNo = rollTbox.Text,
                Grade = gradeTbox.Text,
                Section = secTbox.Text
            };
            var setter = client.Update("StudentList/" + rollTbox.Text, std);
            MessageBox.Show("Data has been updated.");
            PopulateDataGrid();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            var result = client.Delete("StudentList/" + rollTbox.Text);
            MessageBox.Show("Data has been deleted.");
            PopulateDataGrid();
        }

        private void GridBtn_Click(object sender, EventArgs e)
        {            
            PopulateDataGrid();
        }

        private void PopulateDataGrid()
        {
            var res = client.Get("StudentList");
            var data = JsonConvert.DeserializeObject<Dictionary<string, Student>>(res.Body.ToString());

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("Key", "Key");
            dataGridView1.Columns.Add("FullName", "FullName");
            dataGridView1.Columns.Add("Grade", "Grade");
            dataGridView1.Columns.Add("RollNo", "RollNo");
            dataGridView1.Columns.Add("Section", "Section");

            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Key, item.Value.Fullname, item.Value.Grade,
                    item.Value.RollNo, item.Value.Section);
            }
        }
    }
}

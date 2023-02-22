using Donor.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Donor
{
    /// <summary>
    /// Interaction logic for ShowAllWindow.xaml
    /// </summary>
    public partial class ShowAllWindow : Window
    {
        public ShowAllWindow()
        {
            InitializeComponent();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {

            ViewWindow view = new ViewWindow();
            view.Show();
            this.Close();
            Button b = sender as Button;
            Donors empbtn = b.CommandParameter as Donors;
            view.txtview.Text = $"ID Number:  { empbtn.ID }\n name:\t{empbtn.Title} {empbtn.Firstname} {empbtn.Lastname}\n Gender:  {empbtn.Gender} \n Age:  {empbtn.Age} \n BloodGroup: {empbtn.BloodGroup}\n Address: {empbtn.Address}\n Email: {empbtn.Email}\n Contact: {empbtn.Contact}\n Date: {empbtn.Date}";
            // view.imageView.Source = ImageInstance(new Uri(GetImagePath() + empbtn.ImageInformation));
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
            main.btnUpdate.Visibility = Visibility.Visible;

            Button b = sender as Button;
            Donors empbtn = b.CommandParameter as Donors;
            int empID = empbtn.ID;

            main.txtID.Text = empID.ToString();
            main.txtFristname.Text = empbtn.Firstname.ToString();
            main.txtLastname.Text = empbtn.Lastname.ToString();
            main.cmbGender.SelectedItem = empbtn.Gender.ToString();
            main.txtAge.Text = empbtn.Age.ToString();
            main.cmbBloodGroup.SelectedItem = empbtn.BloodGroup.ToString();
            main.txtAddress.Text = empbtn.Address.ToString();
            main.txtEmail.Text = empbtn.Email.ToString();
            main.txtContact.Text = empbtn.Contact.ToString();
            main.cmbTitle.SelectedItem = empbtn.Title.ToString();
            //main.txtDate.Text = empbtn.Date.ToString();
            main.txtID.IsEnabled = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var jsonD = File.ReadAllText(@"Donor.json");
            var jsonObj = JObject.Parse(jsonD);
            JArray empDeleteArray = (JArray)jsonObj["Donor"];

            Button b = sender as Button;
            Donors empbtn = b.CommandParameter as Donors;
            int empID = empbtn.ID;

            if (empID > 0)
            {
                MainWindow main = new MainWindow();

                var DonorToDeleted = empDeleteArray.FirstOrDefault(obj => obj["ID"].Value<int>() == empID);
                empDeleteArray.Remove(DonorToDeleted);

                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                FileInfo thisFile = new FileInfo(GetImagePath() + empbtn.ImageInformation);
                if (thisFile.Name != "defualt.jpg") //Delete image (Not default image)
                {
                    thisFile.Delete();
                }
                File.WriteAllText(@"Donor.json", output);
                MessageBox.Show("Data Deleted Successfully!!!", "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                main.ShowData();
                this.Hide();
                main.AllClear();
            }
            else
            {
                MessageBox.Show("Not Deleted!!!!");
            }
        }
        private string GetImagePath()
        {
            var AddinAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var AddinFolder = System.IO.Path.GetDirectoryName(AddinAssembly.Location);
            string ImagePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AddinFolder, @"..\..\image\"));

            return ImagePath;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();

        }

        

        
    }
}

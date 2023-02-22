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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Donor.models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Donor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FileInfo TempImageFile { get; set; }
        public BitmapImage DefaultImage => new BitmapImage(new Uri(GetImagePath() + "defualt.jpg"));
        public string DefaultImagePath => GetImagePath() + "default.jpg";
        public MainWindow()
        {
            //btnUpdate.Visibility = Visibility.Hidden;
            InitializeComponent();
            List<string> titles = new List<string>()
            {
                "Mr",
                "Miss",
                "Mrs"
            };
            this.cmbTitle.ItemsSource = titles;
            cmbTitle.Text = "Mr.";

            List<string> Gender = new List<string>()
            {
                "Female",
                "Male",
               ""
            };
            this.cmbGender.ItemsSource = Gender;
            cmbGender.Text = "Female.";

            List<string> BloodGroup = new List<string>()
            {
                "O+",
                "O-",
                 "A+",
                "A-",
                 "B+",
                "B-",
                 "AB+",
                "AB-",

            };
            this.cmbBloodGroup.ItemsSource = BloodGroup;
            cmbBloodGroup.Text = "O+";
        }

        

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }

        //private void btnUpdate_Click(object sender, RoutedEventArgs e)
        //{
           
            
        //}
        public void ShowData()
        {
            ShowAllWindow showAll = new ShowAllWindow();
            showAll.Show();
            this.Hide();

            var json = File.ReadAllText(@"Donor.json");
            

            if (!IsValidJson(json))
            {
                return;
            }

            var jsonObj = JObject.Parse(json);
            var empJson = jsonObj.GetValue("Donor").ToString();
            var empList = JsonConvert.DeserializeObject<List<Donors>>(empJson);   //Deserialize to List<Employee>
            empList = empList.OrderBy(x => x.ID).ToList();  //Sorting List<Employee> by Id (Ascending)

            foreach (var item in empList)
            {
                item.ImageShow = ImageInstance(new Uri(GetImagePath() + item.ImageInformation));   //Create image instance for all Employee
            }
            showAll.datashow.ItemsSource = empList;
            showAll.datashow.Items.Refresh();

            GC.Collect();                   //Call garbage collector to release unused image instance resource
            GC.WaitForPendingFinalizers();
        }
        public ImageSource ImageInstance(Uri path)  //Create image instance rather than referencing image file
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = path;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.DecodePixelWidth = 300;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }


        //private void Delete_Click(object sender, RoutedEventArgs e)
        //{



        //}

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            Donors em = new Donors();
            em.ID = Convert.ToInt32(txtID.Text);
            em.Title = cmbTitle.SelectedItem.ToString();
            em.Firstname = txtFristname.Text;
            em.Lastname = txtLastname.Text;
            em.Gender = cmbGender.SelectedItem.ToString();
            em.Age = txtAge.Text;
            em.BloodGroup = cmbBloodGroup.SelectedItem.ToString();
            em.Address = txtAddress.Text;
            em.Email = txtEmail.Text;
            em.Contact = txtContact.Text;
            em.Date = Convert.ToDateTime(date.Text);
            em.ImageInformation= (TempImageFile != null) ? $"{int.Parse(txtID.Text) + TempImageFile.Extension}" : "defualt.jpg";


            var newDonorMember = "{'ID':'" + em.ID + "','Title':'" + em.Title + "','FirstName':'" + em.Firstname + "','LastName':'" + em.Lastname + "','Gender':'" + em.Gender + "','Age':'" + em.Age + "','BloodGroup':'" + em.BloodGroup + "','Address':'" + em.Address + "','Email':'" + em.Email + "','Contact':'" + em.Contact + "','ImageInformation':'" + em.ImageInformation + "','ImageShow':'" + em.ImageShow + "','Date':'" + em.Date + "'}";

            var jsond = File.ReadAllText(@"Donor.json");
            var jsonObj = JObject.Parse(jsond);
            var DonorArray = jsonObj.GetValue("Donor") as JArray;

            var newDonor = JObject.Parse(newDonorMember);
            DonorArray.Add(newDonor);

            jsonObj["Donor"] = DonorArray;
            string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"Donor.json", newJsonResult);
            if (TempImageFile != null)
            {
                TempImageFile.CopyTo(GetImagePath() + em.ImageInformation);
                TempImageFile = null;
                ImageShow.Source = DefaultImage;

            }
      
            ShowData();
            AllClear();

        }
        private bool IsValidJson(string data)
        {
            try
            {
                var temdata = (JContainer)JToken.Parse(data);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public void AllClear()
        {
            txtID.Clear();
            txtFristname.Clear();
            txtLastname.Clear();
            cmbGender.SelectedIndex = -1;
            txtAge.Clear();
            cmbBloodGroup.SelectedIndex = -1;
            txtAddress.Clear();
            txtEmail.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            cmbTitle.SelectedIndex = -1;
            txtID.IsEnabled = true;
        }

        //private void btnUpdate_Click_1(object sender, RoutedEventArgs e)
        //{

            

        //}

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var jsonU = File.ReadAllText(@"Donor.json");
            var jsonObj = JObject.Parse(jsonU);
            JArray empUpdateArray = (JArray)jsonObj["Donor"];

            var ID = int.Parse(txtID.Text);
            var Title = cmbTitle.SelectedItem.ToString();
            var FirstName = txtFristname.Text;
            var LastName = txtLastname.Text;
            var Gender = cmbGender.SelectedItem.ToString();
            var Age = txtAge.Text;
            var BloodGroup = cmbBloodGroup.SelectedItem.ToString();
            var Address = txtAddress.Text;
            var Email = txtEmail.Text;
            var Contact = txtContact.Text;
            var Date = date.Text;


            foreach (var Donor in empUpdateArray.Where(obj => obj["ID"].Value<int>() == ID))
            {
                Donor["Title"] = !string.IsNullOrEmpty(Title) ? Title : "";
                Donor["FirstName"] = !string.IsNullOrEmpty(FirstName) ? FirstName : "";
                Donor["LastName"] = !string.IsNullOrEmpty(LastName) ? LastName : "";
                Donor["Gender"] = !string.IsNullOrEmpty(Gender) ? Gender : "";
                Donor["Age"] = !string.IsNullOrEmpty(Age) ? Age : "";
                Donor["BloodGroup"] = !string.IsNullOrEmpty(BloodGroup) ? BloodGroup : "";
                Donor["Address"] = !string.IsNullOrEmpty(Address) ? Address : "";
                Donor["Email"] = !string.IsNullOrEmpty(Email) ? Email : "";
                Donor["Contact"] = !string.IsNullOrEmpty(Contact) ? Contact : "";
                Donor["Date"] = !string.IsNullOrEmpty(Date) ? Date : "";

            }
            jsonObj["Donor"] = empUpdateArray;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"Donor.json", output);
            MessageBox.Show("Data Updated Successfully!!!!");
            ShowData();
            AllClear();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpload_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png *.gif; *.bmp;";
            fd.Title = "Select an Image";
            if (fd.ShowDialog().Value == true)
            {
                ImageShow.Source = new BitmapImage(new Uri(fd.FileName));
                TempImageFile = new FileInfo(fd.FileName);

                MessageBox.Show(TempImageFile.Extension);
            }
            else
            {
                MessageBox.Show("Cancelled");
            }

        }
        private string GetImagePath()
        {
            var AddinAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var AddinFolder = System.IO.Path.GetDirectoryName(AddinAssembly.Location);
            string ImagePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AddinFolder, @"..\..\image\"));

            return ImagePath;
        }


        
    }
}

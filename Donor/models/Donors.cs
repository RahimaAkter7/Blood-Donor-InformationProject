using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Donor.models
{
    public class Donors
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime Date { get; set; }
        public string ImageInformation { get; set; }
        public ImageSource ImageShow { get; set; }


    }
}

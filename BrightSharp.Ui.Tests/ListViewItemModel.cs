using BrightSharp.Mvvm;
using System;
using System.ComponentModel;

namespace BrightSharp.Ui.Tests
{
    public class CustomerViewModel : ObservableObject
    {
        static Random rnd = new Random();
        private string _customerId;
        private string _companyName;
        private string _contactName;
        private string _contactTitle;
        private string _address;
        private string _city;
        private string _region;
        private string _postalCode;
        private string _country;
        private string _phone;
        private string _fax;
        private string _color;

        public CustomerViewModel()
        {
            byte[] colorBytes = new byte[3];
            rnd.NextBytes(colorBytes);
            Color = System.Windows.Media.Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]).ToString();
        }

        public string CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; RaisePropertyChanged(nameof(CustomerID)); }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; RaisePropertyChanged(nameof(CompanyName)); }
        }

        public string ContactName
        {
            get { return _contactName; }
            set { _contactName = value; RaisePropertyChanged(nameof(ContactName)); }
        }

        public string ContactTitle
        {
            get { return _contactTitle; }
            set { _contactTitle = value; RaisePropertyChanged(nameof(ContactTitle)); }
        }
        [Category("Location")]
        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(nameof(Address)); }
        }
        [Category("Location")]
        public string City
        {
            get { return _city; }
            set { _city = value; RaisePropertyChanged(nameof(City)); }
        }
        [Category("Location")]
        public string Region
        {
            get { return _region; }
            set { _region = value; RaisePropertyChanged(nameof(Region)); }
        }
        [Category("Contact Information")]
        [Description("Postal code for Customer")]
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; RaisePropertyChanged(nameof(PostalCode)); }
        }
        [Category("Location")]
        [Description("Country for Customer")]
        public string Country
        {
            get { return _country; }
            set { _country = value; RaisePropertyChanged(nameof(Country)); }
        }

        [Category("Contact Information")]
        [Description("Phone for Customer")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; RaisePropertyChanged(nameof(Phone)); }
        }

        [Category("Contact Information")]
        [Description("Fax for Customer")]
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; RaisePropertyChanged(nameof(Fax)); }
        }
        [Category("Appearance")]
        public string Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged(nameof(Color)); }
        }

		public int NumberProperty { get; set; }
        public double DoubleNumberProperty { get; set; }


        [Category("PropertyGrid Explore Group")]
        [Description("Indicates that Customer is has active state")]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [Category("PropertyGrid Explore Group")]
        [Description("Indicates that Customer is has active state or it absent")]
        [DisplayName("Is Active Or Not Exists")]
        public bool? IsActiveOrEmpty { get; set; }
    }
}

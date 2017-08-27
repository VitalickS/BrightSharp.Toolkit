using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BrightSharp.Ui.Tests
{
    public class CustomerViewModel : INotifyPropertyChanged
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
        private int? _numberProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomerViewModel()
        {
            byte[] colorBytes = new byte[3];
            rnd.NextBytes(colorBytes);
            Color = System.Windows.Media.Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]).ToString();
        }

        public string CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; RaisePropertyChanged(); }
        }


        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; RaisePropertyChanged(); }
        }

        public string ContactNameCN
        {
            get { return _contactName; }
            set { _contactName = value; RaisePropertyChanged(); }
        }

        public string ContactTitle
        {
            get { return _contactTitle; }
            set { _contactTitle = value; RaisePropertyChanged(); }
        }
        [Category("Location")]
        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(); }
        }
        [Category("Location")]
        public string City
        {
            get { return _city; }
            set { _city = value; RaisePropertyChanged(); }
        }
        [Category("Location")]
        public string Region
        {
            get { return _region; }
            set { _region = value; RaisePropertyChanged(); }
        }
        [Category("Contact Information")]
        [Description("Postal code for Customer")]
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; RaisePropertyChanged(); }
        }
        [Category("Location")]
        [Description("Country for Customer")]
        public string Country
        {
            get { return _country; }
            set { _country = value; RaisePropertyChanged(); }
        }

        [Category("Contact Information")]
        [Description("Phone for Customer")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; RaisePropertyChanged(); }
        }

        [Category("Contact Information")]
        [Description("Fax for Customer")]
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; RaisePropertyChanged(); }
        }
        [Category("Appearance")]
        public string Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged(); }
        }

        public int? NumberProperty
        {
            get { return _numberProperty; }
            set { _numberProperty = value; RaisePropertyChanged(); }
        }

        public double DoubleNumberProperty { get; set; }


        [Category("PropertyGrid Explore Group")]
        [Description("Indicates that Customer is has active state")]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [Category("PropertyGrid Explore Group")]
        [Description("Indicates that Customer is has active state or it absent")]
        [DisplayName("Is Active Or Not Exists")]
        public bool? IsActiveOrEmpty { get; set; }




        private void RaisePropertyChanged([CallerMemberName]string memeberName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memeberName));
        }
    }
}

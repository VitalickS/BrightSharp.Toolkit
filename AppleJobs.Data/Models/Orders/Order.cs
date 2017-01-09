using AppleJobs.Data.Models.ModelsJobs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleJobs.Data.Models
{
    [TsModel]
    public class OrderView
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Contact { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime DateUpdated { get; set; }
        public string UserCreated { get; set; }
        public bool? IsFast { get; set; }
        public bool? IsDrive { get; set; }
        public int? Price { get; set; }

        public int OrderStatuses_Id { get; set; }
        public int Models_Id { get; set; }
        public int? ModelJobs_Id { get; set; }
        public int Customers_Id { get; set; }
        public int? Accessories_Id { get; set; }
        public int? Employees_Id { get; set; }
        public int OrderCategories_Id { get; set; }

        //Joined
        public string Status { get; set; }
        public string Model { get; set; }
        public string ModelJob { get; set; }
        public string Customer { get; set; }
        public string Accessories { get; set; }
        public string Employee { get; set; }
        public string OrderCategory { get; set; }
        public string FilterString { get; set; }
    }

    [TsModel]
    public class Order
    {
        public Order()
        {
            DateCreated = DateUpdated = DateTime.Now;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Contact { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime DateUpdated { get; set; }
        public string UserCreated { get; set; }
        public bool? IsFast { get; set; }
        public bool? IsDrive { get; set; }
        public int? Price { get; set; }

        public int OrderStatuses_Id { get; set; }
        public int Models_Id { get; set; }
        public int? ModelJobs_Id { get; set; }
        public int Customers_Id { get; set; }
        public int? Accessories_Id { get; set; }
        public int? Employees_Id { get; set; }


        [ForeignKey(nameof(OrderStatuses_Id))]
        public virtual OrderStatus OrderStatus { get; set; }

        [ForeignKey(nameof(Models_Id))]
        public virtual Model Model { get; set; }

        [ForeignKey(nameof(ModelJobs_Id))]
        public virtual ModelJob JobModel { get; set; }

        [ForeignKey(nameof(Customers_Id))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(Accessories_Id))]
        public virtual Accessories Accessories { get; set; }

        [ForeignKey(nameof(Employees_Id))]
        public virtual Employee Employee { get; set; }

        public string FilterString
        {
            get
            {
                return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                        Contact, Comment, DateCreated, DateClosed, DateUpdated,
                        OrderStatus?.Name,
                        Model.FilterString,
                        JobModel.FilterString,
                        Customer?.Name,
                        Accessories?.Name,
                        Employee?.Name);
            }
        }
    }
}
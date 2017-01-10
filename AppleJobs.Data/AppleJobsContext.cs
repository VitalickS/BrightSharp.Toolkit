using AppleJobs.Data.Models.Articles;
using AppleJobs.Data.Models.Common;
using AppleJobs.Data.Models.Inventory;
using AppleJobs.Data.Models.ModelsJobs;
using AppleJobs.Data.Models.Orders;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity;

namespace AppleJobs.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AppleJobsContext : IdentityDbContext<ApplicationUser>
    {
        #region Common

        public DbSet<Customer> Customers { get; set; }

        #endregion

        #region Orders

        public DbSet<OrderCategory> OrderCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderView> OrdersView { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        #endregion

        #region Inventory

        public DbSet<Accessories> Accessories { get; set; }
        public DbSet<Employee> Employees { get; set; }

        #endregion

        #region Prices

        public DbSet<Model> Models { get; set; }
        public DbSet<ModelJob> ModelJobs { get; set; }
        public DbSet<ModelCategory> ModelCategories { get; set; }
        public DbSet<ModelJobPriceTemplate> ModelJobPriceTemplates { get; set; }

        #endregion

        #region Articles / News

        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }

        #endregion

        public AppleJobsContext(string connection = "Default") : base(connection)
        {
        }
    }
    public class ApplicationUser : IdentityUser
    {
    }
}

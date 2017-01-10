using AppleJobs.Data.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleJobs.Data.Models.ModelsJobs
{
    [TsModel]
    public class ModelJobPriceTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double? Price { get; set; }
        public bool IsPriceFrom { get; set; }
        public int ModelJobs_Id { get; set; }
        public int Customers_Id { get; set; }

        [ForeignKey(nameof(Customers_Id))]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(ModelJobs_Id))]
        public virtual ModelJob ModelJob { get; set; }

        public string FilterString { get { return string.Format("{0},{1}", Price, ModelJob?.FilterString); } }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleJobs.Data.Models.ModelsJobs
{
    [TsModel]
    public class ModelJob
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Models_Id { get; set; }

        [ForeignKey(nameof(Models_Id))]
        public virtual Model Model { get; set; }

        public string FilterString { get { return string.Format("{0},{1},{2}", Name, Description, Model?.FilterString); } }
    }
}

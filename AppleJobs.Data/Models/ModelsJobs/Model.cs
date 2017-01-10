using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleJobs.Data.Models.ModelsJobs
{
    [TsModel]
    public class Model
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModelCategories_Id { get; set; }

        [ForeignKey(nameof(ModelCategories_Id))]
        public virtual ModelCategory ModelCategory { get; set; }
        public virtual ICollection<ModelJob> ModelJobs { get; set; }

        public string FilterString { get { return string.Format("{0},{1}", Name, ModelCategory?.FilterString); } }

    }
}

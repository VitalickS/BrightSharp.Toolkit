using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleJobs.Data.Models.Articles
{
    [Table("ApjNews")]
    public class News
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int OrderIndex { get; set; }
        public int CharCount { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual NewsCategory NewsCategory { get; set; }

        public string FilterString { get { return string.Join(",", Title, Content, OrderIndex, CharCount, Date, State, NewsCategory.CategoryDescription); } }
    }
}

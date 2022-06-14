using Aspor.EF;
using Aspor.Validation.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Model
{

    [Table("boards")]
    public class Board : IEntityTimestamps
    {

        [Key]
        [OnlyServer]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [OnlyServer]
        public DateTime CreatedOn { get; set; }

        [OnlyServer]
        public DateTime ModifiedOn { get; set; }

        [OnlyServer]
        public DateTime? DeletedOn { get; set; }

    }
}

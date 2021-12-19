using Aspor.EF;
using Aspor.Validation.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Model
{

    [Table("projects")]
    // [RequiredPermission(PermissionAction.READ)]
    public class Project : IEntityTimestamps, IEntityExecutors
    {

        [Key]
        [OnlyServer]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        public bool Public { get; set; }

        [MaxLength(256)]
        public string Topics { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [OnlyServer]
        public DateTime CreatedOn { get; set; }

        [OnlyServer]
        public DateTime ModifiedOn { get; set; }

        [OnlyServer]
        public DateTime? DeletedOn { get; set; }

        [OnlyServer]
        public Guid CreatedBy { get; set; }

        [OnlyServer]
        public Guid ModifiedBy { get; set; }

        [OnlyServer]
        public Guid? DeletedBy { get; set; }

        [InverseProperty("Project")]
        public virtual IList<Board> Boards { get;set;}

    }
}

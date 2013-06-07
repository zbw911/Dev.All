using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CASServer.Models
{
    [Table("UserExtend")]
    public class UserExtend
    {
        #region Instance Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Uid { get; set; }

        [Key]
        public int UserId { get; set; }

        #endregion
    }
}
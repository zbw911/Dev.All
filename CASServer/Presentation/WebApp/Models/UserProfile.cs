using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CASServer.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        #region Instance Properties

        public int? City { get; set; }

        [MaxLength(20)]
        public string NickName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }


        public int? Province { get; set; }
        public int? Sex { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        #endregion
    }
}
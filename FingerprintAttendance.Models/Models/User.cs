using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintAttendance.Models
{
    [Serializable]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public long Id { get; set; }

        [Required]
        [DataMember]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataMember]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataMember]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public long? LeftHandId { get; set; }

        [DataMember]
        [ForeignKey("LeftHandId")]
        [InverseProperty("LeftHand")]
        public FingerPrint LeftHand { get; set; }

        public long? RightHandId { get; set; }

        [DataMember]
        [ForeignKey("RightHandId")]
        [InverseProperty("RightHand")]
        public FingerPrint RightHand { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<AttendanceLog> AttendanceLogs { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}

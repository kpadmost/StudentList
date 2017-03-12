using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace StudentsList.Model {
  public class Student {
      [Key]
      public int StudentId { get; set; }
      [Required, MaxLength(64)]
      public string FirstName { get; set; }
      [Required, MaxLength(640)]
      public string LastName { get; set; }
      [Required, MaxLength(10), Index(IsUnique=true)]
      public string IndexNo { get; set; }
      [MaxLength(64)]
      public string BirthPlace { get; set; }
      public DateTime BirthDate { get; set; }
      [Required]
      public int GroupId { get; set; }
      [Timestamp]
      public Byte[] TimeStamp { get; set; }
      [ForeignKey("GroupId")]
      public virtual Group Group { get; set; }

      public override string ToString()
      {
          return string.Format("{0} {1} {2} {3} "
              , FirstName, LastName, BirthPlace, IndexNo);
      }
  }
}

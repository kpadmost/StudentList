using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace StudentsList.Model
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
        public virtual List<Student> Students { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
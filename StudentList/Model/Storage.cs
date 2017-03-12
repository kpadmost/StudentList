using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsList.Model
{
    public class Storage
    {
        public List<Student> getStudents()
        {
            using (var db = new StorageContext())
            {
                return db.Students.Include("Group").ToList();
            }
        }

        public List<Group> getGroups()
        {
            using (var db = new StorageContext())
            {
                return db.Groups.ToList();
            }
        }

        public void createStudent(string firstName, string lastName
            , string indexNo, int groupId, DateTime birthDate , string birthPlace) 
        {
            using (var db = new StorageContext()) {
            var group = db.Groups.Find(groupId);
            var student = new Student { FirstName = firstName, LastName=lastName, BirthDate = birthDate
                , BirthPlace = birthPlace, IndexNo = indexNo, Group=group };
            db.Students.Add(student);
            db.SaveChanges();
            }
        }
        public void updateStudent(Student st) {
            using (var db = new StorageContext()) {
            var original = db.Students.Find(st.StudentId);
            if (original != null) {
                original.FirstName = st.FirstName;
                original.LastName = st.LastName;
                original.BirthDate = st.BirthDate;
                original.BirthPlace = st.BirthPlace;
                original.GroupId = st.GroupId;
                original.IndexNo = st.IndexNo;
                
                db.SaveChanges();
            }
            }
        }
        public void deleteStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.StudentId);
                if (original != null)
                {
                    db.Students.Remove(original);
                    db.SaveChanges();
                }
            }
        }
    }
}

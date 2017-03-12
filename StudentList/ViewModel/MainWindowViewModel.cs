using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using StudentsList.Model;
using System.Windows;
using StudentsList.Migrations;
namespace StudentsList.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log
          = log4net.LogManager
              .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // student prop
        private string firstName;
        private string lastName;
        private string birthPlace;
        private string birthDate;
        private string indexNo;
        private Group selectedGroup;


        private string filterBirthPlace;
        private Student selectedStudent;
        private Group filterGroup;
        private Group defaultGroup;
        
        private Storage storage;

        private List<Student> students;
        private RelayCommand createStudent;
        private RelayCommand removeStudent;
        private RelayCommand updateStudent;
        private RelayCommand filter;
        private RelayCommand clearFilter;
        #region public members
        public RelayCommand Filter { get { return filter; } }
        public RelayCommand ClearFilter { get { return clearFilter; } }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    onPropertyChanged("FirstName");
                }                  
            }
        }

        

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    onPropertyChanged("LastName");
                }
            }
        }

        public string BirthPlace
        {
            get { return birthPlace; }
            set
            {
                if (birthPlace == value)
                    return;
                birthPlace = value;
                onPropertyChanged("BirthPlace");

            }
        }

        public string BirthDate
        {
            get { return birthDate; }
            set
            {
                if (birthDate == value)
                    return;
                birthDate = value;
                onPropertyChanged("BirthDate");
            }
        }

        public string IndexNo
        {
            get { return indexNo; }
            set
            {
                if (indexNo == value)
                    return;
                indexNo = value;
                onPropertyChanged("IndexNo");
            }
        }

        public Student SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if(value == null)
                {
                    FirstName = LastName = IndexNo = BirthDate = BirthPlace = "";
                    SelectedGroup = defaultGroup;
                }
                else if(selectedStudent != value)
                {
                    selectedStudent = value;
                    FirstName = selectedStudent.FirstName;
                    LastName = selectedStudent.LastName;
                    BirthPlace = selectedStudent.BirthPlace;
                    BirthDate = selectedStudent.BirthDate.ToString();
                    IndexNo = selectedStudent.IndexNo;
                    SelectedGroup = selectedStudent.Group;
                }
                onPropertyChanged("SelectedStudent");
                updateStudent.RaiseCanExecuteChanged();
                createStudent.RaiseCanExecuteChanged();
                removeStudent.RaiseCanExecuteChanged();
            }
        }

        public Group FilterGroup
        {
            get { return filterGroup; }
            set
            {
                if(filterGroup == value)
                    return;
                filterGroup = value;
                onPropertyChanged("FilterGroup");
                ClearFilter.RaiseCanExecuteChanged();
                
            }
        }

        public Group SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                if (selectedGroup == value)
                    return;
                selectedGroup = value;
                onPropertyChanged("SelectedGroup");
            }
        }

        public IList<Student> Students 
        { 
            get { return students; }
            set
            {
                students = value as List<Student>;
                onPropertyChanged("Students");
            }
        }
        public string FilterBirthPlace
        {
            get { return filterBirthPlace; }
            set
            {
                if (filterBirthPlace == value)
                    return;
                filterBirthPlace = value;
                onPropertyChanged("FilterBirthPlace");
                ClearFilter.RaiseCanExecuteChanged();
            }
        }
        public IList<Group> Groups 
        { 
            get 
            {
                List<Group> list = storage.getGroups();
                list.Add(defaultGroup);
                return list; 
            }
 
        }
        public ICommand CreateStudent { get { return createStudent; } }
        public ICommand RemoveStudent { get { return removeStudent; } }
        public ICommand UpdateStudent { get { return updateStudent; } }
        #endregion
        private void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Student> filterList()
        {
            Func<string, bool> placeFilter = x => FilterBirthPlace == "" || x.ToLower().Contains(FilterBirthPlace.ToLower());
            Func<int, bool> groupFilter = x => FilterGroup.GroupId == 0 || x == FilterGroup.GroupId;
            return new List<Student>(Students.Where(x => placeFilter(x.BirthPlace) && groupFilter(x.GroupId)));
        }

        public MainWindowViewModel()
        {
            
            filter = new RelayCommand(
                new Action<object>(delegate(object obj)
                {
                    Students = filterList();
                })
                , new Predicate<object>(delegate(object obj)
                {
                    return true;
                }));
            clearFilter = new RelayCommand(
                new Action<object>(delegate(object obj)
                {
                    FilterGroup = defaultGroup;
                    FilterBirthPlace = "";
                    Students = storage.getStudents();
                })
                , new Predicate<object>(delegate(object obj)
                {
                    return FilterGroup.GroupId != 0 || FilterBirthPlace != "";
                }));
            createStudent = new RelayCommand(
                new Action<object>(delegate(object obj)
                {
                    try
                    {
                        log.Debug("Add command called");
                        storage.createStudent(FirstName, LastName, IndexNo
                            , SelectedGroup.GroupId, Convert.ToDateTime(BirthDate), BirthPlace);
                    }
                    catch (Exception exc)
                    {
                        log.Error("Error adding student " + exc);
                        MessageBox.Show("Błąd: " + exc.InnerException);
                    }
                    Students = storage.getStudents();
                    Students = filterList();
                    SelectedStudent = null;
                })
                , new Predicate<object>(delegate(object obj)
                {
                    return true;
                }));
            removeStudent = new RelayCommand(
                new Action<object>(delegate(object obj)
                {
                    log.Debug("Usuwamy studenta");
                    try
                    {
                        log.Debug("Remove command called");
                        storage.deleteStudent(SelectedStudent);
                    }
                    catch(Exception exc)
                    {
                        log.Error("Error removing student " + exc);
                        MessageBox.Show("Błąd: " + exc.InnerException);
                    }
                    Students = storage.getStudents();
                    Students = filterList();
                    SelectedStudent = null;
                })
                , new Predicate<object>(delegate(object obj)
                {
                    return SelectedStudent != null;
                }));
            updateStudent = new RelayCommand(
                new Action<object>(delegate(object obj)
                {
                    
                    try
                    {
                        log.Debug("Uaktualniamy studenta");
                        Student temp = new Student();
                        temp.StudentId = SelectedStudent.StudentId;
                        temp.FirstName = FirstName;
                        temp.LastName = LastName;
                        temp.BirthDate = Convert.ToDateTime(BirthDate);
                        temp.BirthPlace = BirthPlace;
                        if (SelectedGroup != defaultGroup)
                            temp.GroupId = SelectedGroup.GroupId;
                        else
                            throw new Exception("Wybrano def grupe!");
                        temp.IndexNo = IndexNo;
                        storage.updateStudent(temp);
                    }
                    catch (Exception exc)
                    {
                        log.Error("Error updating student " + exc);
                        MessageBox.Show("Błąd: " + exc);
                    }
                    Students = storage.getStudents();
                    Students = filterList();
                    SelectedStudent = null;
                })
                , new Predicate<object>(delegate(object obj)
                {
                    return SelectedStudent != null;
                }));

            storage = new Storage();
            // filter commands
            students = storage.getStudents();
            defaultGroup = new Group();
            defaultGroup.Name = "";
            filterBirthPlace = "";
            defaultGroup.GroupId = 0;
            FilterGroup = defaultGroup;

            
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

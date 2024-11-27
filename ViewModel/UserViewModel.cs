using Mod08.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mod08.Model;
using Mod08.Services;
using System.Windows.Input;


namespace Mod08.ViewModel
{
    public class UserViewModel : BindableObject
    {
        private readonly UserService _userService;
        public ObservableCollection<User> Users { get; set; }
        

        //FOR CLEARNING INPUTS
        private void ClearInput()
        {
            NameInput = string.Empty;
            GenderInput = string.Empty;
            ContactNoInput = string.Empty;
        }

        private void UpdateEntryField()
        {
            if(SelectedUser != null)
            {
                NameInput = SelectedUser.Name;
                GenderInput = SelectedUser.Gender;
                ContactNoInput = SelectedUser.ContactNo;
            }
           
        }

        //SELECTING USER
        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                UpdateEntryField();
            }
        }

        private string _nameInput;
        public string NameInput
        {
            get => _nameInput;
            set 
              {
                _nameInput = value;
                OnPropertyChanged();
              }
        }

        private string _genderInput;
        public string   GenderInput
        {
            get => _genderInput;
            set
              {
                _genderInput = value;
                OnPropertyChanged();
            }
        }
        private string _contactNoInput;
        public string ContactNoInput
        {
            get => _contactNoInput;
            set
              {
                _contactNoInput = value;
                OnPropertyChanged();
            }
            }
        public UserViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();
            LoadUserCommand = new Command(async () => await LoadUsers());
            AddUserCommand = new Command(async () => await AddUser());
            DeleteCommand = new Command(async () => await DeleteUser());
            UpdateUserCommand = new Command(async () => await UpdateUser());
        }
        public ICommand LoadUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateUserCommand { get; }

        private async Task LoadUsers()
        {
            var users = await _userService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task AddUser()
        {
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(GenderInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput))
            {
                var newUser = new User
                {
                    Name = NameInput,
                    Gender = GenderInput,
                    ContactNo = ContactNoInput
                };
                var result = await _userService.AddUserAsync(newUser);

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadUsers();
                }
            }
        }

        private async Task DeleteUser()
        {
            if(SelectedUser != null)
            {
                var result = await _userService.DeleteUserAsync(SelectedUser.ID);
                await LoadUsers();
            }
        }
            
        private async Task UpdateUser()
        {
            if(SelectedUser != null)
            {
                SelectedUser.Name = NameInput;
                SelectedUser.Gender = GenderInput;
                SelectedUser.ContactNo = ContactNoInput;

                var result = await _userService.UpdateUserAsync(SelectedUser);
                await LoadUsers();
            }
        }
        
    }
}

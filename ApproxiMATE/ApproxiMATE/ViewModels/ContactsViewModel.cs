using ApproxiMATE.Helpers;
using ApproxiMATE.Models;
using ApproxiMATE.ViewModels;
using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApproxiMATE
{
    public class ContactsViewModel : BaseViewModel
    {
        public ContactsViewModel()
        {
            //Navigation = navigation;
            PhoneContacts = new ObservableCollection<PhoneContact>();
            AcquirePhoneContacts(true).ConfigureAwait(true);
        }

        // TODO: make this more efficient
        // .Available status will be overwritten by data read from friendslist
        public async Task AcquireUsersFromPhoneNumbers(UserPhoneNumbers numbers)
        {
            var results = await App.approxiMATEService.PostUserPhoneNumberResultsAsync(numbers);
            var friends = await App.approxiMATEService.GetFriendRequestsAsync(numbers.UserId);
            foreach (var item in results)
            {
                for (int i = 0; i < PhoneContacts.Count; ++i)
                {
                    if (PhoneContacts[i].PhoneNumbers.Contains(item.PhoneNumber))
                    {
                        PhoneContacts[i].ApproxUserId = item.UserId.ToString();
                        PhoneContacts[i].Status = FriendStatus.Available;
                        if (friends.Any(f => f.InitiatorId.Equals(numbers.UserId) && f.TargetId.Equals(item.UserId)))
                        {
                            PhoneContacts[i].Status = FriendStatus.Initiated;
                            if (friends.Any(f => f.InitiatorId.Equals(item.UserId) && f.TargetId.Equals(numbers.UserId)))
                            {
                                PhoneContacts[i].Status = FriendStatus.Mutual;
                            }
                        }
                        else if (friends.Any(f => f.InitiatorId.Equals(item.UserId) && f.TargetId.Equals(numbers.UserId)))
                        {
                            PhoneContacts[i].Status = FriendStatus.PendingRequest;
                        }
                        if(PhoneContacts[i].ApproxUserId.Equals(App.AppUser.id.ToString()))
                        {
                            PhoneContacts[i].Status = FriendStatus.Blocked;
                        }
                    }
                }
            }
        }
        public async Task AcquirePhoneContacts(Boolean sort)
        {
            var permission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (permission.Equals(PermissionStatus.Granted))
            {
                PhoneContacts.Clear();
                List<string> contactNumbers = new List<string>();
                var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                if(sort)
                {
                    contacts = contacts.OrderBy(c => c.Name).ToList();
                }
                string[] splitParam = new string[] { "stringValue=" };
                foreach(Contact c in contacts)
                {
                    // TODO: look at this logic again
                    // Contact.Number contains string listed below:
                    // <CNPhoneNumber: 0x1c4a33520: stringValue=5127994767, initialCountryCode=(null)>
                    PhoneContact pc = new PhoneContact()
                    {
                        Status= FriendStatus.NotRegistered,
                        Name = c.Name,
                        PhoneNumber = ExtractPhoneNumber(c.Number),
                        PhoneNumbers = new List<String>() { ExtractPhoneNumber(c.Number) }
                    };
                    contactNumbers.Add(pc.PhoneNumber);
                    foreach (string str in c.Numbers)
                    {
                        string multi = ExtractPhoneNumber(str);
                        if (!pc.PhoneNumber.Equals(multi))
                        {
                            contactNumbers.Add(multi);
                            pc.PhoneNumbers.Add(multi);
                        }
                    }
                    PhoneContacts.Add(pc);
                }
                UserPhoneNumbers upn = new UserPhoneNumbers()
                {
                    Numbers = contactNumbers,
                    UserId = App.AppUser.id
                };
                await AcquireUsersFromPhoneNumbers(upn);
            }
            else
            {
                throw new InvalidOperationException("contacts permission is not granted");
                //await Navigation.PopAsync();
                //await Navigation.PushAsync(new IssuePage("Please provide permission to access contacts."));
                //await App.Current.MainPage.Navigation.PushAsync(new IssuePage("Please provide permission to access contacts."));
                //await Navigation.PushAsync(new IssuePage("Please provide permission to access contacts."));
                //await Navigation.PopAsync();
            }
        }
        public string RemovePhoneNumberCharacters(string input)
        {
            input = RemoveAllStringInstances(input, "(");
            input = RemoveAllStringInstances(input, ")");
            input = RemoveAllStringInstances(input, "-");
            input = RemoveAllStringInstances(input, " ");
            return input;
        }
        public string RemoveAllStringInstances(string input, string remove)
        {
            while (input.Contains(remove))
            {
                input = input.Replace(remove, "");
            }
            return input;
        }
        public string RemoveAllNonNumeric(string input)
        {
            string result = string.Empty;
            foreach (char c in input)
            {
                if (Char.IsNumber(c))
                    result += c;
            }
            return result;
        }
        public string ExtractPhoneNumber(string input)
        {
            // <CNPhoneNumber: 0x1c4a33520: stringValue=5127994767, initialCountryCode=(null)>
            if (!String.IsNullOrEmpty(input)
                && input.Contains("<CNPhoneNumber:")
                && input.Contains("stringValue="))
            {
                string[] splitParam = new string[] { "stringValue=" };
                string split = input.Split(splitParam, StringSplitOptions.None)[1];
                if (split.Contains(","))
                    return RemoveAllNonNumeric(split.Split(',')[0]);
                    //return RemovePhoneNumberCharacters(split.Split(',')[0]);
            }
            return input;
        }

        public async Task<Boolean> FriendRequestRemove(PhoneContact contact)
        {
            var data = new FriendRequestOld()
            {
                InitiatorId = App.AppUser.id,
                TargetId = Guid.Parse(contact.ApproxUserId),
                TimeStamp = DateTime.Now,
                Type = FriendRequestType.Normal
            };
            var result = await App.approxiMATEService.PutFriendRequestAsync(data);
            return result;
        }

        public async Task<Boolean> FriendRequestCreate(PhoneContact contact)
        {
            var data = new FriendRequestOld()
            {
                InitiatorId = App.AppUser.id,
                TargetId = Guid.Parse(contact.ApproxUserId),
                TimeStamp = DateTime.Now,
                Type = FriendRequestType.Normal
            };
            var result = await App.approxiMATEService.PostFriendRequestAsync(data);
            return result;
        }

        public Command CommandFriend
        {
            get
            {
                return new Command(async (e) =>
                {
                    var item = (e as PhoneContact);
                    switch(item.Status)
                    {
                        case FriendStatus.Available:
                            //send friend request
                            var worked = await FriendRequestCreate(item);
                            if (worked)
                                item.Status = FriendStatus.Initiated;
                            break;
                        case FriendStatus.Initiated:
                            //un-friend
                            var initWorked = await FriendRequestRemove(item);
                            if (initWorked)
                                item.Status = FriendStatus.Available;
                            break;
                        case FriendStatus.Mutual:
                            //un-friend
                            var mutualWorked = await FriendRequestRemove(item);
                            if (mutualWorked)
                                item.Status = FriendStatus.PendingRequest;
                            break;
                        case FriendStatus.NotRegistered:
                            //invite via sms
                            break;
                        case FriendStatus.PendingRequest:
                            //approve
                            var pendingWorked = await FriendRequestCreate(item);
                            if (pendingWorked)
                                item.Status = FriendStatus.Mutual;
                            break;
                    }
                });
            }
        }
        private ObservableCollection<PhoneContact> _phoneContacts { get; set; }
        public ObservableCollection<PhoneContact> PhoneContacts
        {
            get { return _phoneContacts; }
            set
            {
                _phoneContacts = value;
                OnPropertyChanged("PhoneContacts");
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ApproxiMATE.Models
{
    public class PhoneContact : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> PhoneNumbers { get; set; }
        public string ApproxUserId { get; set; }
        private FriendStatus _status { get; set; }
        public FriendStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("ButtonText");
            }
        }
        // TODO: use fontawesome, or images for the button displays
        // https:--//xamarininsider.com/2017/07/12/using-icon-fonts-in-your-xamarin-forms-apps/
        public string ButtonText
        {
            get
            {
                switch (Status)
                {
                    case FriendStatus.NotRegistered:
                        return "invite";
                    case FriendStatus.Initiated:
                        return "->";
                    case FriendStatus.Mutual:
                        return "-";
                    case FriendStatus.PendingRequest:
                        return "<-";
                    case FriendStatus.Available:
                        return "+";
                    case FriendStatus.Blocked:
                        return String.Empty;
                }
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

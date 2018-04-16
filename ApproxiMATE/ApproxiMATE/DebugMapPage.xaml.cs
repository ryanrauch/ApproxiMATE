using ApproxiMATE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DebugMapPage : ContentPage
	{
		public DebugMapPage ()
		{
			InitializeComponent ();
		}
        public async Task ButtonLocationBox_OnClicked(object sender, EventArgs e)
        {
            string box = EntryLocationBox.Text;
            FriendLocationBoxRequest req = new FriendLocationBoxRequest();
            req.BoundingBox = box;
            req.UserId = App.AppUser.id;
            var friends = await App.approxiMATEService.PostFriendLocationBoxAsync(req);
            ListViewBoxes.ItemsSource = friends.Friends;
        }
    }
}
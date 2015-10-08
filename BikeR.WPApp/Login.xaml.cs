using BikeR.WPApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Credentials;
using BikeR.WPApp.DataModel;
using Windows.System.Profile;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Networking.Proximity;
using NdefLibrary.Ndef;
using System.Text;
using BikeR.WPApp.nfcReciever;     

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BikeR.WPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : NfcTagRecieverPage
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();





        public Login()
        {
            this.InitializeComponent();

            //this.NfcRecievers();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            //var g = DeviceExtendedProperties.GetValue("DeviceUniqueId");
        }






        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


 


        private async void btnFacebookLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
            Frame.Navigate(typeof(PivotPage));
        }


        private async void btnGoogleLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
            Frame.Navigate(typeof(PivotPage));
        }


        private async  void btnTwitterLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter);
            Frame.Navigate(typeof(PivotPage));
        }


        private async void btnMictosoftLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
            Frame.Navigate(typeof(PivotPage));
        }

        private async System.Threading.Tasks.Task AuthenticateAsync(MobileServiceAuthenticationProvider authProvider)
        {

            string message;


            while (App.Credential == null)
            {



                try
                {
                    // Login with the identity provider.
   
                    App.User = await App.proxy
                        .LoginAsync(authProvider);

                    // Create and store the user credentials.
                    App.Credential = new PasswordCredential(authProvider.ToString(),
                        App.User.UserId, App.User.MobileServiceAuthenticationToken);
                    App.Vault.Add(App.Credential);
                }
                catch (MobileServiceInvalidOperationException ex)
                {
                    message = "You must log in. Login Required";
                }

                message = string.Format("You are now logged in - {0}", App.User.UserId);
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
  





            
        }








    }


}

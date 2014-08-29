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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BikeR.WPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Login()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;




            var s = GetHardwareId();
            //var g = DeviceExtendedProperties.GetValue("DeviceUniqueId");
        }



        private string GetHardwareId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var hardwareId = token.Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

            byte[] bytes = new byte[hardwareId.Length];
            dataReader.ReadBytes(bytes);

            return BitConverter.ToString(bytes);
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


        private MobileServiceUser user;


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
            // This sample uses the Facebook provider.


            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            while (credential == null)
            {
                try
                {
                    // Try to get an existing credential from the vault.
                    credential = vault.FindAllByResource(authProvider.ToString()).FirstOrDefault();
                }
                catch (Exception)
                {
                    // When there is no matching resource an error occurs, which we ignore.
                }

                if (credential != null)
                {
                    // Create a user from the stored credentials.
                    user = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    user.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.proxy.CurrentUser = user;

                    try
                    {
                        // Try to return an item now to determine if the cached credential has expired.
                        var a =await App.proxy.GetTable<NfcField>().Take(1).ToListAsync();
                       
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // Remove the credential with the expired token.
                            vault.Remove(credential);
                            credential = null;
                            continue;
                        }
                    }
                    catch(Exception e)
                    {
                        var str = e.Message;
                    }
                }
                else
                {
                    try
                    {
                        // Login with the identity provider.
                        user = await App.proxy
                            .LoginAsync(authProvider);

                        // Create and store the user credentials.
                        credential = new PasswordCredential(authProvider.ToString(),
                            user.UserId, user.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        message = "You must log in. Login Required";
                    }
                }
                message = string.Format("You are now logged in - {0}", user.UserId);
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
  





            
        }








    }


}

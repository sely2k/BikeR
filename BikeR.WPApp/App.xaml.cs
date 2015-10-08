using BikeR.WPApp.Common;
using BikeR.WPApp.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace BikeR.WPApp
{


    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {

        public static MobileServiceClient proxy = new MobileServiceClient
        (
            "https://biker.azure-mobile.net/", 
            "rJnxfRUFnQqlCCdaBgGljIwnEBOkil11"
        );



        #region password credential

        public static PasswordVault Vault = new PasswordVault();
        public static PasswordCredential Credential = null;
        public static MobileServiceUser User;


        public static async void RetrievePassFromVault()
        {
            bool isAuthenticate = false;

            foreach (var authProvider in Enum.GetValues(typeof(MobileServiceAuthenticationProvider)))
            {
                try
                {

                    App.Credential = Vault.FindAllByResource(authProvider.ToString()).FirstOrDefault();
                    if (App.Credential != null)
                    {
                        isAuthenticate = true;

                        // Create a user from the stored credentials.
                        App.User = new MobileServiceUser(App.Credential.UserName);
                        App.Credential.RetrievePassword();
                        App.User.MobileServiceAuthenticationToken = App.Credential.Password;

                        // Set the user from the stored credentials.
                        App.proxy.CurrentUser = App.User;

                        try
                        {
                            // Try to return an item now to determine if the cached credential has expired.
                            var a = await App.proxy.GetTable<NfcField>().Take(1).ToListAsync();

                        }
                        catch (MobileServiceInvalidOperationException ex)
                        {
                            if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                // Remove the credential with the expired token.
                                App.Vault.Remove(App.Credential);
                                App.Credential = null;
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                            var str = e.Message;
                        }


                    }

                }
                catch (Exception)
                {
                    // When there is no matching resource an error occurs, which we ignore.
                }
            }


            // Try to get an existing credential from the vault.

        }

        #endregion



        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }


        protected override void OnActivated(IActivatedEventArgs args)
        {
            //if (args.Kind == ActivationKind.Protocol)
            //{
            //    // Retrieves the activation Uri.
            //    var protocolArgs = (ProtocolActivatedEventArgs)args;
            //    var uri = protocolArgs.Uri;

            //    var frame = Window.Current.Content as Frame;

            //    if (frame == null)
            //        frame = new Frame();

            //    // Navigates to MainPage, passing the Uri to it.
            //    frame.Navigate(typeof(MainPage), uri);
            //    Window.Current.Content = frame;

            //    // Ensure the current window is active
            //    Window.Current.Activate();
            //}




            base.OnActivated(args);


            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            {
                App.proxy.LoginComplete(args as WebAuthenticationBrokerContinuationEventArgs);
            }

            //after activation set 

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page.
                rootFrame = new Frame();

                // Associate the frame with a SuspensionManager key.
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: Change this value to a cache size that is appropriate for your application.
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate.
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue.
                    }
                }

                // Place the frame in the current Window.
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter.


                RetrievePassFromVault();
                if (App.Credential == null)
                {
                    if (!rootFrame.Navigate(typeof(Login), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                } else
                {
                    if (!rootFrame.Navigate(typeof(PivotPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }



            }

            // Ensure the current window is active.
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        internal static void DisconnectAccount()
        {
            App.Vault.Remove(App.Credential);
            App.Credential = null;


  
            
          

        }
    }
}

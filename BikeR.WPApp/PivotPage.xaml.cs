﻿using BikeR.WPApp.Common;
using BikeR.WPApp.DataModel;
using BikeR.WPApp.nfcReciever;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace BikeR.WPApp
{
    public sealed partial class PivotPage : NfcTagRecieverPage
    {
        private const string FirstGroupName = "List of Tag";
        private const string SecondGroupName = "Builld your tag";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private IMobileServiceTable<NfcField> NfcTable = App.proxy.GetTable<NfcField>();

        #region DPs




        public ObservableCollection<NfcField> NFCObservableCollection
        {
            get { return (ObservableCollection<NfcField>)GetValue(NFCObservableCollectionProperty); }
            set { SetValue(NFCObservableCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NFCObservableCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NFCObservableCollectionProperty =
            DependencyProperty.Register
            (
                "NFCObservableCollection",
                typeof(ObservableCollection<NfcField>),
                typeof(PivotPage),
                new PropertyMetadata
                (
                    new ObservableCollection<NfcField>()
                )
            );









        public MobileServiceCollection<NfcField, NfcField> NfcItems
        {
            get { return (MobileServiceCollection<NfcField, NfcField>)GetValue(NfcItemsProperty); }
            set { SetValue(NfcItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static DependencyProperty NfcItemsProperty =
            DependencyProperty.Register
            (
                "NfcItems",
                typeof(MobileServiceCollection<NfcField, NfcField>),
                typeof(PivotPage),
                new PropertyMetadata
                (
                    null
                )
            );


        #endregion

        public PivotPage()
        {
            this.InitializeComponent();


            //NfcListView.DataContext = NfcItems;
            NfcListView.DataContext = this;

            RefreshNfcItems();
            //NfcListView.ItemsSource = NFCObservableCollection;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;


            btnDisconnect.IsEnabled = App.Credential != null;

            
            this.NfcRecievers();
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
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            //this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = this.pivot.SelectedIndex == 0 ? FirstGroupName : SecondGroupName;
            //var group = this.DefaultViewModel[groupName] as SampleDataGroup;

            // Scroll the new item into view.
            var container = this.pivot.ContainerFromIndex(this.pivot.SelectedIndex) as ContentControl;
            var listView = container.ContentTemplateRoot as ListView;
            // listView.ScrollIntoView(newItem, ScrollIntoViewAlignment.Leading);
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var clickedNfc = ((NfcField)e.ClickedItem);
            if (!Frame.Navigate(typeof(ItemPage), clickedNfc))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
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



        private async void ReadTag_Tapped(object sender, TappedRoutedEventArgs e)
        {


            NfcFieldTagged nfcFieldTagged = new NfcFieldTagged()
            {
                NfcFieldId = "D23C0071-1CC9-415D-B63E-13936F0E7085",
                Lat="3",
                Lon ="11",
                Address="ciao"
            };
            try
            {

                await App.proxy.GetTable<NfcFieldTagged>().InsertAsync(nfcFieldTagged);

            }
            catch (Exception ex)
            {
                var msr = ex.Message;
            }

            RefreshNfcItems();

            return;



            string message;
            try
            {

                var par = new Dictionary<string, string>();
                par.Add("BikeRId", "1");
                par.Add("y", "1");
                // Asynchronously call the custom API using the POST method. 
                var result = await App.proxy
                    .InvokeApiAsync<int>("isstolenbike",
                    System.Net.Http.HttpMethod.Post, par);



                message = result.ToString();

            }
            catch (MobileServiceInvalidOperationException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }

        }


        private async void BuildTag_Tapped(object sender, TappedRoutedEventArgs e)
        {


            NfcField field = new NfcField()
            {
                TagContent = nfcTagContent.Text,
                TagStatus = "Active",
                Note = nfcNote.Text,
                FriendlyName = nfcFriendlyName.Text,
                TagKind = "selfbuild"
            };
            try
            {

                await App.proxy.GetTable<NfcField>().InsertAsync(field);



            }
            catch (Exception ex)
            {

            }

            RefreshNfcItems();

        }



        private async void RefreshNfcItems()
        {
            // This code refreshes the entries in the list view be querying the TodoItems table.
            // The query excludes completed TodoItems
            //NFCObservableCollection = new ObservableCollection<NfcField>();
            //NFCObservableCollection.Add(new NfcField() { FriendlyName = "ciao" });

            try
            {
                NfcItems = await NfcTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {

            }

        }

        private void Disconnect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.DisconnectAccount();

            Frame.Navigate(typeof(Login));
        }

    }


    public class MarkAllResult
    {
        public int Count { get; set; }
    }

}

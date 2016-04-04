﻿using DROM_Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DROM_Client.Models.NewOrderData;
using DROM_Client.Models.BusinessObjects;
using Windows.UI.Popups;
using DROM_Client.ViewModels;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DROM_Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrderPage : Page
    {
        public CreateOrderPage()
        {
            this.InitializeComponent();
        }

        #region Save click and Popups and cancel
        //From microsoft guide: https://msdn.microsoft.com/da-dk/library/windows/apps/xaml/br208674?cs-save-lang=1&cs-lang=csharp
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Do you want to save this order?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Yes",
                new UICommandInvokedHandler(this.Save_Popup_Yes)));
            messageDialog.Commands.Add(new UICommand("No",
                new UICommandInvokedHandler(this.Save_Popup_No)));

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private async void Save_Popup_Yes(IUICommand command)
        {
            var viewModel = DataContext as CreateOrderPageViewModel;
            
            if(DeliveryCombobox.SelectedItem == null) //If no Delivery method is selected
            {
                CreateAndShowMessageDialog("Please select a delivery method.");
                return;
            }
            //TODO: Check if all information is entered
            switch ((DeliveryCombobox.SelectedItem as ComboBoxItem).Content as string) //Checks if all data is inserted
            {
                case "For serving":
                    if (!All_Information_Entered_For_Serving())
                    {
                        return;
                    }
                    break;
                case "For pickup":
                    if(!All_Information_Entered_For_Pickup())
                    {
                        return;
                    }
                    break;
                case "For delivery":
                    if(!All_Information_Entered_For_Delivery())
                    {
                        return;
                    }
                    break;
            }
            viewModel.SaveOrder();
            Frame.Navigate(typeof(OrderPage));
        }

        #region Checks to ensure that all information has been entered

        /// <summary>
        /// Checks if all information is entered for serving.
        /// </summary>
        /// <returns></returns>
        private bool All_Information_Entered_For_Serving()
        {
            if(Table_Number_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter a table number to serve to.");
                return false;
            }
            int tableNumber;
            if(!(int.TryParse(Table_Number_Text_Box.Text,out tableNumber)))
            {
                CreateAndShowMessageDialog("Please enter a valid table integer number.");
                return false;
            }
            return true;
        }

        private bool All_Information_Entered_For_Pickup()
        {
            return true;
        }

        private bool All_Information_Entered_For_Delivery()
        {
            int phoneNumber;
            if (First_Name_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's first name.");
                return false;
            }
            else if (Last_Name_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's first name.");
                return false;
            }
            else if (Email_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's email address.");
                return false;
            }
            else if (!Email_Text_Box.Text.Contains('@'))
            {
                CreateAndShowMessageDialog("Please enter a valid email address, '@' is missing.");
                return false;
            }
            else if (Phone_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's phone number");
                return false;
            }
            else if (!int.TryParse(Phone_Text_Box.Text, out phoneNumber))
            {
                CreateAndShowMessageDialog("Please enter a valid phone number (use integers only).");
            }
            else if (phoneNumber == 0)
            {
                CreateAndShowMessageDialog("Sorry, but the phone number cannot be '0'");
            }
            else if (Street_And_Number_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's street and number");
            }


            
            return true;
        }

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        #endregion

        private void Save_Popup_No(IUICommand command)
        {
            //Do nothing
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }
        #endregion

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string quanAsString = this.Quantity_Box.Text;
            int quanAsInt;
            if(int.TryParse(quanAsString, out quanAsInt))
            {
                var NewItem = this.Item_Box.SelectedItem as Item;
                var viewModel = this.DataContext as CreateOrderPageViewModel;
                viewModel.AddQuantityAndItem(quanAsInt, NewItem);
            }
            else
            {
                var messageDialog = new MessageDialog("Quantity needs to be an integer value.");
                await messageDialog.ShowAsync();
            }
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Make sure something is selected
            if(this.Items_On_Order_List_View.SelectedItems.Count == 1)
            {
                var selected = (KeyValuePair<Item, int>) this.Items_On_Order_List_View.SelectedItem;

                var viewModel = this.DataContext as CreateOrderPageViewModel;
                viewModel.RemoveItem(selected.Key);
            }
            else
            {
                var messageDialog = new MessageDialog("You need to select one and only one item from the list above.");
                await messageDialog.ShowAsync();
            }
        }

        private async void Send_Test_Post_Call(object sender, RoutedEventArgs e)
        {
            APICaller apiCaller = new APICaller();
            await apiCaller.PostOrderAsync(new NewOrderInfo()
            {
                Table = "1",
                Customer = new Customer()
                {
                    FirstAndMiddleNames = "Bob",
                    LastName = "Bobson",
                    City = "TestCity",
                    Email = "Test@mail.dk",
                    Phone = 12121212,
                    StreetAndNumber = "testgade 12",
                    ZipCode = 2300
                },
                ItemsAndQuantity = new Dictionary<Item, int>()
                {
                    {new Item()
                    {
                        Name = "Lone Star"
                    }, 2},
                    {new Item()
                    {
                        Name = "Cola"
                    }, 4 }
                },
                Notes = "this is a very special and very testy order",
                OrderType = "Delivery",
                OrderDate = DateTime.Now
            });
        }
    }
}
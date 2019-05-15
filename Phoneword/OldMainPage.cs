using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Phoneword
{
    public class OldMainPage : ContentPage
    {
        private Entry phoneNumberText;
        private Button translateButton;
        private Button callButton;

        private string translatedNumber;

        public OldMainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);
            var panel = new StackLayout() { Spacing = 15 };

            panel.Children.Add(new Label
            {
                Text = "Enter a phoneword",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN"
            };

            translateButton = new Button
            {
                Text = "Translate"
            };
            translateButton.Clicked += this.OnTranslate;

            callButton = new Button
            {
                Text = "Call",
                IsEnabled = false
            };
            callButton.Clicked += this.OnCall;

            panel.Children.Add(phoneNumberText);
            panel.Children.Add(translateButton);
            panel.Children.Add(callButton);

            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = phoneNumberText.Text.ToNumber();

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = $"Call {translatedNumber}";
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        private async void OnCall(object sender, EventArgs e)
        {
            if(await DisplayAlert("Dial a Number", $"Would you like to call {translatedNumber}?", "Yes", "No"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
    }
}

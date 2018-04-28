using System;
using System.Collections.Generic;
using MomitKiller.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using MomitKiller.Models;

namespace MomitKiller.Views
{
    public partial class ThermostatView : ContentPage
    {
        private ThermostatViewModel _bindingContext;

        public ThermostatView()
        {
            try
            {
                InitializeComponent();
                _bindingContext = new ThermostatViewModel();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error:", ex.ToString(), "Aceptar");
            }
        }

        protected override async void OnAppearing()
        {
            BindingContext = _bindingContext;
            await _bindingContext.InitAsync();
        }

        private async void ModeChanged(object sender, EventArgs e)
        {
            IsBusy = true;
            var classId = ((Button)sender).ClassId;
            var mode = (ThermostatModes)Enum.Parse(typeof(ThermostatModes), classId);
            await _bindingContext.SetModeAsync(mode);
            IsBusy = false;
        }

        private async void SetpointChanged(object sender, ValueChangedEventArgs e)
        {
            await Task.FromResult(false);
        }
    }
}

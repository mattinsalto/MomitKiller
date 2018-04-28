using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MomitKiller.Services;
using MomitKiller.Models;
using Xamarin.Forms;
using System.Windows.Input;
using System.Configuration;

namespace MomitKiller.ViewModels
{
    public class ThermostatViewModel : ExtendedBindableObject
    {
        private ThermostatModes _mode;
        private Color _enabledColor;
        private Color _disabledColor;
        private Color _offColor;
        private Color _manualColor;
        private Color _calendarColor;
        private decimal _currentTemp;
        private decimal _setpoint;
        private bool _isRelayOn;
        private string _baseUrl;
        private MomitKillerApi _momitKillerApi;

        public ThermostatModes Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    RaisePropertyChanged(() => Mode);
                }
            }
        }
        public Color EnabledColor
        {
            get { return _enabledColor; }
            set
            {
                if (_enabledColor != value)
                {
                    _enabledColor = value;
                }
            }
        }
        public Color OffColor
        {
            get
            {
                return _offColor;
            }
            set
            {
                if (_offColor != value)
                {
                    _offColor = value;
                    RaisePropertyChanged(() => OffColor);
                }
            }
        }
        public Color ManualColor
        {
            get
            {
                return _manualColor;
            }
            set
            {
                if (_manualColor != value)
                {
                    _manualColor = value;
                    RaisePropertyChanged(() => ManualColor);
                }
            }
        }
        public Color CalendarColor
        {
            get
            {
                return _calendarColor;
            }
            set
            {
                if (_calendarColor != value)
                {
                    _calendarColor = value;
                    RaisePropertyChanged(() => CalendarColor);
                }
            }
        }
        public decimal CurrentTemp
        {
            get
            {
                return _currentTemp;
            }
            set
            {
                if (_currentTemp != value)
                {
                    _currentTemp = value;
                    RaisePropertyChanged(() => CurrentTemp);
                }
            }
        }
        public decimal Setpoint
        {
            get
            {
                return _setpoint;
            }
            set
            {
                if (_setpoint != value)
                {
                    _setpoint = value;
                    RaisePropertyChanged(() => Setpoint);
                }
            }
        }

        public decimal MinValue
        {
            get;
        }

        public decimal MaxValue
        {
            get;
        }

        public bool IsRelayOn
        {
            get
            {
                return _isRelayOn;
            }
            set
            {
                if (_isRelayOn != value)
                {
                    _isRelayOn = value;
                    RaisePropertyChanged(() => IsRelayOn);
                }
            }
        }

        public ICommand ReloadCommand => new Command(async () => await ReloadAsync());
        public ICommand SetSetpointCommand => new Command(async () => await SetSetpointAsync());

        public ThermostatViewModel()
        {
            MaxValue = 26m;
            MinValue = 15m;
            _enabledColor = Color.FromHex("#f444a4");
            _disabledColor = Color.Silver;
            _baseUrl = Settings.BaseUrl;
        }

        public async Task InitAsync()
        {
            _momitKillerApi = new MomitKillerApi(_baseUrl);
            var thermostatStatus = await _momitKillerApi.ThermostatGetStatusAsync();

            await SetModeAsync(thermostatStatus.Mode, isInit: true);
            CurrentTemp = thermostatStatus.Temperature;
            Setpoint = thermostatStatus.Setpoint;
            IsRelayOn = thermostatStatus.RelayStatus == RelayStatuses.On;
        }

        public async Task SetModeAsync(ThermostatModes mode, bool isInit = false)
        {
            Mode = mode;

            switch (mode)
            {
                case ThermostatModes.Off:
                    OffColor = _enabledColor;
                    ManualColor = _disabledColor;
                    CalendarColor = _disabledColor;
                    break;
                case ThermostatModes.Manual:
                    ManualColor = _enabledColor;
                    OffColor = _disabledColor;
                    CalendarColor = _disabledColor;
                    break;
                // Not implemented. The button is disabled
                case ThermostatModes.Calendar:
                    CalendarColor = _enabledColor;
                    ManualColor = _disabledColor;
                    OffColor = _disabledColor;
                    break;
            }

            if (!isInit)
            {
                // Changing mode checks operation conditions on the server
                // and returns a ThermostatStatus
                var thermostatStatus = await _momitKillerApi.ThermostatSetModeAsync(Mode);
                CurrentTemp = thermostatStatus.Temperature;
                Setpoint = thermostatStatus.Setpoint;
                IsRelayOn = thermostatStatus.RelayStatus == RelayStatuses.On;
            }
        }

        public async Task ReloadAsync()
        {
            var thermostatStatus = await _momitKillerApi.ThermostatGetStatusAsync();

            await SetModeAsync(thermostatStatus.Mode, isInit: true);
            CurrentTemp = thermostatStatus.Temperature;
            Setpoint = thermostatStatus.Setpoint;
            IsRelayOn = thermostatStatus.RelayStatus == RelayStatuses.On;
        }

        public async Task SetSetpointAsync()
        {
            // Changing the setpoint checks operation conditions on the server
            // and returns a ThermostatStatus
            var thermostatStatus = await _momitKillerApi.ThermostatSetSetpointAsync(Setpoint);
            CurrentTemp = thermostatStatus.Temperature;
            IsRelayOn = thermostatStatus.RelayStatus == RelayStatuses.On;
        }
    }
}

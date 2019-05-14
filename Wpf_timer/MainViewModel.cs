using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Wpf_timer
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private DispatcherTimer Timer = new DispatcherTimer();
        private int _time;
        private int _timeB;
        private string _t;
        private bool _check = false;
        public string Time
        {
            get
            {
                if (_timeB > 0 && _timeB <= 72000)
                {
                    TimeSpan result = TimeSpan.FromSeconds(_timeB);
                    return result.ToString();
                }
                return "0";
            }
            set
            {
                _t = value;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                try
                {
                    _timeB = Convert.ToInt32(_t);
                    if (_timeB <= 72000 && _timeB > 0)
                    {
                        _time = _timeB;
                        _check = true;
                    }
                    else
                    {
                        _time = 0;
                        throw new Exception("Limit on input seconds from 1 to 72000.");
                    }
                }
                catch (Exception e)
                {
                    //if (_time < 1)
                        _check = false;
                    MessageBox.Show(e.Message + "\nString only accepts integers.");
                    _time = _timeB = 0;
                    Time1 = Time2 = "00:00:00";
                }
                OnPropertyChanged("Time");
            }
        }

        private string _time1 = "00:00:00";
        public string Time1
        {
            get { return _time1; }
            set
            {
                _time1 = value;
                OnPropertyChanged("Time1");
            }
        }
        private string _time2 = "00:00:00";
        public string Time2
        {
            get { return _time2; }
            set
            {
                _time2 = value;
                OnPropertyChanged("Time2");
            }
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (_time > 0 && _check == true)
            {
                if (_time % 2 == 0)
                {
                    TimeSpan result = TimeSpan.FromSeconds(_time);
                    Time1 = result.ToString();
                    _time--;

                }
                else if (_time % 2 != 0)
                {
                    TimeSpan result = TimeSpan.FromSeconds(_time);
                    Time2 = result.ToString();
                    _time--;

                }
            }
            else if (_time == 0 && _check == true)
            {
                TimeSpan result = TimeSpan.FromSeconds(_time);
                Time1 = result.ToString();
                Timer.Stop();
                MessageBox.Show("Time is over!");
            }
        }
        public ICommand FocusOn
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Timer.Stop();
                    Timer = new DispatcherTimer();
                });
            }
        }
        public ICommand FocusOut
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Timer.Interval = new TimeSpan(0, 0, 1);
                    Timer.Tick += Timer_Tick;
                    Timer.Start();
                });
            }
        }

    }
}

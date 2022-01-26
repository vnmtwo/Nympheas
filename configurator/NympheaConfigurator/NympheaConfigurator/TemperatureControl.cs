using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NympheaConfigurator
{
    public class DateModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
    public class TemperatureControl:ViewModelBase
    {
        public SeriesCollection Series { get; internal set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        private Dictionary<int, int> Addr = new Dictionary<int, int>();
        private CartesianMapper<DateModel> dayConfig;
        double _axisMax, _axisMin;
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnProperyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnProperyChanged("AxisMin");
                
            }
        }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        public TemperatureControl()
        {
            DateTimeFormatter = value => new DateTime((long)value).ToString("HH:mm:ss");
            dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => dayModel.DateTime.Ticks)
                .Y(dayModel => dayModel.Value);
            
            Series = new SeriesCollection();
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;
            SetAxisLimits();
            var task = Task.Run(async () => {
                for (; ; )
                {
                    await Task.Delay(1*1000);
                    SetAxisLimits();
                }
            });
        }

        DateTime lastUpdate;
        internal void ParsePacket(byte[] buffer)
        {
            if (buffer[0] == 't')
            {
                int a = buffer[1];
                if (a < 255)
                {
                    if (Addr.ContainsKey(a))
                    {
                        int n = Addr[a];
                        Series[n].Values.Add(new DateModel { DateTime = System.DateTime.Now, Value = BitConverter.ToSingle(buffer, 2) });
                        if (Series[n].Values.Count > 300) Series[n].Values.RemoveAt(0);
                        //if (DateTime.Now.Subtract(lastUpdate).Seconds>1)
                        //    SetAxisLimits();
                    }
                    else
                    {
                        LineSeries ls = new LineSeries(dayConfig);
                        ls.Title = "t" + a.ToString();
                        ls.PointGeometry = null;
                        ls.Fill = System.Windows.Media.Brushes.Transparent;
                        ls.Values = new ChartValues<DateModel>();
                        ls.Values.Add(new DateModel { DateTime = System.DateTime.Now, Value = BitConverter.ToSingle(buffer, 2) });
                        Addr.Add(a, Series.Count);
                        Series.Add(ls);
                        //OnProperyChanged("Series");
                    }
                }
            }
        }
        private void SetAxisLimits()
        {
            var now = DateTime.Now;
            AxisMax = now.Ticks + TimeSpan.FromSeconds(10).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(5*60).Ticks; // and 8 seconds behind
            //lastUpdate = now;
        }
    }
}
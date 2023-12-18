using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Windows.Media.Imaging;
using WpfApp4.Model;
using WpfApp4.StartUpHelper;
using WpfApp4.View;

namespace WpfApp4
{
    public partial class MainView : ObservableObject
    {
        private readonly List<DateTimePoint> _values = new();
        private readonly DateTimeAxis _customAxis;
        private readonly ModbusMaster _master = new();
        private readonly ApplicationContex applicationContex = new ApplicationContex();
        private readonly IAbsrtactFactory<GridDB> factory;

        //private readonly IAbsrtactFactory<GridDB> _factory;


        public MainView(IAbsrtactFactory<GridDB> _factory)
        {
            factory = _factory;
            Series = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _values,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null
            }
        };

            _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
            {
                CustomSeparators = GetSeparators(),
                AnimationsSpeed = TimeSpan.FromMilliseconds(0),
                SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
            };

            XAxes = new Axis[] { _customAxis };


            _ = ReadData();
            factory = _factory;
        }

        public ObservableCollection<ISeries> Series { get; set; }

        public Axis[] XAxes { get; set; }

        public object Sync { get; } = new object();

        public bool _IsReading { get; set; } = true;

        public ushort registr = 25;

        public string ip = "127.0.0.1";
        [ObservableProperty]
        public string? register;

        private async Task ReadData()
        {

            while (_IsReading)
            {

                await Task.Delay(100);

                lock (Sync)
                {
                    _values.Add(new DateTimePoint(DateTime.Now, _master.ModbusTcpMasterReadHoldingRegistr(registr, ip)));
                    if (_values.Count > 550) _values.RemoveAt(0);
                    _customAxis.CustomSeparators = GetSeparators();
                    Register = Convert.ToString(_master.ModbusTcpMasterReadHoldingRegistr(registr, ip));
                    _ = WriteData();


                }

            }

        }

        private async Task WriteData()
        {
            DataTimeDB dataTimeDBNew = new DataTimeDB
            {
                DateTime = DateTime.Now,
                Data = _master.ModbusTcpMasterReadHoldingRegistr(registr, ip)
            };
            applicationContex.DataTimeDBs.Add(dataTimeDBNew);
            applicationContex.SaveChanges();
        }

        private double[] GetSeparators()
        {
            var now = DateTime.Now;

            return new double[]
            {
            now.AddSeconds(-60).Ticks,
            now.AddSeconds(-25).Ticks,
            now.AddSeconds(-20).Ticks,
            now.AddSeconds(-15).Ticks,
            now.AddSeconds(-10).Ticks,
            now.AddSeconds(-5).Ticks,
            now.Ticks
            };
        }

        private static string Formatter(DateTime date)
        {
            var secsAgo = (DateTime.Now - date).TotalSeconds;

            return secsAgo < 1
            ? "now"
            : $"{secsAgo:N0}s ago";
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConectTCPCommand))]
        public string? _connect = "TCP OK";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConectTCPCommand))]
        public string registerRead = "25";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConectTCPCommand))]
        public string _IPAdress = "127.0.0.1";

        [RelayCommand]
        private void ConectTCP()
        {
            Connect = "read";
            _IsReading = true;

            registr = ushort.Parse(RegisterRead);

            ip = IPAdress;
        }

        [RelayCommand]
        private void DisconectTCP()
        {
            Connect = "close";
            registr = 0;
            _master.ModbusTcpMasterReadHoldingRegistr(registr, ip);
        }


        [RelayCommand]
        private void OpenWindows()
        {

            factory.Create().Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
namespace NympheaConfigurator.Comtrols
{
    /// <summary>
    /// Логика взаимодействия для CustomLvChartLegend.xaml
    /// </summary>
    public partial class CustomLvChartLegend : UserControl, IChartLegend
    {
        /// <summary>
        /// Orientation of the legend entries
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CustomLvChartLegend), new PropertyMetadata(Orientation.Horizontal));


        public CustomLvChartLegend()
        {
            InitializeComponent();

            itemsControl.DataContext = this;
            this.Loaded += CustomLvChartLegend_Loaded;
            this.Unloaded += CustomLvChartLegend_Unloaded;
        }

        private void CustomLvChartLegend_Unloaded(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)Parent).SizeChanged -= CustomLvChartLegend_SizeChanged;
            this.Unloaded -= CustomLvChartLegend_Unloaded;
        }

        private void CustomLvChartLegend_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CustomLvChartLegend_Loaded;
            this.Height = ((FrameworkElement)Parent).ActualHeight;
            ((FrameworkElement)Parent).SizeChanged += CustomLvChartLegend_SizeChanged;
        }

        private void CustomLvChartLegend_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = ((FrameworkElement)Parent).ActualHeight;
        }

        public ObservableCollection<CustomSeriesViewModel> LegendEntries { get; } = new ObservableCollection<CustomSeriesViewModel>();

        public List<SeriesViewModel> Series
        {
            get => LegendEntries.Select(x => x.SeriesViewModel).ToList();
            set
            {
                Chart ownerChart = GetOwnerChart();

                // note: value only contains the visible series                
                // remove all entries which also have been removed from the chart
                var removedSeries = LegendEntries.Where(x => !ownerChart.Series.Any(s => s == x.View)).ToList();
                foreach (var rs in removedSeries)
                    LegendEntries.Remove(rs);

                foreach (var svm in value)
                {
                    // add entries which are new                                        
                    // The SeriesViewModel instances are always new, so we have to compare using the title
                    if (!LegendEntries.Any(x => x.Title == svm.Title))
                    {
                        // find the series' UIElement by title
                        var seriesView = ownerChart.Series.FirstOrDefault(x => x.Title == svm.Title);
                        LegendEntries.Add(new CustomSeriesViewModel(svm, seriesView));
                    }

                }


                OnPropertyChanged();
            }
        }

        private Chart GetOwnerChart()
        {
            return FindParent<Chart>(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LegendEntries)
            {
                item.IsVisible = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in LegendEntries)
            {
                item.IsVisible = false;
            }
        }
    }

    public class CustomSeriesViewModel : INotifyPropertyChanged
    {
        public string Title { get => SeriesViewModel.Title; }

        public Brush Fill { get => SeriesViewModel.Fill ?? SeriesViewModel.Stroke; }

        public Brush Stroke { get => SeriesViewModel.Stroke; }
        public SeriesViewModel SeriesViewModel { get; }

        public ISeriesView View { get; }

        public bool IsVisible
        {
            get => ((UIElement)View).Visibility == Visibility.Visible;
            set
            {
                if (IsVisible != value)
                {
                    ((UIElement)View).Visibility = value ? Visibility.Visible : Visibility.Hidden;

                    OnPropertyChanged();
                }
            }
        }

        public CustomSeriesViewModel(SeriesViewModel svm, ISeriesView view)
        {
            this.SeriesViewModel = svm;
            this.View = view;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

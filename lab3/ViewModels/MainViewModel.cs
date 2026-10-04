using lab3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
namespace lab3 {
    public class MainViewModel : INotifyPropertyChanged {
        private double _h1 = 1.0;
        private double _h2 = 0.1;
        private bool _showExact = true;
        private bool _showLinear = true;
        private bool _showHermite = true;
        private bool _showError = false;
        private string _errorMessage = string.Empty;
        public double H1{
            get => _h1;
            set { _h1 = value; OnPropertyChanged(); Recalculate(); }
        }
        public double H2{
            get => _h2;
            set { _h2 = value; OnPropertyChanged(); Recalculate(); }
        }
        public bool ShowExact{
            get => _showExact;
            set { _showExact = value; OnPropertyChanged(); }
        }
        public bool ShowLinear{
            get => _showLinear;
            set { _showLinear = value; OnPropertyChanged(); }
        }
        public bool ShowHermite{
            get => _showHermite;
            set { _showHermite = value; OnPropertyChanged(); }
        }
        public bool ShowError{
            get => _showError;
            set { _showError = value; OnPropertyChanged(); Recalculate(); }
        }
        public string ErrorMessage {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public PointCollection ExactPoints { get; set; } = new PointCollection();
        public PointCollection LinearPoints { get; set; } = new PointCollection();
        public PointCollection HermitePoints { get; set; } = new PointCollection();
        public ICommand RecalculateCommand { get; }
        public MainViewModel(){
            RecalculateCommand = new RelayCommand(_ => Recalculate());
            Recalculate();
        }
        private void Recalculate(){
            if (H2 >= H1){
                ErrorMessage = "Помилка: Крок h2 має бути меншим за h1!";
                return;
            }
            if (H1 <= 0 || H2 <= 0){
                ErrorMessage = "Помилка: Кроки повинні бути більшими за 0!";
                return;
            }
            ErrorMessage = string.Empty;
            double xStart = 0;
            double xEnd = 6.28;
            var a1 = new List<DataPoint>();
            for (double x = xStart; x <= xEnd + 0.0001; x += H1){
                a1.Add(new DataPoint(x, Math.Sin(x)));
            }
            Interpolation[] interpolators = new Interpolation[]{
                new LinearInterpolation(),
                new HermiteInterpolation()
            };
            var exactList = new PointCollection();
            var linearList = new PointCollection();
            var hermiteList = new PointCollection();
            for (double x = xStart; x <= xEnd + 0.0001; x += H2){
                double exactY = Math.Sin(x);
                double linY = interpolators[0].Calculate(a1, x);
                double hermY = interpolators[1].Calculate(a1, x);
                if (ShowError){
                    exactList.Add(ToCanvasPoint(x, 0, xStart, xEnd));
                    linearList.Add(ToCanvasPoint(x, Math.Abs(exactY - linY), xStart, xEnd));
                    hermiteList.Add(ToCanvasPoint(x, Math.Abs(exactY - hermY), xStart, xEnd));
                }
                else{
                    exactList.Add(ToCanvasPoint(x, exactY, xStart, xEnd));
                    linearList.Add(ToCanvasPoint(x, linY, xStart, xEnd));
                    hermiteList.Add(ToCanvasPoint(x, hermY, xStart, xEnd));
                }
            }
            ExactPoints = exactList;
            LinearPoints = linearList;
            HermitePoints = hermiteList;
            OnPropertyChanged(nameof(ExactPoints));
            OnPropertyChanged(nameof(LinearPoints));
            OnPropertyChanged(nameof(HermitePoints));
        }
        private System.Windows.Point ToCanvasPoint(double x, double y, double xStart, double xEnd){
            double width = 500;
            double height = 300;
            double cx = (x - xStart) / (xEnd - xStart) * (width - 40) + 20;
            double cy = height / 2 - y * 100;
            if (ShowError){
                cy = height - 30 - y * 300;
            }
            return new System.Windows.Point(cx, cy);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
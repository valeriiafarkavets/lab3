using lab3;
using System.Collections.Generic;
namespace lab3 {
    public sealed class HermiteInterpolation : Interpolation{
        public override double Calculate(IReadOnlyList<DataPoint> points, double x) {
            int n = points.Count;
            if (n == 0) return 0;
            if (x <= points[0].X) return points[0].Y;
            if (x >= points[n - 1].X) return points[n - 1].Y;
            for (int i = 0; i < n - 1; i++) {
                if (x >= points[i].X && x <= points[i + 1].X){
                    double x0 = points[i].X;
                    double x1 = points[i + 1].X;
                    double y0 = points[i].Y;
                    double y1 = points[i + 1].Y;
                    double dx = x1 - x0;
                    double m0 = GetDerivative(points, i);
                    double m1 = GetDerivative(points, i + 1);
                    double t = (x - x0) / dx;
                    double t2 = t * t;
                    double t3 = t2 * t;
                    double h00 = 2 * t3 - 3 * t2 + 1;
                    double h10 = t3 - 2 * t2 + t;
                    double h01 = -2 * t3 + 3 * t2;
                    double h11 = t3 - t2;
                    return h00 * y0 + h10 * dx * m0 + h01 * y1 + h11 * dx * m1;
                }
            }
            return 0;
        }
        private double GetDerivative(IReadOnlyList<DataPoint> points, int i){
            int n = points.Count;
            if (n < 2) return 0;
            if (i == 0)
                return (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
            if (i == n - 1)
                return (points[n - 1].Y - points[n - 2].Y) / (points[n - 1].X - points[n - 2].X);
            return (points[i + 1].Y - points[i - 1].Y) / (points[i + 1].X - points[i - 1].X);
        }
    }
}
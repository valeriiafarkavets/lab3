using lab3;
using System.Collections.Generic;
namespace lab3 {
    public sealed class LinearInterpolation : Interpolation{
        public override double Calculate(IReadOnlyList<DataPoint> points, double x){
            if (points == null || points.Count == 0) return 0;
            if (x <= points[0].X) return points[0].Y;
            if (x >= points[points.Count - 1].X) return points[points.Count - 1].Y;
            for (int i = 0; i < points.Count - 1; i++){
                if (x >= points[i].X && x <= points[i + 1].X){
                    double x0 = points[i].X;
                    double x1 = points[i + 1].X;
                    double y0 = points[i].Y;
                    double y1 = points[i + 1].Y;
                    double t = (x - x0) / (x1 - x0);
                    return y0 + t * (y1 - y0);
                }
            }
            return 0;
        }
    }
}
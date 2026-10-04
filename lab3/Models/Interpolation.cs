using lab3;
using System.Collections.Generic;
namespace lab3 {
    public abstract class Interpolation{
        public abstract double Calculate(IReadOnlyList<DataPoint> points, double x);
    }
}
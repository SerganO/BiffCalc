using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calc
{
    class SplineTuple
    {
        public double a;
        public double b;
        public double c;
        public double d;
        public double x;

        public SplineTuple()
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;
            x = 0;
        }
    }

    class CubeSpline
    {
        private SplineTuple[] splines;
        private int n;

        public void build_spline(double[] x, double[] y, int n)
        {
            this.n = n;

            splines = new SplineTuple[n];
            for (int i = 0; i < n; ++i)
            {
                splines[i] = new SplineTuple();
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = 0.0;

            // GO
            double[] alpha = new double[n - 1];
            double[] beta = new double[n - 1];
            double A = 0, B = 0, C = 0, F = 0, h_i = 0, h_i1 = 0, z = 0;
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                h_i = x[i] - x[i - 1];
                h_i1 = x[i + 1] - x[i];
                A = h_i;
                C = 2.0 * (h_i + h_i1);
                B = h_i1;
                F = 6.0 * ((y[i + 1] - y[i]) / h_i1 - (y[i] - y[i - 1]) / h_i);
                z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            splines[n - 1].c = (F - A * beta[n - 2]) / (C + A * alpha[n - 2]);

            //REVERSE
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }

            for (int i = n - 1; i > 0; --i)
            {
                h_i = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / h_i;
                splines[i].b = h_i * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / h_i;
            }

        }

        public double f(double x)
        {
            if (splines == null)
                return 0;

            SplineTuple s;
            if (x <= splines[0].x)
                s = splines[0];
            else if (x >= splines[n - 1].x)
                s = splines[n - 1];
            else
            {
                int i = 0, j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                        j = k;
                    else
                        i = k;
                }
                s = splines[j];
            }

            double dx = (x - s.x);
            return s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx;
        }

    }
}

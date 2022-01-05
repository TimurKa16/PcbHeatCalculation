namespace Диплом
{
    public static class ThermalModelling
    {
        static void Calculate_X(double rho, double lambda, double alpha, double C,
            double dt, ref double[][] U, ref double[][] u0, double[][]q, int N, int M, double h)
        {
            double[] a = new double[N - 1]; // след узел
            double[] b = new double[N];     // тек узел
            double[] c = new double[N - 1]; // пред узел
            double[] d = new double[N];     // узел на пред шаге
            for (int m = 0; m < M; m++)
            {
                for (int i = 0; i < N - 1; i++)
                    a[i] = 1.0;

                b[0] = -1 * (1 + (alpha * h) / lambda + (C * rho * h * h) / (2 * lambda * dt));
                for (int i = 1; i < N - 1; i++)
                    b[i] = -1 * (2 + (C * rho * h * h) / (lambda * dt));
                b[N - 1] = -1 * (1 + (alpha * h) / lambda + (C * rho * h * h) / (2 * lambda * dt));

                for (int i = 0; i < N - 1; i++)
                    c[i] = 1.0;

                d[0] = ((h * h) / (2 * lambda)) * (q[m][0] / 3 + C * rho / dt * u0[m][0]);
                for (int i = 1; i < N - 1; i++)
                    d[i] = ((h * h) / lambda) * (q[m][i] / 3 + C * rho / dt * u0[m][i]);
                d[N - 1] = ((h * h) / (2 * lambda)) * (q[m][N - 1] / 3 + C * rho / dt * u0[m][N - 1]);
                ////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("f и g:");
                double[] f = new double[N - 1];
                double[] g = new double[N - 1];

                f[0] = -a[0] / b[0];
                g[0] = -d[0] / b[0];

                for (int i = 1; i < N - 1; i++)
                {
                    f[i] = -(a[i] / (b[i] + c[i - 1] * f[i - 1]));
                    g[i] = -((d[i] + c[i - 1] * g[i - 1]) / b[i] + c[i - 1] * f[i - 1]);
                }

                U[m][N - 1] = -(d[N - 1] + c[N - 2] * g[N - 2]) / (b[N - 1] + c[N - 2] * f[N - 2]);
                for (int i = N - 2; i >= 0; i--)
                    U[m][i] = f[i] * U[m][i + 1] + g[i];
            }
        }

        static void Calculate_Y(double rho, double lambda, double alpha, double C,
            double dt, ref double[][] U, ref double[][] u0, double[][] q, int N, int M, double h)
        {
            double[] a = new double[N - 1]; // след узел
            double[] b = new double[N];     // тек узел
            double[] c = new double[N - 1]; // пред узел
            double[] d = new double[N];     // узел на пред шаге
            for (int n = 0; n < N; n++)
            {
                for (int i = 0; i < M - 1; i++)
                    a[i] = 1.0;
                b[0] = -1 * (1 + (alpha * h) / lambda + (C * rho * h * h) / (2 * lambda * dt));
                for (int i = 1; i < M - 1; i++)
                    b[i] = -1 * (2 + (C * rho * h * h) / (lambda * dt));
                b[M - 1] = -1 * (1 + (alpha * h) / lambda + (C * rho * h * h) / (2 * lambda * dt));

                for (int i = 0; i < M - 1; i++)
                    c[i] = 1.0;

                d[0] = ((h * h) / (2 * lambda)) * (q[0][n] / 3 + C * rho / dt * u0[0][n]);
                for (int i = 1; i < M - 1; i++)
                    d[i] = ((h * h) / lambda) * (q[i][n] / 3 + C * rho / dt * u0[i][n]);
                d[M - 1] = ((h * h) / (2 * lambda)) * (q[M - 1][n] / 3 + C * rho / dt * u0[M - 1][n]);
                ////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("f и g:");
                double[] f = new double[M - 1];
                double[] g = new double[M - 1];

                f[0] = -a[0] / b[0];
                g[0] = -d[0] / b[0];

                for (int i = 1; i < M - 1; i++)
                {
                    f[i] = -(a[i] / (b[i] + c[i - 1] * f[i - 1]));
                    g[i] = -((d[i] + c[i - 1] * g[i - 1]) / b[i] + c[i - 1] * f[i - 1]);
                }
                //////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("Температуры:");
                U[M - 1][n] = -(d[M - 1] + c[M - 2] * g[M - 2]) / (b[M - 1] + c[M - 2] * f[M - 2]);
                for (int i = M - 2; i >= 0; i--)
                    U[i][n] = f[i] * U[i + 1][n] + g[i];
            }
        }

        static void Calculate(double rho, double lambda, double alpha, double C,
            double dt, ref double[][] U, ref double[][] u0, double[][] q, int N, int M, double h)
        {
            // По X
            Calculate_X(rho, lambda, alpha, C, dt, ref U, ref u0, q,  N, M, h);
            for (int i = 0; i < U.Length; i++)
                for (int j = 0; j < U[0].Length; j++)
                    u0[i][j] = U[i][j];

            // По Y
            Calculate_Y(rho, lambda, alpha, C, dt, ref U, ref u0, q, N, M, h);
            for (int i = 0; i < U.Length; i++)
                for (int j = 0; j < U[0].Length; j++)
                    if (U[i][j] < 0)
                        U[i][j] = 0;
        }

        static void InitializeQ(double[] Q, ref double[][] q, int[][] pcb, double lz)
        {
            double[] qTemp = new double[Q.Length];

            int[] sizeCounter = new int[Q.Length];
            for (int i = 0; i < pcb.Length; i++)
                for (int j = 0; j < pcb[0].Length; j++)
                    for (int k = 0; k < sizeCounter.Length; k++)
                        if (pcb[i][j] == k + 1)
                            sizeCounter[k]++;

            for (int i = 0; i < Q.Length; i++)
                qTemp[i] = Q[i] / (sizeCounter[i] * lz * 0.0001);

            for (int i = 0; i < q.Length; i++)
                for (int j = 0; j < q[0].Length; j++)
                    for (int k = 0; k < sizeCounter.Length; k++)
                        if (pcb[i][j] == k + 1)
                            q[i][j] = qTemp[k];
        }

        public static void Modelling(double rho, double lambda, double alpha, double C, 
            double lx, double ly, double lz, int dt, ref double[][] U, double temperature, double[] Q, int[][] pcb)
        {
            lx /= 100;  //  перевод из сантиметров в метры
            ly /= 100;
            lz /= 100;
            int t = dt;
            int N = pcb[0].Length;
            int M = pcb.Length;     //  равно размеру пп

            double h = lx / (N - 1);

            double [][] u0 = new double[M][]; //температура на предыдущем шаге
            for (int m = 0; m < M; m++)
                u0[m] = new double[N];

            U = new double[M][]; //температура на текущем шаге
            for (int m = 0; m < M; m++)
                U[m] = new double[N];            

            for (int m = 0; m < u0.Length; m++)
                for (int n = 0; n < u0[0].Length; n++)
                    u0[m][n] = temperature;
            
            double[][] q = new double[M][];
            for (int m = 0; m < M; m++)
                q[m] = new double[N];

            InitializeQ(Q, ref q, pcb, lz);

            Calculate(rho, lambda, alpha, C, dt, ref U, ref u0, q, N, M, h);
            /////////////////////////////////////////////////////////////////////////
            // a ----
            // b ------
            // c   ----
            // d ------
        }
    }
}

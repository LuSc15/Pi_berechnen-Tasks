using System;
using System.Diagnostics;

namespace Pi_berechnen
{
    
  
        internal class Program
        {
        private static void Main(string[] args)
            {
                Console.WriteLine(Math.PI);
                Stopwatch sw = new Stopwatch();
                ThreadedPI(8, sw);
                Console.WriteLine();
                ThreadedPI(4,sw);
                Console.WriteLine();
                ThreadedPI(3, sw);
                Console.WriteLine();
                ThreadedPI(2, sw);
                Console.WriteLine();
                ThreadedPI(1, sw);
                Console.WriteLine();
                NonThreadedPI(sw);
            }
        private static void NonThreadedPI(Stopwatch sw)
            {
                double pi = 0;
                int anzahlAufrufe = 1;

                Console.WriteLine("NonThreaded:");
                sw.Reset();
                sw.Start();
          
                for (int j = 1; j < anzahlAufrufe+1; j++)
                {
                    pi += PI_Berechnung(j, anzahlAufrufe);
                }
                sw.Stop();
                Console.WriteLine(pi);
                Console.WriteLine("Dauer {0:N0} Millisekunden (nonThreaded)", sw.ElapsedMilliseconds);
            }
        private static void ThreadedPI(int threads,Stopwatch sw)
            {
                double pi = 0;
                int anzahlAufrufe = threads;
                List<Task<double>> tasksSammlung = new List<Task<double>>();
                Task<double> aufgabe;

                Console.WriteLine("Mit " + threads + " Threads:");
                sw.Reset();
                sw.Start();

                for (int j = 0; j < threads; j++)
                {
                    aufgabe = Task.Run<double>(() =>
                    {
                        return PI_Berechnung(j, anzahlAufrufe);
                    });
                    tasksSammlung.Add(aufgabe);
                }

              //  Task.WaitAll(tasksSammlung.ToArray());       //nicht nötig, da item.Result wartet 
                //foreach (Task<double> item in tasksSammlung)
                //{
                //    pi += item.Result;
                //}
                pi = tasksSammlung.Sum(x => x.Result); //lambda statt foreach
                sw.Stop();
                Console.WriteLine(pi);
                Console.WriteLine("Dauer {0:N0} Millisekunden ("+threads+"x Thread)", sw.ElapsedMilliseconds);
            }

            // Nach John Machin (http://de.wikipedia.org/wiki/John_Machin)
            private static double PI_Berechnung(int startwert, int schrittweite)
            {
                const double durchlaeufe = 1_000_000_000;
                double x, y = 1 / durchlaeufe;
                double summe = 0;
                double pi;

                for (double i = startwert; i <= durchlaeufe; i += schrittweite)
                {
                    x = y * (i - 0.5);
                    summe += 4.0 / (1 + x * x);
                }

                pi = y * summe;

                return pi;
            }

    }
}


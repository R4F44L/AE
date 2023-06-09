using System.Collections.Generic;

namespace AlgorytmyDE
{
    public class Osobnik
    {
        public double fitness;
        public List<double> wektor;

        public Osobnik()
        {
            wektor = new List<double>();
        }
        public Osobnik(List<double> gotowyWektor)
        {
            wektor = new List<double>(gotowyWektor);
        }


    }
}
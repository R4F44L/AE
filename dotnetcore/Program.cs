using System;
using System.Collections.Generic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace AlgorytmyDE
{
    class Program
    {

        class Algorytm
        {
            public int wielkoscPopulacji;
            public int iloscIteracji;
            public double wspolczynnikAmplifikacji;
            public double wspolczynnikAmplifikacji2;
            public int isMinimum;
            public double wspolczynnikKrzyzowania;
            public int populacja;
            public int wymiary;
            public Random rnd;
            public string filename;
            public double minimum;
            public double maximum;
            public TYP_FUNKCJI typFunkcji;
            public TYP_MUTAJCI typMutacji;

            public Algorytm(double F = 0.5, double F2 = 0.7, double CR = 0.9, int cwielkoscPopulacji = 50, int ciloscIteracji = 100, int cwymiar = 3, double cminimum = -600, double cmaximum = 600, TYP_MUTAJCI tYP_MUTAJCI = TYP_MUTAJCI.rand1Bin, TYP_FUNKCJI tYP_FUNKCJI = TYP_FUNKCJI.Griewanka, int cisMinimum = 0)
            {
                wymiary = cwymiar;
                wspolczynnikAmplifikacji = F;
                wspolczynnikKrzyzowania = CR;
                iloscIteracji = ciloscIteracji;
                wielkoscPopulacji = cwielkoscPopulacji;
                wspolczynnikAmplifikacji2 = F2;
                minimum = cminimum;
                maximum = cmaximum;
                typFunkcji = tYP_FUNKCJI;
                typMutacji = tYP_MUTAJCI;
                rnd = new Random();
                isMinimum = cisMinimum;

            }

            public enum TYP_FUNKCJI
            {
                Rastrigina = 1,
                Griewanka = 2,
                ThreeHumpCamel = 3,
                Sphere = 4,
                Beale = 5,
                Himmelblau = 6,
                Booth = 7,
                Eggholder = 8,
                SchafferNo2 = 9,
                SchafferNo4 = 11,
                KeyReader = 10

            }

            public enum TYP_MUTAJCI
            {
                rand1Bin = 1,
                rand2Bin = 2,
                best1Bin = 3,
                best2Bin = 4,
                custom = 5
            }


            public Populacja CreatePopulation()
            {
                List<Osobnik> individuals = new List<Osobnik>();
                Console.WriteLine(this.wielkoscPopulacji);
                for (int i = 1; i <= wielkoscPopulacji; i++)
                {
                    Osobnik individual = new Osobnik();

                    for (int j = 0; j < wymiary; j++)
                    {
                        double randomNumber = rnd.NextDouble() * (maximum - minimum) + minimum; //random number generation
                        individual.wektor.Add(randomNumber);
                    }

                    individuals.Add(individual);
                }

                return new Populacja(individuals);
            }

            static double RastriginFunction(Osobnik ososbnik, int dimensions)
            {
                double sum = 0;

                foreach (double x in ososbnik.wektor)
                {
                    sum += Math.Pow(x, 2) - 10 * Math.Cos(2 * Math.PI * x);
                }
                return 10 * dimensions + sum;
            }
            static double GriewankaFunction(Osobnik osobnik, int dimensions)
            {
                double sum = 0;

                foreach (double x in osobnik.wektor)
                {
                    sum += Math.Pow(x, 2);
                }
                double product = 1;
                for (int i = 1; i <= osobnik.wektor.Count; i++)
                {
                    product *= Math.Cos(osobnik.wektor[i - 1] / Math.Sqrt(i));
                };
                product = Math.Cos(osobnik.wektor[0]) * Math.Cos(osobnik.wektor[1] / Math.Sqrt(2));

                return (1 + 0.00025 * sum - product);
            }
            static double RosenbrockFunction(Osobnik ososbnik, int dimensions)
            {
                double sum = 0;

                foreach (double x in ososbnik.wektor)
                {
                    sum += Math.Pow(x, 2) - 10 * Math.Cos(2 * Math.PI * x);
                }
                return 10 * dimensions + sum;
            }

            static double Sphere(Osobnik ososbnik, int dimensions)
            {
                double sum = 0;

                foreach (double x in ososbnik.wektor)
                {
                    sum += Math.Pow(x, 2);
                }
                return sum;
            }

            static double Himmelblau(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                return (Math.Pow((Math.Pow(x, 2) + y - 11), 2) + Math.Pow(x + Math.Pow(y, 2) - 7, 2));
            }
            public static double ThreeHumpCamel(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                double term1 = 2 * Math.Pow(x, 2);
                double term2 = -1.05 * Math.Pow(x, 4);
                double term3 = Math.Pow(x, 6) / 6;
                double term4 = x * y;
                double term5 = Math.Pow(y, 2);

                double result = term1 + term2 + term3 + term4 + term5;
                return result;
            }

            static double Booth(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                return (Math.Pow(x + 2 * y - 7, 2) + Math.Pow(2 * x + y - 5, 2));
            }

            public static double Eggholder(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                double term1 = -(y + 47) * Math.Sin(Math.Sqrt(Math.Abs((x / 2) + y + 47)));
                double term2 = -x * Math.Sin(Math.Sqrt(Math.Abs(x - (y + 47))));

                double result = term1 + term2;
                return result;
            }
            public static double SchafferNo4(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                double term1 = Math.Pow(Math.Cos(Math.Sin(Math.Abs(Math.Pow(x, 2) - Math.Pow(y, 2)))), 2) - 0.5;
                double term2 = Math.Pow(1 + 0.001 * (Math.Pow(x, 2) + Math.Pow(y, 2)), 2);

                double result = 0.5 + term1 / term2;
                return result;
            }
            public static double SchafferNo2(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                double term1 = Math.Pow(Math.Sin(Math.Pow(x, 2) - Math.Pow(y, 2)), 2) - 0.5;
                double term2 = Math.Pow(1 + 0.001 * (Math.Pow(x, 2) + Math.Pow(y, 2)), 2);

                double result = 0.5 + term1 / term2;
                return result;
            }
            public static double Beale(Osobnik osobnik)
            {
                double x = osobnik.wektor[0];
                double y = osobnik.wektor[1];
                double term1 = Math.Pow(1.5 - x + x * y, 2);
                double term2 = Math.Pow(2.25 - x + x * Math.Pow(y, 2), 2);
                double term3 = Math.Pow(2.625 - x + x * Math.Pow(y, 3), 2);

                double result = term1 + term2 + term3;
                return result;
            }


            public void OcenFitnes(Populacja populacja, TYP_FUNKCJI typ)
            {

                foreach (Osobnik Osobnik in populacja.elementy)
                {
                    if (typ == TYP_FUNKCJI.Griewanka)
                    {
                        Osobnik.fitness = GriewankaFunction(Osobnik, wymiary);
                    }
                    if (typ == TYP_FUNKCJI.Rastrigina)
                    {
                        Osobnik.fitness = RastriginFunction(Osobnik, wymiary);
                    }
                    if (typ == TYP_FUNKCJI.ThreeHumpCamel)
                    {
                        Osobnik.fitness = ThreeHumpCamel(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.Sphere)
                    {
                        Osobnik.fitness = Sphere(Osobnik, wymiary);
                    }
                    if (typ == TYP_FUNKCJI.Himmelblau)
                    {
                        Osobnik.fitness = Himmelblau(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.Booth)
                    {
                        Osobnik.fitness = Booth(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.Eggholder)
                    {
                        Osobnik.fitness = Eggholder(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.SchafferNo2)
                    {
                        Osobnik.fitness = SchafferNo2(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.SchafferNo4)
                    {
                        Osobnik.fitness = SchafferNo4(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.Beale)
                    {
                        Osobnik.fitness = Beale(Osobnik);
                    }
                    if (typ == TYP_FUNKCJI.KeyReader)
                    {

                        //write to file osobnik dimensions
                        // run python script to evaluate
                        WriteDataToFile(Osobnik.wektor);
                        run_cmd("skryptAE.py", "");
                        // read evaulation from file and asssign it to osobnik
                        Osobnik.fitness = ReadDataFromFile();
                    }
                }
            }

            public Populacja Skrypt()
            {
                Populacja populacja = CreatePopulation();
                using (StreamWriter w = File.AppendText(String.Format("results{0}{1}{2}.txt", wspolczynnikKrzyzowania, wspolczynnikAmplifikacji, typMutacji.ToString())))
                {
                    for (int i = 0; i < iloscIteracji; i++)
                    {
                        Mutacja(populacja, typMutacji);
                        w.WriteLine((-(populacja.xNajlepszych()[0].fitness)).ToString());
                    }

                    // if isMinimum multiply every fitness by -1
                    // if (isMinimum == 1)
                    // {
                    //     foreach (Osobnik osobnik in populacja.elementy)
                    //     {
                    //         osobnik.fitness *= -1;
                    //     }
                    // }
                }
                return populacja;
            }
            public void Mutacja(Populacja populacja, TYP_MUTAJCI typ)
            {
                List<Osobnik> newGen = new List<Osobnik>();

                Osobnik najlepszy = populacja.Najlepszy(isMinimum);
                List<Osobnik> najlepsi = populacja.xNajlepszych(isMinimum);



                OcenFitnes(populacja, typFunkcji);

                foreach (Osobnik osobnik in populacja.elementy)
                {
                    List<int> indexy = new List<int>();

                    if (typ == TYP_MUTAJCI.best1Bin)
                    {
                        List<Osobnik> doOminiecia = new List<Osobnik>
                        {
                            osobnik
                        };
                        List<Osobnik> losowi = populacja.Losowi(rnd, 2, doOminiecia);
                        Osobnik osobnik1 = losowi[0];
                        Osobnik osobnik2 = losowi[1];

                        int i = 0;
                        int R = rnd.Next(populacja.elementy.Count);
                        Osobnik kandydat = new Osobnik();
                        foreach (double param in osobnik.wektor)
                        {
                            double ri = rnd.NextDouble();
                            if (ri < this.wspolczynnikKrzyzowania || i == R)
                            {
                                // simple mutation
                                double newElement = najlepszy.wektor[i] + wspolczynnikAmplifikacji * (osobnik1.wektor[i] - osobnik2.wektor[i]);

                                if (newElement > minimum && newElement < maximum)
                                {
                                    kandydat.wektor.Add(newElement);
                                }
                                else
                                {
                                    kandydat.wektor.Add(param);
                                }
                            }
                            else
                            {
                                kandydat.wektor.Add(param);
                            }

                            i++;
                        }
                        Populacja kandydatList = new Populacja();
                        kandydatList.elementy.Add(kandydat);
                        OcenFitnes(kandydatList, typFunkcji);

                        if (isMinimum == 0)
                        {

                            if (kandydatList.elementy[0].fitness > osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                        else
                        {
                            if (kandydatList.elementy[0].fitness < osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                    }
                    if (typ == TYP_MUTAJCI.best2Bin)
                    {
                        List<Osobnik> osobniksBest = najlepsi.Where(osobnike => osobnik != osobnike).ToList().GetRange(0, 5);
                        Osobnik osobnik1 = osobniksBest[0];
                        Osobnik osobnik2 = osobniksBest[1];
                        Osobnik osobnik3 = osobniksBest[2];
                        Osobnik osobnik4 = osobniksBest[3];
                        Osobnik osobnik5 = osobniksBest[4];

                        int i = 0;
                        int R = rnd.Next(populacja.elementy.Count);
                        int R2 = rnd.Next(populacja.elementy.Count);
                        Osobnik kandydat = new Osobnik();
                        foreach (double param in osobnik.wektor)
                        {
                            double ri = rnd.NextDouble();
                            if (ri < this.wspolczynnikKrzyzowania || i == R || i == R2)
                            {
                                // simple mutation
                                double newElement = osobnik1.wektor[i] + wspolczynnikAmplifikacji * (osobnik2.wektor[i] - osobnik3.wektor[i]) + wspolczynnikAmplifikacji2 * (osobnik4.wektor[i] - osobnik5.wektor[i]);

                                if (newElement > minimum && newElement < maximum)
                                {
                                    kandydat.wektor.Add(newElement);
                                }
                                else
                                {
                                    kandydat.wektor.Add(param);
                                }
                            }
                            else
                            {
                                kandydat.wektor.Add(param);
                            }

                            i++;
                        }
                        Populacja kandydatList = new Populacja();
                        kandydatList.elementy.Add(kandydat);
                        OcenFitnes(kandydatList, typFunkcji);

                        if (isMinimum == 0)
                        {

                            if (kandydatList.elementy[0].fitness > osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                        else
                        {
                            if (kandydatList.elementy[0].fitness < osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                    }
                    if (typ == TYP_MUTAJCI.rand1Bin)
                    {
                        List<Osobnik> doOminiecia = new List<Osobnik>();
                        doOminiecia.Add(osobnik);
                        List<Osobnik> losowi = populacja.Losowi(rnd, 3, doOminiecia);
                        Osobnik osobnik1 = losowi[0];
                        Osobnik osobnik2 = losowi[1];
                        Osobnik osobnik3 = losowi[2];

                        int i = 0;
                        int R = rnd.Next(populacja.elementy.Count);
                        Osobnik kandydat = new Osobnik();
                        foreach (double param in osobnik.wektor)
                        {
                            double ri = rnd.NextDouble();
                            if (ri < this.wspolczynnikKrzyzowania || i == R)
                            {
                                // simple mutation
                                double newElement = osobnik1.wektor[i] + wspolczynnikAmplifikacji * (osobnik2.wektor[i] - osobnik3.wektor[i]);

                                if (newElement > minimum && newElement < maximum)
                                {
                                    kandydat.wektor.Add(newElement);
                                }
                                else
                                {
                                    kandydat.wektor.Add(param);
                                }
                            }
                            else
                            {
                                kandydat.wektor.Add(param);
                            }

                            i++;
                        }
                        Populacja kandydatList = new Populacja();
                        kandydatList.elementy.Add(kandydat);
                        OcenFitnes(kandydatList, typFunkcji);

                        if (isMinimum == 0)
                        {

                            if (kandydatList.elementy[0].fitness > osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                        else
                        {
                            if (kandydatList.elementy[0].fitness < osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                    }
                    if (typ == TYP_MUTAJCI.rand2Bin)
                    {
                        List<Osobnik> doOminiecia = new List<Osobnik>();
                        doOminiecia.Add(osobnik);
                        List<Osobnik> losowi = populacja.Losowi(rnd, 5, doOminiecia);
                        Osobnik osobnik1 = losowi[0];
                        Osobnik osobnik2 = losowi[1];
                        Osobnik osobnik3 = losowi[2];
                        Osobnik osobnik4 = losowi[3];
                        Osobnik osobnik5 = losowi[4];



                        int i = 0;
                        int R = rnd.Next(populacja.elementy.Count);
                        Osobnik kandydat = new Osobnik();
                        foreach (double param in osobnik.wektor)
                        {
                            double ri = rnd.NextDouble();
                            if (ri < this.wspolczynnikKrzyzowania || i == R)
                            {
                                // simple mutation
                                double newElement = osobnik1.wektor[i] + wspolczynnikAmplifikacji * (osobnik2.wektor[i] - osobnik3.wektor[i]) + wspolczynnikAmplifikacji2 * (osobnik4.wektor[i] - osobnik5.wektor[i]);

                                if (newElement > minimum && newElement < maximum)
                                {
                                    kandydat.wektor.Add(newElement);
                                }
                                else
                                {
                                    kandydat.wektor.Add(param);
                                }
                            }
                            else
                            {
                                kandydat.wektor.Add(param);
                            }

                            i++;
                        }
                        Populacja kandydatList = new Populacja();
                        kandydatList.elementy.Add(kandydat);
                        OcenFitnes(kandydatList, typFunkcji);

                        if (isMinimum == 0)
                        {

                            if (kandydatList.elementy[0].fitness > osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                        else
                        {
                            if (kandydatList.elementy[0].fitness < osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }

                    }
                    if (typ == TYP_MUTAJCI.custom)
                    {
                        List<Osobnik> doOminiecia = new List<Osobnik>();
                        doOminiecia.Add(osobnik);
                        List<Osobnik> losowi = populacja.Losowi(rnd, 5, doOminiecia);
                        Osobnik osobnik1 = losowi[0];
                        Osobnik osobnik2 = losowi[1];
                        Osobnik osobnik3 = losowi[2];
                        Osobnik osobnik4 = losowi[3];
                        Osobnik osobnik5 = losowi[4];
                        int i = 0;
                        int R = rnd.Next(populacja.elementy.Count);
                        Osobnik kandydat = new Osobnik();
                        foreach (double param in osobnik.wektor)
                        {
                            double ri = rnd.NextDouble();
                            if (ri < this.wspolczynnikKrzyzowania || i == R)
                            {
                                // simple mutation
                                double newElement = osobnik1.wektor[i] + wspolczynnikAmplifikacji * (3 * osobnik2.wektor[i] - osobnik3.wektor[i] - osobnik4.wektor[i]) + wspolczynnikAmplifikacji2 * (4 * osobnik4.wektor[i] - osobnik5.wektor[i] - osobnik3.wektor[i]);

                                if (newElement > minimum && newElement < maximum)
                                {
                                    kandydat.wektor.Add(newElement);
                                }
                                else
                                {
                                    kandydat.wektor.Add(param);
                                }
                            }
                            else
                            {
                                kandydat.wektor.Add(param);
                            }

                            i++;
                        }
                        Populacja kandydatList = new Populacja();
                        kandydatList.elementy.Add(kandydat);
                        OcenFitnes(kandydatList, typFunkcji);

                        if (isMinimum == 0)
                        {

                            if (kandydatList.elementy[0].fitness > osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                        else
                        {
                            if (kandydatList.elementy[0].fitness < osobnik.fitness)
                                newGen.Add(kandydat);
                            else
                                newGen.Add(osobnik);
                        }
                    }
                }
                populacja.elementy = newGen;

            }

        }
        static double ReadDataFromFile()
        {
            using (StreamReader writer = new StreamReader("result.txt"))
            {
                string res = writer.ReadToEnd();
                return double.Parse(res, NumberStyles.Any, CultureInfo.InvariantCulture);

            }
        }
        static void WriteDataToFile(List<double> data)
        {
            using (StreamWriter writer = new StreamWriter("alg.txt"))
            {
                writer.WriteLine("[[");
                string line = string.Join(";", data).Replace(',', '.').Replace(';', ',');
                writer.WriteLine(line);
                writer.WriteLine("]]");
                writer.Close();
            }

        }
        static string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Python310\python.exe";
            start.Arguments = string.Format("\"{0}\"", cmd);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    process.WaitForExit();
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    Console.WriteLine(result);
                    return result;
                }
            }
        }
        public static List<double> ReadParameters(string fileName)
        {
            var parameters = new List<double>();

            using (var reader = new StreamReader(fileName))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        var value = parts[1];

                        parameters.Add(double.Parse(value));
                    }
                }
            }

            return parameters;
        }





        static void MainV2(string[] args)
        {
            var parameters = ReadParameters("parameters.txt");
            Algorytm alg = new Algorytm(parameters[0], parameters[1], parameters[2], int.Parse(parameters[3].ToString()), int.Parse(parameters[4].ToString()), int.Parse(parameters[5].ToString()), parameters[6], parameters[7], (Algorytm.TYP_MUTAJCI)parameters[8], (Algorytm.TYP_FUNKCJI)parameters[9], int.Parse(parameters[10].ToString()));
            // find all files which starts with name resultsAlg and delete them
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "resultsAlg*.txt");
            foreach (string filePath in filePaths)
                File.Delete(filePath);
            Osobnik najlepszy = new Osobnik();
            najlepszy.wektor.Add(0);
            najlepszy.wektor.Add(0);
            double najlepszyCR = 0;
            double najlepszyF = 0;
            int najlepszyIloscIteracji = 0;
            int najlepszyWielkoscPopulacji = 0;
            int najlepszyTypMutacji = 0;
            najlepszy.fitness = 100;
            Populacja ocenka = new Populacja();
            for (int mutacja = 1; mutacja < 5; mutacja++)
            {
                alg.typMutacji = (Algorytm.TYP_MUTAJCI)mutacja;
                alg.filename = "resultsAlgMutacja{}.txt".Replace("{}", mutacja.ToString());
                ocenka = alg.Skrypt();
                for (double CR = 0.01; CR < 1; CR += 0.88)
                {
                    alg.wspolczynnikKrzyzowania = CR;
                    alg.filename = "resultsAlgCR{}.txt".Replace("{}", CR.ToString());
                    ocenka = alg.Skrypt();
                    for (double F = 0.01; F < 1; F += 0.88)
                    {
                        alg.wspolczynnikAmplifikacji = F;
                        alg.filename = "resultsAlgF{}.txt".Replace("{}", F.ToString());
                        ocenka = alg.Skrypt();
                        // ilosc iteracji
                        for (int iloscIteracji = 100; iloscIteracji <= 1000; iloscIteracji += 500)
                        {
                            alg.iloscIteracji = iloscIteracji;
                            alg.filename = "resultsAlgIloscIteracji{}.txt".Replace("{}", iloscIteracji.ToString());
                            ocenka = alg.Skrypt();
                            // wielkosc populacji
                            for (int wielkoscPopulacji = 50; wielkoscPopulacji <= 500; wielkoscPopulacji += 100)
                            {
                                alg.wielkoscPopulacji = wielkoscPopulacji;
                                alg.filename = "resultsAlgWielkoscPopulacji{}.txt".Replace("{}", wielkoscPopulacji.ToString());
                                ocenka = alg.Skrypt();

                                if (ocenka.Najlepszy(alg.isMinimum).fitness <= najlepszy.fitness)
                                {
                                    najlepszy = ocenka.Najlepszy(alg.isMinimum);
                                    najlepszyCR = CR;
                                    najlepszyF = F;
                                    najlepszyIloscIteracji = iloscIteracji;
                                    najlepszyWielkoscPopulacji = wielkoscPopulacji;
                                    najlepszyTypMutacji = mutacja;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(najlepszy.fitness);
            Console.WriteLine(najlepszy.wektor[0]);
            Console.WriteLine(najlepszy.wektor[1]);
            Console.WriteLine(najlepszyCR);
            Console.WriteLine(najlepszyF);
            Console.WriteLine(najlepszyIloscIteracji);
            Console.WriteLine(najlepszyWielkoscPopulacji);
            Console.WriteLine(najlepszyTypMutacji);
            Console.ReadKey();

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var parameters = ReadParameters("parameters.txt");

            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "results*.txt");
            foreach (string filePath in filePaths)
                File.Delete(filePath);

            double avgScore = 0;
            int iterations = 15;
            for (int i = 0; i < iterations; i++)
            {

                Algorytm alg = new Algorytm(parameters[0], parameters[1], parameters[2], int.Parse(parameters[3].ToString()), int.Parse(parameters[4].ToString()), int.Parse(parameters[5].ToString()), parameters[6], parameters[7], (Algorytm.TYP_MUTAJCI)parameters[8], (Algorytm.TYP_FUNKCJI)parameters[9], int.Parse(parameters[10].ToString()));

                Populacja ocenka = alg.Skrypt();
                avgScore += ocenka.Najlepszy(alg.isMinimum).fitness;

                Console.WriteLine("----------------------------------------- FITNESS \\/");
                Console.WriteLine(ocenka.xNajlepszych(alg.isMinimum)[0].fitness);
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine(ocenka.xNajlepszych(alg.isMinimum)[0].wektor[0]);
                Console.WriteLine(ocenka.xNajlepszych(alg.isMinimum)[0].wektor[1]);
            }
            Console.WriteLine("AVG SCORE");
            Console.WriteLine(avgScore / iterations);
            Console.ReadKey();
        }


    }
}

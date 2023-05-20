using AlgorytmyDE;
using System;
using System.Collections.Generic;
using System.Linq;


public class Populacja
{
	public List<Osobnik> elementy;
	public Populacja()
	{
		elementy = new List<Osobnik>();
	}
	public Populacja(List<Osobnik> Osobniks)
	{
		elementy = Osobniks;
	}
	public Osobnik Najlepszy()
	{
		return elementy.OrderBy(x => x.fitness).FirstOrDefault();
	}

	public List<Osobnik> xNajlepszych()
	{
		return elementy.OrderBy(x => -x.fitness).ToList();
	}

	public List<Osobnik> Losowi(Random rnd, int number, List<Osobnik> doOminiecia)
	{
		List<Osobnik> osobniks = new List<Osobnik>();
		
		for(int i=0; i< number; i++)
		{
			int index = rnd.Next(0, this.elementy.Count);
			if (osobniks.Contains(this.elementy[index]) || doOminiecia.Contains(this.elementy[index]))
			{
				i -= 1;
				continue;
			}
			osobniks.Add(this.elementy[index]);
			
		}
		return osobniks;
	}


}


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
	public Osobnik Najlepszy(int isMinimum = 0)
	{
		if (isMinimum == 0)
			return elementy.OrderBy(x => -x.fitness).FirstOrDefault();
		else
			return elementy.OrderBy(x => x.fitness).FirstOrDefault();
	}

	public List<Osobnik> xNajlepszych(int isMinimum = 0)
	{
		if (isMinimum == 0)
			return elementy.OrderBy(x => -x.fitness).ToList();
		else
			return elementy.OrderBy(x => x.fitness).ToList();
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


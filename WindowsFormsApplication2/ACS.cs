//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System.Collections.Generic;



/// <summary>
/// Created by chayma on 4/27/17.
/// </summary>
public class ACS
{
	private int maxItereration, nbAnt, maxSteps;
	private Ant globalBest;
	private float to, ro, alpha, beta, q0;
	private CNF champs;

	public ACS(int maxItereration, int nbAnt, float to, float ro, float alpha, float beta, float q0, int maxSteps, CNF champs)
	{
		this.maxItereration = maxItereration;
		this.nbAnt = nbAnt;
		this.to = to;
		this.ro = ro;
		this.alpha = alpha;
		this.beta = beta;
		this.q0 = q0;
		this.champs = champs;
		this.maxSteps = maxSteps;
		Feromone.FeromoneInitial = to;
		Feromone.TauxVaporisation = ro;
		Ant.Acs = this;
	}

	public virtual Ant demarer()
	{
		int nbClauses = Ant.Acs.Champs.NombreClauses;
		globalBest = new Ant();
		globalBest.buildSolution();
		globalBest.Feromone.onlineUpdate();
		globalBest.Feromone.checkFeromone();
		globalBest.evaluer();
		for (int i = 0;i < maxItereration;i++)
		{
			if (globalBest.Fitness == nbClauses)
			{
				return globalBest;
			}
			List<Ant> antSet = new List<Ant>();
			for (int j = 0;j < nbAnt;j++)
			{
				Ant ant = new Ant(globalBest);
				ant.buildSolution();
				ant.evaluer();
				ant.improveSearch(maxSteps);
				ant.Feromone.onlineUpdate();
				ant.Feromone.checkFeromone();
				antSet.Add(ant);
			}
			Ant bestIterarion = antSet.Max();
			if (globalBest.CompareTo(bestIterarion) < 0)
			{
				globalBest.Feromone.offlineUpfdate(bestIterarion.Feromone);
				globalBest.Interpretation = bestIterarion.Interpretation;
				globalBest.Feromone.checkFeromone();
				globalBest.Fitness = bestIterarion.Fitness;
			}
		}
		return globalBest;
	}

	public virtual float Beta
	{
		get
		{
			return beta;
		}
		set
		{
			this.beta = value;
		}
	}

	public virtual float Alpha
	{
		get
		{
			return alpha;
		}
		set
		{
			this.alpha = value;
		}
	}

	public virtual float Q0
	{
		get
		{
			return q0;
		}
		set
		{
			this.q0 = value;
		}
	}


	public virtual int MaxItereration
	{
		get
		{
			return maxItereration;
		}
		set
		{
			this.maxItereration = value;
		}
	}


	public virtual Ant GlobalBest
	{
		get
		{
			return globalBest;
		}
		set
		{
			this.globalBest = value;
		}
	}


	public virtual float To
	{
		get
		{
			return to;
		}
		set
		{
			this.to = value;
		}
	}


	public virtual float Ro
	{
		get
		{
			return ro;
		}
		set
		{
			this.ro = value;
		}
	}




	public virtual CNF Champs
	{
		get
		{
			return champs;
		}
		set
		{
			this.champs = value;
		}
	}


	public override string ToString()
	{
		return "ACS{" +
				"maxItereration=" + maxItereration +
				", nbAnt=" + nbAnt +
				", to=" + to +
				", ro=" + ro +
				", alpha=" + alpha +
				", beta=" + beta +
				", q0=" + q0 +
				'}';
	}
}

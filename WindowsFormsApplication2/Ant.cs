//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

/// <summary>
/// Created by chayma on 4/27/17.
/// </summary>
public class Ant : IComparable<Ant>
{

	private HashSet<int> interpretation;
	private static ACS acs;
	private int fitness = -1;
	private Feromone feromone;

	public Ant()
	{
		interpretation = new HashSet<>();
		feromone = new Feromone(this);
	}

	public Ant(Ant ant)
	{
		interpretation = new HashSet<>();
		this.feromone = new Feromone(this,ant.Feromone);
	}

	public Ant(HashSet<int> interpretation)
	{
		this.interpretation = new HashSet<>(interpretation);
	}

	public virtual void improveSearch(int maxSteps)
	{

		if (fitness == Ant.Acs.Champs.NombreClauses)
		{
			return;
		}
		for (int i = 0; i < maxSteps; i++)
		{
			Ant currentAnt = new Ant(interpretation);
			List<Clause> clausesUnsat = new List<Clause>(currentAnt.UnsatisfiedClauses);

			//select une clause non satisfiable aleatoirement
			Clause clause = clausesUnsat[(new Random()).Next(clausesUnsat.Count)];

			//select un litteral aleatoirement
			List<int> litteraux = new List<int>(clause.Litteraux);
			int litteral = litteraux[(new Random()).Next(litteraux.Count)];

			//inverser ce litteral
			currentAnt.interpretation.remove(-litteral);
			currentAnt.interpretation.Add(litteral);
			currentAnt.evaluer();

			if (currentAnt.Fitness > fitness)
			{
				//affecter l'interpretation
				interpretation = new HashSet<>(currentAnt.interpretation);
				//affecter fitness
				fitness = currentAnt.fitness;
				if (fitness == acs.Champs.NombreClauses)
				{
					break;
				}
			}
		}

	}

	public virtual HashSet<Clause> UnsatisfiedClauses
	{
		get
		{
    
			HashSet<Clause> unsatisfiedClause = new HashSet<Clause>();
			HashSet<Clause> clauses = acs.Champs.Clauses;
			foreach (Clause clause in clauses)
			{
				bool contient = false;
				foreach (int variable in clause.Litteraux)
				{
					if (interpretation.Contains(variable))
					{
						contient = true;
						break;
					}
				}
				if (!contient)
				{
					unsatisfiedClause.Add(new Clause(clause));
				}
			}
			return unsatisfiedClause;
		}
	}

	public virtual void evaluer()
	{
		HashSet<Clause> clauses = acs.Champs.Clauses;
		int fitness = 0;
		foreach (Clause clause in clauses)
		{
			foreach (int variable in clause.Litteraux)
			{
				if (interpretation.Contains(variable))
				{
						fitness++;
						break;
				}
			}
		}
		this.fitness = fitness;
	}


	public virtual void buildSolution()
	{
		HashSet<Couple> l = ensembleInitial(); // ensemble des litteraux non selectionnés
		Couple c;

		while (l.Count > 0)
		{
			float q = (float) GlobalRandom.NextDouble;
			//calculer la vecteur de probabilité
			tn(l);
			if (q <= acs.Q0)
			{
				c = l.Max();
				int litteralChoise = c.Litteral;
				interpretation.Add(litteralChoise);
				l.remove(new Couple(litteralChoise));
				l.remove(new Couple(-litteralChoise));
			}
			else
			{
				soustraire(q,l);
				c = l.Min();
				int litteralChoise = c.Litteral;
				interpretation.Add(litteralChoise);
				l.remove(new Couple(litteralChoise));
				l.remove(new Couple(-litteralChoise));
			}
		}
	}

	private void soustraire(float p, HashSet<Couple> l)
	{
		IEnumerator<Couple> itr = l.GetEnumerator();
		while (itr.MoveNext())
		{
			Couple c = itr.Current;
			float prob = Math.Abs(c.Prob - p);
			c.Prob = prob;
		}
	}

	private void tn(HashSet<Couple> l)
	{
		Table frequence = calculeFrequence();
		IEnumerator<Couple> itr = l.GetEnumerator();
		float somme = 0;
		while (itr.MoveNext())
		{
			Couple c = itr.Current;
			c.Prob = (float)(Math.Pow(feromone.getFeromoneOf(c.Litteral),acs.Alpha) * Math.Pow(frequence.getElementOf(c.Litteral),acs.Beta));
			somme = somme + c.Prob;
		}

		itr = l.GetEnumerator();

		while (itr.MoveNext())
		{
			Couple c = itr.Current;
			c.Prob = c.Prob / somme;
		}
	}

	private Table calculeFrequence()
	{

		int nbVariable = acs.Champs.NombreVariable;
		Table frequence = new Table(nbVariable,0f);
		foreach (Clause clause in acs.Champs.Clauses)
		{
			bool containAscendent = false;
			//verifier si la clause est satifiable
			foreach (int i in interpretation)
			{
				if (clause.contains(i))
				{
					containAscendent = true;
					break;
				}
			}
			//si elle n'est pas satisfiable on calcule la fréquence des litteraux
			if (!containAscendent)
			{
				HashSet<int> litteraux = clause.Litteraux;
				foreach (int litteral in litteraux)
				{
					//on incremente
					int f = (int) frequence.getElementOf(litteral);
				   frequence.setElementOf(litteral,f + 1);
				}
			}
		}
		return frequence;
	}

	private HashSet<Couple> ensembleInitial()
	{
		HashSet<Couple> litteralSet = new HashSet<Couple>();
		for (int i = 1;i <= acs.Champs.NombreVariable;i++)
		{
			litteralSet.Add(new Couple(i));
			litteralSet.Add(new Couple(-i));
		}
		return litteralSet;
	}

	public virtual HashSet<int> Interpretation
	{
		get
		{
			return (HashSet<int>) interpretation.clone();
		}
		set
		{
			this.interpretation = value;
		}
	}


	public virtual int Fitness
	{
		get
		{
			return fitness;
		}
		set
		{
			this.fitness = value;
		}
	}


	public virtual Feromone Feromone
	{
		get
		{
			return feromone;
		}
		set
		{
			this.feromone = value;
		}
	}


	public static ACS Acs
	{
		get
		{
			return acs;
		}
		set
		{
			Ant.acs = value;
		}
	}


	public override string ToString()
	{
		return "Ant{" +
				"interpretation=" +
				", fitness=" + fitness +
				", feromone=" + "\n";
	}

	public virtual int CompareTo(Ant o)
	{
		return fitness - o.fitness;
	}

	public override bool Equals(object o)
	{
		if (this == o)
		{
			return true;
		}
		if (o == null || this.GetType() != o.GetType())
		{
			return false;
		}

		Ant ant = (Ant) o;

		return interpretation.Equals(ant.interpretation);
	}

	public override int GetHashCode()
	{
		return interpretation.GetHashCode();
	}
}

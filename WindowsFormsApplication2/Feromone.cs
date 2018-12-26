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
public class Feromone
{
	private List<float> literalPositif;
	private List<float> literlNegatif;
	private static float tauxVaporisation;
	private static float feromoneInitial;
	private Ant ant;

	public Feromone(Ant ant)
	{
		this.ant = ant;
		initializeVectors();
	}

	public Feromone()
	{
		tauxVaporisation = Ant.Acs.Ro;
		feromoneInitial = Ant.Acs.To;
		initializeVectors();
	}

	public Feromone(Ant ant, Feromone feromone)
	{
		this.ant = ant;
		literalPositif = (List<float>) feromone.literalPositif.clone();
		literlNegatif = (List<float>) feromone.literlNegatif.clone();

	}

	private void initializeVectors()
	{
		int taille = Ant.Acs.Champs.NombreVariable;
		literalPositif = new List<>();
		literlNegatif = new List<>();
		for (int i = 0; i < taille; i++)
		{
			literalPositif.Insert(i, feromoneInitial);
			literlNegatif.Insert(i, feromoneInitial);
		}
	}

	public virtual void onlineUpdate()
	{
		 float newValue;
		foreach (int? litteral in ant.Interpretation)
		{
				newValue = (1 - tauxVaporisation) * getFeromoneOf(litteral.Value) + tauxVaporisation * feromoneInitial;
				setFeromoneOf(litteral.Value, newValue);
		}
	}

	public virtual void offlineUpfdate(Feromone feromone)
	{
		int nbVariables = Ant.Acs.Champs.NombreVariable;
		float newValue;
		foreach (int? litteral in ant.Interpretation)
		{
				newValue = (1 - tauxVaporisation) * getFeromoneOf(litteral.Value) + tauxVaporisation * (feromone.getFeromoneOf(litteral.Value) - getFeromoneOf(litteral.Value));
				setFeromoneOf(litteral.Value, newValue);
		}
	}

	public virtual float getFeromoneOf(int i)
	{
		if (i > 0)
		{
			i -= 1;
			return literalPositif[i];
		}
		else
		{
			i = -i - 1;
			return literlNegatif[i];
		}
	}

	public virtual List<float> LiteralPositif
	{
		get
		{
			return literalPositif;
		}
		set
		{
			this.literalPositif = value;
		}
	}


	public virtual List<float> LiterlNegatif
	{
		get
		{
			return literlNegatif;
		}
		set
		{
			this.literlNegatif = value;
		}
	}


	public static float TauxVaporisation
	{
		get
		{
			return tauxVaporisation;
		}
		set
		{
			Feromone.tauxVaporisation = value;
		}
	}


	public static float FeromoneInitial
	{
		get
		{
			return feromoneInitial;
		}
		set
		{
			Feromone.feromoneInitial = value;
		}
	}


	public virtual Ant Ant
	{
		get
		{
			return ant;
		}
		set
		{
			this.ant = value;
		}
	}


	public virtual void setFeromoneOf(int i, float f)
	{
		if (i > 0)
		{
			i -= 1;
			 literalPositif[i] = f;
		}
		else
		{
			i = -i - 1;
			literlNegatif[i] = f;
		}
	}

	public override string ToString()
	{
		return "Feromone{" +
				"literalPositif=" + literalPositif +
				", literlNegatif=" + literlNegatif +
				'}';
	}
	public virtual void checkFeromone()
	{
		for (int i = 0;i < literlNegatif.Count;i++)
		{
			if (literlNegatif[i] < 0 || literalPositif[i] < 0)
			{
				Console.WriteLine("erreur feromone negatif");
			}
		}
	}
}

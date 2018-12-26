//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System.Collections.Generic;

public class Clause
{
	private HashSet<int> litteraux;

	//setters +getters
	public Clause(HashSet<int> litteraux)
	{
		this.litteraux = litteraux;
	}

	public Clause(Clause clause)
	{
		this.litteraux = clause.Litteraux;
	}

	public virtual HashSet<int> Litteraux
	{
		get
		{
			return litteraux;
		}
		set
		{
			this.litteraux = value;
		}
	}


	public override string ToString()
	{
		return "Clause{" +
				"litteraux=" + litteraux +
				'}' + "\n";
	}

	public virtual bool contains(int i)
	{
		return litteraux.Contains(i);
	}
}

//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;

/// <summary>
/// Created by bot on 5/1/17.
/// </summary>
public class Couple : IComparable<Couple>
{
	internal int litteral;
	internal float prob;

	public Couple(int litteral)
	{
		this.litteral = litteral;
		prob = 0;
	}

	public virtual int Litteral
	{
		get
		{
			return litteral;
		}
		set
		{
			this.litteral = value;
		}
	}


	public virtual float Prob
	{
		get
		{
			return prob;
		}
		set
		{
			this.prob = value;
		}
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

		Couple couple = (Couple) o;

		return litteral == couple.litteral;
	}

	public virtual int CompareTo(Couple o)
	{
		return prob > o.prob ? 1 : prob == o.prob ? 0: -1;
	}

	public override string ToString()
	{
		return "Couple{" +
				"litteral=" + litteral +
				", prob=" + prob +
				'}';
	}

	public override int GetHashCode()
	{
		return litteral;
	}
}

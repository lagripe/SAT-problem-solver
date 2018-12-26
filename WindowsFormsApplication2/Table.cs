//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System.Collections.Generic;

/// <summary>
/// Created by bot on 4/30/17.
/// </summary>
public class Table
{
	private IList<float> literalPositif;
	private IList<float> literalNegatif;

	public Table(int nbvariable, float? init)
	{
		initializeVectors(nbvariable, init);
	}
	private void initializeVectors(int taille, float? init)
	{
		literalPositif = new List<>();
		literalNegatif = new List<>();
		for (int i = 0; i < taille; i++)
		{
			literalPositif.Insert(i, init);
			literalNegatif.Insert(i, init);
		}
	}
	public virtual float getElementOf(int i)
	{
		if (i > 0)
		{
			i -= 1;
			return literalPositif[i];
		}
		else
		{
			i = -i - 1;
			return literalNegatif[i];
		}
	}
	public virtual void setElementOf(int i, float element)
	{
		if (i > 0)
		{
			i -= 1;
			literalPositif[i] = element;
		}
		else
		{
			i = -i - 1;
			literalNegatif[i] = element;
		}
	}
	public override string ToString()
	{
		return "Table{" +
				"literalPositif=" + literalPositif +
				", literalNegatif=" + literalNegatif +
				'}';
	}
}

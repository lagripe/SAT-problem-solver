//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

/// <summary>
/// Created by dzcod3r on 3/4/17.
/// </summary>
public class CNF
{
	private HashSet<Clause> clauses;
	private string cheminDuFichier;
	private int nombreVariables, nombreClauses;

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void lireLesClauses() throws java.io.IOException
	private void lireLesClauses()
	{

		string headerReg = "p[ \\t\\n\\x0B\\f\\r]+cnf[ \\t\\n\\x0B\\f\\r]+([0-9]+)[ \\t\\n\\x0B\\f\\r]+([0-9]+)";
		string clauseReg = "(.*)[ \\t\\n\\x0B\\f\\r]+0"; //"[ \\t\\n\\x0B\\f\\r]*(.+)[ \\t\\n\\x0B\\f\\r]+0[ \\t\\n\\x0B\\f\\r]+";
		bool? headerFound = false;

		System.IO.StreamReader bufferedReader;

			bufferedReader = new System.IO.StreamReader(cheminDuFichier);
			string buffer;

			while (!headerFound && !string.ReferenceEquals((buffer = bufferedReader.ReadLine()), null))
			{
				//get header:
				Pattern p = Pattern.compile(headerReg);
				Matcher m = p.matcher(buffer);
				if (m.find() && m.groupCount() > 0)
				{
					headerFound = true;
					nombreVariables = int.Parse(m.group(1));
					nombreClauses = int.Parse(m.group(2));

				}

			}

			if (headerFound.Value)
			{
				clauses = new HashSet<>();
				int cptClauses = 0;
				while (cptClauses != nombreClauses && !string.ReferenceEquals((buffer = bufferedReader.ReadLine()), null))
				{
					//get clauses:
					Pattern p = Pattern.compile(clauseReg);
					Matcher m = p.matcher(buffer);
					if (m.find() && m.groupCount() > 0)
					{
						cptClauses++;
						string clause = m.group(1);

						//get literals:
						clause = clause.Trim();
						string[] literals = clause.Split("[ \\t\\n\\x0B\\f\\r]+", true);
						HashSet<int> uneClause = new HashSet<int>();
						for (int i = 0; i < literals.Length; i++)
						{
							uneClause.Add(Convert.ToInt32(literals[i]));
						}
						clauses.Add(new Clause(uneClause));
					}
				}
			}
			bufferedReader.Close();
	}


	//setters + getters
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public CNF(String cheminDuFichier) throws java.io.IOException
	public CNF(string cheminDuFichier)
	{
		this.cheminDuFichier = cheminDuFichier;
		clauses = new HashSet<>();
		lireLesClauses();

	}

	public virtual HashSet<Clause> Clauses
	{
		get
		{
			return clauses;
		}
		set
		{
			this.clauses = value;
		}
	}


	public virtual string CheminDuFichier
	{
		get
		{
			return cheminDuFichier;
		}
		set
		{
			this.cheminDuFichier = value;
		}
	}


	public virtual int NombreVariable
	{
		get
		{
			return nombreVariables;
		}
		set
		{
			this.nombreVariables = value;
		}
	}


	public virtual int NombreClauses
	{
		get
		{
			return nombreClauses;
		}
		set
		{
			this.nombreClauses = value;
		}
	}


	public override string ToString()
	{
		return "CNF{" +
				"clauses=" + clauses +
				", cheminDuFichier='" + cheminDuFichier + '\'' +
				", nombreVariable=" + nombreVariables +
				", nombreClauses=" + nombreClauses +
				'}';
	}
}

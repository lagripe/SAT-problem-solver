//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;

/// <summary>
/// Created by chayma on 4/27/17.
/// </summary>
public class Main
{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
	public static void Main(string[] args)
	{

		int maxIteration = 2000, nbAnt = 1, maxSteps = 100;
		float to = 0.1f, ro = 0.2057f, alpha = 0.6f, beta = 0.3f, q0 = 0.6f;
		Console.WriteLine("maxIter=" + maxIteration + "  nbAnt=" + nbAnt + "  maxSteps=" + maxSteps + "  to=" + to + "  ro=" + ro + "  alpha=" + alpha + "  beta=" + beta + "  q0=" + q0);

			CNF cnf = new CNF("C:\\uf75-01.cnf");
			ACS acs = new ACS(maxIteration,nbAnt,to,ro,alpha,beta,q0,maxSteps,cnf);

			Ant ant = acs.demarer();

			/*FileWriter fileWriter = new FileWriter(maxIteration + "_" + nbAnt+ "_" + maxSteps + "_" +
			        to+ "_" + ro+ "_" + alpha+ "_" + beta+ "_" + q0, true);
			fileWriter.write("uf75-0" + i + ".cnf\t" + ant.getFitness() + "\t" + temps + "\n");
			fileWriter.close();
			System.out.println("uf75-0" + i + ".cnf\t" + ant.getFitness() + "\t" + temps + " s");
*/
		Console.WriteLine("uf75-01.cnf\t" + ant.Fitness);
	}

}

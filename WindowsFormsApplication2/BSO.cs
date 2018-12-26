using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    public class BSO
    {
        public int max_iter;
        public int flip;
        public int max_local_iter;
        public int chances;
        public HashSet<Bee> taboo;
        public HashSet<Bee> dance_table;

        public BSO(int max_iter, int flip, int max_local_iter, int chances)
        {
            this.chances = chances;
            this.flip = flip;
            this.max_iter = max_iter;
            this.max_local_iter = max_local_iter;
            this.taboo = new HashSet<Bee>();
            this.dance_table = new HashSet<Bee>();
        }

        public virtual Bee Bso_search(int nb_clauses, int nb_var)
        {
            int cpt = 0;
            int nb_chances = chances;
            Bee Sref = new Bee(nb_clauses, nb_var);
            if (Sref.max_sat == nb_clauses)
            {
                Console.WriteLine("solution found");
                return Sref;
            }


            while (cpt < max_iter)
            {
                taboo.Add(Sref);
                cpt++;
                HashSet<Bee> bees = Sref.recruit(flip, nb_clauses, nb_var);
                foreach (Bee b in bees)
                {
                    dance_table.Add(b.maxRechercheLocale(max_local_iter, 1, nb_clauses, nb_var));
                }
                Bee bestbee = dance_table.Max();
                if (bestbee.max_sat == nb_clauses)
                {
                    return bestbee;
                }
                else
                {
                    if (taboo.Contains(bestbee) && dance_table.Count > 0)
                    {
                        dance_table.Remove(bestbee);
                        bestbee = dance_table.Max();
                    }

                    if (bestbee.CompareTo(Sref) > 0)
                    {
                        Sref = bestbee;
                        if (nb_chances < chances)
                        {
                            nb_chances = chances;
                        }

                    }
                    else
                    {
                        nb_chances--;
                        if (nb_chances > 0)
                        {
                            Sref = bestbee;
                        }
                        else
                        {

                            Sref = bestInDiversity(nb_clauses);
                            nb_chances = chances;

                        }



                    }
                    if(API.BestClauseSatisfied < Sref.max_sat)
                    API.BestClauseSatisfied=Sref.max_sat;
                    dance_table.Remove(bestbee);


                }







            }

            return taboo.Max();
        }

        public  Bee bestInDiversity(int nb_clauses)
        {
            Bee bestBee = dance_table.Aggregate((l, r) => l.max_sat > r.max_sat ? l : r);
            //API.BestClauseSatisfied = bestBee.max_sat;
            return bestBee;
        }

     /*   private class ComparatorAnonymousInnerClass : Comparator<Bee>
        {
            private readonly BSO outerInstance;

            private int nb_clauses;

            public ComparatorAnonymousInnerClass(BSO outerInstance, int nb_clauses)
            {
                this.outerInstance = outerInstance;
                this.nb_clauses = nb_clauses;
            }

            public virtual int compare(Bee bee, Bee t1)
            {
                return (bee.diversity(outerInstance.taboo, nb_clauses) - t1.diversity(outerInstance.taboo, nb_clauses));
            }
        }
        */



    }
}

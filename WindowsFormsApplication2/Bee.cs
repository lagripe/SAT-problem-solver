using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    public class Bee : IComparable<Bee>
    {
        public int max_sat;
        public int deb;
        public List<int> localarea;


        public static void flip(List<int> localarea, int i)
        {
            localarea[i] = -localarea[i];

        }



        public Bee(int nb_clauses, int nb_var)
        {
            localarea = new List<int>();
            int x;
            Random rn = new Random();
            for (int i = 0; i < nb_var; i++)
            {
                x = rn.Next(10);
                // System.out.println(x);
                if (x < 5)
                {
                    localarea.Add(i + 1);
                }
                else
                {
                    localarea.Add(-i - 1);
                }
            }
            Console.WriteLine(localarea.ToString());
            Console.WriteLine(localarea[0]);
            dance(nb_clauses);


        }
        public Bee(Bee b, int k, int flip1, int nb_clauses, int nb_var)
        {
            this.localarea = new List<int>(b.localarea);
            for (int i = k; i < nb_var; i += flip1)
            {
                flip(localarea, i);
            }
            dance(nb_clauses);
        }




        public Bee(Bee bee, int nbflip, int nb_clauses, int nb_var)
        {
            localarea = new List<int>(bee.localarea);

            flip(localarea, nbflip);
            dance(nb_clauses);


        }


        public virtual HashSet<Bee> recruit(int flip, int nb_clauses, int nb_var)
        {
            HashSet<Bee> bees = new HashSet<Bee>();
            for (int i = 0; i < flip; i++)
            {
                bees.Add(new Bee(this, i, flip, nb_clauses, nb_var));
            }
            return bees;
        }
        

        public virtual void dance(int nb_clauses)
        {
            bool b = false;
            int s = 0;
            for (int i = 0; i < nb_clauses; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < localarea.Count; k++)
                    {
                        if (API.data_clause[i,j] == localarea[k])
                        {

                            b = true;
                            break;
                        }


                    }
                    if (b == true)
                    {
                        s++;
                        b = false;
                        break;
                    }
                }

            }
            this.max_sat = s;
            // System.out.println("fitness:"+s);

        }

        public virtual int CompareTo(Bee o)
        {
            return this.max_sat - o.max_sat;
        }


        public virtual Bee maxRechercheLocale(int nbRecherchesLocales, int distance, int nb_clauses, int nb_var)
        {
            Bee beeMax = this;
            for (int i = 0; i < nb_var; i++)
            {
                Bee bee = new Bee(beeMax, i, nb_clauses, nb_var);
                if (beeMax.CompareTo(bee) <= 0)
                {
                    beeMax = bee;
                }
            }
            return beeMax;
        }
        public static int distance(Bee b1, Bee b2)
        {
            int s = 0;
            for (int i = 0; i < b1.localarea.Count; i++)
            {
                if (b1.localarea[i] != b2.localarea[i])
                {
                    s++;
                }
            }
            return s;
        }

        public virtual int diversity(HashSet<Bee> taboo, int nb_clauses)
        {
            int x;
            int max = 0;
            Bee bee = this;
            foreach (Bee b in taboo)
            {
                x = distance(bee, b);
                if (max < x)
                {
                    max = x;

                }
            }
            return max;

        }

    }
}

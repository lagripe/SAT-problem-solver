using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class API
    {

        public static int[,] data_clause;
        public static int BestClauseSatisfied = 0;
        public static String CutWhiteSpace(String line)
        {

            String temp = "";
            int cpt = 0;
            foreach (char obj in line)
            {

                if (obj != ' ' && obj != '\t')
                {
                    temp += obj;
                    cpt = 0;
                }
                else
                {
                    if (cpt == 0)
                        temp += obj;
                    cpt++;

                }

            }

            //Console.WriteLine(temp);
            return temp;
        }

        public static List<String> getClause(String Dataset)
        {
            String[] temp = Dataset.Split('\n');
            List<String> data = new List<String>();
            try
            {
                foreach (String line in temp)
                {

                    if (line.Length < 1 || String.IsNullOrEmpty(line))
                        continue;

                    if ((line.Trim()[0] >= '1' && line.Trim()[0] <= '9') || line.Trim()[0] == '-')
                        data.Add(line.Trim());
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine();
            }
            return data;
        }


        public static int[,] ClauseTab(int nbclause, String dataset)
        {

            // Vector of clauses
            int[,] data_clause = new int[nbclause, 4];

            String[] tempData = dataset.Split('\n');
            int cpt = 0;
            int tempNbCl = 1;
            foreach (String line in tempData)
            {
                if (tempNbCl == nbclause + 1)
                    break;


                String temp = line.Trim();
                temp = CutWhiteSpace(temp);
                String[] Lit = temp.Split(' ');
                if (Lit.Count() < 1 || String.IsNullOrEmpty(temp))
                    continue;
                data_clause[cpt, 0] = int.Parse(Lit[0]);
                data_clause[cpt, 1] = int.Parse(Lit[1]);
                data_clause[cpt, 2] = int.Parse(Lit[2]);
                data_clause[cpt, 3] = 0;

                Console.WriteLine(int.Parse(Lit[0]) + "#" + int.Parse(Lit[1]) + "#" + int.Parse(Lit[2]) + "#" + data_clause[cpt, 3]);
                cpt++;
                tempNbCl++;
            }




            return data_clause;
        }

        private static Boolean DntExist(int value, List<int> Closed)
        {
            foreach (int Lit in Closed)
            {
                if (Lit == value)
                    return false;
            }

            return true;
        }

        public static int NombreTester = 0;

        
        // This Method Search By Width 
        public static String ByWidth(int nbvar, int nbclause)
        {
            BestClauseSatisfied = 0;
            List<NoeudTree> Open = new List<NoeudTree>();
            List<NoeudTree> Parent = new List<NoeudTree>();
            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);
            //result
            String result = null;

            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            Racine.FG = new NoeudTree();
            Open.Add(Racine.FG);
            Racine.FD = new NoeudTree();
            Open.Add(Racine.FD);
            Parent.Add(Racine);
            //Return path
            NoeudTree Pre = new NoeudTree();

            //int cpt = Table[0].FD;


            int ShuffleNumber = 1;
            Random rand = new Random();
            int Litteral = 0;
            while (Open.Count != 0 && Closed.Count != nbvar + 1)
            {


                if (TimeToShuffle(ShuffleNumber))
                {
                    do
                    {
                        if (PickedLit.Count - 1 < 0)
                            break;

                        int random = rand.Next(0, PickedLit.Count - 1);
                        int res = PickedLit.ElementAt(random);
                        PickedLit.RemoveAt(random);
                        Litteral = res;

                    } while (!DntExist(Litteral, Closed));
                    Closed.Add(Litteral);
                    NombreTester++;
                    Console.WriteLine(NombreTester);
                }


                Pre = Parent.ElementAt(0);
                Parent.RemoveAt(0);
                //Left Son

                NoeudTree tempNoeudG = Open.ElementAt(0);
                Open.RemoveAt(0);
                tempNoeudG.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + Litteral + " ";


                tempNoeudG.FG = new NoeudTree();
                Open.Add(tempNoeudG.FG);
                tempNoeudG.FD = new NoeudTree();
                Open.Add(tempNoeudG.FD);
                //Add to Parent 
                Parent.Add(tempNoeudG);
                //Solution Found
                if (Evaluate(tempNoeudG.Litteral) == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                NoeudTree tempNoeudD = Open.ElementAt(0);
                Open.RemoveAt(0);
                tempNoeudD.Retourn = Pre;
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + " " + "-" + Litteral + " ";

                tempNoeudD.FG = new NoeudTree();
                Open.Add(tempNoeudD.FG);
                tempNoeudD.FD = new NoeudTree();
                Open.Add(tempNoeudD.FD);
                //Add Parent
                Parent.Add(tempNoeudD);

                //Solution Found

                if (Evaluate(tempNoeudD.Litteral) == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudD.Litteral + " ");

                    result = tempNoeudG.Litteral;

                    break;
                }



                ShuffleNumber++;
            }

            return result;
        }
       
       
        // This Method Search By Depth 
        public static String ByDepth(int nbvar, int nbclause)
        {
            int Depth = 0;
            List<NoeudTreeDepth> Open = new List<NoeudTreeDepth>();
            Stack<NoeudTreeDepth> Parent = new Stack<NoeudTreeDepth>();
            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);
            //result
            String result = null;
            //Init Racine
            NoeudTreeDepth Racine = new NoeudTreeDepth();
            Racine.Litteral = "";
            Racine.Retourn = null;
            Racine.Depth = Depth;
            Racine.FG = new NoeudTreeDepth();
            Open.Add(Racine.FG);

            Parent.Push(Racine);
            //Return path
            NoeudTreeDepth Pre = Racine;


            //To Count Choosen Lit
            Boolean Done = false;


            Random rand = new Random();
            int Litteral = 0;
            while (Open.Count != 0 || Parent.Count != 0)
            {
                
                Console.WriteLine("Start the While ...");
                String ReturnedDepthLit = "";
                NoeudTreeDepth tempNoeudG;
                //NoeudTreeDepth tempNoeudD;
                if (Depth < nbvar)
                {

                    if (Closed.Count != nbvar)
                    {
                        //Choose a Litteral Once for all
                        do
                        {
                            if (PickedLit.Count - 1 < 0)
                                break;

                            int random = rand.Next(0, PickedLit.Count - 1);
                            int res = PickedLit.ElementAt(random);
                            PickedLit.RemoveAt(random);
                            Litteral = res;

                        } while (!DntExist(Litteral, Closed));
                        Closed.Add(Litteral);

                    }
                    //

                    tempNoeudG = Open.ElementAt(0);
                    Open.RemoveAt(0);
                    tempNoeudG.Retourn = Pre;
                    tempNoeudG.Depth = Depth + 1;
                    //Assign litteral
                    if (Done == false)
                    {
                        tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + Litteral + " ";
                        if (Closed.Count() == nbvar)
                            Done = true;
                    }
                    else
                    {
                        NoeudTreeDepth getLitteral = Racine;
                        int depthNoeud = tempNoeudG.Depth;
                        while (getLitteral != null)
                        {

                            if (getLitteral.Depth == depthNoeud)
                            {
                                ReturnedDepthLit = getLitteral.Litteral;
                                break;
                            }
                            getLitteral = getLitteral.FG;


                        }

                        if (Object.ReferenceEquals(tempNoeudG.Retourn.FG, tempNoeudG))
                        {

                            tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + getLastLit(ReturnedDepthLit) + " ";

                        }
                        else
                        {


                            tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " -" + getLastLit(ReturnedDepthLit) + " ";

                        }

                    }

                    if (Depth + 1 != nbvar)
                    {

                        tempNoeudG.FG = new NoeudTreeDepth();
                        Open.Add(tempNoeudG.FG);
                        Pre = tempNoeudG;
                        //Add to Parent 
                        Parent.Push(tempNoeudG);
                    }
                    //Solution Found
                    if (Evaluate(tempNoeudG.Litteral) == nbclause)
                    {
                        Console.Write("Solution Found : ");
                        Console.Write(tempNoeudG.Litteral + " ");

                        result = tempNoeudG.Litteral;
                        break;
                    }

                    Depth++;

                }
                else
                {

                    Pre = Parent.Pop();
                    Pre.FD = new NoeudTreeDepth();
                    Pre.FD.Retourn = Pre;
                    Pre.FD.Depth = Pre.Depth + 1;
                    NoeudTreeDepth getLitteral = Racine;
                    int depthNoeud = Pre.FD.Depth;
                    getLitteral = getLitteral.FG;
                    while (getLitteral != null)
                    {
                        //Console.WriteLine(getLitteral.Depth);
                        if (getLitteral.Depth == depthNoeud)
                        {
                            ReturnedDepthLit = getLitteral.Litteral;
                            break;
                        }
                        getLitteral = getLitteral.FG;


                    }


                    Pre.FD.Litteral = Pre.FD.Retourn.Litteral + " -" + getLastLit(ReturnedDepthLit) + " ";


                    //Solution Found
                    if (Evaluate(Pre.FD.Litteral) == nbclause)
                    {
                        Console.Write("Solution Found : ");
                        Console.Write(Pre.FD.Litteral + " ");

                        result = Pre.FD.Litteral;
                        break;
                    }


                    Pre = Parent.Pop();
                    Pre.FD = new NoeudTreeDepth();
                    Open.Add(Pre.FD);
                    Depth = Pre.Depth;

                }


            }



            return result;
        }
        public static List<int> getFrequency(int[,] mydata, int nbvar)
        {
            Dictionary<int, int> myList = new Dictionary<int, int>();

            List<int> PickedLit = new List<int>();
            int MaxPos = 0, MaxNeg = 0;
            for (int i = 1; i <= nbvar; i++)
            {
                MaxPos = 0; MaxNeg = 0;
                for (int j = 0; j < (data_clause.Length / 4); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (data_clause[j, k] == i)
                        {
                            MaxPos++;
                            break;
                        }
                        if (data_clause[j, k] == (-1 * i))
                        {
                            MaxNeg++;
                            break;
                        }
                    }
                }

                if (MaxPos > MaxNeg)
                    myList[i] = MaxPos;
                else
                    myList[(-1 * i)] = MaxNeg;


            }
            while (myList.Count() != 0)
            {
                //Console.WriteLine("myList Elements => " + myList.Count());
                //System.Collections.Generic.KeyValuePair<int, int> Lit = myList.Max();
                int max = myList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

                // Console.WriteLine(max + " => " + myList[max]);
                PickedLit.Add(max);
                myList.Remove(max);

            }


            return PickedLit;
        }

       
        //This Method Uses the Depth and a Sorted List
        public static String ByFrequency(int nbvar, int nbclause)
        {
            int Depth = 0;
            List<NoeudTreeDepth> Open = new List<NoeudTreeDepth>();
            Stack<NoeudTreeDepth> Parent = new Stack<NoeudTreeDepth>();
            List<int> Closed = new List<int>();
            //Function to get PickedLit By frequency
            List<int> PickedLit = new List<int>();
            /*
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);
            */

            PickedLit = getFrequency(data_clause, nbvar);

            //result
            String result = null;
            //Init Racine
            NoeudTreeDepth Racine = new NoeudTreeDepth();
            Racine.Litteral = "";
            Racine.Retourn = null;
            Racine.Depth = Depth;
            Racine.FG = new NoeudTreeDepth();
            Open.Add(Racine.FG);

            Parent.Push(Racine);
            //Return path
            NoeudTreeDepth Pre = Racine;


            //To Count Choosen Lit
            Boolean Done = false;


            Random rand = new Random();
            int Litteral = 0;
            while (Open.Count != 0 || Parent.Count != 0)
            {
                Console.WriteLine("Start the While ...");
                String ReturnedDepthLit = "";
                NoeudTreeDepth tempNoeudG;
                //NoeudTreeDepth tempNoeudD;
                if (Depth < nbvar)
                {

                    if (Closed.Count != nbvar)
                    {
                        //Choose a Litteral Once for all

                        if (PickedLit.Count - 1 < 0)
                            break;


                        int res = PickedLit.ElementAt(0);
                        PickedLit.RemoveAt(0);
                        Litteral = res;


                        Closed.Add(Litteral);

                    }
                    //

                    tempNoeudG = Open.ElementAt(0);
                    Open.RemoveAt(0);
                    tempNoeudG.Retourn = Pre;
                    tempNoeudG.Depth = Depth + 1;
                    //Assign litteral
                    if (Done == false)
                    {
                        tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + Litteral + " ";
                        if (Closed.Count() == nbvar)
                            Done = true;
                    }
                    else
                    {
                        NoeudTreeDepth getLitteral = Racine;
                        int depthNoeud = tempNoeudG.Depth;
                        while (getLitteral != null)
                        {
                            if (getLitteral.Depth == depthNoeud)
                            {
                                ReturnedDepthLit = getLitteral.Litteral;
                                break;
                            }
                            getLitteral = getLitteral.FG;
                        }

                        if (Object.ReferenceEquals(tempNoeudG.Retourn.FG, tempNoeudG))
                        {
                            String Lit = getLastLit(ReturnedDepthLit);

                            tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + Lit + " ";

                        }
                        else
                        {
                            String Lit = getLastLit(ReturnedDepthLit);
                            if (Lit[0] == '-')
                                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " " + Lit.Substring(1) + " ";
                            else
                                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + " -" + Lit + " ";

                        }

                    }

                    if (Depth + 1 != nbvar)
                    {

                        tempNoeudG.FG = new NoeudTreeDepth();
                        Open.Add(tempNoeudG.FG);
                        Pre = tempNoeudG;
                        //Add to Parent 
                        Parent.Push(tempNoeudG);
                    }
                    //Solution Found
                    if (Evaluate(tempNoeudG.Litteral) == nbclause)
                    {
                        Console.Write("Solution Found : ");
                        Console.Write(tempNoeudG.Litteral + " ");

                        result = tempNoeudG.Litteral;
                        break;
                    }

                    Depth++;

                }
                else
                {

                    Pre = Parent.Pop();
                    Pre.FD = new NoeudTreeDepth();
                    Pre.FD.Retourn = Pre;
                    Pre.FD.Depth = Pre.Depth + 1;
                    NoeudTreeDepth getLitteral = Racine;
                    int depthNoeud = Pre.FD.Depth;
                    getLitteral = getLitteral.FG;
                    while (getLitteral != null)
                    {
                        if (getLitteral.Depth == depthNoeud)
                        {
                            ReturnedDepthLit = getLitteral.Litteral;
                            break;
                        }
                        getLitteral = getLitteral.FG;


                    }


                    Pre.FD.Litteral = Pre.FD.Retourn.Litteral + " -" + getLastLit(ReturnedDepthLit) + " ";


                    //Solution Found
                    if (Evaluate(Pre.FD.Litteral) == nbclause)
                    {
                        Console.Write("Solution Found : ");
                        Console.Write(Pre.FG.Litteral + " ");

                        result = Pre.FG.Litteral;
                        break;
                    }


                    Pre = Parent.Pop();
                    Pre.FD = new NoeudTreeDepth();
                    Open.Add(Pre.FD);
                    Depth = Pre.Depth;

                }


            }



            return result;
        }

        public static List<int> Re_Init(int nbvar, String OldPath)
        {
            Boolean Exist;
            List<int> myList = new List<int>();
            String[] TempPath = OldPath.Trim().Split(' ');
            for (int i = 1; i <= nbvar; i++)
            {
                Exist = false;
                foreach (String Lit in TempPath)
                {
                    if (Lit.Length < 1)
                        continue;


                    int Number = int.Parse(Lit.Trim());

                    if (Number == i || (-1 * Number) == i)
                    {
                        Exist = true;
                        break;
                    }

                }

                if (!Exist) { myList.Add(i); }

            }





            myList = SortedFrequency(myList);
            return myList;
        }
        public static List<int> Re_Init_HeuristicWeight(int nbvar, String OldPath)
        {
            Boolean Exist;
            List<int> myList = new List<int>();
            String[] TempPath = OldPath.Trim().Split(' ');
            for (int i = 1; i <= nbvar; i++)
            {
                Exist = false;
                foreach (String Lit in TempPath)
                {

                    if (Lit.Length < 1)
                        continue;


                    int Number = int.Parse(Lit.Trim());

                    if (Number == i || (-1 * Number) == i)
                    {

                        Exist = true;
                        break;
                    }

                }

                if (!Exist) { myList.Add(i); }

            }





            myList = SortedFrequency(myList);
            return myList;
        }
        public static List<int> SortedFrequency(List<int> OldList)
        {
            List<int> SortedList = new List<int>();
            Dictionary<int, int> myList = new Dictionary<int, int>();
            foreach (int Lit in OldList)
            {
                int Max = 0;

                for (int j = 0; j < (data_clause.Length / 4); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (data_clause[j, k] == Lit)
                        {
                            Max++;
                            break;
                        }
                        if (data_clause[j, k] == (-1 * Lit))
                        {
                            Max++;
                            break;
                        }
                    }


                    myList[Lit] = Max;


                }
            }
            while (myList.Count() != 0)
            {

                int max = myList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                SortedList.Add(max);
                myList.Remove(max);

            }


            return SortedList;
        }

        public static List<int> SortedFrequencyOldPath(List<int> OldList,String Path)
        {
            if(Path != "") { 
            Boolean Exist = false;
            List<int> SortedList = new List<int>();
            Dictionary<int, int> myList = new Dictionary<int, int>();
            String[] TempPath = Path.Trim().Split(' ');
            foreach (int Lit in OldList)
            {
                int Max = 0;

                for (int j = 0; j < (data_clause.Length / 4); j++)
                {
                    Exist = false;
                    for (int k = 0; k < 3; k++)
                    {
                        
                        if (data_clause[j, k] == Lit || data_clause[j, k] == (-1 * Lit))
                        {
                            foreach(String Old in TempPath)
                            {
                                if(data_clause[j, 0] == int.Parse(Old) || data_clause[j, 1] == int.Parse(Old) || data_clause[j, 2] == int.Parse(Old))
                                {
                                    Exist = true;
                                }
                            }
                            if (!Exist) { 
                            Max++;
                            break;
                            }

                        }
                      
                    }


                    myList[Lit] = Max;


                }
            }
            while (myList.Count() != 0)
            {
                //Console.WriteLine("myList Elements => " + myList.Count());
                //System.Collections.Generic.KeyValuePair<int, int> Lit = myList.Max();
                int max = myList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

                // Console.WriteLine(max + " => " + myList[max]);
                SortedList.Add(max);
                myList.Remove(max);

            }


            return SortedList;
            }
            else
            {
                return OldList;
            }
        }
        public static List<int> OldPathSorting(int nbvar, String OldPath)
        {
            Boolean Exist;
            List<int> myList = new List<int>();
            String[] TempPath = OldPath.Trim().Split(' ');
            for (int i = 1; i <= nbvar; i++)
            {
                Exist = false;
                foreach (String Lit in TempPath)
                {
                    //Console.WriteLine("#" + Lit + "#");
                    if (Lit.Length < 1)
                        continue;


                    int Number = int.Parse(Lit.Trim());

                    if (Number == i || (-1 * Number) == i)
                    {

                        Exist = true;
                        break;
                    }

                }

                if (!Exist) { myList.Add(i); }

            }





            myList = SortedFrequencyOldPath(myList, OldPath);
            return myList;
        }

        //This Method Search By Weight and a Dencremented Appearence Variables List (returns to the highest Evaluation from the Unxplored Dictionary)
        public static String ByHeuristicWeightFrequencyHigh(int nbvar, int nbclause)
        {

            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            Dictionary<NoeudTree, int> UnExplored = new Dictionary<NoeudTree, int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);

            PickedLit = SortedFrequency(PickedLit);

            //result
            String result = null;
            //Init Racine
            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            //Return path
            NoeudTree Pre = Racine;

            Random rand = new Random();
            int Litteral = 0;
            Boolean FirstEntrance = true;
            while (UnExplored.Count != 0 || FirstEntrance)
            {
               

                FirstEntrance = false;
                if (PickedLit.Count == 0)
                {
                   
                    Pre = UnExplored.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    UnExplored.Remove(Pre);
                    //Re-initial PickedLit
                    PickedLit = Re_Init(nbvar, Pre.Litteral);
                    
                }

                //Choose a Litteral Once for all

                if (PickedLit.Count - 1 < 0)
                    break;

                //int random = rand.Next(0, PickedLit.Count - 1);
                int res = PickedLit.ElementAt(0);
                //PickedLit.RemoveAt(random);
                PickedLit.RemoveAt(0);
                Litteral = res;




                //

                NoeudTree tempNoeudG = new NoeudTree();
                NoeudTree tempNoeudD = new NoeudTree();
                tempNoeudG.Retourn = Pre;
                tempNoeudD.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + Litteral + " ";
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + "-" + Litteral + " ";
                int EVG = Evaluate(tempNoeudG.Litteral);
                if (EVG == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }
                int EVD = Evaluate(tempNoeudD.Litteral);
                if (EVD == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                if (PickedLit.Count != 0)
                    if (EVG > EVD)
                    { UnExplored[tempNoeudD] = EVD; Pre = tempNoeudG; }
                    else
                    { UnExplored[tempNoeudG] = EVG; Pre = tempNoeudD; }
            }



            return result;
        }



        //This Method Search By Weight and a Dencremented Appearence Variables List (returns to the lowest Evaluation from the Unxplored Dictionary)
        public static String ByHeuristicWeightFrequencyLow(int nbvar, int nbclause)
        {

            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            Dictionary<NoeudTree, int> UnExplored = new Dictionary<NoeudTree, int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);

            PickedLit = SortedFrequency(PickedLit);

            //result
            String result = null;
            //Init Racine
            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            //Return path
            NoeudTree Pre = Racine;

            Random rand = new Random();
            int Litteral = 0;
            Boolean FirstEntrance = true;
            while (UnExplored.Count != 0 || FirstEntrance)
            {


                FirstEntrance = false;
                if (PickedLit.Count == 0)
                {

                    Pre = UnExplored.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    UnExplored.Remove(Pre);
                    //Re-initial PickedLit
                    PickedLit = Re_Init(nbvar, Pre.Litteral);

                }

                //Choose a Litteral Once for all
                if (PickedLit.Count - 1 < 0)
                    break;
                //int random = rand.Next(0, PickedLit.Count - 1);
                int res = PickedLit.ElementAt(0);
                //PickedLit.RemoveAt(random);
                PickedLit.RemoveAt(0);
                Litteral = res;

                NoeudTree tempNoeudG = new NoeudTree();
                NoeudTree tempNoeudD = new NoeudTree();
                tempNoeudG.Retourn = Pre;
                tempNoeudD.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + Litteral + " ";
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + "-" + Litteral + " ";
                int EVG = Evaluate(tempNoeudG.Litteral);
                if (EVG == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }
                int EVD = Evaluate(tempNoeudD.Litteral);
                if (EVD == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                if (PickedLit.Count != 0)
                    if (EVG > EVD)
                    { UnExplored[tempNoeudD] = EVD; Pre = tempNoeudG; }
                    else
                    { UnExplored[tempNoeudG] = EVG; Pre = tempNoeudD; }

            }



            return result;
        }

        //This Method Search By Weight and a Dencremented Appearence Variables List depending on the OldPath Explored at the time (most no inference with litterals in the OldPath)(returns to the lowest Evaluation from the Unxplored Dictionary)

        public static String ByHeuristicWeightFrequencyUsingOldPath(int nbvar, int nbclause)
        {

            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            Dictionary<NoeudTree, int> UnExplored = new Dictionary<NoeudTree, int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);

            //result
            String result = null;
            //Init Racine
            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            //Return path
            NoeudTree Pre = Racine;

            Random rand = new Random();
            int Litteral = 0;
            Boolean FirstEntrance = true;
            while (UnExplored.Count != 0 || FirstEntrance)
            {
                PickedLit = OldPathSorting(nbvar, Pre.Litteral);

                FirstEntrance = false;
                if (PickedLit.Count == 0)
                {

                    Pre = UnExplored.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    UnExplored.Remove(Pre);
                    //Re-initial PickedLit
                    PickedLit = OldPathSorting(nbvar, Pre.Litteral);

                }

                //Choose a Litteral Once for all

                if (PickedLit.Count - 1 < 0)
                    break;

                //int random = rand.Next(0, PickedLit.Count - 1);
                int res = PickedLit.ElementAt(0);
                //PickedLit.RemoveAt(random);
                PickedLit.RemoveAt(0);
                Litteral = res;


                NoeudTree tempNoeudG = new NoeudTree();
                NoeudTree tempNoeudD = new NoeudTree();
                tempNoeudG.Retourn = Pre;
                tempNoeudD.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + Litteral + " ";
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + "-" + Litteral + " ";
                int EVG = Evaluate(tempNoeudG.Litteral);
                if (EVG == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }
                int EVD = Evaluate(tempNoeudD.Litteral);
                if (EVD == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                if (PickedLit.Count != 0)
                    if (EVG > EVD)
                    { UnExplored[tempNoeudD] = EVD; Pre = tempNoeudG; }
                    else
                    { UnExplored[tempNoeudG] = EVG; Pre = tempNoeudD; }
            }



            return result;
        }
        // This Method Search By Weight , Every time it Reachs to the Bottom , it take the Noeud with the Lowest Evaluation
        public static String ByHeuristicWeightLow(int nbvar, int nbclause)
        {

            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            Dictionary<NoeudTree, int> UnExplored = new Dictionary<NoeudTree, int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);
            //result
            String result = null;
            //Init Racine
            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            //Return path
            NoeudTree Pre = Racine;



            Random rand = new Random();
            int Litteral = 0;
            Boolean FirstEntrance = true;
            while (UnExplored.Count != 0 || FirstEntrance)
            {

                FirstEntrance = false;
                if (PickedLit.Count == 0)
                {
                    //Thread.Sleep(5000);
                    Pre = UnExplored.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    UnExplored.Remove(Pre);
                    //Re-initial PickedLit
                    PickedLit = Re_Init_HeuristicWeight(nbvar, Pre.Litteral);
                    
                }


                if (PickedLit.Count - 1 < 0)
                    break;

                int random = rand.Next(0, PickedLit.Count - 1);
                int res = PickedLit.ElementAt(random);
                PickedLit.RemoveAt(random);
                Litteral = res;

//
                NoeudTree tempNoeudG = new NoeudTree();
                NoeudTree tempNoeudD = new NoeudTree();
                tempNoeudG.Retourn = Pre;
                tempNoeudD.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + Litteral + " ";
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + "-" + Litteral + " ";
                int EVG = Evaluate(tempNoeudG.Litteral);
                if (EVG == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }
                int EVD = Evaluate(tempNoeudD.Litteral);
                if (EVD == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                //Add to the UnExplored Dictionary
                if (PickedLit.Count != 0)
                    if (EVG > EVD)
                    { UnExplored[tempNoeudD] = EVD; Pre = tempNoeudG; }
                    else
                    { UnExplored[tempNoeudG] = EVG; Pre = tempNoeudD; }

                

            }



            return result;
        }

        // This Method Search By Weight , Every time it Reachs to the Bottom , it take the Noeud with the Highest Evaluation
        public static String ByHeuristicWeightHigh(int nbvar, int nbclause)
        {
            // This Method Search By Weight , Every time it Reachs to the Bottom , it take the Noeud with the Highest Evaluation

            List<int> Closed = new List<int>();
            List<int> PickedLit = new List<int>();
            Dictionary<NoeudTree, int> UnExplored = new Dictionary<NoeudTree, int>();
            for (int i = 1; i <= nbvar; i++)
                PickedLit.Add(i);
            //result
            String result = null;
            //Init Racine
            NoeudTree Racine = new NoeudTree();
            Racine.Litteral = "";
            Racine.Retourn = null;
            //Return path
            NoeudTree Pre = Racine;



            Random rand = new Random();
            int Litteral = 0;
            Boolean FirstEntrance = true;
            while (UnExplored.Count != 0 || FirstEntrance)
            {

                FirstEntrance = false;
                if (PickedLit.Count == 0)
                {
                    //Thread.Sleep(5000);
                    Pre = UnExplored.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    UnExplored.Remove(Pre);
                    //Re-initial PickedLit
                    PickedLit = Re_Init_HeuristicWeight(nbvar, Pre.Litteral);

                }


                if (PickedLit.Count - 1 < 0)
                    break;

                int random = rand.Next(0, PickedLit.Count - 1);
                int res = PickedLit.ElementAt(random);
                PickedLit.RemoveAt(random);
                Litteral = res;




                //

                NoeudTree tempNoeudG = new NoeudTree();
                NoeudTree tempNoeudD = new NoeudTree();
                tempNoeudG.Retourn = Pre;
                tempNoeudD.Retourn = Pre;
                tempNoeudG.Litteral = tempNoeudG.Retourn.Litteral + Litteral + " ";
                tempNoeudD.Litteral = tempNoeudD.Retourn.Litteral + "-" + Litteral + " ";
                int EVG = Evaluate(tempNoeudG.Litteral);
                if (EVG == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }
                int EVD = Evaluate(tempNoeudD.Litteral);
                if (EVD == nbclause)
                {
                    Console.Write("Solution Found : ");
                    Console.Write(tempNoeudG.Litteral + " ");

                    result = tempNoeudG.Litteral;
                    break;
                }

                //Add to the UnExplored Dictionary
                if (PickedLit.Count != 0)
                    if (EVG > EVD)
                    { UnExplored[tempNoeudD] = EVD; Pre = tempNoeudG; }
                    else
                    { UnExplored[tempNoeudG] = EVG; Pre = tempNoeudD; }



            }



            return result;
        }

        //Ants System
        public static String AntsSystem(int nbvar, int nbclause) {
            int  MaxIterations = 106, AntsNumber = 3;
            double Alpha = 1, Beta = 1, Q0 = 0.5, Evaporation = 0.5, InitialPheromone = 0.02;
            Dictionary<int, double> Pheromone = InitPheromone(nbvar, InitialPheromone);
            Dictionary<int, int> NextLit = InitNextLit(nbvar);
           
            for (int i = 0; i < MaxIterations; i++) {

                for (int Ant = 0; Ant < AntsNumber; Ant++)
                {
                    Dictionary<int, int> tempNextLit = new Dictionary<int, int>(NextLit);
                      int cpt = 1;
                    //Construct Solution
                    String Solution = "";
                    do {
                        int myChoice = ProbabilityChoice(Pheromone, Alpha,Beta,Q0, tempNextLit,nbclause);

                        Console.WriteLine(myChoice + "===>" + Evaluate(""+myChoice));
                        Thread.Sleep(1000);
                        Solution += " " + myChoice;
                        tempNextLit.Remove(myChoice);
                        tempNextLit.Remove(-(myChoice));
                        cpt++;
                    } while (cpt <= nbvar);
                    Console.WriteLine(Solution);
                    //Evaluate Suolution
                    int EV = Evaluate(Solution);
                    if (EV == nbclause) { 
                        Console.Write("Solution Found : ");
                        Console.Write(Solution);

                        return Solution;
                    }

                   // OnlineUpdatePheromoneStepByStep(Pheromone, Solution, Evaporation, EV, nbclause);



                }//END #Ants


            }//END #Iteration


            return null;
        }
        public static void OnlineUpdatePheromoneStepByStep(Dictionary<int, double> Pheromone,String Solution,double Evaporation,int EV,int nb_clause) {
            String[] Solutions = Solution.Trim().Split(' ');
            
           
            foreach (String Lit in Solutions)
            {
                Pheromone[int.Parse(Lit.Trim())] = (1 - Evaporation) * Pheromone[int.Parse(Lit.Trim())] + (Evaporation * (EV/nb_clause));
            }


        }
        public static Dictionary<int, double> InitPheromone(int nbvar,double InitialPheromone) {
            Dictionary<int, double> myInit = new Dictionary<int, double>();
            for (int i = 1; i <= nbvar; i++) { 
                myInit.Add(i, InitialPheromone); myInit.Add(-i, InitialPheromone); }

            return myInit;
        }

        public static Dictionary<int, int> InitNextLit(int nbvar)
        {
            Dictionary<int, int> myInit = new Dictionary<int, int>();
            for (int i = 1; i <= nbvar; i++)
            {
                myInit.Add(i, 0); myInit.Add(-i, 0);
            }

            return myInit;
        }
        public static int ProbabilityChoice(Dictionary<int, double> Pheromone, double Alpha, double Beta, double Q0, Dictionary<int, int> NextLit, int nb_clause) {
            
            Dictionary<int, double> myProbabilities = new Dictionary<int, double>();
            //Init T_SCALAR_N
            List<KeyValuePair<int, int>> myLit = NextLit.ToList();
           

           
            foreach (KeyValuePair<int, int> Lit in myLit)
            {
                





                double Pheromone_SUM = Math.Pow(Pheromone[Lit.Key], Alpha)* Math.Pow(Evaluate("" + Lit.Key) , Beta)+(Math.Pow(Pheromone[-Lit.Key], Alpha) * Math.Pow(Evaluate("" + (-Lit.Key)), Beta));

                
                myProbabilities.Add(Lit.Key, Math.Pow(Pheromone[Lit.Key], Alpha) * Math.Pow(Evaluate("" + Lit.Key) , Beta) / Pheromone_SUM);
                

            }
            //Console.WriteLine( "==>" + myProbabilities.Aggregate((r, l) => r.Value > l.Value ? r : l).Key);
            return myProbabilities.Aggregate((r,l) => r.Value > l.Value ? r : l).Key;

        }
        private static string getLastLit(String input)
        {
            Console.WriteLine(input);
            if (input.Trim().Split(' ').Length < 2)
                return input.Trim();

            String[] All = input.Trim().Split(' ');
            return All[(All.Length - 1)];
        }

        public static int Evaluate(string input)
        {

            int[,] tempClause = (int[,])data_clause.Clone();
            int cpt = 0;
            String[] Litterales = input.Split(' ');
            
            foreach (String Lit in Litterales)
            {



                if (Lit.Trim().Length < 1)
                    continue;


                int Number = int.Parse(Lit.Trim());

                for (int i = 0; i < (data_clause.Length / 4); i++)
                {

                    if (tempClause[i, 3] == 1)
                        continue;

                    for (int j = 0; j < 3; j++)
                    {

                        if (tempClause[i, j] == Number)
                        {

                            tempClause[i, 3] = 1;

                            cpt++;
                            break;


                        }
                    }

                }


            }

           // Console.WriteLine("Nombre de clause pour " + input + "===>" + cpt);
            if (cpt > BestClauseSatisfied)
                BestClauseSatisfied = cpt;

            tempClause = null;
            return cpt;
        }

        public static Boolean TimeToShuffle(int ShuffleNumber)
        {
            int cpt = 0;
            String x = Convert.ToString(ShuffleNumber, 2);
            foreach (char c in x)
                if (c.Equals('1'))
                    cpt++;
            if (cpt == 1)
                return true;

            return false;
        }

    }
}

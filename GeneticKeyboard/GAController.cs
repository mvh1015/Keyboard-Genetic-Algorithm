using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticKeyboard
{
    class GAController
    {
        List<KeyValuePair<char[][], float>> population;

        Random rnd = new Random();

        KeyboardManager kbManager;
        FitnessCalc fitnessCalc;
        SettingsManager settingsManager;
        string stringToCalc;

        public GAController(KeyboardManager _kbManager, FitnessCalc _fitnessCalc, SettingsManager _settingsManager, string _stringToCalc)
        {
            kbManager = _kbManager;
            fitnessCalc = _fitnessCalc;
            settingsManager = _settingsManager;
            stringToCalc = _stringToCalc;

            population = new List<KeyValuePair<char[][], float>>();

            
            


        }

        public char [][] Initialize()
        {
            for (int i = 0; i < Values.POPULATION_SIZE; i++)
            {
                char[][] kb = kbManager.RandomizeKeyboard();
                population.Add(new KeyValuePair<char[][], float>(kb, CalcFitness(kb)));
            }

            return ShowLeadingChromosome();
        }

        public char [][] NextGeneration()
        {
            List<KeyValuePair<char[][], float>> newPopulation = new List<KeyValuePair<char[][], float>>();

            while (newPopulation.Count < population.Count() - Values.NUM_BEST_TO_ADD)
            {
                //Crossover
                Tuple<char[][],char[][]> children = CrossoverPMX(RouletteWheel());

                //Mutation
                children = new Tuple<char[][],char[][]>(ExchangeMutation(children.Item1), ExchangeMutation(children.Item2));

                newPopulation.Add(new KeyValuePair<char[][], float>(children.Item1, CalcFitness(children.Item1)));
                newPopulation.Add(new KeyValuePair<char[][], float>(children.Item2, CalcFitness(children.Item2)));
            }
            

            //ELITISM
            for (int i=0; i < Values.NUM_BEST_TO_ADD; ++i)
            {
                char[][] currentBest;
                currentBest = ShowLeadingChromosome();

                if (currentBest != null)
                {

                    newPopulation.Add(new KeyValuePair<char[][], float>(currentBest, CalcFitness(currentBest)));

                    population.Remove(population.First(population => population.Key.Equals(currentBest)));
                }
            }

            population = newPopulation;


            return ShowLeadingChromosome();
        }

        float CalcFitness(char[][] _kb)
        {
            return fitnessCalc.CalculateString(stringToCalc,_kb, settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting]);
        }





        public char[][] ShowLeadingChromosome()
        {
            var max = from x in population where x.Value == population.Max(v => v.Value) select x.Key;

            foreach (char[][] actualMax in max)
            {
                return actualMax;
            }

            return null;
        }

        /// <summary>
        /// Orders the population by fitness value, distributes pieces of the pie based on rank, and randomly chooses a pie. 
        /// </summary>
        /// <returns></returns>
        public Tuple<char[][],char[][]> RouletteWheel()
        {
            //Order population by rank
            var ordered = population.OrderBy(x => x.Value);

            int n = population.Count;

            //number of "pie pieces" to distribute
            double pieCount = (Math.Pow(n, 2) + n) / 2;

            //chooses two pieces of this pie randomly
            int selection1 = rnd.Next(0, (int)(pieCount + 1));
            int selection2 = rnd.Next(0, (int)(pieCount + 1));

            //Figures out which population owns what piece of pie
            int selection1Index = FindPieValue(selection1, pieCount, n) - 1;
            int selection2Index = FindPieValue(selection2, pieCount, n) - 1;

            while (selection1Index == selection2Index)
            {
                selection2 = rnd.Next(0, (int)(pieCount + 1));
                selection2Index = FindPieValue(selection2, pieCount, n) - 1;
            }


            //returns both chromosomes in a Tuple!
            Tuple<char[][],char[][]> twoChromosomes = new Tuple<char[][],char[][]>(ordered.ElementAt(selection1Index).Key, ordered.ElementAt(selection2Index).Key);

            return twoChromosomes;
        }

        int FindPieValue(int selection, double pieCount, int n)
        {
            if (selection >= (pieCount - n))
            {
                return n;
            } else
            {
                return FindPieValue(selection, pieCount - n, n - 1);
            }
        }

        char[][] ExchangeMutation(char[][] originalChromosome)
        {
            char[] chromosome = Utilities.FlattenArray(originalChromosome);

            if (rnd.NextDouble() > Values.MUTATION_RATE) return originalChromosome;

            int pos1 = rnd.Next(0, chromosome.Length);
            
            int pos2 = pos1;

            while (pos1 == pos2)
                pos2 = rnd.Next(0, chromosome.Length);

            Utilities.swap(ref chromosome[pos1], ref chromosome[pos2]);

            return Utilities.FlatToKeyboard(chromosome);

        }


        public Tuple<char[][], char[][]> CrossoverPMX(Tuple<char[][], char[][]> parents)
        {
            if (rnd.NextDouble() > Values.CROSSOVER_RATE)
                return parents;

            char[] mom = Utilities.FlattenArray(parents.Item1);
            char[] dad = Utilities.FlattenArray(parents.Item2);

            char[] baby1 = new char[mom.Length];
            Array.Copy(mom, baby1, mom.Length);

            char[] baby2 = new char[dad.Length];
            Array.Copy(dad, baby2, dad.Length);

            int beg = rnd.Next(0, mom.Length - 1);

            int end = beg;

            while (end <= beg)
            {
                end = rnd.Next(0, mom.Length);
            }

            //iterate through matched pairs of genes from begininng to end
            for (int pos = beg; pos < end +1; ++pos)
            {

                //get char from position of each chromosome
                char gene1 = mom[pos];
                char gene2 = dad[pos];

                //[5,4,7,9]     //mom
                //[7,9,4,5]     //dad

                //gene1 is 5, gene2 is 7

                if (gene1 != gene2)
                {
                    //Find the element of the chars in baby1
                    int posGene1 = Array.FindIndex(baby1, element => element.Equals(gene1));
                    int posGene2 = Array.FindIndex(baby1, element => element.Equals(gene2));

                    //posgene1 is index 0   (5 in baby1)
                    //posgene2 is index 2   (7 in baby1)

                    Utilities.swap(ref baby1[posGene1], ref baby1[posGene2]);

                    //[7,4,5,9]     //baby1 after swap
                    //[7,9,4,5]     //baby2

                    //Find the element of the char in baby2
                    posGene1 = Array.FindIndex(baby2, element => element.Equals(gene1));
                    posGene2 = Array.FindIndex(baby2, element => element.Equals(gene2));

                    //posgene1 is index 3   (5 in baby2)
                    //posgene2 is index 0   (7 in baby2)

                    Utilities.swap(ref baby2[posGene1], ref baby2[posGene2]);

                    //[7,4,5,9]     //baby1 after swap
                    //[5,9,4,7]     //baby2 after swap

                }


            }

            return new Tuple<char[][], char[][]>(Utilities.FlatToKeyboard(baby1), Utilities.FlatToKeyboard(baby2));

        }

        
    }
}

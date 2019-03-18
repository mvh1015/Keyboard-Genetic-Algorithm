using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticKeyboard
{
    public static class Values
    {
        static public float ALTERNATING_HAND = 0.7f;
        static public float ALTERNATING_FINGER = 0.9f;

        static public float POPULATION_SIZE = 70;

        static public float CROSSOVER_RATE = 0.7f;
        static public float MUTATION_RATE = 0.1f;
        static public int NUM_BEST_TO_ADD = 2;  //should be even number

        static public int FINGER_IDLE = 3; // if finger gets tired it goes back to its rest position.
    }
}

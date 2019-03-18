using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticKeyboard
{
    static class Utilities
    {
        public static Point FindInDimensions(this char[][] target, char searchTerm)
        {
            var colUpperLimit = target.Length;

            for (int col = 0; col < colUpperLimit; col++)
            {
                for (int row = 0; row < target[col].Length; row++)
                {
                    if (target[col][row] == searchTerm)
                        return new Point(row, col);

                }
            }

            return new Point(-1, -1);
        }

        public static char[] FlattenArray(this char[][] target)
        {
            int arrayLength = 0;
            for (int k = 0; k < target.GetLength(0); k++)
                arrayLength += target[k].Length;

            char[] flattenedArray = new char[arrayLength];
            int counter = 0;

            for (int k = 0; k < target.GetLength(0); k++)
                for (int l = 0; l < target[k].Length; l++)
                {
                    flattenedArray[counter++] = target[k][l];
                }

            return flattenedArray;
        }

        public static char[][] FlatToKeyboard(this char[] target)
        {
            char[][] keyboard = new char[][] {
            new char[12],
            new char[12],
            new char[11]
            };

            int counter = 0;

            for (int k = 0; k < keyboard.GetLength(0); k++)
                for (int l = 0; l < keyboard[k].Length; l++)
                {
                    keyboard[k][l] = target[counter++];
                }


            return keyboard;
        }

        public static int Convert2DToIndex(Point point, char[][] target)
        {
            int counter = 0;

            for (int k = 0; k < target.GetLength(0); k++)
                for (int l = 0; l < target[k].Length; l++)
                {

                    if (point == new Point(l, k))
                    {
                        return counter;
                    }
                    counter++;

                }

            return -1;
        }

        public static Point ConvertIndexTo2D(this char[][] target, int searchInt)
        {
            int counter = 0;


            for (int col = 0; col < target.GetLength(0); col++)
            {
                for (int row = 0; row < target[col].Length; row++)
                {
                    
                    if (counter == searchInt)
                        return new Point(row, col);

                    counter++;

                }
            }

            return new Point(-1, -1);
        }

        public static void swap(ref char a, ref char b)
        {
            char temp = a;
            a = b;
            b = temp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticKeyboard
{
    class KeyboardManager
    {

        Random rnd = new Random();

        char[] charactersToUse = new char[35] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', '[', ']', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ';', '\'', '\\', '`', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', ',', '.', '/' };
        char[] dvorakSimplified = new char[35] { '\'', ',', '.', 'P', 'Y', 'F', 'G', 'C', 'R', 'L', '/', '[', 'A', 'O', 'E', 'U', 'I', 'D', 'H', 'T', 'N', 'S', ']', '\\', '`', ';', 'Q', 'J', 'K', 'X', 'B', 'M', 'W', 'V', 'Z' };

        public bool[] lockedChars = new bool[35];
        public int[] fingersOnEachKey = new int[35];

        //public Finger[];

        public KeyboardManager()
        {
            RandomizeKeyboard();

            

        }
        

        public char[][] RandomizeKeyboard()
        {
            char[][] randomizedKeyboard = new char[][] {
            new char[12],
            new char[12],
            new char[11]
            };

            char[] randomizedArray = new char[35];
            charactersToUse.CopyTo(randomizedArray, 0);

            for (int t = 0; t < randomizedArray.Length; t++)
            {
                char tmp = randomizedArray[t];
                int r = rnd.Next(t, randomizedArray.Length);
                randomizedArray[t] = randomizedArray[r];
                randomizedArray[r] = tmp;
            }

            //Keep locked chars
            for (int t = 0; t < randomizedArray.Length; t++)
            {
                if (lockedChars[t])
                {
                    char tmp = randomizedArray[t];
                    int r = Array.FindIndex(randomizedArray, s => s.Equals(charactersToUse[t]));
                    randomizedArray[t] = randomizedArray[r];
                    randomizedArray[r] = tmp;
                    
                }
            }

            int counter = 0;

            for (int k = 0; k < randomizedKeyboard.GetLength(0); k++)
                for (int l = 0; l < randomizedKeyboard[k].Length; l++)
                {
                    randomizedKeyboard[k][l] = randomizedArray[counter++];
                }

            return randomizedKeyboard;
        }

        public enum KeysLayout { QWERTY,DVORAK};
        public KeysLayout currentKeyLayout = KeysLayout.QWERTY;

        public char[][] ReturnQWERTY()
        {
            char[][] qwertyKeyboard = new char[][] {
            new char[12],
            new char[12],
            new char[11]
            };

            int counter = 0;

            for (int k = 0; k < qwertyKeyboard.GetLength(0); k++)
                for (int l = 0; l < qwertyKeyboard[k].Length; l++)
                {
                    switch (currentKeyLayout)
                    {
                        case KeysLayout.QWERTY:
                            qwertyKeyboard[k][l] = charactersToUse[counter++];
                            break;
                        case KeysLayout.DVORAK:
                            qwertyKeyboard[k][l] = dvorakSimplified[counter++];
                            break;
                    }

                }
            

            return qwertyKeyboard;
        }

        #region HelperMethods

        public string ChangeToString(Label lbl, char charToAdd)
        {
            lbl.Font = new Font(lbl.Font.FontFamily, 12);
            switch (charToAdd)
            {

                case '`':
                    return "~\n" + charToAdd.ToString();
                case '[':
                    return "{\n" + charToAdd.ToString();
                case ']':
                    return "}\n" + charToAdd.ToString();
                case '\\':
                    return "|\n" + charToAdd.ToString();
                case ';':
                    return ":\n" + charToAdd.ToString();
                case '\'':
                    return "\"\n" + charToAdd.ToString();
                case ',':
                    return "<\n" + charToAdd.ToString();
                case '.':
                    return ">\n" + charToAdd.ToString();
                case '/':
                    return "?\n" + charToAdd.ToString();
                default:
                    lbl.Font = new Font(lbl.Font.FontFamily, 18);
                    return charToAdd.ToString();
            }
        }
        #endregion


    }
}

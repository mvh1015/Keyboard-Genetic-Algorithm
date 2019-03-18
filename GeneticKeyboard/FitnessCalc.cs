using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeneticKeyboard
{
    class FitnessCalc
    {

        char? firstCharacter;
        char secondCharacter;

        List<Finger> fingerList;

        


        public float InputCharacter(char newCharacter, char[][] keyboard, int[] finger)
        {
            // Find finger to use

            if (firstCharacter != null)
            {
                firstCharacter = char.ToUpper(newCharacter);
                return CalcDistance(Utilities.FindInDimensions(keyboard, (char)firstCharacter), Utilities.FindInDimensions(keyboard, newCharacter), 0);
            }

            return 0;
        }

        public float CalculateString(string stringToCalculate, char[][] keyboard, FingerKeySet fingerKeys)
        {
            List<Finger> fingerList = fingerKeys.GetFingerList();
            Finger[] fingerArray = new Finger[8];

            float fitnessValue = 0;
            int characterCount = 0;

            //Change fingerlist into an array to keep consistent arrays
            foreach (Finger f in fingerList)
            {
                fingerArray[(int)f.fingerID] = f;
            }

            int? oldFingerIndex = null;
            bool? lastHandIsRight = null;

            foreach (char c in stringToCalculate)
            {
               

                float fitnessValueModifier = 1;
                
                firstCharacter = char.ToUpper(c);
                firstCharacter = CheckShiftKey(firstCharacter);

                //Find character on keyboard
                Point characterOnKeyboard = Utilities.FindInDimensions(keyboard, (char)firstCharacter);

                if (characterOnKeyboard == new Point(-1, -1))
                    continue;

                characterCount++;

                //Convert keyboard to 2D
                int characterIndex = Utilities.Convert2DToIndex(characterOnKeyboard, keyboard);

                //Find out which finger goes to this key
                int fingerIndex = fingerKeys.keysPerFinger[characterIndex];

                fitnessValueModifier *= fingerArray[fingerIndex].fingerBonus();

                if (lastHandIsRight != null && lastHandIsRight != isRightHand(fingerIndex))
                {
                    fitnessValueModifier *= Values.ALTERNATING_HAND;
                } else if (oldFingerIndex != null && oldFingerIndex != fingerIndex)
                {
                    fitnessValueModifier *= Values.ALTERNATING_FINGER;
                }

                lastHandIsRight = isRightHand(fingerIndex);

                fitnessValue += (CalcDistance(fingerArray[fingerIndex].CurrentPosition, characterOnKeyboard, 0) * fitnessValueModifier);

                fingerArray[fingerIndex].CurrentPosition = characterOnKeyboard;

                for(int i = 0; i < fingerArray.Length; i++)
                {
                    if (fingerArray[i] != null)
                    {
                        fingerArray[i].Idle();
                    }
                }
                
            }

            return 1/(fitnessValue / characterCount);
        }

        char? CheckShiftKey(char? c)
        {
            switch (c)
            {
                
                case '~':
                    return '`';
                case '{':
                    return '[';
                case '}':
                    return ']';
                case '|':
                    return '\\';
                case ':':
                    return ';';
                case '"':
                    return '\'';
                case '<':
                    return ',';
                case '>':
                    return '.';
                case '?':
                    return '/';
                default:
                    return c;
            }

        }

        bool isRightHand(int fingerIndex)
        {
            if (fingerIndex > 3)
                return true;
            else
                return false;
        }


        
        public float CalcDistance(Point key1, Point key2, float distance)
        {

                if (key1 == key2)
                    return distance;                            //E to E

                int differenceX = Math.Abs(key1.X - key2.X);
                int differenceY = Math.Abs(key1.Y - key2.Y);

                if (key1.Y == key2.Y)
                {
                    return differenceX + distance;              //E to U
                }

                if (key1.X == key2.X)
                {
                    return differenceY + distance;              //D to E
                }

                switch (key1.Y)
                {
                    case 0:
                        if (differenceX == 1 && differenceY == 1)
                        {
                            if (key1.X > key2.X)                //Y to G
                                return 1.5f + distance;
                            else
                                return 2.0f + distance;         //Y to J
                        }
                        else
                        {
                            if (key1.X > key2.X)
                                return CalcDistance(new Point(key1.X - 1, key1.Y + 1), key2, distance + 1.5f);          //Y to D
                            else
                                return CalcDistance(new Point(key1.X + 1, key1.Y + 1), key2, distance + 2);             //Y to L

                        
                        }
                    
                    case 1:
                        if (differenceX == 1 && differenceY == 1)
                        {
                            if (key1.Y > key2.Y)
                            {
                                if (key1.X < key2.X)            // G to Y
                                    return 1.5f + distance;
                                else                            // G to R
                                    return 2.0f + distance;
                            }
                            else
                                return 1f + distance;                    // G to V ... or G to B
                        }
                        else
                        {
                            int direction = 0;
                            if (key1.Y > key2.Y)
                                direction = 1;
                            else
                                direction = -1;

                            if (key1.X > key2.X)
                                return CalcDistance(new Point(key1.X - 1, key1.Y - direction), key2, distance + 1.5f);          //D to Q
                            else
                                return CalcDistance(new Point(key1.X + 1, key1.Y - direction), key2, distance + 2);             //G to P
                        }

                    default:
                        if (differenceX == 1 && differenceY == 1)
                        {
                                return 1f + distance;                    // V to G ... or V to F
                        }
                        else
                        {
                                return CalcDistance(new Point(key1.X - 1, key1.Y - 1), key2, distance + 1f);
                        }
                }

        }

        

    }
}

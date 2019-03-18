using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeneticKeyboard
{
    class FingerKeySet
    {
        public enum FingerSettings { SimpleSetting, FourFingerSetting, ComplexSetting, DeveloperCut }

        public int[] keysPerFinger;
        public List<int> defaultFingerKeys;


        public FingerKeySet(FingerSettings _setting, int[] _keysPerFinger, List<int> _defaultFingerKey)
        {
            keysPerFinger = _keysPerFinger;
            defaultFingerKeys = _defaultFingerKey;
        }

        public List<Finger> GetFingerList()
        {
            //Initialize list of fingers
            List<Finger> fingerList = new List<Finger>();

            int counter = 0;

            //For each finger in keyboard setting
            foreach (int f in defaultFingerKeys)
            {
                

                //Create a finger and add it to the FingerKeySet list to return
                Finger newFinger = new Finger(Utilities.ConvertIndexTo2D(Form1.shownKeyboard, f), keysPerFinger[defaultFingerKeys[counter]]);

                //add each key to the finger
                int keysToFind = keysPerFinger[f];

                
                for (int i = 0; i < keysPerFinger.Count(); i++)
                {
                    if (keysPerFinger[i] == keysToFind)
                    {
                        Point coordinates = Utilities.ConvertIndexTo2D(Form1.shownKeyboard, i);
                        newFinger.keySet.Add(Form1.shownKeyboard[coordinates.Y][coordinates.X]);
                    }
                }
                fingerList.Add(newFinger);

                counter++;
            }

            return fingerList;
        }

        void FindFinger()
        {
            
        }


    }
}

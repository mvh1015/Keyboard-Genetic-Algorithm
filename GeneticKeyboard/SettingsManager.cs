using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeneticKeyboard
{
    class SettingsManager
    {
        public enum ProgramState { DefaultMode, FingerChooseMode, KeyboardLockMode};

        public ProgramState stateOfProgram = ProgramState.DefaultMode;

        public int assignedFinger = 0;
        public FingerKeySet.FingerSettings currentFingerSetting = FingerKeySet.FingerSettings.SimpleSetting;

        //FingerSettings
        public List<FingerKeySet> fingerKeySets = new List<FingerKeySet>();

        public SettingsManager()
        {
            fingerKeySets.Add(new FingerKeySet((FingerKeySet.FingerSettings)0,
                                                new int[35] { 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4 },
                                                new List<int>(new int[] { 15, 18 })));

            fingerKeySets.Add(new FingerKeySet((FingerKeySet.FingerSettings)1,
                                                new int[35] { 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 5, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 5, 2, 2, 2, 3, 3, 3, 4, 4, 5, 5, 5 },
                                                new List<int>(new int[] { 13, 15, 18, 20 })));

            fingerKeySets.Add(new FingerKeySet((FingerKeySet.FingerSettings)2,
                                                new int[35] { 0, 1, 2, 3, 3, 4, 4, 5, 6, 7, 7, 7, 0, 1, 2, 3, 3, 4, 4, 5, 6, 7, 7, 7, 0, 0, 1, 2, 3, 3, 4, 4, 5, 6, 7 },
                                                new List<int>(new int[] { 12, 13, 14, 15, 18, 19, 20, 21 })));

            fingerKeySets.Add(new FingerKeySet((FingerKeySet.FingerSettings)3,
                                                new int[35] { 1, 1, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 0, 1, 2, 3, 3, 3, 4, 4, 4, 4, 4, 4, 0, 0, 3, 3, 3, 3, 4, 4, 4, 4, 4 },
                                                new List<int>(new int[] { 9, 12, 13, 14, 15, 18})));

        }

        public Color GetColor()
        {
            switch (assignedFinger)
            {
                case 0:
                    return Color.Red;

                case 1:
                    return Color.FromArgb(0, 255, 0);

                case 2:
                    return Color.Blue;

                case 3: 
                    return Color.Cyan;

                case 4:
                    return Color.FromArgb(255, 0, 255);

                case 5:
                    return Color.FromArgb(167, 167, 0);
                    
                case 6:
                    return Color.FromArgb(111, 115, 103);

                case 7:
                    return Color.FromArgb(197, 162, 111);

                default:
                    return Color.Transparent;
            }
        }





    }
}

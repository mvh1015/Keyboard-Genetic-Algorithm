using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeneticKeyboard
{
    public class Finger
    {
        public enum FingerType { LPinky, LRing, LMiddle, LPointer, RPointer, RMiddle, RRing, RPinky};

        public FingerType fingerID;

        public Point restPosition;
        private Point currentPosition;

        public List<char> keySet;


        public Point CurrentPosition
        {
            get {
                idleCounter = 0;
                return currentPosition;
            }
            set
            {
                currentPosition = value;
            }
        }

        public Finger(Point _restPosition, int finger)
        {
            restPosition = _restPosition;
            currentPosition = _restPosition;

            fingerID = (FingerType)finger;

            keySet = new List<char>();
        }

        int idleCounter = 0; 

        public void Idle()
        {
            idleCounter++;

            if (idleCounter > Values.FINGER_IDLE)
            {
                currentPosition = restPosition;
            }
        }

        public float fingerBonus()
        {
            switch(fingerID)
            {
                case FingerType.LPinky:
                    return 0.99f;
                case FingerType.LRing:
                    return 1;
                case FingerType.LMiddle:
                    return 0.97f;
                case FingerType.LPointer:
                    return 0.95f;
                case FingerType.RPinky:
                    return 0.99f;
                case FingerType.RRing:
                    return 1;
                case FingerType.RMiddle:
                    return 0.97f;
                case FingerType.RPointer:
                    return 0.95f;
                default:
                    return 1;
            }
        }

        
    }
}

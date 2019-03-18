using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeneticKeyboard
{
    class FileReader
    {
        public static string ReadFile(Form1 form, string path, FitnessCalc calc)
        {
            
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();

                    /*
                    float fitnessValue = 0;
                    while (sr.Peek() >= 0)
                    {

                        fitnessValue += calc.InputCharacter((char)sr.Read());

                        form.PrintChar((char)sr.Read());
                    }

                    */
                }
            }
            catch (Exception e)
            {
                
                Console.WriteLine("The process failed: {0}", e.ToString());
                return null;
            }

        }
    }
}

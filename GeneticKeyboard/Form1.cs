using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GeneticKeyboard
{
    public partial class Form1 : Form
    {

        KeyboardManager keyboardManager;
        SettingsManager settingsManager;
        FitnessCalc fitnessValue;

        static public char[][] shownKeyboard;

        

        public Form1()
        {
            InitializeComponent();
            keyboardManager = new KeyboardManager();
            settingsManager = new SettingsManager();
            fitnessValue = new FitnessCalc();

            

            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);

            settingsManager.currentFingerSetting = FingerKeySet.FingerSettings.SimpleSetting;

            PrintKeyBoard(keyboardManager.ReturnQWERTY());

            //Console.WriteLine(fitnessValue.CalcDistance(Utilities.FindInDimensions(shownKeyboard, 'E'), Utilities.FindInDimensions(shownKeyboard, 'V'), 0));

            SetKeyColors();
        }


        bool isFirst = false;

        #region HelperMethods

        public void PrintKeyBoard(char [][] keyboardToPrint)
        {
            shownKeyboard = keyboardToPrint;

            for (int k = 0; k < keyboardToPrint.GetLength(0); k++)
                for (int l = 0; l < keyboardToPrint[k].Length; l++)
                {
                    string ID = "lbl_key_" + l.ToString() + "_" + k.ToString();
                    string fingerID = "pic_box_" + l.ToString() + "_" + k.ToString();


                    Label lbl = this.Controls.Find(ID, true).FirstOrDefault() as Label;
                    lbl.Text = keyboardManager.ChangeToString(lbl, keyboardToPrint[k][l]);

                    if (!isFirst)
                    {
                        PictureBox picBox = this.Controls.Find(fingerID, true).FirstOrDefault() as PictureBox;
                    
                        lbl.Controls.Add(picBox);
                        picBox.Location = new Point(20, 22);
                        picBox.BringToFront();

                        
                    }

                    lbl.Invalidate();
                    lbl.Update();
                    lbl.Refresh();

                }
            isFirst = true;
            
        }

        #endregion

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            PrintKeyBoard(keyboardManager.RandomizeKeyboard());
        }

        private void btnfingerChoose_Click(object sender, EventArgs e)
        {
            
        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Point charCoordinates;
            string ID;
            Label lbl;

            if (char.IsLower(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }

            switch (settingsManager.stateOfProgram)
            {
                case SettingsManager.ProgramState.DefaultMode:
                    return;
                case SettingsManager.ProgramState.KeyboardLockMode:


                    if (e.KeyChar == (char)Keys.Escape)
                    {
                        lblProgramMode.Visible = false;
                        settingsManager.stateOfProgram = SettingsManager.ProgramState.DefaultMode;
                        return;
                    }

                    charCoordinates = Utilities.FindInDimensions(shownKeyboard, e.KeyChar);
                    if (charCoordinates.X < 0)
                        return;

                    int lockedIndex = Utilities.Convert2DToIndex(charCoordinates, shownKeyboard);
                    keyboardManager.lockedChars[lockedIndex] = !keyboardManager.lockedChars[lockedIndex];

                    ID = "lbl_key_" + charCoordinates.X.ToString() + "_" + charCoordinates.Y.ToString();

                    lbl = this.Controls.Find(ID, true).FirstOrDefault() as Label;

                    if (lbl.BorderStyle == BorderStyle.None)
                        lbl.BorderStyle = BorderStyle.Fixed3D;
                    else
                        lbl.BorderStyle = BorderStyle.None;
                    return;

                case SettingsManager.ProgramState.FingerChooseMode:


                    if (e.KeyChar == (char)Keys.Escape)
                    {
                        lblProgramMode.Visible = false;
                        settingsManager.stateOfProgram = SettingsManager.ProgramState.DefaultMode;
                        return;
                    }
                    else if (e.KeyChar >= 49 && e.KeyChar <= 56)   //1-8 on keyboard
                    {
                        settingsManager.assignedFinger = (int)e.KeyChar - 49;
                        lblProgramMode.Text = "Assign to Finger " + (settingsManager.assignedFinger + 1).ToString() + ". Press # to Assign new Finger. Press ESC to exit mode.";
                    }
                    else
                    {
                        charCoordinates = Utilities.FindInDimensions(shownKeyboard, e.KeyChar);

                        //Set specific key to finger array in keyboard
                        keyboardManager.fingersOnEachKey[Utilities.Convert2DToIndex(charCoordinates, shownKeyboard)] = settingsManager.assignedFinger;

                        if (charCoordinates.X < 0)
                            return;

                        ID = "lbl_key_" + charCoordinates.X.ToString() + "_" + charCoordinates.Y.ToString();

                        lbl = this.Controls.Find(ID, true).FirstOrDefault() as Label;

                        lbl.BackColor = settingsManager.GetColor();




                    }



                    return;
            }

        }

        List<PictureBox> visiblePictureBoxes = new List<PictureBox>();


        private void SetKeyColors()
        {

            keyboardManager.fingersOnEachKey = settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting].keysPerFinger;

            for (int i = 0; i < keyboardManager.fingersOnEachKey.Length; i++)
            {
                
                settingsManager.assignedFinger = keyboardManager.fingersOnEachKey[i];
                Point coordinates = Utilities.ConvertIndexTo2D(shownKeyboard, i);
                string ID = "lbl_key_" + coordinates.X.ToString() + "_" + coordinates.Y.ToString();

                Label lbl = this.Controls.Find(ID, true).FirstOrDefault() as Label;

                lbl.BackColor = settingsManager.GetColor();

            };

            foreach (PictureBox v in visiblePictureBoxes)
            {
                v.Visible = false;
            }

            visiblePictureBoxes.Clear();

            for (int j = 0; j < settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting].defaultFingerKeys.Count; j++)
            {
                int indexedKey = settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting].defaultFingerKeys[j];

                Point coordinates = Utilities.ConvertIndexTo2D(shownKeyboard, indexedKey);
                string fingerID = "pic_box_" + coordinates.X.ToString() + "_" + coordinates.Y.ToString();

                PictureBox picBox = this.Controls.Find(fingerID, true).FirstOrDefault() as PictureBox;


                visiblePictureBoxes.Add(picBox);
                picBox.Visible = true;

            }
        }

        


        private void btnLockControls_Click(object sender, EventArgs e)
        {
            PrintKeyBoard(keyboardManager.ReturnQWERTY());
            lblProgramMode.Text = "Choose Keys To Lock. Press ESC to exit mode.";
            lblProgramMode.Visible = true;
            
            settingsManager.stateOfProgram = SettingsManager.ProgramState.KeyboardLockMode;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                paragraphText.Text = "";
                paragraphText.Invalidate();
                paragraphText.Update();
                paragraphText.Refresh();

                paragraphText.Text = FileReader.ReadFile(this, file, fitnessValue);
            }
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
            
        }

        public void PrintChar(char letter)
        {
            if (char.IsLower(letter))
            {
                letter = char.ToUpper(letter);
            }
            Point charCoordinates = Utilities.FindInDimensions(shownKeyboard, letter);

            paragraphText.Text += letter;
            paragraphText.Invalidate();
            paragraphText.Update();
            paragraphText.Refresh();

            if (charCoordinates.X < 0)
                return;

            string ID = "lbl_key_" + charCoordinates.X.ToString() + "_" + charCoordinates.Y.ToString();

            Label lbl = this.Controls.Find(ID, true).FirstOrDefault() as Label;

            Color oldColor = lbl.BackColor;

            lbl.BackColor = Color.Yellow;
            lbl.Invalidate();
            lbl.Update();
            lbl.Refresh();

            

            System.Threading.Thread.Sleep(50);
            lbl.BackColor = oldColor;
            lbl.Invalidate();
            lbl.Update();
            lbl.Refresh();

            System.Threading.Thread.Sleep(50);

        }

        private void twoFingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolStrips(twoFingerToolStripMenuItem);

            settingsManager.currentFingerSetting = FingerKeySet.FingerSettings.SimpleSetting;

            SetKeyColors();
        }

        private void fourFingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolStrips(fourFingerToolStripMenuItem);

            settingsManager.currentFingerSetting = FingerKeySet.FingerSettings.FourFingerSetting;
            SetKeyColors();
        }

        private void tenFingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolStrips(tenFingerToolStripMenuItem);
            settingsManager.currentFingerSetting = FingerKeySet.FingerSettings.ComplexSetting;
            SetKeyColors();
        }

        private void directorsCutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolStrips(directorsCutToolStripMenuItem);
            settingsManager.currentFingerSetting = FingerKeySet.FingerSettings.DeveloperCut;
            SetKeyColors();
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolStrips(customToolStripMenuItem);
            PrintKeyBoard(keyboardManager.ReturnQWERTY());
            lblProgramMode.Text = "Assign to Finger 1. Press # to Assign new Finger. Press ESC to exit mode.";
            lblProgramMode.Visible = true;
            settingsManager.stateOfProgram = SettingsManager.ProgramState.FingerChooseMode;

        }

        private void ResetToolStrips(ToolStripMenuItem checkedTool)
        {
            settingsManager.stateOfProgram = SettingsManager.ProgramState.DefaultMode;
            lblProgramMode.Visible = false;

            ToolStripMenuItem[] tools = new ToolStripMenuItem[] { twoFingerToolStripMenuItem, fourFingerToolStripMenuItem, tenFingerToolStripMenuItem, directorsCutToolStripMenuItem, customToolStripMenuItem };

            for (int i = 0; i < tools.Length; i++)
            {
                tools[i].Checked = false;
            }

            checkedTool.Checked = true;
        }

        private void btnFindFV_Click(object sender, EventArgs e)
        {
            lblFV.Text = "Fitness Value: " + fitnessValue.CalculateString(paragraphText.Text, shownKeyboard, settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting]).ToString();
        }

        private void qWERTYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dVORAKToolStripMenuItem.Checked = false;
            qWERTYToolStripMenuItem.Checked = true;

            keyboardManager.currentKeyLayout = KeyboardManager.KeysLayout.QWERTY;
            PrintKeyBoard(keyboardManager.ReturnQWERTY());
        }

        private void dVORAKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dVORAKToolStripMenuItem.Checked = true;
            qWERTYToolStripMenuItem.Checked = false;

            keyboardManager.currentKeyLayout = KeyboardManager.KeysLayout.DVORAK;
            PrintKeyBoard(keyboardManager.ReturnQWERTY());
        }

        GAController ga;
        int generation = 0;

        Thread thread;

        private void btnGA_Click(object sender, EventArgs e)
        {
            if (!geneticAlgorithmPlaying)
            {
                InitializeBenchmarks();
                generation = 0;
                thread = new Thread(() => StartGA());
                thread.Start();
                geneticAlgorithmPlaying = true;
                btnGA.Text = "Pause";
            } else
            {
                thread.Abort();
                geneticAlgorithmPlaying = false;
                btnGA.Text = "Play";

            }

        }

        

        public bool geneticAlgorithmPlaying = false;

        private void StartGA()
        {
            while (geneticAlgorithmPlaying)
            {
                char[][] gaKeyboard;

                if (ga == null)
                {
                    ga = new GAController(keyboardManager, fitnessValue, settingsManager, paragraphText.Text);

                    gaKeyboard = ga.Initialize();
                }
                else
                {
                    gaKeyboard = ga.NextGeneration();
                }

                this.Invoke(new MethodInvoker(delegate             
                {
                    
                    lblGen.Text = (++generation).ToString();
                    
                    float fv = fitnessValue.CalculateString(paragraphText.Text, gaKeyboard, settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting]);
                    lblFV.Text = "Fitness Value: " + fv.ToString();
                    if (CheckBenchmarks(fv))
                    {
                        PrintKeyBoard(gaKeyboard);
                    };
                }));
            }
        }

        private void buttonRst_Click(object sender, EventArgs e)
        {
            qwertyBenchmark = 0;
            dvorakBenchmark = 0;
            lastChange = 0;
            lastChangeCounter = 0;
            generation = 0;

            lblQWERTYGen.Text = "INCOMPLETE";
            lblDVORAKGen.Text = "INCOMPLETE";
            InitializeBenchmarks();

            char[][] gaKeyboard;

            
            ga = new GAController(keyboardManager, fitnessValue, settingsManager, paragraphText.Text);

            gaKeyboard = ga.Initialize();

            
        }

        float qwertyBenchmark = 0;
        float dvorakBenchmark = 0;
        float lastChange = 0;
        int lastChangeCounter = 0;
        

        private void InitializeBenchmarks()
        {
            KeyboardManager.KeysLayout temp = keyboardManager.currentKeyLayout;

            keyboardManager.currentKeyLayout = KeyboardManager.KeysLayout.QWERTY;
            qwertyBenchmark = fitnessValue.CalculateString(paragraphText.Text, keyboardManager.ReturnQWERTY(), settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting]);
            lblQWERTY.Text = qwertyBenchmark.ToString();

            keyboardManager.currentKeyLayout = KeyboardManager.KeysLayout.DVORAK;
            dvorakBenchmark = fitnessValue.CalculateString(paragraphText.Text, keyboardManager.ReturnQWERTY(), settingsManager.fingerKeySets[(int)settingsManager.currentFingerSetting]);
            lblDVORAK.Text = dvorakBenchmark.ToString();
        }

        
        private bool CheckBenchmarks(float valueToCheck)
        {
            if (qwertyBenchmark > 0)
            {
                if (valueToCheck > qwertyBenchmark)
                {
                    lblQWERTYGen.Text = generation.ToString();
                    qwertyBenchmark = -1;
                }
            }

            if (dvorakBenchmark > 0)
            {
                if (valueToCheck > dvorakBenchmark)
                {
                    lblDVORAKGen.Text = generation.ToString();
                    dvorakBenchmark = -1;
                }
            }

            lastChangeCounter++;

            bool change = false;

            if (valueToCheck > lastChange)
            {
                change = true;
                lastChangeCounter = 0;
            }

            lastChange = valueToCheck;

            lblChange.Text = lastChangeCounter.ToString();

            return change;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void devToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devToolsToolStripMenuItem.Checked)
            {
                devToolsToolStripMenuItem.Checked = false;
                btnFindFV.Visible = false;
                btnRandomize.Visible = false;
                btnLockControls.Visible = false;
                customToolStripMenuItem.Enabled = false;
            } else
            {
                devToolsToolStripMenuItem.Checked = true;
                btnFindFV.Visible = true;
                btnRandomize.Visible = true;
                btnLockControls.Visible = true;
                customToolStripMenuItem.Enabled = true;
            }
        }

       
    }
}

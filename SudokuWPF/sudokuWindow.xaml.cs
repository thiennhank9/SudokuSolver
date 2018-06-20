using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

using System.Threading;
using System.IO;
using System.Xml;

using System.Timers;

namespace SudokuWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class sudokuWindow : Window
    {
        Thread runThread;
        SudokuC sudoku;
        string fromIndexComboVal = "0";
        bool findAllSol = false;
        int lastModField = -1;
        OpenFileDialog openFile;
        SaveFileDialog saveFile;
        int lastSolNum = 0;
        public Cursor cur = new Cursor(Environment.CurrentDirectory.ToString() + "\\Cursors\\pencil.cur");
        System.Timers.Timer mainTimer;

        public sudokuWindow()
        {
            InitializeComponent();
            sudoku = new SudokuC(this);
            mainTimer = new System.Timers.Timer();
            openFile = new OpenFileDialog();
            saveFile = new SaveFileDialog();
            mainTimer.Interval = 1;
            mainTimer.Stop();
            mainTimer.Elapsed += mainTimer_Tick;
            
            
            for (int i = 0; i < 81; ++i) fromIndexCombo.Items.Add(Convert.ToString(i));
            fromIndexRadio.IsChecked = true;
            fromIndexCombo.SelectedIndex = 0;
            // --------------------------------------------------------------
            // ------------   OPEN / SAVE DIALOG SETTINGS -------------------
            // --------------------------------------------------------------
            openFile.Title = "Open sudoku file";
            saveFile.Title = "Save sudoku file";
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if(runThread != null) runThread.Abort();
            Close();
        }

        public void LockButtons()
        {
            addFieldButton.IsEnabled = false;
            generateButton.IsEnabled = false;
            leftButton.IsEnabled = false;
            rightButton.IsEnabled = false;
            actSolution.IsEnabled = false;
            addFieldButton.IsEnabled = false;
            generateButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            resetButton.IsEnabled = false;
            openMenu.IsEnabled = false;
            saveMenu.IsEnabled = false;
            fromIndexCombo.IsEnabled = false;
            fromIndexRadio.IsEnabled = false;
            randRadio.IsEnabled = false;
            bestRadio.IsEnabled = false;
            flashCheckedFields.IsEnabled = false;
            sudoku.LockTable();
        }

        delegate void UnlockButtonsDelegate();
        public void UnlockButtons()
        {
            if (startButton.Dispatcher.Thread != Thread.CurrentThread || clearButton.Dispatcher.Thread != Thread.CurrentThread || resetButton.Dispatcher.Thread != Thread.CurrentThread)
            {
                UnlockButtonsDelegate del = new UnlockButtonsDelegate(UnlockButtons);
                startButton.Dispatcher.Invoke(del, new object[] { });
            }
            else
            {
                addFieldButton.IsEnabled = true;
                generateButton.IsEnabled = true;
                leftButton.IsEnabled = true;
                rightButton.IsEnabled = true;
                actSolution.IsEnabled = true;

                addFieldButton.IsEnabled = true;
                generateButton.IsEnabled = true;

                clearButton.IsEnabled = true;
                resetButton.IsEnabled = true;
                fromIndexCombo.IsEnabled = true;
                fromIndexRadio.IsEnabled = true;
                randRadio.IsEnabled = true;
                bestRadio.IsEnabled = true;
                openMenu.IsEnabled = true;
                saveMenu.IsEnabled = true;
                flashCheckedFields.IsEnabled = true;
                sudoku.UnlockTable();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.InitialDirectory = Directory.GetCurrentDirectory() + "\\Tables";
            openFile.Filter = "Xml files (*.xml)|*.xml";
            if ((bool)openFile.ShowDialog())
            {
                LoadFromFile(openFile.FileName);
            }
            leftButton.Visibility = Visibility.Hidden;
            rightButton.Visibility = Visibility.Hidden;
            actSolution.Visibility = Visibility.Hidden;
            solutionLabel.Visibility = Visibility.Hidden;
        }

        private void SaveToFile(string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            xmlWriter.WriteStartElement("Root");
            xmlWriter.Close();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;

            XmlElement childNode2 = xmlDoc.CreateElement("Field");
            XmlText textNode = xmlDoc.CreateTextNode("b");
            textNode.Value = "c";
            root.AppendChild(childNode2);
            childNode2.SetAttribute("name", tableName.Text);
            childNode2.AppendChild(textNode);
            textNode.Value = tableName.Text;

            
            for (int i = 0; i < 81; ++i)
            {
                 childNode2 = xmlDoc.CreateElement("Field");
                 textNode = xmlDoc.CreateTextNode("b");
                textNode.Value = "c";
                root.AppendChild(childNode2);
                childNode2.SetAttribute("index", i.ToString());
                childNode2.AppendChild(textNode);
                textNode.Value = sudoku.GetSudokuField(i);
            }
            xmlDoc.Save(filename);
        }

        private void LoadFromFile(string filename)
        {
            for (int i = 0; i < 81; ++i) sudoku.SetSudokuField(i, "");
            XmlTextReader reader = new XmlTextReader(filename);
            int actPlace = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        while (reader.MoveToNextAttribute())
                            if (reader.Name == "name")
                            {
                                tableName.Text = reader.Value;
                                reader.Read();
                            } 
                            if (reader.Name == "index") actPlace = Int32.Parse(reader.Value);
                        break;
                    case XmlNodeType.Text:
                        sudoku.SetSudokuField(actPlace, reader.Value);
                        break;
                }
            }
            reader.Close();
        }
                
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile.InitialDirectory = Directory.GetCurrentDirectory() + "\\Tables";
            saveFile.Filter = "Xml files (*.xml)|*.xml";
            if ((bool)saveFile.ShowDialog())
            {
                SaveToFile(saveFile.FileName);
            }
        }
        
        private void resetButton_Click(object sender, EventArgs e)
        {
            sudoku.ResetTable();
            backTrackNumber.Content = "0";
            leftButton.Visibility = Visibility.Hidden;
            rightButton.Visibility = Visibility.Hidden;
            actSolution.Visibility = Visibility.Hidden;
            solutionLabel.Visibility = Visibility.Hidden;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            sudoku.ClearTable();
            backTrackNumber.Content = "0";
            tableName.Text = "Click here to set name";
            infoLabel.Content = "Fill the table!";
            leftButton.Visibility = Visibility.Hidden;
            rightButton.Visibility = Visibility.Hidden;
            actSolution.Visibility = Visibility.Hidden;
            solutionLabel.Visibility = Visibility.Hidden;
        }

        private void startClick(object sender, EventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Start")
            {
                if ((bool)findAllSolutionCheckBox.IsChecked)
                {
                    solutionLabel.Visibility = Visibility.Visible;
                    leftButton.Visibility = Visibility.Visible;
                    rightButton.Visibility = Visibility.Visible;
                    actSolution.Visibility = Visibility.Visible;
                    actSolution.Text = "0";
                }
                else
                {
                    solutionLabel.Visibility = Visibility.Hidden;
                    leftButton.Visibility = Visibility.Hidden;
                    rightButton.Visibility = Visibility.Hidden;
                    actSolution.Visibility = Visibility.Hidden;
                }
                sudoku.isFlash = (bool)flashCheckedFields.IsChecked;
                pauseButton.Content = "Pause";
                sudoku.isPaused = false;
                pauseButton.Visibility = Visibility.Visible;
                (sender as Button).Content = "Stop";
                (sender as Button).ToolTip = "Stop solving";
                sudoku.processTime = flashCheckedFields.IsChecked;
                sudoku.ResetTable();
                sudoku.FieldsToNumbers();

                LockButtons();

                if (!sudoku.IsValidInput())
                {
                    infoLabel.Content = "Not a valid input";
                    (sender as Button).Content = "Start";
                    (sender as Button).ToolTip = "Start";
                    UnlockButtons();
                    sudoku.isStopped = true;
                    pauseButton.Visibility = Visibility.Hidden;
                    return;
                }


                runThread = new Thread(sudoku.Run);
                //sudoku.FieldsToNumbers();
                runThread.Start();
                Thread.Sleep(10);
                mainTimer.Start();
            }
            else
            {
                sudoku.isStopped = true;
            }
        }
        
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate()
            {
                infoLabel.Content = sudoku.GetTime();
                if (sudoku.isfindAllSolutionCheckBox)
                {
                    solutionLabel.Content = sudoku.getSolutionNumber();
                }
                backTrackNumber.Content = sudoku.backTrackNumber.ToString();
            });
            
            sudoku.GetSudokuFieldsToForm();
            int actField = sudoku.lastModField;
            Brush actFieldColor = sudoku.lastModFieldColor;
            if (lastModField >= 0 && sudoku.heuristic.state != State.FindSolution) sudoku.SetBackColor(lastModField, Brushes.White);
            if (actField >= 0) sudoku.SetBackColor(actField, actFieldColor);
            lastModField = actField;

            if (sudoku.state != State.Running && sudoku.state != State.GenerateRunning)
            {
                sudoku.GetSudokuFieldsToForm();
                mainTimer.Stop();
                UnlockButtons();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate()
                {
                    if (sudoku.state == State.Complete)
                    {
                        infoLabel.Content = "Finished: " + sudoku.finishTime;
                        if (sudoku.isfindAllSolutionCheckBox)
                        {
                            if (sudoku.solutionNumber > 0)
                            {
                                sudoku.SetSudokuTableFromSolution(sudoku.solutionsList[0]);
                                sudoku.GetSudokuFieldsToForm2(sudoku.solutionsList[0]);
                                actSolution.Text = "1";
                                lastSolNum = 1;
                            }
                            else
                            {
                                actSolution.Text = "0";
                            }
                        }
                    }
                    if (sudoku.state == State.NotValidInput) infoLabel.Content = "Not a valid input";
                    if (sudoku.state == State.Stopped)
                    {
                        infoLabel.Content = "Stopped";
                        if (sudoku.solutionNumber > 0)
                        {
                            sudoku.SetSudokuTableFromSolution(sudoku.solutionsList[0]);
                            sudoku.GetSudokuFieldsToForm2(sudoku.solutionsList[0]);
                            actSolution.Text = "1";
                            lastSolNum = 1;
                        }
                    }
                    if (sudoku.state == State.GenerateComplete || sudoku.state == State.GenerateStop) sudoku.ResetTable();
                    startButton.Content = "Start";
                    startButton.ToolTip = "Start solving";
                    pauseButton.Visibility = Visibility.Hidden;
                    SetArrowButtonsEnabled();
                });
                if (lastModField >= 0) sudoku.SetBackColor(lastModField, Brushes.White); 
            }
        }

        private void SetFieldsToWhite()
        {
            for (int i = 0; i < 81; i++)
            {
                if (!sudoku.isFixed[i]) sudoku.SetBackColor(i, Brushes.White);
            }
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (sudoku != null)
            {
                if ((bool)fromIndexRadio.IsChecked) sudoku.heur = HeruisticEnum.FromIndex;
                if ((bool)randRadio.IsChecked) sudoku.heur = HeruisticEnum.RandomIndex;
                if ((bool)bestRadio.IsChecked) sudoku.heur = HeruisticEnum.BestPos;
            }
        }

        public string GetFromIndexComboValue()
        {
            return fromIndexComboVal;
        }

        public bool GetFindAllSolutionCombo()
        {
            return findAllSol;
        }

        private void fromIndexCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            fromIndexComboVal = fromIndexCombo.SelectedValue.ToString();
        }

        private void speedBar_Scroll(object sender, EventArgs e)
        {
            sudoku.setWaitTime((int)speedBar.Value);
            string str = ((speedBar.Value) / 1000.0).ToString();
            byte length = (byte)(str.Length < 5 ? str.Length : 5 );
            stepLabel.Content = "Step: " + str.Substring(0,length) + "s";
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (runThread != null) runThread.Abort();
        }

        private void tableName_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (tableName.Text == "Click here to set name") tableName.Text = "";
            tableName.SelectionStart = 0;
            tableName.SelectionLength = tableName.Text.Length;
        }

        private void findAllSolClick(object sender, RoutedEventArgs e)
        {
            findAllSol = (bool)findAllSolutionCheckBox.IsChecked;
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            if (actSolution.Text.Length > 0)
            {
                if (int.Parse(actSolution.Text) - 1 > 0 && int.Parse(actSolution.Text) - 1 <= sudoku.solutionNumber)
                {
                    actSolution.Text = (int.Parse(actSolution.Text) - 1).ToString();
                    sudoku.SetSudokuTableFromSolution(sudoku.solutionsList[int.Parse(actSolution.Text) - 1]);
                    lastSolNum = int.Parse(actSolution.Text);
                }
                else
                {
                    actSolution.Text = lastSolNum.ToString();
                } 
                SetArrowButtonsEnabled();
            }
            else
            {
                actSolution.Text = lastSolNum.ToString();
            }
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            if (actSolution.Text.Length > 0)
            {
                if (int.Parse(actSolution.Text) + 1 <= sudoku.solutionNumber && int.Parse(actSolution.Text) + 1 > 0)
                {
                    actSolution.Text = (string)(int.Parse(actSolution.Text) + 1).ToString();
                    sudoku.SetSudokuTableFromSolution(sudoku.solutionsList[int.Parse(actSolution.Text) - 1]);
                    lastSolNum = int.Parse(actSolution.Text);
                }
                else
                {
                    actSolution.Text = lastSolNum.ToString();
                }
                SetArrowButtonsEnabled();
            }
            else
            {
                actSolution.Text = lastSolNum.ToString();
            }
        }


        private void solutionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && actSolution.Text.Length > 0)
            {
                if (int.Parse(actSolution.Text) > 0 && int.Parse(actSolution.Text) <= sudoku.solutionNumber)
                {
                    sudoku.SetSudokuTableFromSolution(sudoku.solutionsList[int.Parse(actSolution.Text) - 1]);
                    lastSolNum = int.Parse(actSolution.Text);
                    SetArrowButtonsEnabled();
                }
                else
                {
                    actSolution.Text = lastSolNum.ToString();
                }
            }
            if (e.Key == Key.Enter && actSolution.Text.Length == 0)
            {
                actSolution.Text = lastSolNum.ToString();
            }

            if (e.Key.ToString() == "D1" || e.Key.ToString() == "D2" || e.Key.ToString() == "D3" ||
                e.Key.ToString() == "D4" || e.Key.ToString() == "D5" || e.Key.ToString() == "D6" ||
                e.Key.ToString() == "D7" || e.Key.ToString() == "D8" || e.Key.ToString() == "D9" || e.Key.ToString() == "D0" ||
                e.Key.ToString() == "NumPad1" || e.Key.ToString() == "NumPad2" || e.Key.ToString() == "NumPad3" ||
                e.Key.ToString() == "NumPad4" || e.Key.ToString() == "NumPad5" || e.Key.ToString() == "NumPad6" ||
                e.Key.ToString() == "NumPad7" || e.Key.ToString() == "NumPad8" || e.Key.ToString() == "NumPad9" || e.Key.ToString() == "NumPad0" ||
                e.Key.ToString() == "Tab")
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void SetArrowButtonsEnabled()
        {
            leftButton.IsEnabled = false;
            rightButton.IsEnabled = false;
            actSolution.IsEnabled = false;
            if (sudoku.solutionNumber > 0)
            {
                actSolution.IsEnabled = true;
                if (int.Parse(actSolution.Text) > 1) leftButton.IsEnabled = true;
                if (int.Parse(actSolution.Text) < sudoku.solutionNumber) rightButton.IsEnabled = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Subtract && rightButton.Visibility == Visibility.Visible) && !tableName.IsFocused) leftButton_Click(this, null);
            if ((e.Key == Key.Add && rightButton.Visibility == Visibility.Visible) && !tableName.IsFocused) rightButton_Click(this, null);
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (pauseButton.Content.ToString() == "Pause")
            {
                pauseButton.Content = "Resume";
                sudoku.isPaused = true;
            }
            else
            {
                pauseButton.Content = "Pause";
                sudoku.isPaused = false;
                sudoku.lastTime = DateTime.Now;
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
                solutionLabel.Visibility = Visibility.Hidden;
                leftButton.Visibility = Visibility.Hidden;
                rightButton.Visibility = Visibility.Hidden;
                actSolution.Visibility = Visibility.Hidden;
               
                pauseButton.Content = "Pause";
                sudoku.isPaused = false;
                pauseButton.Visibility = Visibility.Visible;
                startButton.Content = "Stop";
                sudoku.ResetTable();
                LockButtons();

                sudoku.FieldsToNumbers();
                sudoku.GenerateTable(12, 2);
                LockButtons();
                runThread = new Thread(sudoku.RunGenerate);
                
                runThread.Start();
                Thread.Sleep(10);
                mainTimer.Start();
        }

        private void addFieldButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.AddGenerateField();
        }

        private void tablePlace_MouseEnter(object sender, MouseEventArgs e)
        {
            if (clearButton.IsEnabled == true) (sender as Grid).Cursor = cur;
            else (sender as Grid).Cursor = null;
        }

        private void tableName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) startButton.Focus();
        }

    }
}

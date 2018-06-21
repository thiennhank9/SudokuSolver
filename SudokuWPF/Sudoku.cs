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
using System.Threading;

namespace SudokuWPF
{
    public class SudokuC
    {
        Random rand;
        public SudokuField[] sudokuFields = new SudokuField[81];
        public bool[] isFixed = new bool[81];
        public Heruistic heuristic;
        public int backTrackNumber;
        public sudokuWindow parentWindow;
        public State state;
        public string[] numbers = new string[81];
        public string finishTime;
        public HeruisticEnum heur;
        public int wait = 0;
        /* Current positon. */
        public bool showPos = true;
        public int lastModField = -1;
        public Brush lastModFieldColor;
        public bool isStopped;
        public bool isfindAllSolutionCheckBox;
        public int solutionNumber;
        public List<Solution> solutionsList = new List<Solution>();
        public bool isPaused = false;
        public bool isFlash = true;
        public int maxWait = 2000;
        public int maxSolutonsNumber = 0;
        private Brush changeSolutionColor = Brushes.Orange;
        public bool processTime = true;

        public void GenerateTable(byte needFilledField, int _maxSolutonsNumber)
        {
            maxSolutonsNumber = _maxSolutonsNumber;
            for (int i = 0; i < 81; i++)
            {
                isFixed[i] = false;
                numbers[i] = "";
            }
            FieldsToNumbers();
            byte filledFields = 0;
            for (byte i = 0; i < 81; ++i) if (numbers[i] != "") filledFields++;
            for (byte i = filledFields; i < needFilledField; ++i) AddGenerateField();
        }

        public void AddGenerateField()
        {
            long runed = 0;
            long maxRun = 99999;
            Heruistic localHeur = new FromIndexHeuristic(this);
            bool isOk;
            int rn;
            do
            {
                isOk = false;
                rn = rand.Next(0, 81);
                if (numbers[rn] == "")
                {
                    int rn2 = rand.Next(1, 10);
                    numbers[rn] = rn2.ToString();
                    isOk = localHeur.UtkozesCsekk(rn);
                    if (!isOk) numbers[rn] = "";
                    else
                    {
                        isFixed[rn] = true;
                    }
                    //else SetBackColor(rn, Brushes.Silver);
                }
                runed++;
            } while (!isOk && runed < maxRun);
            if (runed >= maxRun) MessageBox.Show("Can't generate field!");
            GetSudokuFieldsToForm();
        }

        public void Flash(List<int> array, Brush color)
        {
            if (wait > 0 && isFlash)
            {
                foreach (byte b in array) if (!isFixed[b]) SetBackColor(b, color);
                Thread.Sleep(wait * 2);
                heuristic.Pause();
                foreach (byte b in array) if (!isFixed[b]) SetBackColor(b, Brushes.White);
                if (isStopped)
                {
                    heuristic.state = State.Stopped;
                }
            }
        }

        public bool IsValidInput()
        {
            heuristic = new BestPositionHeuristic(this);
            for (int i = 0; i < 81; i++)
            {
                if (numbers[i] != "" && !heuristic.UtkozesCsekk(i)) return false;
            }
            return true;
        }

        public void FindDifferenceTwoSolution(int a, int b)
        {
            List<byte> array = new List<byte>();
            array.Clear();
            for (byte i = 0; i < 81; i++) if (solutionsList[a].numbers[i] != solutionsList[b].numbers[i]) array.Add(i);
            string str = "";
            foreach (byte ba in array) str += ba.ToString() + "; ";
            int fixNumbersCount = array.Count / 4;
            while (fixNumbersCount > 0)
            {
                int fixNumberIndex = rand.Next(0, array.Count());
                isFixed[array[fixNumberIndex]] = true;
                fixNumbersCount--;
            }
        }

        public void findSolution()
        {
            if (wait > 0)
            {

                for (byte i = 0; i < 81; ++i) if (!isFixed[i]) SetBackColor(i, heuristic.forwardColor);
                if (wait * 50 > maxWait) Thread.Sleep(maxWait);
                else
                    Thread.Sleep(wait * 50);
                heuristic.Pause();
                for (byte i = 0; i < 81; ++i) if (!isFixed[i]) SetBackColor(i, Brushes.White);
            }
        }

        public void Set(int number, string text)
        {
            numbers[number] = text;
        }

        public void changeActiveSudokuField(int num)
        {
            sudokuFields[num].Focus();
        }

        public string GetFromIndexComboValue()
        {
            return parentWindow.GetFromIndexComboValue();
        }

        public void SetSudokuTableFromSolution(Solution sol)
        {
            LockTable();
            numbers = sol.numbers;
            GetSudokuFieldsToForm2(sol);
            UnlockTable();
        }

        public bool GetFromAllSolutionCheckbox()
        {
            return parentWindow.GetFindAllSolutionCombo();
        }

        public string Get(int number)
        {
            return numbers[number].ToString();
        }

        public string GetSudokuField(int number)
        {
            if (sudokuFields[number].Background == Brushes.Silver) return sudokuFields[number].Text;
            else return "";
        }

        public void SetSudokuField(int number, string value)
        {
            sudokuFields[number].Text = value.ToString();
        }

        public delegate void SetBackColorDelegate(int number, Brush color);

        public void SetBackColor(int number, Brush color)
        {
            if (!sudokuFields[number].Dispatcher.CheckAccess())
            {
                sudokuFields[number].Dispatcher.BeginInvoke(new SetBackColorDelegate(SetBackColor), new object[] { number, color });
                return;

            }
            sudokuFields[number].Background = color;
        }

        public SudokuC(sudokuWindow _parentWindow)
        {
            for (int i = 0; i < 81; i++)
            {
                isFixed[i] = false;
                numbers[i] = "";
            }
            rand = new Random();
            heur = HeruisticEnum.FromIndex;
            parentWindow = _parentWindow;
            state = State.Running;
            const int marginX = -210;
            const int marginY = -284;
            const int numberDistance = 50;
            const int blockDistance = 10;
            int three = 0;
            int lthree = 0;
            int nine = 0;
            int x = marginX;
            int y = marginY;

            // ---------------------------------------------------
            // ------------   TÁBLA RAJZOLÁS ---------------------
            // ---------------------------------------------------

            for (byte i = 0; i < 81; ++i)
            {
                if (three == 3)
                {
                    x = x + blockDistance;
                    three = 0;
                }
                if (nine == 9)
                {
                    y = y + numberDistance;
                    nine = 0;
                    x = marginX;
                    ++lthree;
                }
                if (lthree == 3)
                {
                    y = y + blockDistance;
                    lthree = 0;
                }
                sudokuFields[i] = new SudokuField(x, y, i, this);
                parentWindow.tablePlace.Children.Add(sudokuFields[i]);
                ++three;
                ++nine;
                x = x + numberDistance;
            }

        }

        public void LockTable()
        {
            foreach (SudokuField sf in sudokuFields) sf.Lock();
        }

        public void UnlockTable()
        {
            foreach (SudokuField sf in sudokuFields) sf.Unlock();
        }

        public void ResetTable()
        {
            for (int i = 0; i < 81; i++)
            {
                if (!isFixed[i]) numbers[i] = "";
            }
            foreach (SudokuField sf in sudokuFields) sf.ResetField();
        }

        public void ClearTable()
        {
            for (int i = 0; i < 81; i++)
            {
                isFixed[i] = false;
                numbers[i] = "";
            }
            foreach (SudokuField sf in sudokuFields) sf.ClearField();
        }

        public void FieldsToNumbers()
        {
            for (int i = 0; i < 81; i++)
            {
                isFixed[i] = false;
                numbers[i] = "";
                if (sudokuFields[i].Background == Brushes.Silver)
                {
                    numbers[i] = sudokuFields[i].Text;
                    isFixed[i] = true;
                }
            }
        }

        public void RunGenerate()
        {
            solutionNumber = maxSolutonsNumber + 1;
            state = State.GenerateRunning;
            startTimer();
            while (solutionNumber > maxSolutonsNumber && state != State.NotValidInput)
            {
                solutionsList.Clear();
                for (int i = 0; i < 81; i++) if (!isFixed[i]) numbers[i] = "";
                isPaused = false;
                isStopped = false;
                solutionNumber = 0;
                state = State.GenerateRunning;
                switch (heur)
                {
                    case HeruisticEnum.FromIndex: heuristic = new FromIndexHeuristic(this); break;
                    case HeruisticEnum.RandomIndex: heuristic = new RandomIndexHeuristic(this); break;
                    case HeruisticEnum.BestPos: heuristic = new BestPositionHeuristic(this); break;
                }
                setWaitTime(wait);
                solutionNumber = 0;
                state = heuristic.Run();
            }
            finishTime = GetTime();
            int ff = 0;
            for (int i = 0; i < 81; i++) if (!isFixed[i]) { numbers[i] = ""; ff++; }
            if (heuristic.state != State.GenerateStop) MessageBox.Show("Filled fields: " + (81 - ff).ToString() + Environment.NewLine + "Solutions: " + solutionNumber.ToString(), "Table generated ");
        }

        public void Run()
        {
            isPaused = false;
            solutionsList.Clear();
            isStopped = false;
            solutionNumber = 0;
            state = State.Running;
            startTimer();
            switch (heur)
            {
                case HeruisticEnum.FromIndex: heuristic = new FromIndexHeuristic(this); break;
                case HeruisticEnum.RandomIndex: heuristic = new RandomIndexHeuristic(this); break;
                case HeruisticEnum.BestPos: heuristic = new BestPositionHeuristic(this); break;
            }
            setWaitTime(wait);
            state = heuristic.Run();
            finishTime = GetTime();
        }

        // ---------------------------------------------------
        // ------------        TIMER     ---------------------
        // ---------------------------------------------------
        private DateTime startDate;
        private TimeSpan runTime;
        public DateTime lastTime;

        private void startTimer()
        {
            startDate = DateTime.Now;
            runTime = TimeSpan.Zero;
            lastTime = DateTime.Now;
        }

        public string getSolutionNumber()
        {
            if (isfindAllSolutionCheckBox) return "/" + solutionNumber.ToString();
            else return "";
        }

        public string GetTime()
        {
            string TimeInString = "";
            if (!processTime)
            {
                if (!isPaused) runTime += DateTime.Now - lastTime;
            }
            else
            {
                if (!isPaused) runTime += heuristic.cycleTime;
                heuristic.cycleTime = TimeSpan.Zero;
            }

            int hour = runTime.Hours;
            int min = runTime.Minutes;
            int sec = runTime.Seconds;
            int msec = runTime.Milliseconds;

            TimeInString += (hour < 10) ? "0" + hour.ToString() : hour.ToString();
            TimeInString += ":" + ((min < 10) ? "0" + min.ToString() : min.ToString());
            TimeInString += ":" + ((sec < 10) ? "0" + sec.ToString() : sec.ToString());
            //TimeInString += "." + ((msec < 10) ? "0" + msec.ToString() : msec.ToString());
            //if (msec < 10000) TimeInString += "." + "0000" + msec.ToString();
            /*if (msec < 1000) TimeInString += "." + "000" + msec.ToString();
            else if (msec < 100) TimeInString += "." + "00" + msec.ToString();
            else if (msec < 10) TimeInString += "." + "0" + msec.ToString();
            else*/
            //if (msec < 10 && msec < 100) TimeInString += "." + "0" + msec.ToString();
            //if (msec < 10) TimeInString += "." + "00" + msec.ToString();
            if (100 <= msec && msec < 1000) TimeInString += "." + msec.ToString();
            else if (10 <= msec && msec < 100) TimeInString += "." + "0" + msec.ToString();
            else if (0 <= msec && msec < 10) TimeInString += "." + "00" + msec.ToString();
            //TimeInString += "." + msec.ToString();

            lastTime = DateTime.Now;
            return TimeInString;
        }

        public void GetSudokuFieldsToForm()
        {
            parentWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
            {
                for (int i = 0; i < 81; i++)
                {
                    if (numbers[i] != "10") sudokuFields[i].Text = numbers[i];
                    if (sudokuFields[i].Background == Brushes.White && isFixed[i]) SetBackColor(i, Brushes.Silver);
                }
                parentWindow.backTrackNumber.Content = backTrackNumber.ToString();
            });
        }

        public void GetSudokuFieldsToForm2(Solution sol)
        {
            for (int i = 0; i < 81; i++)
            {
                if (sol.isFixed[i]) sudokuFields[i].Background = Brushes.Silver;
                else
                {
                    if (sudokuFields[i].Text != numbers[i].ToString()) sudokuFields[i].Background = changeSolutionColor;//Brushes.Orange; //Brushes.LightGoldenrodYellow; //Brushes.Yellow;
                    else
                        sudokuFields[i].Background = Brushes.White;
                }
                sudokuFields[i].Text = numbers[i];
            }
            parentWindow.backTrackNumber.Content = backTrackNumber.ToString();
        }

        public void setWaitTime(int time)
        {
            wait = time;
            if (heuristic != null) heuristic.setWait(wait);
        }

    }
}

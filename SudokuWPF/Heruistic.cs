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
    /* Megoldás állapota. */
    public enum State { NotValidInput, Complete, Running, Stopped, FindSolution, GenerateRunning, Find2Solutoins, GenerateComplete, GenerateStop };
    /* Heurisztikák. */
    public enum HeruisticEnum { FromIndex, RandomIndex, BestPos };
    /* Absztakt heurisztika osztály. */
    public abstract class Heruistic
    {
        /* Futtatás. */
        public abstract State Run();
        /* Irány enum. */
        protected enum Way { Right, Left };
        /* Utolsó módosított mező. */
        protected int lastModField;
        /* Aktuális mező. */
        protected int actfield;
        /* Irány. */
        protected Way way;
        /* Állapot. */
        public State state;
        /* Sudoku tábla. */
        protected SudokuC sudokuTable;
        /* Index tömb. */
        protected int[] Index = new int[81];
        /* Várakozás. */
        protected int wait;
        /* Ciklus idő. */
        public TimeSpan cycleTime;
        /* Utolsó ciklus végének időpontja. */
        public DateTime lastCycleEndTime;
        /* Várakozás beállítása */
        public void setWait(int time) { wait = time; }
        /* Előrelépés színe. */
        public Brush forwardColor = Brushes.LightGreen;
        /* Visszalépés színe. */
        public Brush backColor = Brushes.Red;
        /* Szabályokat nem sértő szám találásának jelzése. */
        public Brush flashOk = Brushes.YellowGreen;
        /* Szabályt sértő szám találásának jelzése. */
        public Brush flashNotOk = Brushes.PaleVioletRed;

        /* Pause. */      
        public void Pause()
        {
            while (sudokuTable.isPaused && !sudokuTable.isStopped) { Thread.Sleep(1); lastCycleEndTime = DateTime.Now; }
        }

        /* Megoldás elmentése. */
        public void SaveSolution()
        {
            Solution newSol = new Solution(sudokuTable.numbers, sudokuTable.isFixed, sudokuTable.solutionNumber);
            sudokuTable.solutionsList.Add(newSol);
        }

        /* Egy mezőre megvizsgálja, hogy a tartalma szabályos-e. */
        public bool UtkozesCsekk(int szam)
        {
            if (SorCsekk(szam) && OszlopCsekk(szam) && DobozCsekk(szam)) return true;
            else return false;
        }

        /* Egy mezőre megvizsgálja, hogy a tartalma megfelel-e az adott sornak. */
        public bool SorCsekk(int szam)
        {
            int elsoelem = (szam / 9) * 9;
            int utolsoelem = elsoelem + 8;
            bool ok = true;
            List<int> array = new List<int>();
            for (int i = elsoelem; i <= utolsoelem; i++)
            {
                array.Add(i);
                if (i != szam)
                {
                    if (sudokuTable.Get(i) == sudokuTable.Get(szam)) ok = false;
                }
            }
            if (ok) sudokuTable.Flash(array, flashOk);
            else sudokuTable.Flash(array, flashNotOk);
            return ok;
        }

        /* Egy mezőre megvizsgálja, hogy a tartalma megfelel-e az adott oszlopnak. */
        public bool OszlopCsekk(int szam)
        {
            int elsoelem = (szam % 9);
            int utolsoelem = elsoelem + 72;
            bool ok = true;
            List<int> array = new List<int>(); 
            for (int i = elsoelem; i <= utolsoelem; i = i + 9)
            {
                array.Add(i); 
                if (i != szam)
                {
                    if (sudokuTable.Get(i) == sudokuTable.Get(szam)) ok = false;
                }
            }
            if (ok) sudokuTable.Flash(array, flashOk);
            else sudokuTable.Flash(array, flashNotOk); return ok;
        }

        /* Egy mezőre megvizsgálja, hogy a tartalma megfelel-e az adott négyzetnek. */
        public bool DobozCsekk(int szam)
        {
            bool ok = true;
            int FSarok = szam / 9 / 3 * 3 * 9 + (szam % 9) / 3 * 3;
            List<int> array = new List<int>(); 
            for (int j = 0; j <= 2; ++j)
                for (int i = 0; i <= 2; ++i)
                {
                    array.Add(FSarok + (j * 9) + i); 
                    if (FSarok + (j * 9) + i != szam)
                    {
                        if (sudokuTable.Get(FSarok + (j * 9) + i) == sudokuTable.Get(szam)) ok = false;
                    }
                }
            if (ok) sudokuTable.Flash(array, flashOk);
            else sudokuTable.Flash(array, flashNotOk); return ok;
        }
    }

    /* Legjobb poziciót kihasználó huerisztika. */
    public class BestPositionHeuristic : Heruistic
    {
        /* Azokat a számok melyek még megoldások lehetnek egy adott helyre. */
        public HashSet<int> goodNumbers = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /* Konstruktor. */
        public BestPositionHeuristic(SudokuC _sudokuTable)
        {
            sudokuTable = _sudokuTable;
            state = sudokuTable.state;
        }

        /* Feladvány megoldásának indítása. */
        public override State Run()
        {
            // ---------------------------------------------------
            // ------------   DEKLARÁCIÓK    ---------------------
            // ---------------------------------------------------
            int[] heurnum = new int[81];
            int[] backTrackNum = new int[81];
            sudokuTable.isfindAllSolutionCheckBox = sudokuTable.GetFromAllSolutionCheckbox();
            cycleTime = TimeSpan.Zero;
            // ---------------------------------------------------
            // ------------   DEFAULT ÉRTÉKEK --------------------
            // ---------------------------------------------------
            state = sudokuTable.state; //State.Running;
            lastModField = -1;
            sudokuTable.lastModField = lastModField;
            sudokuTable.backTrackNumber = 0;
            actfield = 0;
            way = Way.Right;
            int lastbfi, bfi = 0;

            while (state == State.Running || state == State.GenerateRunning)
            {
                lastCycleEndTime = DateTime.Now;
                // ------------  PAUSE  -------------------
                Pause(); 
                Thread.Sleep(wait);
                lastbfi = bfi;
                bfi = getBestField();
                // ------------  SZABÁLYTALAN INPUT  -------------------
                if (actfield < 0)
                {
                    if (sudokuTable.isfindAllSolutionCheckBox && state == State.Running)
                    {
                        state = State.Complete;
                    }
                    else if (state == State.GenerateRunning)
                    {
                        if (sudokuTable.solutionNumber == 0) state = State.NotValidInput;
                        else state = State.GenerateComplete;
                    }
                    else if (state == State.Running)
                    {
                        state = State.NotValidInput;
                    }
                    return state;
                }
                // ------------  VÉGEZTÜNK  -------------------
                if (bfi > 80)
                {
                    if (sudokuTable.isfindAllSolutionCheckBox && state == State.Running)
                    {
                        state = State.FindSolution;
                        sudokuTable.solutionNumber++;
                        sudokuTable.findSolution();
                        SaveSolution();
                        actfield--;
                        bfi = Index[actfield];
                        way = Way.Left;
                        state = State.Running;
                    }
                    else if (state == State.GenerateRunning)
                    {
                        sudokuTable.solutionNumber++;
                        SaveSolution();
                        actfield--;
                        bfi = Index[actfield];
                        way = Way.Left;
                        if (sudokuTable.solutionNumber > sudokuTable.maxSolutonsNumber)
                        {
                            sudokuTable.FindDifferenceTwoSolution(0, 1);
                            state = State.Find2Solutoins;
                            return state;
                        }
                    }
                    else if (state == State.Running)
                    {
                        state = State.Complete;
                        return state;
                    }
                }
                // ------------  LEÁLLÍTVA  -------------------
                if (sudokuTable.isStopped)
                {
                    if (state == State.GenerateRunning) state = State.GenerateStop;
                    if (state == State.Running) state = State.Stopped;
                    return state;
                }
                if (way == Way.Right)
                {
                    Index[actfield] = bfi;
                    setGoodNumbersSet(Index[actfield]);

                    if (fillField(goodNumbers, Index[actfield], backTrackNum[actfield]))
                    {
                        lastModField = Index[actfield];
                        actfield++;
                    }
                    else
                    {
                        backTrackNum[actfield] = 0;
                        sudokuTable.Set(Index[actfield], "");
                        lastModField = Index[actfield];
                        way = Way.Left;
                        actfield--;
                    }
                }
                else
                {
                    // BACKTRACK
                    sudokuTable.backTrackNumber++;
                    backTrackNum[actfield]++;
                    setGoodNumbersSet(Index[actfield]);
                    if (fillField(goodNumbers, Index[actfield], backTrackNum[actfield]))
                    {
                        way = Way.Right;
                        lastModField = Index[actfield];
                        actfield++;
                    }
                    else
                    {
                        backTrackNum[actfield] = 0;
                        sudokuTable.Set(Index[actfield], "");
                        lastModField = Index[actfield];
                        actfield--;
                    }
                }
                sudokuTable.lastModField = lastModField;
                if (way == Way.Right) sudokuTable.lastModFieldColor = forwardColor;
                else sudokuTable.lastModFieldColor = backColor;
                // CYCLE TIME REFRESH
                cycleTime += DateTime.Now - lastCycleEndTime - TimeSpan.FromMilliseconds(wait);
            }
            return State.Complete;
        }

        /* Legkevesebb számmal kitölthető mező megkeresése. */
        private int getBestField()
        {
            int bestFieldIndex = 81;
            int bestValue = 10;
            for (int i = 0; i < 81; ++i)
            {
                if (!sudokuTable.isFixed[i] && sudokuTable.Get(i) == "")
                {
                    setGoodNumbersSet(i);
                    if (goodNumbers.Count < bestValue)
                    {
                        bestValue = goodNumbers.Count;
                        bestFieldIndex = i;
                    }
                }
            }
            return bestFieldIndex;
        }

        /* Lehetségesk számok tömbjéből a kizárt számok eltávolítása. */
        private void setGoodNumbersSet(int i)
        {
            goodNumbers = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            goodNumbers = getSorHeur(goodNumbers, i);
            goodNumbers = getOszolopHeur(goodNumbers, i);
            goodNumbers = getDobozHeur(goodNumbers, i);
        }

        /* Mező kitöltése értékkel. */
        private bool fillField(HashSet<int> goodNumbers, int szam, int backTrackNum)
        {
            bool isOk = false;
            int aktNum = 0;
            foreach (int numb in goodNumbers)
            {
                if (aktNum == backTrackNum)
                {
                    sudokuTable.Set(szam, numb.ToString());
                    isOk = true;
                }
                aktNum++;
            }
            return isOk;
        }


        /* GoodNumbers tömbből eltávolítja azokat a számokat amik az adott sor miatt kizáródnak. */
        private HashSet<int> getSorHeur(HashSet<int> goodNumbers, int szam)
        {
            int elsoelem = (szam / 9) * 9;
            int utolsoelem = elsoelem + 8;
            for (int i = elsoelem; i <= utolsoelem; i++)
            {
                if (i != szam)
                {
                    if (sudokuTable.Get(i) != "")
                        if (goodNumbers.Contains(Convert.ToInt32(sudokuTable.Get(i)))) goodNumbers.Remove(Convert.ToInt32(sudokuTable.Get(i)));
                }
            }
            return goodNumbers;
        }

        /* GoodNumbers tömbből eltávolítja azokat a számokat amik az adott oszlop miatt kizáródnak. */
        private HashSet<int> getOszolopHeur(HashSet<int> goodNumbers, int szam)
        {
            int elsoelem = (szam % 9);
            int utolsoelem = elsoelem + 72;
            for (int i = elsoelem; i <= utolsoelem; i = i + 9)
            {
                if (i != szam)
                {
                    if (sudokuTable.Get(i) != "")
                        if (goodNumbers.Contains(Convert.ToInt32(sudokuTable.Get(i)))) goodNumbers.Remove(Convert.ToInt32(sudokuTable.Get(i)));
                }
            }
            return goodNumbers;
        }

        /* GoodNumbers tömbből eltávolítja azokat a számokat amik az adott doboz miatt kizáródnak. */
        private HashSet<int> getDobozHeur(HashSet<int> goodNumbers, int szam)
        {
            int FSarok = szam / 9 / 3 * 3 * 9 + (szam % 9) / 3 * 3;
            for (int j = 0; j <= 2; ++j)
                for (int i = 0; i <= 2; ++i)
                {
                    if (FSarok + (j * 9) + i != szam)
                    {
                        if (sudokuTable.Get(FSarok + (j * 9) + i) != "")
                            if (goodNumbers.Contains(Convert.ToInt32(sudokuTable.Get(FSarok + (j * 9) + i)))) goodNumbers.Remove(Convert.ToInt32(sudokuTable.Get(FSarok + (j * 9) + i)));
                    }
                }
            return goodNumbers;
        }

    }

    /* Sorfolytonos heurisztika. */
    public class FromIndexHeuristic : Heruistic
    {

        /* Konstrukor. */
        public FromIndexHeuristic(SudokuC _sudokuTable)
        {
            sudokuTable = _sudokuTable;
            state = sudokuTable.state; //State.Running;
        }

        /* Kezdeti index beállítása. */
        public virtual void SetIndex()
        {
            for (int i = 0; i < 81; ++i) Index[i] = (i + int.Parse(sudokuTable.GetFromIndexComboValue())) % 81;
        }

        /* Feladvány megoldásának indítása. */
        public override State Run()
        {
            // ---------------------------------------------------
            // ------------   DEFAULT ÉRTÉKEK --------------------
            // ---------------------------------------------------
            cycleTime = TimeSpan.Zero;
            state = sudokuTable.state;
            lastModField = -1;
            sudokuTable.lastModField = lastModField;
            sudokuTable.backTrackNumber = 0;
            actfield = 0;
            way = Way.Right;
            SetIndex();
            sudokuTable.isfindAllSolutionCheckBox = sudokuTable.GetFromAllSolutionCheckbox();
            
            while (state == State.Running || state == State.GenerateRunning)
            {
                lastCycleEndTime = DateTime.Now;
                // ------------  PAUSE  -------------------
                Pause(); 
                Thread.Sleep(wait);
                // ------------  SZABÁLYTALAN INPUT  -------------------
                if (actfield < 0)
                {
                    if (sudokuTable.isfindAllSolutionCheckBox && state == State.Running)
                    {
                        state = State.Complete;
                    }
                    else if (state == State.GenerateRunning)
                    {
                        if (sudokuTable.solutionNumber == 0) state = State.NotValidInput;
                        else state = State.GenerateComplete;
                    }
                    else if (state == State.Running)
                    {
                        state = State.NotValidInput;
                    }
                    return state;
                }
                // ------------  VÉGEZTÜNK  -------------------
                if (actfield > 80)
                {
                    if (sudokuTable.isfindAllSolutionCheckBox && state == State.Running)
                    {
                        state = State.FindSolution;
                        sudokuTable.solutionNumber++;
                        sudokuTable.findSolution();
                        SaveSolution();
                        actfield = 80;
                        way = Way.Left;
                        state = State.Running;
                    }
                    else if (state == State.GenerateRunning)
                    {
                        sudokuTable.solutionNumber++;
                        SaveSolution();
                        actfield = 80;
                        way = Way.Left;
                        if (sudokuTable.solutionNumber > sudokuTable.maxSolutonsNumber)
                        {
                            sudokuTable.FindDifferenceTwoSolution(0, 1);
                            state = State.Find2Solutoins;
                            return state;
                        }
                    }
                    else if (state == State.Running)
                    {
                        state = State.Complete;
                        return state;
                    }
                }
                // ------------  LEÁLLÍTVA  -------------------
                if (sudokuTable.isStopped)
                {
                    if (state == State.GenerateRunning) state = State.GenerateStop;
                    if (state == State.Running) state = State.Stopped;
                    return state;
                }
                if (!sudokuTable.isFixed[Index[actfield]])
                {
                    if (sudokuTable.Get(Index[actfield]) == "") sudokuTable.Set(Index[actfield], "0");
                    sudokuTable.Set(Index[actfield], Convert.ToString(Convert.ToInt32(sudokuTable.Get(Index[actfield]) )+1));  
                    while (!UtkozesCsekk(Index[actfield]) && (Convert.ToInt32(sudokuTable.Get(Index[actfield])) < 10))
                    {
                        sudokuTable.Set(Index[actfield], Convert.ToString(Convert.ToInt32(sudokuTable.Get(Index[actfield]) )+1)); 
                    }

                    if (Convert.ToInt32(sudokuTable.Get(Index[actfield])) == 10) 
                    {
                        sudokuTable.Set(Index[actfield], "");
                        lastModField = Index[actfield];
                        actfield--;
                        sudokuTable.backTrackNumber++;
                        way = Way.Left;
                    }
                    else
                    {
                        lastModField = Index[actfield];
                        actfield++;
                        way = Way.Right;
                    }
                }
                else
                {
                    if (way == Way.Right) actfield++;
                    else actfield--;
                }
             
                sudokuTable.lastModField = lastModField;
                if (way == Way.Right) sudokuTable.lastModFieldColor = forwardColor;
                else sudokuTable.lastModFieldColor = backColor;
                // CYCLE TIME REFRESH
                cycleTime += DateTime.Now - lastCycleEndTime - TimeSpan.FromMilliseconds(wait);
            }
            return State.Complete;
        }
    }

    /* Véletlen indexű heurisztika. */
    public class RandomIndexHeuristic : FromIndexHeuristic
    {
        /* Konstruktor. */
        public RandomIndexHeuristic(SudokuC _sudokuTable) : base(_sudokuTable) { }

        /* Véletlen indexek beállítása. */
        public override void SetIndex()
        {
            int rn;
            Random r = new Random();

            for (int i = 0; i < 81; ++i) Index[i] = 81;
            for (int i = 0; i < 81; ++i)
            {
                rn = r.Next(0, 80);
                int tav = 0;
                while (Index[(rn + tav) % 81] != 81) tav = tav + 1;
                Index[(rn + tav) % 81] = i;
            }
        }

    }

}

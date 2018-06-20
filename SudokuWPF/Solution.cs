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
    public class Solution
    {
        public string[] numbers = new string[81];
        public bool[] isFixed = new bool[81];
        public int solNumber;

        public Solution(string[] _numbers, bool[] _isFixed, int _solNumber)
        {
            numbers = (string[])_numbers.Clone();
            isFixed = (bool[])_isFixed.Clone();
            solNumber = _solNumber;
        }
    }
}

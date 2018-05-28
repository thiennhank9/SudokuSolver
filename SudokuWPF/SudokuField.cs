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

namespace SudokuWPF
{
    /* Sudoku mező. */
    public class SudokuField : System.Windows.Controls.TextBox 
    {
        /* Mező indexe. */
        private int myIndex;
        /* Szülő objektum. */
        private SudokuC parent;
        /* Lezárt mező? */
        private bool isLocked = false;

        /* Mező lezárása. */
        public void Lock()
        {
            TextChanged -= SetFieldBackColor;
            isLocked = true;
            Focusable = false;
        }

        /* Mező feloldása. */
        public void Unlock()
        {
            TextChanged += SetFieldBackColor;
            isLocked = false;
            Focusable = true;
        }

        /* Konstruktor. */
        public SudokuField(int xPos, int yPos, int _index, SudokuC _parent)
        {
            parent = _parent;
            myIndex = _index;
            Margin = new System.Windows.Thickness(xPos, yPos, 10, 10); 
            TextAlignment = System.Windows.TextAlignment.Center;
            Width = 25;
            Height = 25;
            MaxLength = 1;
            FontFamily = new System.Windows.Media.FontFamily("Kristen ITC, Arial"); 
            FontSize = 14;
            VerticalContentAlignment = VerticalAlignment.Center;
            TextAlignment = TextAlignment.Center;
            Background = Brushes.White;
            TextChanged += SetFieldBackColor;
            PreviewKeyDown += SudokuFieldPKeyDown;
            MouseEnter += mouseEnterSudokuField;
        }

        /* Fókusz elvesztése. */
        void SudokuField_LostFocus(object sender, RoutedEventArgs e)
        {
            BorderBrush = Brushes.Green; 
        }

        /* Fókusz megkapása. */
        void SudokuField_GotFocus(object sender, RoutedEventArgs e)
        {
            BorderBrush = Brushes.Red;
        }

        /* Billentyű nyomás kezelése. */
        private void SudokuFieldPKeyDown(Object sender, KeyEventArgs e)
        {
            int newIndex;
            if (e.Key == Key.Up)
            {
                newIndex = myIndex - 9;
                if (newIndex < 0) newIndex = 81 + newIndex;
                parent.changeActiveSudokuField(newIndex);
            }
            if (e.Key == Key.Left)
            {
                newIndex = myIndex - 1;
                if (myIndex % 9 == 0) newIndex = myIndex + 8;
                parent.changeActiveSudokuField(newIndex);
            }
            if (e.Key == Key.Down)
            {
                newIndex = myIndex + 9;
                if (newIndex > 80) newIndex = newIndex - 81;
                parent.changeActiveSudokuField(newIndex);
            }
            if (e.Key == Key.Right)
            {
                newIndex = myIndex + 1;
                if (newIndex % 9 == 0) newIndex = myIndex - 8;
                parent.changeActiveSudokuField(newIndex);
            }

            if (e.Key.ToString() == "D1" || e.Key.ToString() == "D2" || e.Key.ToString() == "D3" ||
                e.Key.ToString() == "D4" || e.Key.ToString() == "D5" || e.Key.ToString() == "D6" ||
                e.Key.ToString() == "D7" || e.Key.ToString() == "D8" || e.Key.ToString() == "D9" ||
                e.Key.ToString() == "NumPad1" || e.Key.ToString() == "NumPad2" || e.Key.ToString() == "NumPad3" ||
                e.Key.ToString() == "NumPad4" || e.Key.ToString() == "NumPad5" || e.Key.ToString() == "NumPad6" ||
                e.Key.ToString() == "NumPad7" || e.Key.ToString() == "NumPad8" || e.Key.ToString() == "NumPad9" ||
                e.Key.ToString() == "Tab" || e.Key == Key.Delete || e.Key == Key.Back)
            {
                Text = "";
                e.Handled = false;
            }
            else e.Handled = true;
        }

        /* Mező háttérszínének beállítása. */
        private void SetFieldBackColor(object sender, EventArgs e)
        {
            (sender as TextBox).Background = Brushes.White;
            if ((sender as TextBox).Text.Length != 0)(sender as TextBox).Background = Brushes.Silver;  
        }

        /* Mező törlése. */
        public void ClearField()
        {
            Text = "";
        }

        /* Mező törlése, ha nem fixált. */
        public void ResetField()
        {
            if (Background != Brushes.Silver) Text = "";
        }

        /* Kurzor lecserélése. */
        private void mouseEnterSudokuField(object sender, MouseEventArgs e)
        {
            if (!isLocked) Cursor = parent.parentWindow.cur;
            else Cursor = Cursors.Arrow;
        }
    }
}

﻿#pragma checksum "..\..\sudokuWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "05B80D49650569033A3B8D7800343604"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SudokuWPF {
    
    
    /// <summary>
    /// sudokuWindow
    /// </summary>
    public partial class sudokuWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\sudokuWindow.xaml"
        internal System.Windows.Shapes.Rectangle rectangle1;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\sudokuWindow.xaml"
        internal System.Windows.Shapes.Rectangle rectangle2;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\sudokuWindow.xaml"
        internal System.Windows.Shapes.Rectangle rectangle3;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Button startButton;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Button clearButton;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Button resetButton;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Button exitButton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label infoLabel;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.RadioButton fromIndexRadio;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.RadioButton randRadio;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.RadioButton bestRadio;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.ComboBox fromIndexCombo;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label label2;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label backTrackLabel;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Menu menu1;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.MenuItem mainMenu;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label backTrackNumber;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Grid tablePlace;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Slider speedBar;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\sudokuWindow.xaml"
        internal System.Windows.Controls.Label stepLabel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SudokuWPF;component/sudokuwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\sudokuWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.rectangle1 = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 2:
            this.rectangle2 = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.rectangle3 = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 4:
            this.startButton = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\sudokuWindow.xaml"
            this.startButton.Click += new System.Windows.RoutedEventHandler(this.startClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.clearButton = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\sudokuWindow.xaml"
            this.clearButton.Click += new System.Windows.RoutedEventHandler(this.clearButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.resetButton = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\sudokuWindow.xaml"
            this.resetButton.Click += new System.Windows.RoutedEventHandler(this.resetButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.exitButton = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\sudokuWindow.xaml"
            this.exitButton.Click += new System.Windows.RoutedEventHandler(this.exitButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.infoLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.fromIndexRadio = ((System.Windows.Controls.RadioButton)(target));
            
            #line 19 "..\..\sudokuWindow.xaml"
            this.fromIndexRadio.Checked += new System.Windows.RoutedEventHandler(this.CheckedChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.randRadio = ((System.Windows.Controls.RadioButton)(target));
            
            #line 20 "..\..\sudokuWindow.xaml"
            this.randRadio.Checked += new System.Windows.RoutedEventHandler(this.CheckedChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.bestRadio = ((System.Windows.Controls.RadioButton)(target));
            
            #line 21 "..\..\sudokuWindow.xaml"
            this.bestRadio.Checked += new System.Windows.RoutedEventHandler(this.CheckedChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            this.fromIndexCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\sudokuWindow.xaml"
            this.fromIndexCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.fromIndexCombo_SelectedIndexChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.label2 = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            this.backTrackLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 16:
            this.menu1 = ((System.Windows.Controls.Menu)(target));
            return;
            case 17:
            this.mainMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 26 "..\..\sudokuWindow.xaml"
            this.mainMenu.Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 29 "..\..\sudokuWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.exitButton_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.backTrackNumber = ((System.Windows.Controls.Label)(target));
            return;
            case 20:
            this.tablePlace = ((System.Windows.Controls.Grid)(target));
            return;
            case 21:
            this.speedBar = ((System.Windows.Controls.Slider)(target));
            
            #line 34 "..\..\sudokuWindow.xaml"
            this.speedBar.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.speedBar_Scroll);
            
            #line default
            #line hidden
            return;
            case 22:
            this.stepLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

   M S B A M L   `   `   `  ���� 
  Sudoku    SudokuWPF.sudokuWindow   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    -     � Title$  Sudoku solver��$	��374��$	��678�� � 
ResizeMode$ NoResize=�$y�Visible=�.��� $	��658��$	��350�� � ColumnDefinitions �� $
��327* ��� $
��331* �.+��� -   $��
rectangle6��
�������$��2��$��309,-78,-40,-40q��� -   $��
rectangle2��
����·�$��309,85,-31,0q�$��18��$	��Top=�$��2���� -   $��
rectangle3��
����·�$��18��$��309,0,-31,122q�$��Bottom=�$��2���� -   $��startButton��$��3.738,46,0,0q�$��1��$��28��$	��Top=�$
��Bold"�$
��Left=�$��77.262��$y�Visible=� � ToolTip$ Start sloving��.��Start�� -   $��clearButton��$��28��$��	87,46,0,0q�$	��Top=�$��1��$
��Bold"�$
��Left=�$��75��$ Clear table��.��Clear�� -   $��resetButton��$��28��$��Right=�$��	0,46,88,0q�$	��Top=�$��75��$��1��$
��Bold"�$ Reset table��.��Reset�� -   $��
exitButton��$��28��$��Right=�$��0,46,9,0q�$	��Top=�$��75��$��1��$
��Bold"�.��Exit�� -	   $��	infoLabel��$��1��$��35��$��	88,7,40,0q�$	��Top=�$��16(�$
��Bold"�.��Fill the table!�� -
   $��label1��$��16(�$
��Bold"�$��31��$��
13,98,20,0q�$	��Top=�$��1��.��Heuristic type:	� -   $��fromIndexRadio��$��1��$��16��$��
42,137,0,0q�$	��Top=�$��12(�$
��Bold"� d� 	IsChecked$
 TrueY�$
��Left=�$	��120��$Q KChoose where to start from and then go from left to right and row after row��.��From index:	� -   $��	randRadio��$��12(�$
��Bold"�$��42,162,0,172q�$��1��$
��Left=�$	��120��$/ )Choose the next position comletely random��.��Random index	� -   $��	bestRadio��$��12(�$
��Bold"�$��16��$��
42,0,0,144q�$��Bottom=�$��1��$
��Left=�$	��120��$M GCalculate the position which has the least posibilities and write there��.��Best position�� -   $��fromIndexCombo��$��1��$��23��$��138,133,143,0q�$	��Top=��� -   $��label2��$��16(�$
��Bold"�$��31��$��	13,0,0,93q�$��Bottom=�$��1��$
��Left=�$��68.262��.��Speed�� -   $��	stepLabel��$��12(�$
��Bold"�$��25��$��
25,0,20,88q�$��Bottom=�$��1��$��Center=�.��
Step: 0s� -   $��
tablePlace��$��0,39,18,-50q� � ShowGridLines .  .+��� -   $��
rectangle4��$��31,20,41,103q�
���   �
���   ��� -   $��	tableName�� �� AcceptsReturn .   �� 
AcceptsTab .  $��23��$��	0,29,18,0q�$	��Top=�$��Center=�$��0q�$��14(�$
��Bold"�
������ .��Click here to set name�� -   $��findAllSolutionCheckBox��$��1��$��30��$��	10,0,0,10q�$��Bottom=�$
��Bold"�$
��Left=�$	��153��$��16(�.��Find all solutions�� -   $��solutionLabel��$��12(�$
��Bold"�$��26.523��$��	0,0,30,14q�$��Bottom=�$��1��$��Right=�$��54��$
��Left=�$y�Hidden=�.��/1000�� -   $��
rectangle5��
����·�$��2��$��18��$��309,0,-31,48q�$��Bottom=��� -   $��rightButton��$
��Bold"�$��24.523��$��Right=�$��	0,0,16,15q�$��Bottom=�$y�Hidden=�$��22��$��1�� �� ContentStringFormat$ Next solution��.��>�� -   $��
leftButton��$
��Bold"�$��24.523��$��Right=�$��
0,0,130,14q�$��Bottom=�$y�Hidden=�$��22��$��1��$ Previous solution��.��<�� -   $��actSolution��$��1��$��24.477��$��Right=�$��	0,0,83,14q�$��Bottom=�$��37��$��Right=�$y�Hidden=�$��Segoe UI)�.��1000�� -   $��speedBar��$��1��$��21��$��
25,0,20,72q�$��Bottom=�	 � Value$	 0Q�
 � Maximum$

 1000Q�$��RightToLeft=� � SmallChange$ 1Q��� -   $��backTrackLabel��$��16(�$
��Bold"�$��31��$��42,0,134,18q�$��Bottom=�.��Backtrack number:�� -   $��backTrackNumber��$��16(�$
��Bold"�$��31��$��0,0,8,18q�$��Bottom=�$��Right=�$	��120��.��0�� -   $��
rectangle1��$��	0,7,0,-21q�
����·�$��Right=�$��18���� -   $��pauseButton��$
��Bold"�$��28��$
��Left=�$��3.738,10.297,0,0q�$	��Top=�$y�Hidden=�$��77.262��$��1��.��Pause�� -   $��generateButton��$
��Bold"�$��Right=�$��0,166,30,156q�$y�Visible=�$��77.262��$��1��$ Generate new table��.��
Generate�� -    $��addFieldButton��$
��Bold"�$��28��$��Right=�$��
0,125,30,0q�$	��Top=�$y�Visible=�$��77.262��$��1��$ Add new random field��.��	Add fieldo� -!   $��menu1��$��22��$	��Top=�.@�m� -"   $��mainMenu��$
��File��.@�m� -#   $��openMenu��$
��Open��m� -$   $��saveMenu��$
��Save��m� -%   $
��Exit��m� $��Options��$y�Visible=�.@�m� -&   $��flashCheckedFields��$��Flash checked fields�� m� IsCheckable .  m� 	IsChecked .  
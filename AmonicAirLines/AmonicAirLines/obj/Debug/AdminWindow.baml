   M S B A M L   `   `   `  ����I  EAmonicAirLines, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AmonicAirLinesAmonicAirLines  5   	        AmonicAirLines.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:AmonicAirLines     5   	     � Title$  AdminWindow��5   	   $	��500��6   $	��700��6*    � 	Resources 5	         RowStyle �� 
TargetType)  #System.Windows.Controls.DataGridRow �� Triggers �� Binding �� Value �� Property �� Value��%  &	        �� 5
   
   "  6!    5      �� 5       6   �� *
HiddenData+ 16=   .a�5      ��  � ��
 �  ��63   5      �� 5       6   �� *
HiddenData+ 06=   .a�5      ��  � ��
 �����63   5      5      5      .��5      � .+�5   
   �� -   5        $��Add user��5      $
��Left=�6$   $��	20,10,0,0q�6?   $	��Top=�6R   6j   �� 5   
   -   5        $
��Exit��5      $
��Left=�6    $��	76,10,0,0q�6;   $	��Top=�6N   $��43��6f   6q   �� 5   
   $��Office:��6   $
��Left=�6"   $��	24,47,0,0q�6=   $	��Top=�6P   �� 5   
   -   5        $��OfficeComboBox��5   .   $
��Left=�6   $��	73,49,0,0q�6D   $	��Top=�6W   $	��120��6o   6{   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5      	   AutoGenerateColumns	 .  $	��300��6A   $��0,0,20,0q�6N   
   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 * 6`   $��Center=�5         CanUserAddRows .  $��Right=�5   $   $	��660��6@      RowStyle# [  5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5       ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%      Width$ 70 64   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   �� *	FirstName+  5!      $ 	Last name��6%   $	 100 69    6E   �� *
LastName+  5"      $	 Age��6%   $ 50 63    6>   �� *Age+  5#      $ 	User Role��6%   $	 150 69    6E   �� *
UserRole+  5$      $ Email Address��6%   $	 150 6=    6I   �� *EmailAddress+  5%      $ Office��6%   $	 100 66    6B   �� *Office+  5&      $ HiddenColumn��6%      
Visibility$ 	Collapsed=�6Z    6;   �� *
HiddenData+6Z   5)      �� 5*   
   -   5        $��Change Role��5*      $��Right=�6'   $��
0,0,602,49q�6C   $��Bottom=�6W   $��78��6r   6}   �� 5+   
   -   5        $��Enable/Disable Login��5+      $��Right=�60   $��
0,0,444,49q�6L   $��Bottom=�6`   $	��147��6{   6�   �� 5,   
   -   5        $��Clear Office��5,      $
��Left=�6(   $��
204,50,0,0q�6C   $	��Top=�6W   6o   5-      5.      
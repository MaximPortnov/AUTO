   M S B A M L   `   `   `  ����I  EAmonicAirLines, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AmonicAirLinesAmonicAirLines  5   	       AmonicAirLines.UserWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:AmonicAirLines     5   	   -   5          � Title$  AMONIC Airlines��5	   	   $	��450��6!   $	��800��6.    � 	Resources 5
         RowStyle �� 
TargetType)  #System.Windows.Controls.DataGridRow �� Triggers �� Binding �� Value �� Property �� Value��%  &	        �� 5   
   "  6!    5      �� 5       6   �� *
LogoutTime+ --6=   .a�5      ��  � ��
 �  ��63   5      �� 5       6   �� *
LogoutTime+ 6=   Z� .a�5      ��  � ��
 �����63   5      5      5      .��5      � .+�5   
   �� -   5         ��Name$ timeTextBlock��5      $��14(�6+   
���   �$
��Wrap=�6L   $��	TextBlock��6`   $��10,14,493,379q�6q   �� 5   
   -   5        $ crashTextBlock��5      $��14(�6,   
���   �$
��Wrap=�6M   $��	TextBlock��6a   $��10,42,493,351q�6r   &   System.Windows.Controls.DataGrid  5   
   -   5        	  Name$	 ActivitiesGrid��5      
   AutoGenerateColumns
 .  $��Stretch=�5       $��Stretch=�5!      $��20,100,20,20q�5"         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5#         CanUserAddRows .     RowStyle# [  5%         Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��6%      Width$	 100 64   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� *Date+  5)      $ 
Login Time��6%   $	 100 6:    6F   �� *	LoginTime+  5*      $ Logout Time��6%   $	 105 6;    6G   �� *
LogoutTime+  5+      $ Time spent on  system��6%   $ * 6E    6O   �� *TimeSpentOnSystem+  5,      $  Unsuccessful logout reason��6%   $ * 6J    6T   �� *UnsuccessfulLogoutReason+5.      5/      50      
   M S B A M L   `   `   `  ����%  !KMA.ProgrammingInCSharp2019.Lab05/    )KMA.ProgrammingInCSharp2019.Lab05.Window1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	     � Title$  ProcessesList��5   	   $	��530��6   $
��1000��6,   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $��65 �6#   �� 5      $��* �6#   .+�5      �� $��0��6   $��5q�6"   $��Sort by:��6-   �� 5      $��0��6   $��1��6%   $��5q�65   $��30��6@   $
��Left=�6L   $	��Top=�6g    �� SelectedIndex$ 0��6�    �� SelectedValue 6   �� *	SortEntry+ �� Mode$ OneWayToSource=� �� UpdateSourceTrigger$ PropertyChanged=�.@�5      �� .��6   Name6   �� 5      .��6   Id6   �� 5      .��6   Active6    �� 5      .��6   	CPU usage6#   �� 5       .��6   Memory usage6&   �� 5!      .��6   Threads count6'   �� 5"      .��6   User6   �� 5#      .��6   Path6   �� 5$      .��6   Launch time6%   5%      �� 5&      $��1��6   $��Stretch=�6#   $��Center=�6A   $��5q�6\   $
��Sort��6g   ��6v   �� *Sort+5(      �� 5)   
   $��2��6   $��Stretch=�6   $��Center=�6=   $��5q�6X   $��Open folder��6c   ��6y   �� *
OpenFolder+�� 5*   
   $��3��6   $��Stretch=�6   $��Center=�6=   $��5q�6X   $��	Terminate��6c   ��6w   �� *	Terminate+"  FontAwesome.WPF.ImageAwesome  5,   
   $��1��6      Icon$ Refresh=�6'   $��50��66   $��50��6A      Spin . $��
15 15 15 5q�6Y   y�6m   �� *LoaderVisibility+$ OneWay=�$ PropertyChanged=�6�   &   System.Windows.Controls.DataGrid  5.   
   -   5        	  Name$	 General��5.      $��1��6$   
   
IsReadOnly
 .    AutoGenerateColumns .  $	��360��50   z      CanUserSortColumns .  y�5.   _   �� *DataGridVisibility+$ OneWay=�$ PropertyChanged=���5/      �� *	Processes+$ OneWay=�$ PropertyChanged=� �� SelectedItem 50      �� *SelectedProcess+$ OneWayToSource=�$ PropertyChanged=�   Columns 51      0  *System.Windows.Controls.DataGridTextColumn  52      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Name+  53      $ Id��6%    61   �� *Id+  54      $ Active��6%    65   �� *
IsActive+  55      $ 	CPU usage��6%    68   �� *CPU+  56      $ Memory usage��6%    6;   �� *MemoryUsage+  57      $ Threads count��6%    6<   �� *ThreadsCount+  58      $
 User��6%    63   �� *User+  59      $
 Path��6%      MaxWidth$	 150Q�6L    63   �� *Path+6L     5:      $ Launch time��6%    6:   �� *
LaunchTime+   RowDetailsTemplate 5<      �� 5=      .R�5>      ��  �� Padding$ 10q�6   ._�5?      �  5@      �� 5A   "   $
��Auto �60   �� 5B   "   $
��Auto �60    5D      �� 5E   "   $��* �63   �� 5F   "   $��* �63   .+�5I      �� $��0��6$   $��0��62   $��5q�6B   $��Modules��6M     5K      $	 ProcessModules��6'   $��1��6?   $��0��6L   
 .  . $
��Left=�6�   $	��150��5L   }    .     MaxColumnWidth$	 150Q�6�   ��6+   �� *	Modules+$ OneWay=�$ PropertyChanged=�5M      �� 5O      $��0��6$   $��1��63   $��5q�6C   $��Threads��6N     5Q      $	 ProcessThreads��6'   $��1��6?   $��1��6L   
 .  .  .  $	��150��5R   }   $
��Left=�6�   ��6+   �� *	Threads+$ OneWay=�$ PropertyChanged=�5S      5T      5U      5V      5X      5Y      5Z      
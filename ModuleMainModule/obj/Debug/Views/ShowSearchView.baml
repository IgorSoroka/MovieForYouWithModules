   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      +    %ModuleMainModule.Views.ShowSearchView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @
converters)clr-namespace:ModuleMainModule.Converters     5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      O KEO.Wpf, Version=17.1.14.0, Culture=neutral, PublicKeyToken=e92353a6bf73fffc1eo(http://schemas.essentialobjects.com/wpf/  5	      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $
��auto��5      $	��500��6   $
��auto��6*   $	��600��68    � 	Resources 5      6   0ModuleMainModule.Converters.NullToFalseConverter   NullToFalseConverter��%   &	          5   
   6*   5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   � ���	#4C000000 � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��15, 15, 15, 8q�6   $
��auto��6(   $
��auto��65   
�������$��2q�6[    �� CornerRadius$ 5��6o   ._�5      ��  �� Orientation$ Vertical=�6   .+�5      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *NameSearching+]� 5      ��. $��10q�61   .+�5      �� $��Right=�6!   $��14(�68   ��6F   �� *NavigateCommandNameSearch+{�6t   �� *	CanSave+��6�   �� *Find+�� 5      $��14(�6"   ��60   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� �� ValidatesOnDataErrors . .��6�    � ToolTip 5      _� 5      .��5   "   �� ��6,   �� *Regulations+5      .��5   /   	 J� InputBindings	 5       �� 5!   "   

 �� Key$
 Return��6[    �� Command 6-   �� *NavigateCommandNameSearch+6[   .��5"   5   5#      5$      5%      5&      �� 5'   
   $��15, 8, 15, 15q�6   $��1��6(   $
��auto��65   $
��auto��6B   
�������$��2q�6h   $ 5��6|   ._�5(      �� .+�5)      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *Year+�� 5*      -   5         ��Name$ CmYear��5*      $��10q�6+   {�67   �� *SelectedItem+ �� ElementName$ CbFirst�� �� 	Converter# [  ��5+      ��  �� Path$ 	YearsList� �� SelectedItem 6B   �� *SelectedYear+$ TwoWay=��� 5,      
�������$��3q�62   $��Center=�6=   $��17(�6a   ��6o   �� *
Interval+� 5-       � ColumnDefinitions 5.      �� 5/      �� 50      .+�52      ]� $��0��6    $��5q�60   .+�53      �� 
�������$��12(�6:   ��6H   �� *From+�� 54      -   5        $ CbFirst��54   #   ��64   �� *	YearsList+ 6V   �� *SelectedFirstYear+$ TwoWay=�{�55   #   �� *SelectedItem+$ CmYear��# [  56      ]� 57      $��1��6    $��5q�60   .+�58      �� 
�������$��12(�6:   ��6H   �� *To+6a   �� 59      -   5        $ CbLast��59   #   {�63   �� *SelectedItem+$ CmYear��# [  ��5:   #   �� $ 	YearsList� 6J   �� $ SelectedLastYear�$ TwoWay=�5;      5<      �� 5=      
�������$��3q�62   $��Center=�6=   $��17(�6a   ��6o   �� *	Raiting+  EO.Wpf.Slider  5>      
�������$��10q�62    � Minimum$ 0Q�6>    � Maximum$ 10Q�6J     EO.Wpf.SliderBase   TickPlacement$
 Both=�6W      TickFrequency$ 1Q�6l    � Value 5?      �� *SelectedRating+$ PropertyChanged=�� 5@       5A      �� 5B      $��* �6+   �� 5C      $��* �6+   .+�5E      �� $��0��6!   $��5q�61   $��14(�6<   $	��150��6J   ��6Y   �� $ NavigateCommandReset���6�   �� *
ResetAll+�� 5F      $��1��6!   $��5q�61   $��14(�6<   $	��150��6J   ��6Y   �� $ NavigateCommandSearch���6�   �� *Find+5G      5H      5I      5J      5K      5L      
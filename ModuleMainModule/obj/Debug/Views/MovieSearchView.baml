   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      ,    &ModuleMainModule.Views.MovieSearchView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      O KEO.Wpf, Version=17.1.14.0, Culture=neutral, PublicKeyToken=e92353a6bf73fffc2wpf(http://schemas.essentialobjects.com/wpf/  5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      :
converters)clr-namespace:ModuleMainModule.Converters  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $	��500��5      $	��685��6    � 	Resources 5      6   0ModuleMainModule.Converters.NullToFalseConverter   NullToFalseConverter��%   &	          5   
   6*   5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   � ���	#4C000000 � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��15, 15, 15, 8q�6   $
��auto��6(   $
��auto��65   
�������$��2q�6[    �� CornerRadius$ 5��6o   ._�5      ��  �� Orientation$ Vertical=�6   .+�5      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *NameSearching+]� 5      ��. $��10q�61   .+�5      �� $��70��6!   $��Right=�6,   $��14(�6C   ��6Q   �� *NavigateCommandNameSearch+{�6   �� *	CanSave+��6�   �� *Find+�� 5      $��16(�6"   ��60   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� �� ValidatesOnDataErrors . .��6�    � ToolTip 5      _� 5      .��5   "   �� ��6,   �� *Regulations+5       .��5!   /   	 J� InputBindings	 5"      �� 5#   "   

 �� Key$
 Return��6[    �� Command 6-   �� *NavigateCommandNameSearch+6[   .��5$   5   5%      5&      5'      5(      �� 5)   
   $��15, 8, 15, 8q�6   $��1��6'   $
��auto��64   $
��auto��6A   
�������$��2q�6h   $ 5��6|   ._�5*      �� .+�5+      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *Year+�� 5,      -   5         ��Name$ CmYear��5,   +   $��10q�6   {�6;   �� *SelectedItem+ �� ElementName$ CbFirst�� �� 	Converter# [  ��5-      �� *	YearsList+ �� SelectedItem 5.      �� *SelectedYear+$ TwoWay=��� 5/      
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *
Interval+� 50       � ColumnDefinitions 51      �� 52      �� 53      .+�55      ]� $��0��6    $��5q�61   .+�56      �� $��12(�6$   
���������6H   �� *From+�� 57      -   5        $ CbFirst��57   #   {�64   �� *SelectedItem+$ CmYear��# [  ��58   #   ��  �� Path$ 	YearsList� 6J   �� $ SelectedFirstYear�$ TwoWay=�59      ]� 5:      $��1��6    $��5q�60   .+�5;      �� $��12(�6$   
���������6H   �� *To+�� 5<      -   5        $ CbLast��5<   #   {�63   �� *SelectedItem+$ CmYear��# [  ��5=   #   �� $ 	YearsList� 6J   �� $ SelectedLastYear�$ TwoWay=�5>      5?      �� 5@      
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *	Raiting+  EO.Wpf.Slider  5A      
�������$��10q�63    � Minimum$ 0Q�6?    � Maximum$ 10Q�6K     EO.Wpf.SliderBase   TickPlacement$
 Both=�6X      TickFrequency$ 1Q�6m    � Value 5B      �� $ SelectedRating�$ PropertyChanged=�� 5C       5D      �� 5E      $��* �6+   �� 5F      $��* �6+   .+�5H      �� $��5q�6!   $��0��6,   $	��150��6<   $��14(�6K   ��6Y   �� $ NavigateCommandReset���6�   �� *
ResetAll+�� 5I      $��5q�6!   $	��150��6,   $��1��6;   $��14(�6K   ��6Y   �� $ NavigateCommandSearch���6�   �� *Find+5J      5K      5L      �� 5M   
   $��15, 8, 15, 8q�6   $��2��6'   $
��auto��64   $
��auto��6A   
�������$��2q�6g   $ 5��6{   ._�5N      �� $ Vertical=�6   .+�5O      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *GenreSearching+�� 5P      $��5q�6   ��6&   �� $ Genres� 6J   �� $ SelectGenre�$ TwoWay=��� 5Q      $��3q�6   $	��100��6(   $��14(�67   ��6E   �� $  NavigateCommandGenreSearch���6{   �� *Find+5R      5S      �� 5T   
   $��15, 8, 15, 15q�6   $��3��6(   $
��auto��65   $
��auto��6B   
�������$��2q�6h   $ 5��6|   ._�5U      �� $ Vertical=�6   .+�5V      �� 
�������$��3q�62   $��17(�6=   $��Center=�6K   ��6o   �� *CompanySearching+�� 5W      $��5q�6   ��6&   �� *	Companies+ 6H   �� $ SelectedCompany�$ TwoWay=��� 5X      $��3q�6   $	��100��6(   $��14(�67   ��6E   �� $" NavigateCommandCompanySearch���6}   �� *Find+5Y      5Z      5[      5\      5]      
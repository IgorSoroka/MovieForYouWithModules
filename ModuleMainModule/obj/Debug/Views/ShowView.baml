   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      %    ModuleMainModule.Views.ShowView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @
converters)clr-namespace:ModuleMainModule.Converters     5      X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5	      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $
��auto��5      $
��1100��6   $
��auto��6+   $
��1000��69   $	��300��6J    � 	Resources 5      3   -ModuleMainModule.Converters.ImageUrlConverter   PathConverter5   /ModuleMainModule.Converters.NullToTrueConverter  NullToTrueConverter1   +ModuleMainModule.Converters.GenresConverter  GenresConverter3   -ModuleMainModule.Converters.KeywordsConverter  KeywordsConverter3   -ModuleMainModule.Converters.CountiesConverter  CountriesConverter2   ,ModuleMainModule.Converters.NetworkConverter  NetworkConverter��%�   &	        &	      &	 &     &	 9     &	 L     &	 _       5   
   6'     5   
   6)     5   
   6%     5   
   6'     5   
   6'     5   
   6&   5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator     IsBusy 6   �� *BusyIndicatorValue+ �� UpdateSourceTrigger$ PropertyChanged=�   BusyContent 6d   �� *LoadingData+.��5   
   �� $��Visible=�6   .��5   
   � -   5         �Name$ GridMain��5   &   ���	#4C000000 � RowDefinitions 5      �� 5      �� 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      .	  (System.Windows.Interactivity.Interaction 	  Triggers 5      F
  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger
  5       3  -System.Windows.Interactivity.EventTriggerBase   SourceObject 62   �� *NotificationRequest+	 �� Mode$	 OneWay=�.  (System.Windows.Interactivity.TriggerBase
   Actions.
 5!      +  %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�6=   !   CenterOverAssociatedObject .  6a   5"      .+�5$      � $	��150��66   $	��300��6F    � DataContext 6   �� *
DirectShow+ � ColumnDefinitions 5%      �� 5&      $	��100��6'   $��* �66   �� 5'      $��5* �6'   .+�5)      �� $	��100��6   $��3q�6(   ��63   �� *Poster+ �� 	Converter# [  �� 5*      $��1��6    �� Orientation$ Vertical=�6-   .+�5+      �� 
�������$��20(�67   ��6E   �� *Name+�� 5,      $��16(�6   
���������6@   �� *Plot+ 5-      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >� �� Path$ DataContext��� 5.      
�������$
��Wrap=�66   $��3q�6J   $��14(�6U   ��6c   �� *
Overview+5/      50      �� 51      $��1��6   .+�52      �  53      �� 54      $��* �6/   �� 55      $��* �6/   .+�57      �� $��0��6!   $��16(�61   $��35��6?   ��6K   �� *AddFavorites+��6l   �� *NavigateCommandAddToDb+{�6�   �� *
CanAddToDb+$ PropertyChanged=� 58      ��  �� *FindAncestor+" >�$ DataContext��� 59      $��1��6!   $��16(�61   $��35��6?   ��6K   �� *DelFavorites+��6l   �� *NavigateCommandDellFromDb+{�6�   �� *CanDelFromDb+$ PropertyChanged=� 5:      ��  �� *FindAncestor+" >�$ DataContext�5;      �� 5<      $��16(�6   $��35��6+   ��67   �� *	Trailer+��6S   �� *NavigateCommandShowTrailler+ 5=      ��  �� *FindAncestor+" >�$ DataContext�{�5>      �� *
VideoUrl+# [ 5?      �� 5@      $��2��6   .+�5A      �� $��16(�6   
���������6<   �� *
ShowCast+�� 5B      $��Disabled=�6   ��6P   �� *Crew+��5C      �� 5D       �� 
TargetType" ��6    .�5E      ��  �� Property � �� �� Value �	#4C0000006;   �� 5F       � �� �	#FF0000006;   �� 5G       � ��$ 1q�67   �� 5H       � ��$ 1q�68   �� 5I       � ��$ 1q�6@   �� 5J       � ��
 �   �6<    �� Triggers 5K      P� 5L   "    P� Property �@��
IsSelected P� Value . . �5M   &   ��  � ��
 �����6C   �� 5N   &    � ��$ 1q�6H   �� 5O   &    � ��
 �   �6D   5P   #   5R      ��5T      �� 5U      .R�5V      �  K� IsItemsHost . ���	#4C0000006;   5W      ��5Y      �� 5Z      .R�5[      �� $ Vertical=�6)   $��Center=�6@   .+�5\   "   � $	��130��6'   $	��100��64   .+�5]   &   �� $��3��6,   $��Uniform=�6=   $��Stretch=�6�   ��6P   �� *	Profile+# [  6�   �� 5^   &   $��1��6E   $��Center=�6R   $��Stretch=�6v   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Name+��5_   *   �� 5`   .    �� Color$ Azure��6>    �� Opacity$	 0.5Q�6L   5b   '   �� 5c   &   $��2��6D   $��Center=�6Q   $��Stretch=�6u   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Job+��5d   *   �� 5e   .   $ Azure��6>   $	 0.5Q�6L   5g   '    5h   &   �� 5i   *   $��* �68   �� 5j   *   $��0.29* �68   �� 5k   *   $��0.29* �68   5m   #   5n      5o      5q      5r      �� 5s      $��3��6   .+�5t      �� $��16(�6   
���������6<   �� *	MainRoles+�� 5u      $��Disabled=�6   ��6P   �� *Cast+ �� SelectedItem 5v      �� *SelectedActor+��5w      �� 5x      " ��6    .�5y      ��  � �� �	#4C0000006;   �� 5z       � �� �	#FF0000006;   �� 5{       � ��$ 1q�67   �� 5|       � ��$ 1q�68   �� 5}       � ��$ 1q�6@   �� 5~       � ��
 �   �6<    5      P� 5�   "    �@��
IsSelected . . �5�   &   ��  � ��
 �����6C   �� 5�   &    � ��$ 1q�6H   �� 5�   &    � ��
 �   �6D   5�   #   5�      ��5�      �� 5�      .R�5�      �  . ���	#4C0000006;   5�      ��5�      �� 5�      .R�5�      �� $ Vertical=�6)   $��Center=�6@   .+�5�   "   � $	��130��6'   $	��100��64   .+�5�   &   �� $��3��6,   $��Uniform=�6=   $��Stretch=�6�   ��6P   �� *	Profile+# [  6�   �� 5�   &   $��1��6E   $��Center=�6R   $��Stretch=�6v   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Name+��5�   *   �� 5�   .   $ Azure��6>   $	 0.5Q�6L   5�   '   �� 5�   &   $��2��6J   $��Center=�6W   $��Stretch=�6{   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *	Character+��5�   *   �� 5�   .   $ Azure��6>   $	 0.5Q�6L   5�   '    5�   &   �� 5�   *   $��* �68   �� 5�   *   $��0.29* �68   �� 5�   *   $��0.29* �68   5�   #   �� 5�   "   $��12(�6)   ��67   �� *
ReadMore+��6T   �� $$ NavigateCommandShowDirectActor� 5�   )   ��  �� *FindAncestor+" >�$ DataContext���5�   &   �� 5�   *   " ��60   .�5�   .   ��  � y�$ 	Collapsed=�6K    5�   .   �� 5�   2    �� Binding 6>   ��  ��   �� Mode$  FindAncestor=�" ��$ 
IsSelected�! �� Value! True5�   k   .a�5�   6   ��  � y�$ Visible=�6S   5�   3   5�   +   5�   #   5�      5�      5�      5�      �� 5�      $��4��6    6&   �� $ 
DirectShow���5�      �� 5�      .�5�      ��  � ��
 �����6A   �� 5�       � ��$ 3q�6=   �� 5�       �@��Orientation$ Vertical=�6C   5�      .+�5�      �� $��16(�6   
���������6=   �� *	AboutShow+ 6[   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *OriginalName+ 6b   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *OriginalName+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *SeasonsNumber+ 6c   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *SeasonCount+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *SeriesNumber+ 6b   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *EpisodeCount+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *
Premiere+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *FirstAirDate+" �� StringFormat$" D��# �� ConverterCulture$# ru-RU��5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *
LastSeries+ 6`   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *LastAirDate+$" D��$# ru-RU��5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *	Raiting+ 6]   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *VoteAverage+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *	VoteCount+ 6_   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *	VoteCount+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *Genres+ 6\   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *Genres+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *
Networks+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *
Networks+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *	Countries+ 6_   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *	Countries+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�63   ��6A   �� *
Keywords+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *
Keywords+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *Status+ 6\   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *Status+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��14(�63   ��6A   �� *
HomePage+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *
HomePage+5�      5�      5�      5�      5�      5�      
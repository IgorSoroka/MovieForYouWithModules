   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5
      &     ModuleMainModule.Views.MovieView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      :
converters)clr-namespace:ModuleMainModule.Converters  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5
      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $
��auto��5      $
��1100��6   $
��auto��6+   $
��1000��69   $	��300��6J    � 	Resources 5      3   -ModuleMainModule.Converters.ImageUrlConverter   PathConverter5   /ModuleMainModule.Converters.NullToTrueConverter  NullToTrueConverter1   +ModuleMainModule.Converters.GenresConverter  GenresConverter4   .ModuleMainModule.Converters.CompaniesConverter  CompaniesConverter3   -ModuleMainModule.Converters.KeywordsConverter  KeywordsConverter3   -ModuleMainModule.Converters.CountiesConverter  CountriesConverter��%�   &	        &	      &	 &     &	 9     &	 L     &	 _       5   
   6'     5   
   6)     5   
   6%     5   
   6(     5   
   6'     5   
   6'   5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator     IsBusy 6   �� *BusyIndicatorValue+ �� UpdateSourceTrigger$ PropertyChanged=�   BusyContent 6d   �� *LoadingData+.��5   
   �� $��Visible=�6   .��5   
   � -   5         �Name$ GridMain��5   &   ���	#4C000000 � RowDefinitions 5      �� 5      �� 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      .	  (System.Windows.Interactivity.Interaction 	  Triggers 5      F
  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger
  5       3  -System.Windows.Interactivity.EventTriggerBase   SourceObject 62   �� *NotificationRequest+	 �� Mode$	 OneWay=�.  (System.Windows.Interactivity.TriggerBase
   Actions.
 5!      +  %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�6=   !   CenterOverAssociatedObject .  6a   5"      
  5#       62   �� *NotificationRequestNull+$	 OneWay=�.
 5$         . $ CenterOwnerX�6=    .  6a   5%      .+�5'      � $	��150��67   $	��300��6G    � DataContext 6   �� *DirectMovie+ � ColumnDefinitions 5(      �� 5)      $	��100��6'   $��* �66   �� 5*      $��5* �6'   .+�5,      �� $	��100��6   $��3q�6'   ��62   �� *Poster+ �� 	Converter# [  �� 5-      $��1��6    �� Orientation$ Vertical=�6-   .+�5.      �� 
�������$��20(�66   ��6D   �� *Title+�� 5/      
�������$��14(�66   ��6D   �� *	TagLine+�� 50      $��16(�6   
���������6@   �� *Plot+ 51      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >� �� Path$ DataContext��� 52      
�������$
��Wrap=�66   $��3q�6J   $��14(�6U   ��6c   �� $ Overview�53      54      �� 55      $��1��6   .+�56      �  57      �� 58      $��* �6/   �� 59      $��* �6/   .+�5;      �� $��0��6!   $��16(�61   $��35��6?   ��6K   �� *AddFavorites+��6l   �� *NavigateCommandAddToDb+{�6�   �� *
CanAddToDb+$ PropertyChanged=� 5<      ��  �� *FindAncestor+" >�$ DataContext��� 5=      $��1��6!   $��16(�61   $��35��6?   ��6K   �� *DelFavorites+��6l   �� *NavigateCommandDellFromDb+{�6�   �� *CanDelFromDb+$ PropertyChanged=� 5>      ��  �� *FindAncestor+" >�$ DataContext�5?      �� 5@      $��16(�6   $��35��6+   ��67   �� *	Trailer+��6S   �� *NavigateCommandShowTrailler+ 5A      ��  �� *FindAncestor+" >�$ DataContext�{�5B      �� *
VideoUrl+# [ 5C      �� 5D      $��2��6   .+�5E      �� $��16(�6   
���������6<   �� *
ShowCast+�� 5F      $��Disabled=�6   ��6P   �� *Crew+��5G      �� 5H       �� 
TargetType" ��6    .�5I      ��  �� Property � �� �� Value �	#4C0000006;   �� 5J       � �� �	#FF0000006;   �� 5K       � ��$ 1q�67   �� 5L       � ��$ 1q�68   �� 5M       � ��$ 1q�6@   �� 5N       � ��
 �   �6<    �� Triggers 5O      P� 5P   "    P� Property �@��
IsSelected P� Value . . �5Q   &   ��  � ��
 �����6C   �� 5R   &    � ��$ 1q�6H   �� 5S   &    � ��
 �   �6D   5T   #   5V      ��5X      �� 5Y      .R�5Z      �  K� IsItemsHost . ���	#4C0000006;   5[      ��5]      �� 5^      .R�5_      �� $ Vertical=�6)   $��Center=�6@   .+�5`   "   � $	��130��6'   $	��100��64   .+�5a   &   �� $��3��6,   $��Uniform=�6=   $��Stretch=�6�   ��6P   �� *	Profile+# [  6�   �� 5b   &   $��1��6E   $��Center=�6R   $��Stretch=�6v   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Name+��5c   *   �� 5d   .    �� Color$ Azure��6>    �� Opacity$	 0.5Q�6L   5f   '   �� 5g   &   $��2��6D   $��Center=�6Q   $��Stretch=�6u   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Job+��5h   *   �� 5i   .   $ Azure��6>   $	 0.5Q�6L   5k   '    5l   &   �� 5m   *   $��* �68   �� 5n   *   $��0.29* �68   �� 5o   *   $��0.29* �68   5q   #   5r      5s      5u      5v      �� 5w      $��3��6   .+�5x      �� $��16(�6   
���������6<   �� *	MainRoles+�� 5y      $��Disabled=�6   ��6P   �� *Cast+ �� SelectedItem 5z      �� $ SelectedActor�$	 TwoWay=���5{      �� 5|      " ��6    .�5}      ��  � �� �	#4C0000006;   �� 5~       � �� �	#FF0000006;   �� 5       � ��$ 1q�67   �� 5�       � ��$ 1q�68   �� 5�       � ��$ 1q�6@   �� 5�       � ��
 �   �6<    5�      P� 5�   "    �@��
IsSelected . . �5�   &   ��  � ��
 �����6C   �� 5�   &    � ��$ 1q�6H   �� 5�   &    � ��
 �   �6D   5�   #   5�      ��5�      �� 5�      .R�5�      �  . ���	#4C0000006;   5�      ��5�      �� 5�      .R�5�      �� $ Vertical=�6)   $��Center=�6@   .+�5�   "   � $	��130��6'   $	��100��64   .+�5�   &   �� $��3��6,   $��Uniform=�6=   $��Stretch=�6�   ��6P   �� *	Profile+# [  6�   �� 5�   &   $��1��6E   $��Center=�6R   $��Stretch=�6v   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *Name+��5�   *   �� 5�   .   $ Azure��6>   $	 0.5Q�6L   5�   '   �� 5�   &   $��2��6J   $��Center=�6W   $��Stretch=�6{   $��12(�6�   
���   �$��Bottom=�6�   ��6,   �� *	Character+��5�   *   �� 5�   .   $ Azure��6>   $	 0.5Q�6L   5�   '    5�   &   �� 5�   *   $��* �68   �� 5�   *   $��0.29* �68   �� 5�   *   $��0.29* �68   5�   #   �� 5�   "   $��12(�6)   ��67   �� *
ReadMore+��6T   �� $$ NavigateCommandShowDirectActor� 5�   )   ��  �� *FindAncestor+" >�$ DataContext���5�   &   �� 5�   *   " ��60   .�5�   .   ��  � y�$ 	Collapsed=�6K    5�   .   �� 5�   2    �� Binding 6>   ��  ��   �� Mode$  FindAncestor=�" ��$ 
IsSelected�! �� Value! True5�   k   .a�5�   6   ��  � y�$ Visible=�6S   5�   3   5�   +   5�   #   5�      5�      5�      5�      �� 5�      $��4��6    6&   �� *DirectMovie+��5�      �� 5�      .�5�      ��  � ��
 �����6A   �� 5�       � ��$ 3q�6=   �� 5�       �@��Orientation$ Vertical=�6C   5�      .+�5�      �� 
�������$��18(�6.   ��6<   �� *
AboutMovie+ 6[   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *OriginalName+ 6a   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��3q�6    $��14(�6+   ��69   �� *OriginalTitle+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *
Premiere+ 6]   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��3q�6    $��16(�6+   ��69   �� *ReleaseDate+" �� StringFormat$" D��# �� ConverterCulture$# ru-RU��5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *
Duration+ 6]   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *	Runtime+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *Budget+ 6[   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *Budget+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *	Revenue+ 6\   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *	Revenue+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *	Raiting+ 6\   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *VoteAverage+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *	VoteCount+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $
��Wrap=�6    $��16(�64   ��6B   �� *	VoteCount+5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *Genres+ 6[   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *Genres+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *	Countries+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *	Countries+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *
Keywords+ 6]   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *
Keywords+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *	Companies+ 6^   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��16(�6    ��6.   �� *	Companies+# [ 5�      �� 5�      $��5q�6   .+�5�      �� 
�������$��16(�62   ��6@   �� *
HomePage+ 6]   ��  �� *FindAncestor+" >�$ DataContext��� 5�      $��14(�6    ��6.   �� *
HomePage+5�      5�      5�      5�      5�      5�      
   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5
      E'clr-namespace:ModuleMainModule.BehaviorModuleMainModule.Behavior  5      &     ModuleMainModule.Views.ActorView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @
converters)clr-namespace:ModuleMainModule.Converters     5      X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5
      6behavior'clr-namespace:ModuleMainModule.Behavior  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $
��auto��5      $
��1100��6   $
��auto��6+   $	��300��69   $
��1000��6H    � 	Resources 5      3   -ModuleMainModule.Converters.ImageUrlConverter   PathConverter��%   &	          5   
   6'   5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator     IsBusy 6   �� *BusyIndicatorValue+ �� UpdateSourceTrigger$ PropertyChanged=�   BusyContent 6d   �� *LoadingData+.��5   
   �� $��Visible=�6   .��5      � -   5         �Name$ GridMain��5   "   ���	#4C000000 � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6   �� 5      �� 5      .  (System.Windows.Interactivity.Interaction   Triggers 5      F  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  5      3  -System.Windows.Interactivity.EventTriggerBase   SourceObject 62   �� *NotificationRequest+	 �� Mode$	 OneWay=�.  (System.Windows.Interactivity.TriggerBase
   Actions.
 5      +  %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�6=   !   CenterOverAssociatedObject .  6a   5      .+�5   
   �� $��1q�6   
���   �._�5       � $	��150��6<   $	��300��6L    � DataContext 6   ��  �� Path$ DirectActor� � ColumnDefinitions 5!      �� 5"      $	��150��6'   $��* �66   �� 5#      $��4* �6'   .+�5%      �� ��6   �� *Poster+ �� 	Converter# [  �� 5&      $��1��6   $��3q�6-    �� Orientation$ Vertical=�68   .+�5'      �� $��20(�6(   
���������6L   �� *Name+�� 5(      
�������$��16(�6:   ��6H   �� *
AboutActor+ 5)      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >�$ DataContext��� 5*      
�������$��16(�6:   ��6H   �� *ActorBirthday+ 5+      ��  �� *FindAncestor+" >�$ DataContext��� 5,      
�������$��3q�6>   $��14(�6I   ��6W   �� *
BirthDay+ �� StringFormat$ D�� �� ConverterCulture$ ru-RU���� 5-      
�������$��16(�6:   ��6H   �� *ActorBirthPlace+ 5.      ��  �� *FindAncestor+" >�$ DataContext��� 5/      
�������$��3q�6>   $��14(�6I   ��6W   �� *
BirthPlace+�� 50      
�������$��16(�6:   ��6H   �� *ActorHomePage+ 51      ��  �� *FindAncestor+" >�$ DataContext��� 52      
�������$��3q�6>   $��14(�6I   ��6W   �� *
HomePage+53      54      55      �� 56   
   $��2��6   
���   �$��1q�62   ._�57      ��  6   �� *DirectActor+.+�58      �� $��16(�6    
���������6D   �� *	Biography+ 59       ��  �� *FindAncestor+" >�$ DataContext��� 5:      
�������$��3q�6:   $
��Wrap=�6E   $��14(�6Y   ��6g   �� *	Biography+5;      5<      � 5=   
   $��1��6    5>      �� 5?      $��* �6&   62   �� 5@      $��* �6&   62   .+�5B      �� $��0��6   $��16(�6-   $��35��6;   ��6G   �� *AddFavorites+��6h   �� *NavigateCommandAddToDb+{�6�   �� *
CanAddToDb+$ PropertyChanged=� 5C      ��  �� *FindAncestor+" >�$ DataContext��� 5D      $��1��6   $��16(�6-   $��35��6;   ��6G   �� *DelFavorites+��6h   �� *NavigateCommandDellFromDb+{�6�   �� *CanDelFromDb+$ PropertyChanged=� 5E      ��  �� *FindAncestor+" >�$ DataContext�5F      ]� 5G   
   $��3��6   .+�5H      �� $��16(�6   $	��Top=�6"   
���������6M   �� *Filmography+�� 5I      ��6   �� $ ActorMovies� �� SelectedItem 6A   �� $ SelectedActorMovie�$	 TwoWay=���5J      �� 5K       �� 
TargetType" ��6    .�5L      ��  �� Property � �� �� Value �	#4C0000006;   �� 5M       � �� �	#FF0000006;   �� 5N       � ��$ 1q�67   �� 5O       � ��$ 1q�68   �� 5P       � ��$ 1q�6@   �� 5Q       � ��
 �   �6<   �� 5R      0	   *ModuleMainModule.Behavior.MouseDoubleClick �@	 Command 6R   �� *,*DataContext.NavigateCommandShowDirectMovie+ �� *FindAncestor+" >��� 5S       �@	 CommandParameter 6[   �� *#!DataContext.SelectedActorMovie.Id+ �� *FindAncestor+" >� �� Triggers 5T   "   P� 5U   "    P� Property �@��
IsSelected P� Value . . �5V   &   ��  � ��
 �����6C   �� 5W   &    � ��$ 1q�6H   �� 5X   &    � ��
 �   �6D   5Y   #   5[      ��5]      �� 5^      .R�5_   "   A� $
��1090��6.   $	��540��6>   ���	#4C000000 A� Columns$ 3��6d   5`      ��5b      �� 5c      .R�5d      �� $ 
Horizontal=�6)   $��1q�6B   $	��180��6M   $	��330��6\   .+�5e   "   �� $��70��6(   $��50��64   ��6?   �� *Poster+# [  �� 5f   "   $ Vertical=�6-   .+�5g   &   �� $��1q�60   $��12(�6;   ��6I   �� *Title+�� 5h   &   $��1q�60   $��12(�6;   ��6I   �� *ReleaseDate+$ D��$ ru-RU��5i   #   5j      5k      5m      5n      5o      5p      5q      5r      
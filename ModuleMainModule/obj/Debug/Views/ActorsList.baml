   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      E'clr-namespace:ModuleMainModule.BehaviorModuleMainModule.Behavior  5
      '    !ModuleMainModule.Views.ActorsList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @
converters)clr-namespace:ModuleMainModule.Converters     5      X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5	      6behavior'clr-namespace:ModuleMainModule.Behavior  5
      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $
��auto��5      $
��auto��6   $	��350��6)   $��Center=�68   $��Center=�6U    � 	Resources 5      3   -ModuleMainModule.Converters.ImageUrlConverter   PathConverter��%   &	          5   
   6'   5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator     IsBusy 6   �� *BusyIndicatorValue+ �� UpdateSourceTrigger$ PropertyChanged=�   BusyContent 6d   �� *LoadingData+.��5   
   ]� -   5         ]�Name$ DockPanelMain��5   )   ��. .  (System.Windows.Interactivity.Interaction   Triggers 5   
   F  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  5      3  -System.Windows.Interactivity.EventTriggerBase   SourceObject 6.   �� *NotificationRequest+ �� Mode$ OneWay=�.  (System.Windows.Interactivity.TriggerBase	   Actions.	 5      +  %Prism.Interactivity.PopupWindowAction  
   IsModal
 .    WindowStartupLocation$ CenterOwnerX�69   !   CenterOverAssociatedObject .  6]   5        5       6.   �� *NotificationRequestNull+$ OneWay=�.	 5        
 . $ CenterOwnerX�69    .  6]   5      .+�5   
   �� $��5q�6   $	��Top=�6   
���iii�$��2q�6H    �� CornerRadius$ 5��6\   ._�5      �� $��15(�6   $��Heavy"�6"   $��Center=�65   ��6Y   �� *Title+5      �� 5   
   $
��Auto=�6   $
��Auto=�6:   ��. .��5      �� -   5         ��Name$ LbMain��5      ���	#4C000000��6'   �� *
ActorsList+ �� SelectedItem 6J   ��  �� Path$ SelectedSearchedActor�$ TwoWay=���5       �� 5!       �� 
TargetType" ��6   .�5"      ��  �� Property � �� �� Value �	#4C00000063   �� 5#       � �� �	#FF0000006G   �� 5$       � ��$ 1q�6/   �� 5%       � ��$ 1q�60   �� 5&       � ��$ 1q�68   �� 5'       � ��
 �   �64   �� 5(      0	   *ModuleMainModule.Behavior.MouseDoubleClick �@	 Command 6J   �� *,*DataContext.NavigateCommandShowDirectActor+ �� RelativeSource �� *FindAncestor+ �� AncestorType" >��� 5)       �@	 CommandParameter 6S   �� *&$DataContext.SelectedSearchedActor.Id+ �� *FindAncestor+" >� �� Triggers 5*      P� 5+       P� Property �@��
IsSelected P� Value . . �5,      ��  � ��
 �����6;   �� 5-       � ��$ 1q�6@   �� 5.       � ��
 �   �6<   5/      51      ��53      �� 54      .R�55      ��  �� Orientation$ 
Horizontal=�6!   $��5q�6:   .+�56   "   �� $	��150��6l   $	��150��6y   ��6(   �� *Poster+ �� 	Converter# [  6y   �� 57   "   $ Vertical=�6-   .+�58   "   �� 
�������$��3q�6X   $��14(�6c   ��6B   �� *Name+6c   �� 59   "   
�������$��3q�6\   $��14(�6g   ��6B   �� *
BirthDay+6g   5:      5;      5<      5>      5?      5@      5A      5B      
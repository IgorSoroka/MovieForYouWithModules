   M S B A M L   `   `   `  ����K  GModuleMainModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ModuleMainModule.ConvertersModuleMainModule.Converters  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      E'clr-namespace:ModuleMainModule.BehaviorModuleMainModule.Behavior  5
      &     ModuleMainModule.Views.ShowsList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @
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
   $��Bottom=�6   $��5q�6)   
���iii�$��2q�6J   $ 5��6^   ._�5   
   � $��30��6   $��5q�6    � ColumnDefinitions 5       �� 5!      $��* �6#   �� 5"      $��* �6#   .+�5$      �� $��15(�6   $��Center=�6'   $��3q�6I   ��6T   �� *
Privious+��6q   �� *!NavigateCommandShowPriviousPage+�� 5%      $��15(�6   $��Center=�6'   $��3q�6I   $��1��6T   ��6d   �� *Next+��6}   �� *NavigateCommandShowNextPage+5&      5'      �� 5(      $��5q�6   $
��Auto=�6&   $
��Auto=�6I   ��. .��5)      �� ���	#4C000000��6   �� *Shows+ �� SelectedItem 64   �� *SelectedShow+$ TwoWay=���5*      �� 5+       �� 
TargetType" ��6   .�5,      ��  �� Property � �� �� Value �	#4C00000063   �� 5-       � �� �	#FF0000006G   �� 5.       � ��$ 1q�6/   �� 5/       � ��$ 1q�60   �� 50       � ��$ 1q�68   �� 51       � ��
 �   �64   �� 52      0	   *ModuleMainModule.Behavior.MouseDoubleClick �@	 Command 6J   �� *+)DataContext.NavigateCommandShowDirectShow+ �� RelativeSource �� *FindAncestor+ �� AncestorType" >��� 53       �@	 CommandParameter 6S   �� *DataContext.SelectedShow.Id+ �� *FindAncestor+" >� �� Triggers 54      P� 55       P� Property �@��
IsSelected P� Value . . �56      ��  � ��
 �����6;   �� 57       � ��$ 1q�6@   �� 58       � ��
 �   �6<   59      5;      ��5=      �� 5>      .R�5?      ��  �� Orientation$ Vertical=�6%   $��Center=�6<   .+�5@      � $	��200��6#   $	��310��60   .+�5A   &   �� $��2��6,   $
��Fill=�6=    J� RenderTransformOrigin$ 0.5,0.5&�6L   $��Stretch=�6�   ��6l   �� *
Backdrop+ �� 	Converter# [  z�5B   *   �� 5C   .   ScaleTransform�� �� ScaleX$
 0.97Q�6U    �� ScaleY$
 1.03Q�6c    � Triggers 5E   *   :� 5F   .    :� RoutedEvent$ 
MouseEnter��6;   .[�5G   2   �� .��5H   6   �� .��5I   :   W�  �� 
TargetName$ ScaleTransform��6J     �� TargetProperty$  ScaleX�6q   ! W� From$
! 0.98X�6�   	" W� To$
" 1.02X�6�   # g� Duration$# 0:0:2D�6�   $ g� AutoReverse$ . % g� RepeatBehavior$% 1x��6�   W� 5J   :   $ ScaleTransform��6J   $  ScaleY�6q   $
! 0.98X�6�   $
" 1.02X�6�   $# 0:0:2D�6�   $ . $% 1x��6�   5K   7   5L   3   5M   /   :� 5N   .   $ 
MouseLeave��6;   .[�5O   2   �� .��5P   6   �� .��5Q   :   W� $ ScaleTransform��6J   $  ScaleX�6q   $
! 1.00X�6�   $
" 1.00X�6�   $ . $% Forever��6�   W� 5R   :   $ ScaleTransform��6J   $  ScaleY�6q   $
! 1.00X�6�   $
" 1.00X�6�   $ . $% Forever��6�   5S   7   5T   3   5U   /   5W   '   �� 5X   "   $��1��6I   $��Center=�6V   $��Stretch=�6z   $��15(�6�   
���   �$��Bottom=�6�   ��6(   �� *OriginalName+��5Y   &   �� 5Z   *   & �� Color$& Azure��6:   ' �� Opacity$	' 0.7Q�6H   5\   #   ( � RowDefinitions( 5]   "   �� 5^   &   $��* �64   �� 5_   &   $��* �64   5a      5b      5c      5e      5f      5g      5h      5i      
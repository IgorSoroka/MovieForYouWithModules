   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5   	   ` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35tNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary!Microsoft.Windows.Controls.Ribbon 5
   	    
TestModule   TestModule.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   P LPrism.Wpf, Version=6.2.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5   	   O KEO.Wpf, Version=17.1.14.0, Culture=neutral, PublicKeyToken=e92353a6bf73fffc2wpf(http://schemas.essentialobjects.com/wpf/  5   	   [ribbonNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary  5
   	   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $	��475��5   !   $	��475��60   $	��800��6@   $
��1200��6M    � WindowStartupLocation$ CenterScreen=�6Z    � Title 6	   �� *Title+.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $	��300��6   $��9* �6.   �� 5      $	��150��6   $	��450��6.   $��4* �6=   .   (System.Windows.Interactivity.Interaction   Triggers 5   
   F  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  5      3   -System.Windows.Interactivity.EventTriggerBase   SourceObject 6.   �� *NotificationRequestConnection+ �� Mode$ OneWay=�.   (System.Windows.Interactivity.TriggerBase   Actions. 5      +  %Prism.Interactivity.PopupWindowAction  	   IsModal	 . 
   WindowStartupLocation$
 CenterOwnerX�69   !   CenterOverAssociatedObject .  6]   5      .+�5   
   .  (Microsoft.Windows.Controls.Ribbon.Ribbon  $��2��6   
����������	#FF595959   ApplicationMenu 5      =  7Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenu  5      ���	#4C000000���	#FF000000.@�5      A	  ;Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenuItem	  -   5         	 Name$ MenuItemMin��5   I   $��Свернуть��67    m� Command 6^   �� *ApplicationCommandMinimize+	  5       -   5        $ MenuItemOpen��5    K   $��Развернуть��67    6a   �� *ApplicationCommandMaximaze+	  5!      -   5        $ MenuItemClose��5!   H   $��Закрыть��67    6_   �� *ApplicationCommandClose+5"      .@�5$      4
  .Microsoft.Windows.Controls.Ribbon.RibbonButton
  $��	StartView��6e    
  Label 6"   �� *
MainView+��6=   �� *NavigateCommandMain+6e   1  +Microsoft.Windows.Controls.Ribbon.RibbonTab  5%      ��6   �� *Movies+.@�5&      3  -Microsoft.Windows.Controls.Ribbon.RibbonGroup  .@�5'      
  $��Center=�6A   $	��100��6^   $
��Best��6�    6*   �� *Best+ 
  LargeImageSource 6j   �� *
BestPath+��6�   �� *NavigateCommandListMovie+6�   5(        5)      .@�5*      
  $��Center=�6D   $	��100��6a   $��Popular��6�    6*   �� *	Popular+ 6m   �� *PopularPath+��6�   �� *NavigateCommandListMovie+6�   5+        5,      $�� ��6%   .@�5-      
  $��Center=�6E   $	��100��6b   $��Future��6�    6*   �� *
UpComing+ 6n   �� *FutureMoviePath+��6�   �� *NavigateCommandListMovie+6�   5.        5/      .@�50      
  $��Center=�6G   $	��100��6d   $	��Now��6�    6*   �� *
NowPlaying+ 6p   �� *NowMoviePath+��6�   �� *NavigateCommandListMovie+6�   51      52        53      ��6   �� *Shows+.@�54        .@�55      
  $��Center=�6A   $	��100��6^   $
��Best��6�    6*   �� *Best+ 6j   �� *
BestPath+��6�   �� *NavigateCommandListShow+6�   56        57      .@�58      
  $��Center=�6D   $	��100��6a   $��Popular��6�    6*   �� *	Popular+ 6m   �� *PopularPath+��6�   �� *NavigateCommandListShow+6�   59        5:      .@�5;      
  $��Center=�6D   $	��100��6a   $	��Now��6�    6*   �� *	NowOnTv+ 6m   �� *NowShowPath+��6�   �� *NavigateCommandListShow+6�   5<      5=        5>      ��6   �� *Search+.@�5?        .@�5@      
  $	��100��6H   $��Center=�6T   $��MovieSearchView��6�    6*   �� *SearchMovie+ 6q   �� *SearchMoviePath+��6�   �� *NavigateCommandMain+6�   5A        5B      .@�5C      
  $	��100��6G   $��Center=�6S   $��ShowSearchView��6�    6*   �� *
SearchShow+ 6p   �� *SearchShowPath+��6�   �� *NavigateCommandMain+6�   5D        5E      .@�5F      
  $	��100��6H   $��Center=�6T   $��ActorSearchView��6�    6*   �� *SearchActor+ 6q   �� *SearchActorPath+��6�   �� *NavigateCommandMain+6�   5G      5H        5I      ��6   �� *
Selected+.@�5J        .@�5K      
  $	��100��6K   $��Center=�6W   $��Favorite��6�    6*   �� *SelectedMovies+ 6t   �� *FavoriteMoviePath+��6�   �� *NavigateCommandListMovie+6�   5L        5M      .@�5N      
  $	��100��6J   $��Center=�6V   $��Favorite��6�    6*   �� *SelectedShows+ 6s   �� *FavoriteShowPath+��6�   �� *NavigateCommandListShow+6�   5O        5P      .@�5Q      
  $	��100��6K   $��Center=�6W   $��Favorite��6�    6*   �� *SelectedActors+ 6t   �� *FavoritActorPath+��6�   �� *NavigateCommandListActor+6�   5R      5S      5T      ]� 5U   
   $��1��6   $��1��6!   ��. ��5V      �� 5W       j� Stretch$ UniformToFill=�6N    �� ImageSource 6   �� *MainRegionBackgroundPath+6N   .+�5Y      �� !  Prism.Regions.RegionManager   
RegionName$ 
ListRegion��6!   $��Stretch=�6M   $��Stretch=�6k   5Z      ]� 5[   
   $��1��6   $��0��6!   ��5\      �� 5]      $ UniformToFill=�6N    6   �� *ListRegionBackgroundPath+6N   .+�5_      �� $ 
MainRegion��6   $��Stretch=�6I   $��Stretch=�6g   5`      5a      5b      
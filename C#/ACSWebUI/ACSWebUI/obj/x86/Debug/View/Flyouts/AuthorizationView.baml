   M S B A M L   `   `   `  ����C  ?ACSWebUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ACSWebUIACSWebUI  5      -    'ACSWebUI.View.Flyouts.AuthorizationView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      +acsWebUiclr-namespace:ACSWebUI     5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;controls,http://metro.mahapps.com/winfx/xaml/controls  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      R NMahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      #  MahApps.Metro.Controls.Flyout    CloseButtonIsCancel  .     CloseButtonVisibility$ 	Collapsed=�5         IsAutoCloseEnabled .     IsModal .    Position$	 Top=�5         Theme$
 Dark=�5      ��5
      �� *ActualWidth+ �� Mode$ OneWay=� �� RelativeSource �� *FindAncestor+ �� AncestorType   ACSWebUI.MainWindow"  ��5      �� *ActualHeight+$ OneWay=� �� *FindAncestor+"  	   IsOpen	 5      �� *IsFlyoutOpen+$ OneWay=�.  (System.Windows.Interactivity.Interaction
   Triggers
 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *OnLoadedCommand+5      .��5      �  � RowDefinitions 5   
   �� 5      $	��67* �6   �� 5      $	��67* �6   �� 5       $	��67* �6   .+�5#   
   �� $��0��6   $��Bottom=�5$      $��Center=�5%      $��Center=�5&      $��Tahoma)�5'      $��30(�5(      y�5)      �� *IsAuthorizationVisible+.��5*      �� $��yПожалуйста, проверьте ваше устройство на административной панели��6    �� TextAlignment$ Center=�5+      $
��Wrap=�5,      5-      �� 5/   
   $��1��6   $��
50,20,50,0q�50      $	��Top=�51       �� AutoWordSelection . $��30(�53       �� 
IsReadOnly .  �� IsReadOnlyCaretVisible .   �� TextAlignment$ Center=�57       �� TextWrapping$ NoWrap=�58      ��56      �� *
UniqueId+y�59      �� *IsAuthorizationVisible+�� 5;   
   $��1��6   $��80��5<      $��30��5=      $��
0,0,100,60q�5>         SquareButtonStyle#��[  5@      ��5?      �� *CheckCommand+y�5A      �� *IsAuthorizationVisible+.��6?   Проверить5C      �� 5E   
   $��1��6   $��80��5F      $��30��5G      $��
100,0,0,60q�5H        AccentedSquareButtonStyle#��[ 5J      ��5I      �� *ExitCommand+y�5K      �� *IsAuthorizationVisible+.��6?   
Выход5M      )  #MahApps.Metro.Controls.ProgressRing  5P   
   $��0��6    $��3��5Q          IsActive 5R       �� *IsProgressRingActive+�� 5T   
   $��0��6   $��3��5U      $��Center=�5V      $��Center=�5W      y�5X      �� *!IsAuthorizationResponceVisibile+.��5Y      �� $��Tahoma)�6   $��30(�5Z      $ Center=�5\      $
��Wrap=�5]      ��5[      �� *AuthorizationResponce+5]      5^      �� 5`   
   $��1��6   $��
0,0,250,60q�5a      $��Center=�5b      $��Center=�5c        MetroCircleButtonStyle#��[ 5e      ��5d      �� *OpenSettingCommand+y�5f      �� *IsAuthorizationVisible+.��5g      .  (MahApps.Metro.IconPacks.PackIconMaterial  $��30��6)   $��30��5h   )   �	  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null]] 	  Kind$ Settings=�5i   )   5j      �� 5l   
   $��2��6   .+�5m      ��  �� Orientation$ 
Horizontal=�6   .+�5n      �� $	��300��6   $��Center=�5o      $��Tahoma)�5p      $��30(�5q      $
 Left=�5r      y�5s      �� *IsSettingsVisibility+#!Адрес подключения5u      �� 5v      -   5         ��Name$ textBox��5v      $	��500��5w      $��20,0,0,0q�5x      $��Center=�5y      $	��Top=�5z       . $��30(�5|       .  .  $
 Left=�5�      $ NoWrap=�5�      ��5      �� *GetRequestAddress+$ TwoWay=�y�5�      �� *IsSettingsVisibility+5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� $	��300��6   $��Center=�5�      $��Tahoma)�5�      $��30(�5�      $
 Left=�5�      y�5�      �� *IsSettingsVisibility+
Домен5�      �� 5�      $	��500��5�      $��20,0,0,0q�5�      $��Center=�5�      $	��Top=�5�       . $��30(�5�       .  .  $
 Left=�5�      $ NoWrap=�5�      ��5�      �� *ActualWidth+ �� ElementName$ textBox��$ OneWay=���5�      �� *DomenAddress+$ TwoWay=�y�5�      �� *IsSettingsVisibility+5�      �� 5�      $��80��6   $��30��5�      $��0,0,0,0q�5�      $
��Left=�5�      #��[  5�      ��5�      �� *SaveChahgesCommand+y�5�      �� *IsSettingsVisibility+.��6A   Сохранить5�      5�      5�      5�      
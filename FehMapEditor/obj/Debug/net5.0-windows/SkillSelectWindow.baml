   M S B A M L   `   `   `  ????G  CFehMapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:FehMapEditorFehMapEditor  5   	   $    FehMapEditor.SkillSelectWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:FehMapEditor     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	      MaterialDesignBody#???   5	   	   $??Regular"?5
   	   $??13(?5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=?5   	      TextRenderingMode$
 Auto=?5   	     MaterialDesignPaper#???  5   	     MaterialDesignFont#???  5   	    ? Title$ SkillSelectWindow??5   	   $	??700??6#   $	??800??60   .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $	??200 ?6   ?? 5      .+?5   
   ?? .+?5      ?? -   5         ??Name$ SkillSearchField??5        MaterialDesignFilledTextBox#??[ 60   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Search Name??5      .  (MaterialDesignThemes.Wpf.TextFieldAssist   HasClearButton$
 True??6B   $?? ??5       ? Cursor$ IBeam??6   6,   ?? 5      -   5         ??Name$ WeaponTypeFilter??5       " MaterialDesignOutlinedComboBox#??[ 61   $
 True??6i   $ 
WeaponType??5      6A   ?? 5      -   5        $ MoveTypeFilter??5      #??[ 6/   $
 True??6g   $ MoveType??5      6?   ?? 5      -   5        	 ??Name$	 AssistChecker??5      -  'MaterialDesignThemes.Wpf.CheckBoxAssist
   CheckBoxSize$
 30Q?6.    d? 	IsChecked$ FalseY?5      $??Assist??5       ?? 5!      -   5        $	 SpecialChecker??5!      $
 30Q?6/   $ FalseY?5"      $??Special??5#      ?? 5$      -   5        $	 AChecker??5$      $
 30Q?6)   $ FalseY?5%      $??A??5&      ?? 5'      -   5        $	 BChecker??5'      $
 30Q?6)   $ FalseY?5(      $??B??5)      ?? 5*      -   5        $	 CChecker??5*      $
 30Q?6)   $ FalseY?5+      $??C??5,      ?? 5-      -	   5        $	 ExclusiveChecker??5-      $
 30Q?61   $ FalseY?5.      $??	Exclusive??5/      ]? 50      .+?51      ?? $??SP??6   $??Center=?6&   $??10,10,10,10q?6A   ?? 52      -
   5         ??Name$ SpSlider??52       ? Value$	 100Q?6+    ? Minimum$	 100Q?67    ? Maximum$	 500Q?6E      MaterialDesignDiscreteSlider#??[ 6S    ?? Ticks$% 0, 200, 240, 300, 350, 400, 500R?53       ?? TickPlacement$
 Both=?6=   6R   54      ?? 56      -   5        $	 RefineChecker??56      $
 30Q?6.   $ FalseY?57      $??Refined??58      ?? 59      -   5        $	 CantoChecker??59      $
 30Q?6-   $ FalseY?5:      $??Canto??5;      5<      ?? 5=   
   -   5         ??Name$ 	SkillList??5=      $??1??6%   ??5>      ?? 5?      .R?5@      ?? .+?5A      #  MaterialDesignThemes.Wpf.Chip     IconBackground
 ???? ??6.   ?? *Name+   Icon 5C      ?? 5D   "   ??6(   ?? *Icon+5F      ?? 5G       ! MaterialDesignOutlinedTextBox#??[ 5I   !    ?? AcceptsReturn$
 True??5J   !    ?? TextWrapping$
 Wrap=?5K   !   ${?False??5L   !   ??5H   !   ?? *Description+ ?? Mode$ OneWay=?5L   !   5M      5N      5P      5Q      5R      
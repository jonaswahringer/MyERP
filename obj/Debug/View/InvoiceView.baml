   M S B A M L   `   `   `  ����e  aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089wTclr-namespace:System.Windows.Controls.Ribbon;assembly=System.Windows.Controls.RibbonSystem.Windows.Controls.Ribbon  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   @ <MyERP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MyERP.View
MyERP.View 5   	   1clr-namespace:MyERP.ViewModelMyERP.ViewModel 5	   	      MyERP.View.View   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   gribbonTclr-namespace:System.Windows.Controls.Ribbon;assembly=System.Windows.Controls.Ribbon     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   *localclr-namespace:MyERP.View     5   	   -srcclr-namespace:MyERP.ViewModel     5	   	     � Title$
  View��5   	   $	��450��6   $	��800��6#    � DataContext 5      &  MyERP.ViewModel.InvoiceViewModel  5   
   .��5      �� .+�5   
   ]� .+�5      +   %System.Windows.Controls.Ribbon.Ribbon  -   5        $	��Top=�5         QuickAccessToolBar 5      =   7System.Windows.Controls.Ribbon.RibbonQuickAccessToolBar  5      .@�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      1   +System.Windows.Controls.Ribbon.RibbonButton     SmallImageSource$ /images/cut_24dp.png��62   $��Center=�6Z     5      $ /images/copy_24dp.png��62   $��Center=�6[     5      $ /images/paste_24dp.png��62   $��Center=�6\   5      5         ApplicationMenu 5      :   4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5      5   /System.Windows.Controls.Ribbon.RibbonMenuButton   SmallImageSource$  /images\menu_open_24dp.png��63   .@�5       >   8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   5          Name$ 	MenuItem1��5!   7   $��Hello _Ribbon��5    ;   3   -System.Windows.Controls.Ribbon.RibbonMenuItem   ImageSource$ /images\print_24dp.png��5"   7   	 m� Command	 5#   ;   �� 
 �� Path$
 PrintCommand�5$      .@�5'      .	   (System.Windows.Controls.Ribbon.RibbonTab	  $��Start��6#   .@�5(      0
   *System.Windows.Controls.Ribbon.RibbonGroup
  .@�5)           Label$	 Cut��62      LargeImageSource$ /images/cut_24dp.png��6>     5*      $
 Copy��62   $ /images/copy_24dp.png��6?     5+      $ Paste��62   $ /images/paste_24dp.png��6@   5,      
  5-      .@�5.        $	 Cut��62   $ /images/cut_24dp.png��6>     5/      $
 Copy��62   $ /images/copy_24dp.png��6?     50      $ Paste��62   $ /images/paste_24dp.png��6@   51      52      	  53      $
��File��6#   .@�54      
  .@�55        $ Print��62   $ /images/print_24dp.png��6@   .@�56        	 64   �� $
 PrintCommand�57      58      
  59      .@�5:        $ Print Preview��62   $ /images/preview_24dp.png��6H   5;      
  5<      .@�5=        $
 Info��62   $ /images/info_24dp.png��6?   5>      
  5?      .@�5@        $
 Exit��62   $ /images/exit_24dp.png��6?   5A      5B      	  5C      $	��New��6#   ��61   �� $
 InvoiceToAdd�.@�5D      
  .@�5E        .@�5F      �� $��/images/customer_24dp.png��6$   2   ,System.Windows.Controls.Ribbon.RibbonTextBox  5G         Command 63   �� $
 InvoiceToAdd.CustomerName�5H        5J      .@�5K      �� $��/images/address_24dp.png��6$     5L       63   �� $"
 InvoiceToAdd.CustomerAddress�5M        5O      .@�5P      �� $��/images/amount_24dp.png��6$     5Q       63   �� $
 InvoiceToAdd.Amount�5R        5S      .@�5T      �� $��/images/vat_24dp.png��6$     5U       63   �� $
 InvoiceToAdd.Vat�5V      5W      5X      5Y      5Z      �� 5[   
   .@�5\      �� $��	Dashboard��6   .��5]      �  � ColumnDefinitions 5^      �� 5_      �� 5`       � RowDefinitions 5b      �� 5c      $
��0.5* �6(   �� 5d      $
��0.5* �6(   .+�5g      �� $��0��6!   $��0��61   $ 
Horizontal=�6>   $
��Left=�6W   .+�5h      �� $��10q�6    .��6,   Name62   �� 5i      $��80��6R   $��10q�6]   ��6"   �� $
 InvoiceToAdd.CustomerName�6k   �� 5j      $��10q�6    .��6,   	Adresse65   �� 5k      $��80��6U   $��10q�6`   ��6"   �� $"
 InvoiceToAdd.CustomerAddress�6n   �� 5l      $��10q�6    .��6,   Amount64   �� 5m      $��80��6L   $��10q�6W   ��6"   �� $
 InvoiceToAdd.Amount�6e   �� 5n      $��10q�6    .��6,   Vat61   �� 5o      $��80��6I   $��10q�6T   ��6"   �� $
 InvoiceToAdd.Vat�6b   5p      �� 5q      $ 
Horizontal=�6!   $��0��6:   $
��Left=�6G   $��	667,0,0,0q�6b   .+�5r      �� $	��Add��6!   $��10q�6/   $��46.4933333333333��6_   ��6;   �� $
 
AddCommand�6z   �� 5s      $��Remove��6!   ��62   �� $
 RemoveCommand�6[   5t      �� 5v      $��0��6!   $��1��61   .+�5w      �� ��6#   �� $
 InvoiceList� �� SelectedItem 6L   �� $
 SelectedInvoice� �� View 5x      �� 5y   "   .P�5z   &   �� $
��Name��65    �� Width$	 150��6C    �� DisplayMemberBinding 6O   �� $
 CustomerName��� 5{   &   $��Address��65   $	 150��6F    6R   �� $
 CustomerAddress��� 5|   &   $��Amount��65   $	 150��6E    6Q   �� $
 Amount��� 5}   &   $
��Date��65   $	 150��6C    6O   �� $
 InvoiceDate��� 5~   &   $	��Vat��65   $	 150��6B    6N   �� $	
 Vat�5   #    � ContextMenu 5�      �� 5�   "   .@�5�   &   m� $��Remove��6/   	 6?   �� $
 RemoveCommand�6h   5�   #   5�      5�      �� 5�      $��0��6!   $��1��61   .+�5�      ��  �� SelectionMode$ Multiple=�6Z   $ SelectedPositions��6s   ��6#   �� $
 SelectedInvoice.Positions� 5�      �� 5�   "   .P�5�   &   �� $��ItemNr��65   $	 150��6E    6Q   �� $
 ItemNr��� 5�   &   $��Price��65   $	 150��6D    6P   �� $
 Price��� 5�   &   $	��Qty��65   $	 150��6B    6N   �� $	
 Qty�5�   #    5�      �� 5�   "   .@�5�   &   m� $��Remove��6/   	 6?   �� $
 RemoveCommand�6h   5�   #   5�      5�      �� 5�      $��1��6!   .+�5�        LiveCharts.Wpf.PieChart  -   5        $��Chart��5�   '   $��1��64   &   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�5�         InnerRadius$	 100Q�6,      Series 5�   A   �� *SeriesAmountInvoicePosition+5�      5�      5�      5�      �� 5�      $��BubbleChart��6   .��5�      �� $
��Test��6   5�      5�      5�      5�      
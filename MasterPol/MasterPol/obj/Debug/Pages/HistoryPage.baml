   M S B A M L   `   `   `  ����D  @MasterPol, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MasterPol.PagesMasterPol.Pages  5      !    MasterPol.Pages.HistoryPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:MasterPol.Pages     5        N� Title$j  dИС Мастер Пол - истории реализации продукции партнером��5	       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $��70 �6   6*   �� 5      $��* �6   6)   �� 5      $��70 �6   6*   .+�5   
   �� $O��IИстории реализации продукции партнёром��6   $��Center=�6A   $��Center=�5      $
��Bold"�5      $��25(�5      6    &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ HistoryDataGrid��5      $��1��6,      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  $��Center=�5      $��Center=�5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Наименование��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 5       �� *Production.NameOfProduction+6G     5!      $- 'Количество продукции��6%   	 5"      �� *CountOfProduction+6=     5#      $ Дата продажи��6%   	 5$      �� *
DateOfSale+
 �� StringFormat$
 {0:yyyy-MM-dd}��6W   5&      �� 5(   
   -   5         ��Name$ 
BackButton��5(      $��2��6%   $��
Назад��5)      $��Center=�6!   5*   ,   5+      5,      
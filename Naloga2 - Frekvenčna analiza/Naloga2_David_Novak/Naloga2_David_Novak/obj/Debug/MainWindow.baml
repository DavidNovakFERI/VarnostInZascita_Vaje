   M S B A M L   `   `   `  ����  Naloga2_David_Novak#    FrekvenčnaAnaliza.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Frekvenčna Analiza��5   	   $	��450��6$   $	��800��61   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Šifrirano besedilo:��6   $��0��6.   �� 5   
   -   5         ��Name$ EncryptedTextBox��5      $��1��6,   $	��0,5q�69   �� 5   
   $��Referenčno besedilo:��6   $��2��6/   $��0,10,0,0q�6<   �� 5   
   -   5        $ ReferenceTextBox��5      $��3��6,   $	��0,5q�69   �� 5   
   -   5        $$��Analiziraj šifrirano besedilo��5      $��4��69   $��0,10,630,10q�6F   6[   �� 5   
   -   5        $%��Analiziraj referenčno besedilo��5      $��4��6:   $��150,10,485,226q�6G   $��2��6�   �� 5   
   -   5        $��Delno razkrij besedilo��5      $��4��62   $��300,10,330,226q�6?   $��2��6y   �� 5   
   -   5        $!��Odpravi shranjeno razkritje��5      $��4��67   $��
450,10,0,0q�6D   6X   �� 5   
   -   5        $ DecryptedTextBox��5      $��5��6,   $
��0,10q�69    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6\   �� 5   
   $$��Ročna zamenjava črk ključa:��6   $��5��66   $��0,49,0,0q�6C   �� 5   
   -   5        $ ManualReplacementTextBox��5      $��5��64   $��0,65,0,0q�6A   �� 5    
    �� Orientation$ 
Horizontal=�6   $��5��6.   $��0,49,0,-151q�6;   .+�5!      �� -	   5        $%��Odpiranje šifriranega besedila��5!      $��0,0,10,0q�6>   $��23��6p   �� 5"      -
   5        $&�� Odpiranje referenčnega besedila��5"      $��0,0,10,0q�6?   $��17��6q   �� 5#      -   5        $"��Shrani dešifrirano besedilo��5#      $��21��6[   5$      5%      5&      
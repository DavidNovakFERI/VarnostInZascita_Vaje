   M S B A M L   `   `   `  ����M  INaloga3_DavidNovak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Naloga3_DavidNovakNaloga3_DavidNovak  5   	   #    Naloga3_DavidNovak.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Naloga3_DavidNovak     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� -   5         ��Name$	 AES��5
      $	��AES��6   $��Center=�6+   $��36��6H   $��0,10,0,0q�6T   $	��Top=�6f   $	��780��6~   $��20(�6�   ���	#FFAD9B9B6�   �� 5   
   -   5        $ ��Nalaganje Izvorne Datoteke��5      $
��Left=�66   $��	10,66,0,0q�6Q   $	��Top=�6d   6|   �� 5   
   -   5        $3��-Nalaganje in shranjevanje šifrirane datoteke��5      $
��Left=�6H   $��
10,104,0,0q�6c   $	��Top=�6w   $	��236��6�   6�   �� 5   
   -   5        $(��"Shranjevanje dešifrirane datoteke��5      $
��Left=�6=   $��
10,141,0,0q�6X   $	��Top=�6l   $	��182��6�   6�   �� 5   
   $��Shrani AES Dekripani file��6   $
��Left=�65   $��25��6P   $��
629,51,0,0q�6\   $	��Top=�6p   $	��133��6�   �� 5   
   $)��#Shrani AES Encrypted file kot tekst��6   $
��Left=�6?   $��
572,86,0,0q�6Z   $	��Top=�6n    J� RenderTransformOrigin$ 0.862,0.512&�6�   $	��190��6�   �� 5   
   $��Save IV��6   $
��Left=�6#   $��629,115,0,0q�6>   $	��Top=�6S   $	��133��6k   �� 5   
   $��Shrani Encryption key��6   $
��Left=�61   $��629,151,0,0q�6L   $	��Top=�6a   $ 1.594,0.191&�6y   $	��133��6�   �� 5   
   $	��RSA��6   $��Center=�6   $��	0,217,0,0q�6;   $	��Top=�6N   $	��780��6f   ���	#FF89A6FF$��20(�6�   �� 5   
   -   5        $ ��Nalaganje Izvorne Datoteke��5      $
��Left=�66   $��
10,250,0,0q�6Q   $	��Top=�6e   6}   �� 5   
   $��Set public key��6   $
��Left=�6*   $��
10,297,0,0q�6E   $	��Top=�6Y   $	��109��6q   $ 0.5,0.5&�6}   $��22��6�   �� 5   
   $��Set private key��6   $
��Left=�6+   $��
10,324,0,0q�6F   $	��Top=�6Z   $	��109��6r   �� 5   
   $��Save RSA Decrypted File��6   $
��Left=�63   $��629,272,0,0q�6N   $	��Top=�6c   $	��133��6{   �� 5   
   $��Save RSA Encrypted File��6   $
��Left=�63   $��629,297,0,0q�6N   $	��Top=�6c   $	��133��6{   �� 5   
   $��Save RSA Private Key��6   $
��Left=�60   $��627,322,0,0q�6K   $	��Top=�6`   $	��135��6x   �� 5   
   $��Save RSA Public Key��6   $
��Left=�6/   $��629,347,0,0q�6J   $	��Top=�6_   $	��133��6w   �� 5   
   -   5         ��Name$ comboBoxAlgorithm��5      $
��Left=�6.   $��629,186,0,0q�6I   $	��Top=�6^   $	��120��6v   .@�5      �� $	��AES��6   �� 5      $	��RSA��6   5      �� 5   
   -   5        $ comboBoxKeyLength��5      $
��Left=�6.   $��629,385,0,0q�6I   $	��Top=�6^   $	��120��6v   .@�5       �� $��AES 128 bitov��6   �� 5!      $��AES 192 bitov��6   �� 5"      $��AES 256 bitov��6   �� 5#      $��RSA 1024 bitov��6   �� 5$      $��RSA 2048 bitov��6   5%      5&      5'      
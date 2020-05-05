; rcx - wskaŸnik pocz¹tku tablicy z tekstem jawnym
; rdx - d³ugoœæ tekstu jawnego, wykorzystana jako licznik pêtli
; r8  - wskaŸnik pocz¹tku tablicy z kluczem
; r9  - wskaŸnik pocz¹tku bufora na tekst zaszyfrowany
; r10 - rejsetr wykorzystany do zapamiêtania pocz¹tku tablicy z kluczem
; r11 - rejsetr wykorzystany do zapamiêtania przesuniêcia w tablicy z kluczem
.data

.code

;Funkcja wykonuje w³aœciwe szyfrowanie. Pêtla w funkcji wykonuje siê tyle razy, ile podano w argumencie plainTextLength.
;Przy ka¿dym obiegu pêtli obs³ugiwana jest jedna litera. Indeks litery jest pobierany, przeliczany na odpowiednie przesuniêcie,
;nastêpnie wskaŸnik na element klucza jest odpowiednio przesuwany tak, by wskazywa³ na pierwsz¹ z dwóch liter, na które nale¿y 
;zmieniæ dany znak. Pierwsza, a nastêpnie druga litera zostaj¹ zapisane w odpowiednie miejsce bufora.

ADFGXAsm PROC
mov r10,r8					;zapamiêtanie w r10 pocz¹tku tablicy z kluczem
petla:						;etykieta pocz¹tku pêtli
cmp rdx, 0					;sprawdzenie, czy rdx=0, co znaczy, ¿e pêtla wykona³a siê rdx razy
jE koniec					;jeœli rdx=0, nastêpuje skok do etykiety koniec
mov r8,r10					;zresetowanie wskaŸnika na klucz, wskazuje on teraz na pocz¹tek tablicy z kluczem
movzx eax, byte ptr[rcx]	;pobranie do rax indeksu ASCII przetwarzanej litery
cvtsi2sd xmm5, eax			;konwersja 32-bitowej wartoœci ca³kowitoliczbowej do 64-bitowej wartoœci zmiennoprzecinkowej
mov eax, -65				;pobranie do eax wartoœci -65, ujemnego indeks znaku 'A' w ASCII
cvtsi2sd xmm6, eax			;konwersja 32-bitowej wartoœci ca³kowitoliczbowej do 64-bitowej wartoœci zmiennoprzecinkowej
addsd xmm5,xmm6				;sumowa indeksu ASCII przetwarzanej litery z ujemnym indeksem znaku 'A' w ASCII
addsd xmm5,xmm5				;podwojenie wartoœci xmm5 przez dodanie jej do siebie
cvttsd2si rax, xmm5			;konwersja 64-bitowej wartoœci zmiennoprzecinkowej do 32-bitowej wartoœci ca³kowitoliczbowej		
mov r11,rax					;w r11 jest zapisane przesuniêcie
mov rax, r8					;pobranie do rax wskaŸnika na pocz¹tek tablicy z kluczem
add rax, r11				;przesuniêcie wskaŸnika o policzone wczeœniej przesuniêcie
mov r8, rax					;zapisanie do r8 wskaŸnika na indeks pierwszej z dwóch liter, które zast¹pi¹ szyfrowan¹ aktualnie literê
mov rax, [r8]				;pobranie do rax indeksu, na który wskazuje w.w. wskaŸnik
mov byte ptr [r9], al		;zapisanie do aktualnej pozycji w buforze na tekst zaszyfrowany w.w. indeksu
mov rax,r9					;pobranie do rax aktualnej pozycji bufora
add rax,1					;zmiana pozycji bufora, inkrementacja wskaŸnika
mov r9, rax					;zapisanie do wskaŸnika na bufor zinkrementowanej pozycji bufora
mov rax,r8					;pobranie do rax wskaŸnika na indeks pierwszej z dwóch liter, które zast¹pi¹ szyfrowan¹ aktualnie literê
add rax,1					;inkrementacja w.w. wskaŸnika, wskazuje on teraz na indeks drugiej litery, która zast¹pi aktualnie szyfrowan¹ literê
mov r8,rax					;zapisanie do r8 w.w. wskaŸnika
mov rax, [r8]				;pobranie do rax indeksu drugiej litery, na króry wskazuje w.w. wskaŸnik
mov byte ptr [r9],al		;zapisanie do aktualnej pozycji w buforze w.w. indeksu
mov rax, r9					;pobranie do rax aktualnej pozycji bufora
add rax,1					;zmiana pozycji bufora, inkrementacja wskaŸnika
mov r9, rax					;zapisanie do wskaŸnika na bufor zinkrementowanej pozycji bufora
mov rax,rcx					;pobranie do rax aktualnej pozycji wskaŸnika na tekst jawny
add rax,4					;inkrementacja w.w wskaŸnika, przesuniêcie o 4, poniewa¿ wskaŸnik wskazuje na typ 4-bajtowy
mov rcx,rax					;zapisanie do wskaŸnika na tekst jawny zinkermentowanej pozycji wskaŸnika na tekst jawny
mov rax, rdx				;pobranie do rax licznika pêtli
sub rax,1					;dekrementacja licznika pêtli	
mov rdx,rax					;zapisanie do rdx zdekrementowanego licznika pêtli				
jmp petla					;skok do etykiety petla		
koniec:						;etykieta, do której odwo³anie koñczy wykonanie procesu
ret
ADFGXAsm endp
end
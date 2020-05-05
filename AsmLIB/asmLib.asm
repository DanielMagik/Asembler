; rcx - wska�nik pocz�tku tablicy z tekstem jawnym
; rdx - d�ugo�� tekstu jawnego, wykorzystana jako licznik p�tli
; r8  - wska�nik pocz�tku tablicy z kluczem
; r9  - wska�nik pocz�tku bufora na tekst zaszyfrowany
; r10 - rejsetr wykorzystany do zapami�tania pocz�tku tablicy z kluczem
; r11 - rejsetr wykorzystany do zapami�tania przesuni�cia w tablicy z kluczem
.data

.code

;Funkcja wykonuje w�a�ciwe szyfrowanie. P�tla w funkcji wykonuje si� tyle razy, ile podano w argumencie plainTextLength.
;Przy ka�dym obiegu p�tli obs�ugiwana jest jedna litera. Indeks litery jest pobierany, przeliczany na odpowiednie przesuni�cie,
;nast�pnie wska�nik na element klucza jest odpowiednio przesuwany tak, by wskazywa� na pierwsz� z dw�ch liter, na kt�re nale�y 
;zmieni� dany znak. Pierwsza, a nast�pnie druga litera zostaj� zapisane w odpowiednie miejsce bufora.

ADFGXAsm PROC
mov r10,r8					;zapami�tanie w r10 pocz�tku tablicy z kluczem
petla:						;etykieta pocz�tku p�tli
cmp rdx, 0					;sprawdzenie, czy rdx=0, co znaczy, �e p�tla wykona�a si� rdx razy
jE koniec					;je�li rdx=0, nast�puje skok do etykiety koniec
mov r8,r10					;zresetowanie wska�nika na klucz, wskazuje on teraz na pocz�tek tablicy z kluczem
movzx eax, byte ptr[rcx]	;pobranie do rax indeksu ASCII przetwarzanej litery
cvtsi2sd xmm5, eax			;konwersja 32-bitowej warto�ci ca�kowitoliczbowej do 64-bitowej warto�ci zmiennoprzecinkowej
mov eax, -65				;pobranie do eax warto�ci -65, ujemnego indeks znaku 'A' w ASCII
cvtsi2sd xmm6, eax			;konwersja 32-bitowej warto�ci ca�kowitoliczbowej do 64-bitowej warto�ci zmiennoprzecinkowej
addsd xmm5,xmm6				;sumowa indeksu ASCII przetwarzanej litery z ujemnym indeksem znaku 'A' w ASCII
addsd xmm5,xmm5				;podwojenie warto�ci xmm5 przez dodanie jej do siebie
cvttsd2si rax, xmm5			;konwersja 64-bitowej warto�ci zmiennoprzecinkowej do 32-bitowej warto�ci ca�kowitoliczbowej		
mov r11,rax					;w r11 jest zapisane przesuni�cie
mov rax, r8					;pobranie do rax wska�nika na pocz�tek tablicy z kluczem
add rax, r11				;przesuni�cie wska�nika o policzone wcze�niej przesuni�cie
mov r8, rax					;zapisanie do r8 wska�nika na indeks pierwszej z dw�ch liter, kt�re zast�pi� szyfrowan� aktualnie liter�
mov rax, [r8]				;pobranie do rax indeksu, na kt�ry wskazuje w.w. wska�nik
mov byte ptr [r9], al		;zapisanie do aktualnej pozycji w buforze na tekst zaszyfrowany w.w. indeksu
mov rax,r9					;pobranie do rax aktualnej pozycji bufora
add rax,1					;zmiana pozycji bufora, inkrementacja wska�nika
mov r9, rax					;zapisanie do wska�nika na bufor zinkrementowanej pozycji bufora
mov rax,r8					;pobranie do rax wska�nika na indeks pierwszej z dw�ch liter, kt�re zast�pi� szyfrowan� aktualnie liter�
add rax,1					;inkrementacja w.w. wska�nika, wskazuje on teraz na indeks drugiej litery, kt�ra zast�pi aktualnie szyfrowan� liter�
mov r8,rax					;zapisanie do r8 w.w. wska�nika
mov rax, [r8]				;pobranie do rax indeksu drugiej litery, na kr�ry wskazuje w.w. wska�nik
mov byte ptr [r9],al		;zapisanie do aktualnej pozycji w buforze w.w. indeksu
mov rax, r9					;pobranie do rax aktualnej pozycji bufora
add rax,1					;zmiana pozycji bufora, inkrementacja wska�nika
mov r9, rax					;zapisanie do wska�nika na bufor zinkrementowanej pozycji bufora
mov rax,rcx					;pobranie do rax aktualnej pozycji wska�nika na tekst jawny
add rax,4					;inkrementacja w.w wska�nika, przesuni�cie o 4, poniewa� wska�nik wskazuje na typ 4-bajtowy
mov rcx,rax					;zapisanie do wska�nika na tekst jawny zinkermentowanej pozycji wska�nika na tekst jawny
mov rax, rdx				;pobranie do rax licznika p�tli
sub rax,1					;dekrementacja licznika p�tli	
mov rdx,rax					;zapisanie do rdx zdekrementowanego licznika p�tli				
jmp petla					;skok do etykiety petla		
koniec:						;etykieta, do kt�rej odwo�anie ko�czy wykonanie procesu
ret
ADFGXAsm endp
end
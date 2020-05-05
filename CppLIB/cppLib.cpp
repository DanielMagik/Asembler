#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

//Eksportowanie funkcji w j�zyku c++ do biblioteki dll
extern "C" 
{
	//plainTextStart  - wska�nik pocz�tku tablicy z tekstem jawnym
	//plainTextLength - d�ugo�� tekstu jawnego, wykorzystana jako licznik p�tli
	//keyTable		  - wska�nik pocz�tku tablicy z kluczem
	//buffer          - wska�nik pocz�tku bufora na tekst zaszyfrowany

	//Funkcja wykonuje w�a�ciwe szyfrowanie. P�tla w funkcji wykonuje si� tyle razy, ile podano w argumencie plainTextLength.
	//Przy ka�dym obiegu p�tli obs�ugiwana jest jedna litera. Indeks litery jest pobierany, przeliczany na odpowiednie przesuni�cie,
	//nast�pnie wska�nik na element klucza jest odpowiednio przesuwany tak, by wskazywa� na pierwsz� z dw�ch liter, na kt�re nale�y 
	//zmieni� dany znak. Pierwsza, a nast�pnie druga litera zostaj� zapisane w odpowiednie miejsce bufora.
	__declspec(dllexport) void ADFGXCpp(int *plainTextStart, int plainTextLength, char *keyTable, char *buffer) 
	{
		char*start = keyTable;										   //zapami�tanie pocz�tku klucza
		int indeks;													   //zmienna do zapami�tania *plainTextStart-65
		for (plainTextLength; plainTextLength > 0; plainTextLength--)  //p�tla wykonuje si� plainTextLength razy
		{
			keyTable = start;										   //zresetowanie wska�nika na klucz	
			indeks = *plainTextStart - 65;							   //odj�cie od indeksu przetwarzanej litery warto�ci 65 (indeks 'A' w ASCII)
			keyTable = keyTable + indeks;							   //przesuni�cie wska�nika na klucz o wy�ej policzon� warto��
			keyTable = keyTable + indeks;					           //ponowne przesuni�cie wska�nika na klucz o wy�ej policzon� warto��
			*buffer = *keyTable;									   //zapisanie do bufora indeksu aktualnie wskazywanego przez wska�nik klucza
			buffer++;												   //inkrementacja bufora na tekst zaszyfrowany
			keyTable++;											       //inkrementacja wska�nika klucza
			*buffer = *keyTable;									   //zapisanie do bufora indeksu aktualnie wskazywanego przez wska�nik klucza
			buffer++;												   //inkrementacja bufora na tekst zaszyfrowany
			plainTextStart++;										   //inkrementacja wska�nika na tekst jawny
		}
	}


}

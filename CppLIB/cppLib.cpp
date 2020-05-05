#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

//Eksportowanie funkcji w jêzyku c++ do biblioteki dll
extern "C" 
{
	//plainTextStart  - wskaŸnik pocz¹tku tablicy z tekstem jawnym
	//plainTextLength - d³ugoœæ tekstu jawnego, wykorzystana jako licznik pêtli
	//keyTable		  - wskaŸnik pocz¹tku tablicy z kluczem
	//buffer          - wskaŸnik pocz¹tku bufora na tekst zaszyfrowany

	//Funkcja wykonuje w³aœciwe szyfrowanie. Pêtla w funkcji wykonuje siê tyle razy, ile podano w argumencie plainTextLength.
	//Przy ka¿dym obiegu pêtli obs³ugiwana jest jedna litera. Indeks litery jest pobierany, przeliczany na odpowiednie przesuniêcie,
	//nastêpnie wskaŸnik na element klucza jest odpowiednio przesuwany tak, by wskazywa³ na pierwsz¹ z dwóch liter, na które nale¿y 
	//zmieniæ dany znak. Pierwsza, a nastêpnie druga litera zostaj¹ zapisane w odpowiednie miejsce bufora.
	__declspec(dllexport) void ADFGXCpp(int *plainTextStart, int plainTextLength, char *keyTable, char *buffer) 
	{
		char*start = keyTable;										   //zapamiêtanie pocz¹tku klucza
		int indeks;													   //zmienna do zapamiêtania *plainTextStart-65
		for (plainTextLength; plainTextLength > 0; plainTextLength--)  //pêtla wykonuje siê plainTextLength razy
		{
			keyTable = start;										   //zresetowanie wskaŸnika na klucz	
			indeks = *plainTextStart - 65;							   //odjêcie od indeksu przetwarzanej litery wartoœci 65 (indeks 'A' w ASCII)
			keyTable = keyTable + indeks;							   //przesuniêcie wskaŸnika na klucz o wy¿ej policzon¹ wartoœæ
			keyTable = keyTable + indeks;					           //ponowne przesuniêcie wskaŸnika na klucz o wy¿ej policzon¹ wartoœæ
			*buffer = *keyTable;									   //zapisanie do bufora indeksu aktualnie wskazywanego przez wskaŸnik klucza
			buffer++;												   //inkrementacja bufora na tekst zaszyfrowany
			keyTable++;											       //inkrementacja wskaŸnika klucza
			*buffer = *keyTable;									   //zapisanie do bufora indeksu aktualnie wskazywanego przez wskaŸnik klucza
			buffer++;												   //inkrementacja bufora na tekst zaszyfrowany
			plainTextStart++;										   //inkrementacja wskaŸnika na tekst jawny
		}
	}


}

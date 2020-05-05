using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//System.OutOfMemoryException

namespace GUI
{
    public partial class Form1 : Form
    {
        //funcja importowana z dll-ki, napisana w asemblerze, odpowiada za przekształcenie bufora, będącego tablicą byte,
        //na podstawie przekazanego tekstu jawnego i klucza
        [DllImport("AsmLIB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ADFGXAsm")]
        public extern static unsafe void AsmEncrypt(int* plainTextStart, int plainTextLength, byte* keyTable, byte* buffer);
        //funcja importowana z dll-ki, napisana w c++, odpowiada za przekształcenie bufora, będącego tablicą byte,
        //na podstawie przekazanego tekstu jawnego i klucza
        [DllImport("CppLIB.dll", EntryPoint = "ADFGXCpp")]
        public extern static unsafe void CppEncrypt(int* plainTextStart, int plainTextLength, byte* keyTable, byte* buffer);

        String plaintext;//tekst jawny, pobrany z pliku
        String keyText;//klucz, zawiera kolejno informacje dla każdel litery alfabetu, na jakie litery ta litera jest zamieniana
        String cipherText;//tekst zaszyfrowany

        bool goodTextFile;//informacja, czy wczytano poprawny tekst jawny
        bool goodKeyFile;//informacja, czy wczytano poprawny klucz
        byte [] ADFGXTable;//klucz w postaci tablicy byte-ów
        int [] plainTextInt;//tekst jawny w postaci tablicy int-ów
        byte[] cipherBuffer;//bufor na tekst zaszyfrowany w postaci tablicy byte-ów
        bool operationDone;//czy wykonano szyfrowanie
        int threads;//liczba wątków

        //Konstruktor. Funkcja wywoływana przy każdym uruchomieniu programu. Ustawia początkową, optymalną
        //liczbę wątków, ustawia czcionkę w oknie aplikacji oraz wartości początkowe zmiennych.
        public Form1()
        {
            InitializeComponent();
            //ustawia początkowej, optymalnej liczby wątków
            setThreads();
            //ustawienie czcionki w oknie aplikacji
            keyTextBox.Font = new Font(keyTextBox.Font.FontFamily, 12);
            goodTextFile = false;//informacja, że nie otwarto pliku z tekstem jawnym
            goodKeyFile = false;//informacja, że nie otwarto pliku z kluczem
            operationDone = false;//informacja, że nie wykonano szyfrowania
            this.CenterToScreen();
            //Ustawienie stringów jako puste i stworzenie bufora na klucz
            cipherText = "";
            plaintext = "";
            keyText = "";
            ADFGXTable = new byte[52];
        }
        //Funkcja ustawia początkową, optymalną liczbę wątków i wyświetla ją w oknie aplikacji
        private void setThreads()
        {
            int cores = 0;//licznik
            //pobranie liczby wątków procesora
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                cores += int.Parse(item["NumberOfLogicalProcessors"].ToString());
            }
            //wpisanie liczby wątków do pola edycyjnego
            numericUpDown1.Value = cores;
            //przypisanie liczby wątków do zmiennej threads
            threads = cores;
            //wyświetlenie informacji o ilości wątków
            threadsLabel.Text = "Threads: ";
            threadsLabel.Text += cores;
        }
        //Funkcja pobiera stringa, usuwa z niego znaki nie będące literami i zwraca przerobionego stringa
        private string OnlyLetters(String textToConvert)
        {
            String copy = "";//początkowo pusty string, bufor na przerobiony tekst
            int iteredLetter;//indeks aktualnie przetwarzanej litery
            //pętla dla wszystkich znaków w stringu
            for (int i = 0; i < textToConvert.Length; i++)
            {
                //pobranie indeksu przetwarzanej litery
                iteredLetter = (int)(textToConvert[i]);
                //indeks jest w zakresie wielkich liter
                if (iteredLetter >= 65 && iteredLetter <= 90)
                {
                    copy += textToConvert[i];//dodanie do tekstu tej samej (wielkiej) litery
                }
                //indeks jest w zakresie małych liter
                else
                if (iteredLetter >= 97 && iteredLetter <= 122)
                {
                    copy += (char)(iteredLetter - 32);//dodanie do tekstu przetworzonej (z małej na wielką) litery
                }
            }
            return copy;//zwrócenie przerobionego stringa
        }

        //Funkcja wywoływana, gdy kliknięto przycisk ustawiania liczby wątków ("Set threads"). Sprawdza ona, czy
        //wpisana w pole tekstowe wartość jest liczbą, jeśli jest, ustawia ona aktualną liczbę wątków dla szyfrowania
        private void threadsButtonClick(object sender, EventArgs e)
        {
            try
            {
                //pobranie liczby z pola
                int threadsInt = Int32.Parse(numericUpDown1.Text);
                //wyświetlenie informacji o liczbie wątków
                threadsLabel.Text = "Threads: ";
                threadsLabel.Text += threadsInt;
                //ustawienie zmiennej
                threads = threadsInt;
            }
            //puste pole, kontrola błędów
            catch (System.FormatException)
            {
                MessageBox.Show("The number of threads is empty!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Funkcja wywoływana, gdy kliknięto przycisk otwarcia pliku tekstowego z tekstem jawnym ("Open text file"). Pobiera ona tekst z pliku, sprawdza,
        //czy plik nie jest pusty oraz czy zawiera przynajmniej jedną literę, następnie przypisuje pobrany tekst do odpowiednich zmiennych
        //i wyświetla go w oknie aplikacji
        private async void showButtonClick(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true, Multiselect = false })
                {
                    //odczyt z pliku
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            //wyczyszczenie pola z tekstem jawnym
                            plainTextBox.Clear();
                            String line;//bufor na pojedynczą linię pliku tekstowego
                            //zresetowanie zmiennych
                            plaintext = "";
                            cipherText = "";
                            //odczytywanie pojedynczych linii
                            while ((line = await sr.ReadLineAsync()) != null)
                            {
                                plaintext += line;
                            }
                            //przerobienie tekstu tak, że zostają w nim same litery
                            plaintext = OnlyLetters(plaintext);
                            //brak liter w pliku
                            if (plaintext.Length == 0)
                            {
                                //inforamcja, że nie otwarto odpowiedniego pliku i nie wykonano szyfrowania
                                operationDone = false;
                                goodTextFile = false;
                                MessageBox.Show("There are 0 allowed characters in the file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //wyświetlenie tekstu w okienku i stworzenie buforów na tekst
                            else
                            {
                                //dodanie tekstu do okna aplikacji
                                plainTextBox.AppendText(plaintext);
                                //bufor na tekst jawny
                                plainTextInt = new int[plaintext.Length];
                                //dodanie liter do bufora
                                for (int i = 0; i < plaintext.Length; i++)
                                {
                                    plainTextInt[i] = (int)(plaintext[i]);
                                }
                                //bufor na tekst zaszyfrowany
                                cipherBuffer = new byte[plaintext.Length * 2];
                                //informacja, że otwarto odpowiedni plik
                                goodTextFile = true;
                            }
                        }
                    }
                }
            }
            //zbyt duży rozmiar pliku tekstowego, kontrola błędów
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("File size too large!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Funkcja wywoływana, gdy kliknięto przycisk zapisu tekstu zaszyfrowanego do pliku ("Save text file"). Zapisuje ona tekst zaszyfrowany 
        //do pliku, wcześniej sprawdzając, czy szyfrowanie zostało wykonane
        private async void saveButtonClick(object sender, EventArgs e)
        {
            //nie wykonano szyfrowania lub otwarto zły plik
            if (operationDone == false)
            {
                MessageBox.Show("No encryption performed or a bad file has opened!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //zapisanie do pliku
            else
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            await sw.WriteLineAsync(cipherText);
                            //informacja o udanym zapisie
                            MessageBox.Show("You have been successfully saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
        //Funkcja wywoływana, gdy kliknięto przycisk otwarcia pliku z kluczem (Szachownicą Polibiusza). Sprawdza ona, czy w pliku jest odpowiedni tekst
        //(wszystkie, litery alfabetu, pojedynczo, z wyjątkiem "J", w dowolnej kolejności), a następnie ustawia ona zmienne odpowiedzialne za zapamiętanie klucza
        private async void ChessButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true, Multiselect = false })
                {
                    //otwarcie pliku
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            String line;//bufor na pojedynczą linię
                            keyText = "";//zresetowanie klucza
                            while ((line = await sr.ReadLineAsync()) != null)
                            {
                                keyText += line;
                                if(keyText.Length>25)//przekroczono rozmiar klucza
                                {
                                    break;
                                }
                            }
                            //przerobienie tekstu tak, że zostają w nim same litery
                            keyText = OnlyLetters(keyText);

                            if (keyText.Length != 25)//niepoprawny tekst w pliku
                            {
                                keyTextBox.Clear();//wyczyszczenie pola z kluczem
                                goodKeyFile = false;//informacja, że otwarto zły plik z kluczem
                                MessageBox.Show("Invalid key file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                operationDone = false;//informacja, że nie wykonano szyfrowania
                            }
                            //w pliku jest 25 znaków
                            else
                            {
                                //założenie, żę plik wstępnie jest poprawny
                                bool allSigns = true;
                                int actualIndex = 65;//Litera A w ASCII
                                //Sprawdzenie, czy są znaki A-Z
                                for (int i = 0; i < 26; i++)
                                {
                                    if (!keyText.Contains((char)(actualIndex)))
                                    {
                                        if (actualIndex != 74)//Litera 'J', która jest zbędna
                                        {
                                            //brakuje litery, informacja o niepopranym pliku i przerwanie pętli
                                            allSigns = false;
                                            break;
                                        }
                                    }
                                    actualIndex++;
                                }
                                //Brakuje którejś litery
                                if (allSigns == false)
                                {
                                    keyTextBox.Text = "";
                                    keyText = "";
                                    goodKeyFile = false;
                                    MessageBox.Show("Invalid key file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    operationDone = false;
                                }
                                //są wszystkie litery w pliku
                                else
                                {
                                    //ustawienie wyświetlania klucza w polu tekstowym
                                    String keyInTextBox = "";
                                    for (int i = 1; i <= 25; i++)
                                    {
                                        keyInTextBox += keyText[i - 1];
                                        if (i % 5 == 0)
                                        {
                                            keyInTextBox += Environment.NewLine;//dodanie znaku nowej linii do wyświetlanego tekstu co 5 znaków
                                        }
                                    }
                                    //wyświetlenie klucza
                                    keyTextBox.Text = keyInTextBox;
                                    //ustawienie tablicy int-ów, reprezentującej klucz
                                    setKeySequence();
                                    goodKeyFile = true;
                                }
                            }
                        }
                    }
                }
            }
            //zbyt duży rozmiar pliku tekstowego, kontrola błędów
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("File size too large!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Funkcja ustawia tablicę byte-ów reprezentującą klucz. Szyfr polega na zastąpieniu każdej litery dwoma innymi literami. W tablicy zostają 
        //zapisane litery, na które należy zmienić kolejne litery alfabetu
        private unsafe void setKeySequence()
        {
            //Indeksy liter
            //A-65
            //D-68
            //F-70
            //G-71
            //X-88
            //tablica z kolejnymi indeksami liter
            int[] tabLetter = { 65, 68, 70, 71, 88 };
            int actualLetter;//indeks aktualnej litery
            int actualIndex1 = 0;//aktualny indeks pierwszej litery szyfru dla pojedynczego znaku, również indeks wiersza
            int actualIndex2 = 0;//aktualny indeks drugiej litery szyfru dla pojedynczego znaku, również indeks kolumny
            int licznik = 0;//licznik pętl

            //pętla wykonująca się 5 razy dla 5 wierszy
            for (int i = 0; i < 5; i++)
            {
                actualIndex2 = 0;
                //pętla wykonująca się 5 razy 5 liter w wierszu
                for (int j = 0; j < 5; j++)
                {
                    //pobieranie liter po kolei
                    actualLetter = (int)(keyText[licznik]);
                    actualLetter -= 65;//położenie w tablicy
                    licznik++;
                    //ustawienie pierwszej litery dla znaku
                    ADFGXTable[actualLetter * 2] = (byte)(tabLetter[actualIndex1]);
                    //ustawienie drugiej litery dla znaku
                    ADFGXTable[actualLetter * 2 + 1] = (byte)(tabLetter[actualIndex2]);

                    actualIndex2++;
                }
                actualIndex1++;
            }
            //Indeksy litery "J", równe indeksowi litery "I"
            ADFGXTable[18] = ADFGXTable[16]; ;
            ADFGXTable[19] = ADFGXTable[17];
        }

        //Funkcja wywoływana, gdy kliknięto przycisk wykonania szyfrowania("Encrypting"). Sprawdza ona, czy wybrano poprawny plik z
        //tekstem jawnym i z kluczem. Następnie tworzy ona odpowiednie wskaźniki do tablic z tekstem jawnym, kluczem i buforem na
        //tekst zaszyfrowany, i w zależności od wyboru użytkownika, wywołuje odpowiednią funkcję, która wykonuje szyfrowanie w języku c++ lub w asemblerze
        private unsafe void encryptButton_Click(object sender, EventArgs e)
        {
            //wyczyszczenie pola z tekstem zaszyfrowanym
            cipherTextBox.Clear();
            //nie wybrano odpowiedniego pliku z tekstem jawnym
            if (!goodTextFile)
            {
                MessageBox.Show("Please choose a good plain text file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            //nie wybrano odpowiedniego pliku z kluczem
            if (!goodKeyFile)
            {
                MessageBox.Show("Please choose a good key file", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //można przeprowadzić szyfrowanie
            else
            {
                //stworzenie wskaźników na tablicę z kluczem i buforem na tekst zaszyfrowany
                fixed (byte* keyTablePtr = &ADFGXTable[0], cipherBufferPtr = &cipherBuffer[0])
                {
                    //stworzenie wskaźnika na tablicę z tekstem jawnym
                    fixed (int* bytePtr = &plainTextInt[0])
                    {
                        //wybrano jęzek c++
                        if (radioButtonCpp.Checked == true)
                        {
                            Stopwatch timer = new Stopwatch();
                            timer.Reset();
                            //rozpoczęcie pomiaru czasu
                            timer.Start();
                            //właściwa funkcja w c++, działająca na wielu wątkach
                           
                            
                                CPPencryptFunc(bytePtr, plaintext.Length, keyTablePtr, cipherBufferPtr);
                            
                            timer.Stop();
                            //koniec pomiaru, wyświetlenie czasu
                            infoTextBox.Text = "Encrypt,";
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += "Threads: ";
                            infoTextBox.Text += threads;
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += "Time CPP:  ";
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += timer.ElapsedMilliseconds;
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += " ms";
                        }
                        //wybrano jęzek asembler
                        else
                        {
                            Stopwatch timer = new Stopwatch();
                            timer.Reset();
                            timer.Start();
                            //rozpoczęcie pomiaru czasu
                            //właściwa funkcja asemblerowa, działająca na wielu wątkach
                            
                            
                                AsmEncryptFunc(bytePtr, plaintext.Length, keyTablePtr, cipherBufferPtr);
                            
                            timer.Stop();
                            //koniec pomiaru, wyświetlenie czasu
                            infoTextBox.Text = "Encrypt,";
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += "Threads: ";
                            infoTextBox.Text += threads;
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += "Time Asm:  ";
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += timer.ElapsedMilliseconds;
                            infoTextBox.Text += Environment.NewLine;
                            infoTextBox.Text += " ms";

                        }
                    }
                }
                cipherText = "";//zresetowanie tekstu zaszyfrowanego
                //stworzenie stringa z tekstem zaszyfrowanym na podstawie tablicy byte-ów
                cipherText= Encoding.UTF8.GetString(cipherBuffer, 0, cipherBuffer.Length);
                cipherTextBox.AppendText(cipherText);
                operationDone = true;
            }
        }
        //Funkcja zapwenia obsługę wielowątkowości. Najpierw tworzy odpowiednie tablice, zawierające dla każdego wątku: wskaźnik początku
        //tekstu jawnego, liczbę znaków do przetworzenia oraz wskaźnik początku bufora. Następnie wywyływana jest właściwa funkcja w
        //c++ na danej liczbie wątków
        private unsafe void CPPencryptFunc(int* plainTextPtr, int plaintextLength, byte* keyTablePtr, byte* bufferPtr)
        {
            int charsInt = plaintext.Length;//liczba znaków w całym tekście jawnym
            int charsForEach = charsInt / threads;//liczba znaków dla każdego wątku
            int extraChars = charsInt % threads;//liczba wątków, które obsłużą jednen dodatkowy znak
            int [] charsForThreads = new int[threads];//liczba znaków dla danego wątku
            int * [] charsPointers = new int*[threads];//wskaźnik początku tekstu dla danego wątku
            byte*[] bufferPointers = new byte*[threads];//wskaźnik początku bufora dla danego wątku
            //wskaźniki pomocnicze do ustawienia wskaźników dla każdego wątku
            int* charPtr = plainTextPtr;//aktualna pozycja w tekście jawnym
            byte* bufferPointer = bufferPtr;//aktualna pozycja w buforze na tekst zaszyfrowany
            
            //ustawienie tablic dla wszystkich wątków
            for (int i = 0; i < threads; i++)
            {
                //wątki, które przetworzą jeden dodatkowy znak
                if (i< extraChars)
                {
                    //liczba znaków dla wątku
                    charsForThreads[i] = charsForEach;
                    charsForThreads[i] += 1;
                    //wskaźnik początku tekstu jawnego dla wątku
                    charsPointers[i] = charPtr;
                    //wskaźnik początku bufora dla wątku
                    bufferPointers[i] = bufferPointer;
                    //odpowiednie przesunięcie wskaźników
                    charPtr += charsForEach;
                    charPtr += 1;
                    bufferPointer += charsForEach * 2;
                    bufferPointer += 2;
                }
                else
                //wątki, które przetworzą standardową liczbę znaków
                {
                    //liczba znaków dla wątku
                    charsForThreads[i] = charsForEach;
                    //wskaźnik początku tekstu jawnego dla wątku
                    charsPointers[i] = charPtr;
                    //wskaźnik początku bufora dla wątku
                    bufferPointers[i] = bufferPointer;
                    //odpowiednie przesunięcie wskaźników
                    charPtr += charsForEach;
                    bufferPointer += charsForEach * 2;
                }
            }
            //właściwa funkcja szyfrująca w c++,  wywyływana w wielu wątkach
            ParallelOptions parOpt = new ParallelOptions();
            parOpt.MaxDegreeOfParallelism = threads;
            Parallel.For(0, threads,parOpt, i =>
            {
                CppEncrypt(charsPointers[i], charsForThreads[i], keyTablePtr, bufferPointers[i]);
            });
        }
        //Funkcja zapwenia obsługę wielowątkowości. Najpierw tworzy odpowiednie tablice, zawierające dla każdego wątku: wskaźnik początku
        //tekstu jawnego, liczbę znaków do przetworzenia oraz wskaźnik początku bufora. Następnie wywyływana jest właściwa funkcja w
        //asemblerze na danej liczbie wątków
        private unsafe void AsmEncryptFunc(int* plainTextPtr, int plaintextLength, byte* keyTablePtr, byte* bufferPtr)
        {
            int charsInt = plaintext.Length;//liczba znaków w całym tekście jawnym
            int charsForEach = charsInt / threads;//liczba znaków dla każdego wątku
            int extraChars = charsInt % threads;//liczba wątków, które obsłużą jednen dodatkowy znak
            int[] charsForThreads = new int[threads];//liczba znaków dla danego wątku
            int*[] charsPointers = new int*[threads];//wskaźnik początku tekstu dla danego wątku
            byte*[] bufferPointers = new byte*[threads];//wskaźnik początku bufora dla danego wątku
            //wskaźniki pomocnicze do ustawienia wskaźników dla każdego wątku
            int* charPtr = plainTextPtr;//aktualna pozycja w tekście jawnym
            byte* bufferPointer = bufferPtr;//aktualna pozycja w buforze na tekst zaszyfrowany

            //ustawienie tablic dla wszystkich wątków
            for (int i = 0; i < threads; i++)
            {
                //wątki, które przetworzą jeden dodatkowy znak
                if (i < extraChars)
                {
                    //liczba znaków dla wątku
                    charsForThreads[i] = charsForEach;
                    charsForThreads[i] += 1;
                    //wskaźnik początku tekstu jawnego dla wątku
                    charsPointers[i] = charPtr;
                    //wskaźnik początku bufora dla wątku
                    bufferPointers[i] = bufferPointer;
                    //odpowiednie przesunięcie wskaźników
                    charPtr += charsForEach;
                    charPtr += 1;
                    bufferPointer += charsForEach * 2;
                    bufferPointer += 2;
                }
                else
                //wątki, które przetworzą standardową liczbę znaków
                {
                    //liczba znaków dla wątku
                    charsForThreads[i] = charsForEach;
                    //wskaźnik początku tekstu jawnego dla wątku
                    charsPointers[i] = charPtr;
                    //wskaźnik początku bufora dla wątku
                    bufferPointers[i] = bufferPointer;
                    //odpowiednie przesunięcie wskaźników
                    charPtr += charsForEach;
                    bufferPointer += charsForEach * 2;
                }
            }
            //właściwa funkcja szyfrująca w asemblerze,  wywyływana w wielu wątkach
            ParallelOptions parOpt = new ParallelOptions();
            parOpt.MaxDegreeOfParallelism = threads;
            Parallel.For(0, threads, parOpt, i =>
            {
                AsmEncrypt(charsPointers[i], charsForThreads[i], keyTablePtr, bufferPointers[i]);
            });
        }
    }
}



using System;
using System.Windows;

namespace HammingCodeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // метод для добавления проверочных разрядов
        private void AddCheckBits_Click(object sender, RoutedEventArgs e)
        {
            string input = BinaryInput.Text;

            // проверка длины и двоичных символов
            if (input.Length == 8 && IsBinary(input))
            {
                string hammingCode = AddHammingCheckBits(input);
                HammingCodeInput.Text = hammingCode;
                ResultText.Text = "Проверочные разряды добавлены!";
            }
            else
            {
                ResultText.Text = "Пожалуйста, введите корректный 8-битный двоичный код.";
            }
        }

        // проверка, что введенный код является двоичным
        private bool IsBinary(string input)
        {
            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                    return false;
            }
            return true;
        }

        // функция добавления проверочных разрядов для кода Хемминга
        private string AddHammingCheckBits(string input)
        {
            char[] hammingCode = new char[12];

            // вставляем проверочные разряды
            hammingCode[0] = '0'; // P1
            hammingCode[1] = '0'; // P2
            hammingCode[3] = '0'; // P4
            hammingCode[7] = '0'; // P8

            // вставляем информационные разряды в код
            hammingCode[2] = input[0]; // D1
            hammingCode[4] = input[1]; // D2
            hammingCode[5] = input[2]; // D3
            hammingCode[6] = input[3]; // D4
            hammingCode[8] = input[4]; // D5
            hammingCode[9] = input[5]; // D6
            hammingCode[10] = input[6]; // D7
            hammingCode[11] = input[7]; // D8

            // рассчитываем проверочные разряды
            hammingCode[0] = CalculateParity(hammingCode, new int[] { 2, 4, 6, 8, 10 }); // P1
            hammingCode[1] = CalculateParity(hammingCode, new int[] { 2, 5, 6, 9, 10 }); // P2
            hammingCode[3] = CalculateParity(hammingCode, new int[] { 4, 5, 6, 11 });     // P4
            hammingCode[7] = CalculateParity(hammingCode, new int[] { 8, 9, 10, 11 });    // P8

            return new string(hammingCode);
        }

        // метод для вычисления четности (проверочного разряда)
        private char CalculateParity(char[] hammingCode, int[] positions)
        {
            int parity = 0;
            foreach (int pos in positions)
            {
                parity ^= (hammingCode[pos] - '0'); // преобразуем char в int
            }
            return (char)(parity + '0');
        }

        // метод для проверки кодовой комбинации
        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            string hammingCode = HammingCodeInput.Text;

            // проверка длины и двоичных символов
            if (hammingCode.Length == 12 && IsBinary(hammingCode))
            {
                int errorPosition = CheckHammingCode(hammingCode);

                if (errorPosition == 0)
                {
                    ResultText.Text = "Ошибок не обнаружено!";
                }
                else
                {
                    ResultText.Text = $"Ошибка в позиции {errorPosition}.";
                }
            }
            else
            {
                ResultText.Text = "Пожалуйста, введите корректный 12-битный код Хемминга.";
            }
        }

        // функция для проверки кода Хемминга
        private int CheckHammingCode(string hammingCode)
        {
            int[] positions = new int[4];

            positions[0] = (hammingCode[0] - '0') ^ (hammingCode[2] - '0') ^ (hammingCode[4] - '0') ^ (hammingCode[6] - '0') ^ (hammingCode[8] - '0') ^ (hammingCode[10] - '0');
            positions[1] = (hammingCode[1] - '0') ^ (hammingCode[2] - '0') ^ (hammingCode[5] - '0') ^ (hammingCode[6] - '0') ^ (hammingCode[9] - '0') ^ (hammingCode[10] - '0');
            positions[2] = (hammingCode[3] - '0') ^ (hammingCode[4] - '0') ^ (hammingCode[5] - '0') ^ (hammingCode[6] - '0') ^ (hammingCode[11] - '0');
            positions[3] = (hammingCode[7] - '0') ^ (hammingCode[8] - '0') ^ (hammingCode[9] - '0') ^ (hammingCode[10] - '0') ^ (hammingCode[11] - '0');

            int errorPosition = positions[0] * 1 + positions[1] * 2 + positions[2] * 4 + positions[3] * 8;

            return errorPosition;
        }
    }
}

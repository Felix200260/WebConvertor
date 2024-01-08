using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebConvertor // Объявление пространства имён для проекта.
{
    public class ViewModel : BindableObject // Класс ViewModel, наследуется от BindableObject для реализации механизма привязки данных.
    {
        // Объявление приватных переменных для хранения состояния.
        private string _convertedValue; // Хранение сконвертированного значения.

        private string _pickerFrom;// Хранение выбранной исходной валюты.

        private string _pickerTo;// Хранение выбранной целевой валюты.

        private float _entryFrom;// Хранение вводимого значения.

        private DateTime selectedDate; // Хранение выбранной даты.

        private Parser _xmlFileParser; // Экземпляр класса Parser для обработки XML.
        private DateTime _datePicker; // Хранение значения даты из элемента выбора даты.

        public ViewModel()
        {
            // Подписка на событие изменения свойства.
            PropertyChanged += ViewModel_PropertyChanged;

        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Вызов метода для получения курсов валют при изменении свойств.
            OnGetExchangeRatesClicked();
        }

        // Свойства для доступа и изменения полей с уведомлением о изменении.
        public string MConvertedValue
        {
            get { return _convertedValue; }// Возвращение текущего значения. 
            set
            {
                if(_convertedValue != value)
                {
                    _convertedValue = value;
                    OnPropertyChanged(nameof(MConvertedValue));// Уведомление об изменении.
                    Debug.WriteLine("ConvertevValue = " + _convertedValue); // Отладочная печать.
                }
            }
        }
        // Аналогичные свойства для других полей.
        public string MPickerFrom
        {
            get { return _pickerFrom; }
            set
            {
                if(_pickerFrom != value)
                {
                    _pickerFrom = value;
                    OnPropertyChanged(nameof(MPickerFrom));
                    Debug.WriteLine("PickerFrom = " + _pickerFrom);
                }
            }
        }

        public string MPickerTo
        {
            get { return _pickerTo; }
            set
            {
                if( _pickerTo != value)
                {
                    _pickerTo = value;
                    OnPropertyChanged(nameof(MPickerTo));
                    Debug.WriteLine("PickerTo = " + _pickerTo);
                }
            }
        }

        public float MEntryFrom
        {
            get { return _entryFrom; }
            set
            {
                if(  value != _entryFrom )
                {
                    _entryFrom = value;
                    OnPropertyChanged(nameof(MEntryFrom));
                    Debug.WriteLine("EntryFrom = " + _entryFrom);
                }
            }
        }

        public DateTime MDatePicker
        {
            get { return _datePicker; }
            set
            {
                if( _datePicker != value)
                {
                    _datePicker = value;
                    OnPropertyChanged(nameof(MDatePicker));
                    Debug.WriteLine("Date = " + _datePicker);
                }
            }
        }

         

        public void OnGetExchangeRatesClicked() // Метод для обновления курсов валют.
        {
            _xmlFileParser = new Parser(); // Создание нового экземпляра Parser.


            // Получаем выбранную дату из MDatePicker
            selectedDate = MDatePicker;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Запрашиваем курсы валют на указанную дату \ // Запрос курсов валют и расчёт сконвертированного значения.
            if (MPickerFrom != null && MPickerTo != null)
            {                

                var currencyRateFrom = _xmlFileParser.ParseXML(formattedDate, MPickerFrom.ToString());

                var currencyRateTo = _xmlFileParser.ParseXML(formattedDate, MPickerTo.ToString());

                if (currencyRateFrom != 0 && currencyRateTo != 0)
                {
                    var newValue = MEntryFrom * currencyRateFrom / currencyRateTo;


                    MConvertedValue = newValue.ToString();
                }
            }
        }

    }


    // Класс MainPage для пользовательского интерфейса.
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();// Инициализация компонентов интерфейса.

            BindingContext = new ViewModel(); // Установка контекста привязки данных.
        }

        
    }
}

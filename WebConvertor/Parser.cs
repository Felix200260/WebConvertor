using System.Net; // Подключение пространства имён для работы с протоколами интернета, например, для загрузки данных.
using System.Xml.Linq; // Подключение пространства имён для работы с XML, особенно LINQ to XML.

namespace WebConvertor; // Объявление пространства имён для вашего проекта.

public class Parser // Объявление класса Parser.
{
    public float ParseXML(string selectedDate, string charCode) // Метод ParseXML, который принимает дату и код валюты, возвращает float.
    {
        // Формирует URL, используя указанную дату для запроса данных о валюте.
        string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={selectedDate}";

        WebClient client = new(); // Создание экземпляра WebClient для загрузки данных из интернета.
        string xmlData = client.DownloadString(url); // Загрузка XML-данных с указанного URL.

        XDocument doc = XDocument.Parse(xmlData); // Разбор полученных XML-данных в XDocument.

        IEnumerable<XElement> t1 = doc.Descendants("Valute").ToList(); // Получение всех элементов 'Valute' из XML.

        // Поиск первого элемента 'Valute', у которого значение 'CharCode' соответствует указанному коду валюты.
        var c = t1.FirstOrDefault(x => x.Element("CharCode").Value == charCode);

        var v = c.Element("Value").Value; // Получение значения элемента 'Value' из найденного элемента 'Valute'.

        // LINQ-запрос для поиска и выбора значения валюты, соответствующего указанному коду валюты.
        string selectedCurrency = (
            from currency in t1
            where currency.Element("CharCode").Value == charCode
            select currency.Element("Value").Value
        ).FirstOrDefault();

        // Проверка наличия полученного значения валюты и его преобразование в float.
        if (selectedCurrency != null)
        {
            return float.Parse(selectedCurrency);
        }
        else // В случае отсутствия значения возвращается 0.
        {
            return 0;
        }
    }
}

# Облако тэгов

Приложение для построения облака тэгов по тексту из переданного файла, с графическим интерфейсом.
При генерации облака можно настраивать:
  * Форму облака
  * Шрифт слов
  * Цвет слов
  * Размер изображения
  * Фильтрация исходного текста(словарь запрещенных слов, определенные части речи)

Запускать TagsCloudContainer.exe

План презентации
  1. Краткое описание
  2. Точки расширения
  3. Структура проекта
  ```
  ├── TagCloudBuilder
  |   ├── Domain
  |   |   ├── CircularCloudBuilder
  |   |   ├── CircularCloudLayouter
  |   |   ├── Cloud
  |   |   ├── PngDrawer
  |   |   ├── Settings
  |   |   ├── TextParser
  |   |   ├── TextRectangle
  |   |   ├── TxtReader
  |   |   ├── WordsBounder
  |   |   ├── WordsEditor
  |   |   └── WordsFilter
  |   └── Infrastructure
  |      ├── ParseResultExtensions
  |      ├── PointExtensions
  |      ├── Quarter
  |      ├── Result
  |      └── ResultQueryExpressionExtensions
  ├── TagCloudBuilder.App
  |   ├── Resourses
  |   |   ├── stopwords.txt
  |   |   └── ...(тексты ввода)
  |   ├── AppTagCloud
  |   ├── ForRegister
  |   ├── ImplementationName
  |   └── Program
  └── TagCloudBuilder.Tests
      ├── CircularCloudBuilder_Should
      ├── CircularCloudLayouter_Should
      ├── CloudDrawer
      ├── Result_Should
      ├── ResultQueryExpression_Should
      ├── TxtReaderShould
      └── WordsFilterShould



  __Summary__: 3 directories, n files, m line numer
  ```
  4. Использование DI-контейнера
  5. Тестирование

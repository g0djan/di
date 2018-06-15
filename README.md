# Облако тэгов
![alt text](cloud.png)

Приложение для построения облака тэгов по тексту из переданного файла, с графическим интерфейсом.
При генерации облака можно настраивать:
  * Форму облака
  * Шрифт слов
  * Цвет слов
  * Размер изображения
  * Фильтрация исходного текста(словарь запрещенных слов, определенные части речи)

# Запускать TagCloudBuilder.App.exe
  Чтобы облако построилось по переданному тексту, его нужно закинуть
  в TagCloudBuilder.App\Resources

  Построенную картинку можно найти в TagCloudBuilder.App/bin/Release(ну или Debug)

План презентации
  1. Краткое описание
  2. Точки расширения
  3. Структура проекта
  ```
  ├── TagCloudBuilder
  |   ├── Domain
  |   |   ├── CloudBuilder
  |   |   ├── CircularCloudLayouter
  |   |   ├── Cloud  
  |   |   ├── Settings
  |   |   ├── TextRectangle  
  |   |   └── WordsBounder  
  |   └── Infrastructure
  |      ├── ParseResultExtensions
  |      ├── PointExtensions
  |      ├── PunctuationParser
  |      ├── TxtReader
  |      ├── WordsEditor
  |      ├── WordsFilter
  |      ├── Quarter
  |      ├── Result
  |      └── ResultQueryExpressionExtensions
  ├── TagCloudBuilder.App
  |   ├── Resourses
  |   |   ├── stopwords.txt
  |   |   └── ...(тексты ввода)
  |   ├── AppTagCloud
  |   ├── ForRegister
  |   ├── PngDrawer
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

# UI
  ![alt text](ui.png)

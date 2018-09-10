# Tag cloud
![alt text](cloud.png)

App for building tag cloud from text file, with GUI.
In future I'll implement 3 extension points, chnging of:
  * Cloud form
  * Font
  * Color of words
It was implemented, changing of:
  * Size of image
  * Dictionary of forbidden words

# How to run?
 1. Build project, .NET 7.2
 2. Run TagCloudBuilder.App.exe
 3. Put .txt file in TagCloudBuilder.App\Resources
 4. Press build button to create cloud
 5. You can find it in TagCloudBuilder.App/bin/Release(or Debug)

# Directory tree
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

# UI
  ![alt text](ui.png)

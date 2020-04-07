# Voice Recongition

You can find the english documentacion below.

## Documentación en español

Reconocedor de voz y gramática en node.js sin necesidad de navegador ni plugins. Además no es necesario tener el foco en la aplicación para que el programa reconconozca la voz.

Esta nueva versión de **voice-recognition** ha dejado de usar **edge-js** para pasar a convertirse en un módulo completamente nativo de node.js ofreciendo un rendimiento más eficiente y garantizando su funcionamiento en todos los entornos de node.js, también se evitan los dolores de cabeza que en ocasiones puede dar compilar _edge-js_ para entornos como NWJ o Electrón.

**¡NOTA!**

El proyecto aun está en desarrollo y no están implementada todas las funciones, pero puedes crear un reconocimiento perfecto si usas un archivo de reconocimiento XML siguiendo la documentación que puedes encontrar en <a href="https://www.w3.org/TR/speech-grammar">Speech Grammar W3C</a>

## Build

El paquete viene compilado para la versión de node.js 12.16.0, es posible hacer reconstrucciones para otras versiones de node, pero no se garantiza un correcto funcioamiento para versiones anteriores anteriores a la 12 ya que el módulo hace uso de _Worker Threads_ que está disponible solo a partir de la versión 11.7.0 de node.js y otros complementos que solo están disponibles en las versiones más modernas de nodejs.

Para construir el paquete para node, tan solo ejecutar el siguiente comando después de instalar las dependencias de desarrollo:

```shell
$ npm run rebuild
```

Si se quiere hacer uso del paquete en algún entorno de ejecución distinto como por ejemplo NWJS o Electrón hay dos maneras de compilar el módulo. La primera es editar el _package.json_ y modificar las siguientes líneas para ajustarlo a tu versión de NWJS o Electrón:

```javascript
"scripts": {
    "test": "node test/test.js",
    "clean": "cmake-js clean",
    "build": "cmake-js build",
    "rebuild": "cmake-js rebuild",
    "build-nw": "cmake-js build --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64",
    "rebuild-nw": "cmake-js rebuild --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64",
    "build-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64",
    "rebuild-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64"
  }
```

Reemplazar el texto *\<\<YOUR-XX-VERSION>>* por la versión de NWJS o electrón en la que se vaya a compilar el módulo y luego ejecutar el script correspondiente en función de donde estés compilando el módulo

```shell
# Para NWJS
$ npm run rebuild-nw

# Para Electrón
$ npm run rebuild-electron
```

También puedes realizar una compilación manual más específicas siguiendo las instrucciones de <a href="https://github.com/cmake-js/cmake-js">cmake-js</a>.

### Iniciar el reconocedor

```javascript
const vr = require("voice-recognition");
const recognizer = new vr();
```

### Constructor

Al instanciar reconocedor el motor de reconocimiento se ejecutará con el idioma predeterminado del sistema operativo conocido como cultura. Es posible instanciar el reconocedor para una cultura concreta que esté instalada en el sistema operatvio, para hacerlo hay que instanciar el reconocedor con una de las culturas aceptadas.

```javascript
const recognizer = new vr("es-ES");
```

Las culturas disponibles con reconocimiento de voz en Windows 10 son:

- Español - España: (es-ES)
- Español - México: (es-MX)
- Inglés - EEUU: (en-US)
- Inglés - Reino Unido: (en-GB)
- Inglés - Canadá: (en-CA)
- Inglés - Australiz: (en-AU)
- Inglés - India: (en-IN)
- Francés - Francia: (fr-FR)
- Francés - Canadá: (fr-CA)
- Alemán - Alemania: (de-DE)
- Italiano - Italia: (it-IT)
- Portugués - Brasil: (pt-BR)
- Japonés - Japón: (ja-JP)
- Chino - China (zh-CN)
- Chino - Taiwan (zh-TW)
- Chino - Hong Kong (zh-HK)

Puedes instalar las diferentes culturas en Configuración -> Hora e idioma -> Idioma

### Opciones

Se pueden configurar algunas opciones para el motor de reconocimiento.

- **continuos**: _Boolean: (def: true)_
- **sameThread**: _Boolean: (def: false)_

#### continuos

La opción _continuos_ inidica si el motoro de reconocimiento debe permanecer escuchando hasta que se le ordene parar. Por defecto es _**true**_, si se establece en _false_ el motor de reconocimiento se detendrá una vez haya realizado un reconocimiento.

```javascript
recognizer.continuos = false;
```

#### sameThread

Indica si el motor de reconocimiento debe funcionar en el hilo principal de node o ejecutarse en otro hilo. Por defecto es _**false**_ ya que para evitar el bloqueo de la aplicación este se va a ejecutar en un hilo separado. Si se establece en _true_ el reconocedor de voz se ejecutará en el hilo principal y la aplicación quedará bloqueada hasta que se vayan realizando reconocimientos o el reconocedor se detenga.

```javascript
recognizer.sameThread = true;
```

### Escuchar el micrófono y reconocer el texto

Para empezar el reconocimiento de voz ejecutar el método _listen_

```javascript
recognizer.listen();
```

### Obtener culturas instaladas

Se pueden obtener las culturas instaladas en el sistema operativo con el siguiente método:

```javascript
let cultures = recognizer.get_installed_cultures()
```

Esto devolverá un array con las diferentes culturas instaladas ej.: ["es-ES","en-US","en-GB"]

### Obtener cultura del motor

Podemos obtener la cultura en la que se está ejecutando el motor de reoncoimiento con el siguiente método:

```javascript
let cultura = recognizer.get_engine_culture()
```

Esto devolverá un string con la cultura en la que se está ejecutando el motor de reconocimiento, ej.: _es-ES_

### Agregar gramáticas en XML

El método *add_grammar_from_xml* añade gramáticas en XML al motor de reconocimiento de voz. Este mátodo admite dos parámetros:

* param 1: Ruta al archivo de gramática XML.
* param 2: Nombre que queramos dar a la gramática.

```javascript
recognizer.add_grammar_from_xml( path.join( __dirname, "my-grammar.xml" ), "myGrammarName" );
```

### Escucha de eventos

El reconocedor enviará enventos cuando el motor realice tareas como devolver un texto reconocido o enviar un texto que no ha reconocido en la gramática.

Los eventos que están disponible para la escucha son:

#### vc:detected

Este evento ocurre cuando el motor de reconocimiento empieza a reconocer voz o texto.

```javascript
recognizer.on( "vc:detected", ( audio ) => {
	console.log( audio );
})
```

#### vc:recognized

Devuelve el resultado con el texto reconocido en una de las gramátcias enviadas.

```javascript
recognizer.on( "vc:recognized", ( result ) => {
	console.log( result );
})
```

#### vc:hypothesized

Devuelve resultados hipotéticos que el motor de reconocimiento cree que pueden coincidir con alguna de las reglas gramáticas tenga cargadas.

```javascript
recognizer.on( "vc:hypothesized", ( result ) => {
	console.log( result );
})
```

#### vc:rejected

Este evento es lanzado cuando el motor de reconocimiento detecta que está recibiendo datos pero no coinciden con ninguna de las gramáticas cargadas en el motor.

```javascript
recognizer.on( "vc:rejected", ( result ) => {
	console.log( result );
})
```

#### vc:audioState

Devuelve el estado del audio, por ejemplo silice o stopped

```javascript
recognizer.on( "vc:audioState", ( state ) => {
	console.log( state );
})
```

#### vc:audioLevel

Devuelve el nivel de audio que está recibiendo el motor de reconocimiento en un rango de 0 a 1 en numáros enteros.

```javascript
recognizer.on( "vc:audioLevel", ( level ) => {
	console.log( level );
})
```

#### vc:audioProblem

Este evento es lanzado cuando el motor de reconocimiento detecta un problema de audio recibido.

```javascript
recognizer.on( "vc:audioProblem", ( problem ) => {
	console.log( problem );
})
```
_!**NOTA**! Poco a poco iré implementando más funcionalidades hasta entonces es completamente funcional con estas caracterásticas descritas._

<br><br>

______________________

<br><br>

## English documentation

_Sorry for the english (Google Translator)_

Speech and grammar recognizer in node.js without browser or plugins. Furthermore, it is not necessary to have the focus on the application for the program to recognize the voice.

This new version of ** voice-recognition ** has stopped using ** edge-js ** to become a completely native node.js module offering more efficient performance and guaranteeing its operation in all node environments .js, headaches that can sometimes give compiling _edge-js_ for environments like NWJ or Electron are also avoided.

**!NOTE!**

The project is still under development and not all functions are implemented, but you can create a perfect recognition if you use an XML recognition file following the documentation you can find on <a href="https://www.w3.org/TR/speech-grammar">Speech Grammar W3C</a>

## Build

The package is compiled for the node.js version 12.16.0, it is possible to make reconstructions for other node versions, but a correct operation of the versions prior to 12 is not guaranteed. The module makes use of _Worker Threads_ which is Available only from version 11.7.0 of node.js and other plugins that are only available in the newer versions of nodejs.

To build the node package, just run the following command after installing the development dependencies:

```shell
$ npm run rebuild
```

If you want to use the package in a different execution environment such as NWJS or Electron, there are two ways to compile the module. The first is to edit the _package.json_ and modify the following lines to suit your version of NWJS or Electron:

```javascript
"scripts": {
    "test": "node test/test.js",
    "clean": "cmake-js clean",
    "build": "cmake-js build",
    "rebuild": "cmake-js rebuild",
    "build-nw": "cmake-js build --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64",
    "rebuild-nw": "cmake-js rebuild --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64",
    "build-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64",
    "rebuild-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64"
  }
```

Replace the text *\<\<YOUR-XX-VERSION>>* with the NWJS or electron version in which the module will be compiled and then execute the corresponding script depending on where you are compiling the module

```shell
# For NWJS
$ npm run rebuild-nw

# For Electrón
$ npm run rebuild-electron
```

You can also do a more specific manual compilation by following the instructions in <a href="https://github.com/cmake-js/cmake-js">cmake-js</a>.

### Start the recognizer

```javascript
const vr = require("voice-recognition");
const recognizer = new vr();
```

### Constructor

Upon instantiating recognizer the recognition engine will run with the default language of the operating system known as culture. It is possible to instantiate the recognizer for a specific culture that is installed in the operating system, to do so you must instantiate the recognizer with one of the accepted cultures.

```javascript
const recognizer = new vr("es-ES");
```

Las culturas disponibles con reconocimiento de voz en Windows 10 son:

- Spanish - Spain: (es-ES)
- Spanish - Mexico: (es-MX)
- English - USA: (en-US)
- English - United Kingdom: (en-GB)
- English - Canada: (en-CA)
- English - Australiz: (en-AU)
- English - India: (en-IN)
- French - France: (fr-FR)
- French - Canada: (fr-CA)
- German - Germany: (de-DE)
- Italian - Italy: (it-IT)
- Portuguese - Brazil: (pt-BR)
- Japanese - Japan: (ja-JP)
- Chinese - China (zh-CN)
- Chinese - Taiwan (zh-TW)
- Chinese - Hong Kong (zh-HK)

You can install the different cultures in Settings -> Time and language -> Language

### Options

Some options can be configured for the recognition engine.

- **continuos**: _Boolean: (def: true)_
- **sameThread**: _Boolean: (def: false)_

#### continuos

The _continuous_ option indicates whether the recognition engine should continue listening until it is ordered to stop. The default is _**true**_, if it is set to _false_ the recognition engine will stop once it has made a recognition.

```javascript
recognizer.continuos = false;
```

#### sameThread

Indicates whether the recognition engine should run on the main node thread or run on another thread. By default it is _**false**_ since to avoid blocking the application it will be executed in a separate thread. If set to _true_ the speech recognizer will run on the main thread and the application will be blocked until acknowledgments are performed or the recognizer stops.

```javascript
recognizer.sameThread = true;
```

### Listen

To start voice recognizer exec _listen_ method.

```javascript
recognizer.listen();
```

### Get installed cultures

The cultures installed in the operating system can be obtained with the following method:

```javascript
let cultures = recognizer.get_installed_cultures()
```

This will return an array with the different cultures installed eg: ["es-ES","en-US","en-GB"]

### Get engine culture

We can obtain the culture in which the recollection engine is running with the following method:

```javascript
let cultura = recognizer.get_engine_culture()
```

This will return a string with the culture in which the recognition engine is running, e.g .: _es-ES_

### Add grammars in XML

The *add_grammar_from_xml* method adds grammars in XML to the speech recognition engine. This method supports two parameters:

* param 1: Path to the XML grammar file.
* param 2: Name we want to give the grammar.

```javascript
recognizer.add_grammar_from_xml( path.join( __dirname, "my-grammar.xml" ), "myGrammarName" );
```

### Event listeners

The recognizer will send events when the engine performs tasks such as returning a recognized text or sending a text that has not been recognized in the grammar.

The events that are available for listening are:

#### vc:detected

This event occurs when the recognition engine begins to recognize voice or text.

```javascript
recognizer.on( "vc:detected", ( audio ) => {
	console.log( audio );
})
```

#### vc:recognized

Returns the result with the text recognized in one of the grammar sent.

```javascript
recognizer.on( "vc:recognized", ( result ) => {
	console.log( result );
})
```

#### vc:hypothesized

Returns hypothetical results that the revival engine believes may match any of the grammar rules loaded.

```javascript
recognizer.on( "vc:hypothesized", ( result ) => {
	console.log( result );
})
```

#### vc:rejected

This event is launched when the recognition engine detects that it is receiving data but does not match any of the grammars loaded in the engine.

```javascript
recognizer.on( "vc:rejected", ( result ) => {
	console.log( result );
})
```

#### vc:audioState

Returns the audio status, for example silice or stopped

```javascript
recognizer.on( "vc:audioState", ( state ) => {
	console.log( state );
})
```

#### vc:audioLevel

Returns the level of audio the recognition engine is receiving in a range of 0 to 1 in whole numbers.

```javascript
recognizer.on( "vc:audioLevel", ( level ) => {
	console.log( level );
})
```

#### vc:audioProblem

This event is launched when the recognition engine detects an audio problem received.

```javascript
recognizer.on( "vc:audioProblem", ( problem ) => {
	console.log( problem );
})
```
_!**NOTE**! Little by little I will be implementing more functionalities until then it is fully functional with these described characteristics._

__________________

## Release Notes

#### **1.0.0** version

- The way the module is built is changed.
- A rudimentary error control is added.
- A method is added to obtain cultures installed in the system.
- Added a method to get the culture in which the recognition engine is running.

#### **0.3.0** version

- Now the speech recognizer runs by default on a different thread than the main one in node.js, preventing the application listening process to block the main thread.



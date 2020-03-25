# Voice Recongition

You can find the english documentacion below.

## Documentación en español

Reconocedor de voz y gramática en node.js sin necesidad de navegador ni plugins. Además no es necesario tener el foco en la aplicación para que el programa reconconozca la voz.

El paquete utiliza **edge-js** para crear el reconocimiento de voz implementado en **C#**.

**¡NOTA!**

El proyecto aun está en desarrollo y no están implementada todas las funciones, pero puedes crear un reconocimiento perfecto si usas un archivo de reconocimiento XML siguiendo la documentación que puedes encontrar en <a href="https://www.w3.org/TR/speech-grammar">Speech Grammar W3C</a>

## Build

El paquete viene compilado para la versión de node.js 12.16.0, es posible hacer reconstrucciones para otras versiones de node, pero no se garantiza un correcto funcioamiento para versiones anteriores.

Para construir el paquete para node, tan solo ejecutar el siguiente comando después de instalar las dependencias de desarrollo:

```shell
$ npm run rebuild
```

Si se quiere hacer uso del paquete en algún entorno de ejecución distinto como por ejemplo NW o Electrón hay que editar el _package.json_ y ajustar la configuracion como se indica en la documentación <a href="https://github.com/cmake-js/cmake-js">cmake-js</a>.

### Iniciar el reconocedor

```javascript
const recognizer = require("voice-recognition");
```

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
_!**NOTA**! Poco a poco iré implementando más funcionalidades y cuando esté un poco más desarrollado lo colgare en Github. Hasta entonces es completamente funcional con estas caracterásticas descritas._

<br><br>

______________________

<br><br>

## English documentation

_Sorry for the english (Google Translator)_

Voice and grammar recognizer in node.js without the need for a browser or plugins. In addition it is not necessary to have the focus on the application for the program to recognize the voice.

The package uses **edge-js** to create the voice recognition implemented in **C#**.

**!NOTE!**

The project is still under development and not all functions are implemented, but you can create a perfect recognition if you use an XML recognition file following the documentation you can find on <a href="https://www.w3.org/TR/speech-grammar">Speech Grammar W3C</a>

### Start the recognizer

```javascript
const recognizer = require("voice-recognition");
```

### Add grammars in XML

The *add_grammar_from_xml* method adds grammars in XML to the speech recognition engine. This method supports two parameters:

* param 1: Path to the XML grammar file.
* param 2: Name we want to give the grammar.

```javascript
recognizer.add_grammar_from_xml( path.join( __dirname, "my-grammar.xml" ), "myGrammarName" );
```

### Event listeners

207/5000
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
_!**NOTE**! Little by little I will be implementing more functionalities and when I am a little more developed I will hang it on Github. Until then it is fully functional with these features described._



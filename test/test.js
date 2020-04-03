/**
 * EJECUTA TEST Y DI= "Tengo XX años"; 
 * 
 * RUN TEST AND SAY = "I am XX years old"
 */

const path = require("path");
const { VoiceRecognizer } = require("../src/voice-recognition.js");

const recognizer = new VoiceRecognizer("es-ES");

console.log( "Installed cultures: " + JSON.stringify( recognizer.installed_cultures()));
console.log( "Engine culture: " + recognizer.get_engine_culture());

// Spanish grammar
recognizer.add_grammar_from_xml( path.resolve( "test", "grammar-es.xml"), "edad");
// English grammar (Delete comment and comment Spanish grammar line if you want use english grammar)
// recognizer.add_grammar_from_xml( path.resolve( "test", "grammar-en.xml"), "age");

recognizer.listen();

recognizer.on( "vc:audioLevel", (result) => {
	console.log( result );
});

recognizer.on( "vc:recognized", ( result ) => {
	console.log( result );
});
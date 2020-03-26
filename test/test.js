/**
 * EJECUTA EL ARCHIVO Y DI= "Tengo XX años"; 
 */


const path = require("path");
const { VoiceRecognizer } = require("../src/voice-recognition.js");


const recognizer = new VoiceRecognizer();

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
const events = require("events");
console.log("HOLA");
const addon = require("../build/Release/voice_recognition.node");
console.log("HOLA");


console.log(addon.hello());
/*
class VoiceRecognizer extends events.EventEmitter {
	constructor() {
		super();

		//this.recognizer = null;
		this.continuos = true;
		this.options = null;

		this._stoped = true;
	}

	/**
	 * @method	listen
	 *
	 * Pone a la escucha el motor de reconocimiento para esuchar una frase.
	 *
	 * @returns	{void}
	 */
/*listen() {
	this._stoped = false;
	addon.listen();
}*/

/**
 * @method	stop
 *
 * Detiene el reconocimiento de voz
 *
 * @returns	{void}
 */
/*stop() {
	this._stoped = true;
}
}

module.exports.VoiceRecognizer = VoiceRecognizer;
*/

/*

let hola = addon.sayHello();
console.log( "Hola que tal est�s" );
console.log( hola );

*/
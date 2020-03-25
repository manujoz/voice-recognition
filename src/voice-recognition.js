const fs = require("fs");
const addon = require('bindings')('voice-recognition');
const events = require( "events" ).EventEmitter;

class VoiceRecognizer extends events {
	constructor() 
	{
		super();

		//this.recognizer = null;
		this.continuos = true;
		this.options = null;
		this._stoped = true;
		
		// Le enviamos al addon el emiter

		addon._call_emit(this._get_result.bind(this));
	}

	/**
	 * @method	listen
	 *
	 * Pone a la escucha el motor de reconocimiento para esuchar una frase.
	 *
	 * @returns	{void}
	 */
	listen() 
	{
		this._stoped = false;
		setImmediate(() => {
			addon.listen();
		})
	}

	/** TODO: Esta función está por implemtar en C#
	 * @method	set_input_from_wav
	 * 
	 * Le dice al reconocedor desde que archivo de audio va a reconocer. Si este no se asigna,
	 * el reconocedor, lo hará directamentes desde el micrófono.
	 * 
	 * @param 	{string} 	file 			Ruta al archivo de audio desde el que queremos el reconocimiento
	 * @returns	{void}
	 */
	set_input_from_wav( file = null )
	{
		if( !this.recognizer ) {
			setTimeout(() => {
				this.set_input_from_wav( file );
			}, 500);
			return;
		}

		if( !file || !fs.existsSync( file ) ) {
			console.error( "[voice-recognition]: No se ha pasado un archivo válido para el reconocedor." )
		}

		console.warn( "[voice-recognition]: Esta función aun no está implementada." )
	}

	/**
	 * @method	add_grammar_from_xml
	 * 
	 * Asigna una gramática al reconocedor desde un archivo.
	 * 
	 * @param	{string}	file 			Ruta al archivo XML que contiene la grmática.
	 * @returns	{void}
	 */
	add_grammar_from_xml( file = null, name = "mygrammar" ) 
	{
		if( !file || !fs.existsSync( file ) ) {
			console.error( "[voice-recognition]: Grammar file does not exists." )
			return false;
		}
		
		addon.add_grammar_XML( file, name );
	}

	/** TODO: Esta función está por implementar-
	 * @method	add_grammar
	 * 
	 * Añade gramática programática al programa de reconocimiento de voz.
	 * 
	 * @param 	{object} 	grammar 		Objeto con la grmática que queremos añadir
	 * @returns	{void}
	 */
	add_grammar( grammar = null ) 
	{
		if( !this.recognizer ) {
			setTimeout(() => {
				this.add_grammar( grammar );
			}, 500);
			return;
		}

		console.warn( "[voice-recognition]: Esta función aun no está implementada." )

		// TODO: Hacer lo que sea.

		//this.recognizer.add_grammar();
	}

	/** TODO: Esta función está por implementar.
	 * @method	rem_grammar
	 * 
	 * Elimina una gramática del reconocedor
	 * 
	 * @param 	{???} 		grammar 		La gramática que queremos eliminar del objeto
	 * @returns	{void}
	 */
	rem_grammar( grammar ) 
	{
		console.warn( "[voice-recognition]: Esta función aun no está implementada." )
		return;
	}

	/**
	 * @method	stop
	 *
	 * Detiene el reconocimiento de voz
	 *
	 * @returns	{void}
	 */
	stop() 
	{
		this._stoped = true;
	}

	/**
	 * @method	_get_result
	 * 
	 * Metodo que es llamado desde el addon cuando el motor de reconociemiento devuelve algún
	 * evento
	 * 
	 * @param	{string}	evName		Nombre del evento al que hay que llamar
	 * @param	{string}	result		Resultado del evento del motor de reconocimiento
	 * @returns	{void}
	 */
	_get_result( evName, result )
	{
		if( evName == "vc:audioState" ){
			this._audio_state(result);
		} else if( evName == "vc:audioLevel" ){
			this._audio_level(result);
		} else if( evName == "vc:audioProblem" ){
			this._audio_problem(result);
		} else if( evName == "vc:detected" ){
			this._detected(result);
		} else if( evName == "vc:recognized" ){
			this._recognized(result);
		} else if( evName == "vc:hypothesized" ){
			this._hypothesized(result);
		} else if( evName == "vc:rejected" ){
			this._rejected(result);
		} else if( evName == "vc:completed" ){
			this._completed(result);
		}
	}

	/**
	 * @method	_audio_state
	 * 
	 * Recibe el evento del estado del audio enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_audio_state( result )
	{
		let rJSON = JSON.parse( result );

		this.emit("vc:audioState", rJSON.AudioState );
	}

	/**
	 * @method	_audio_level
	 * 
	 * Recibe el evento del nivel del audio enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_audio_level( result )
	{
		let rJSON = JSON.parse( result );
		this.emit("vc:audioLevel", parseInt( rJSON.AudioLevel ));
	}

	/**
	 * @method	_audio_problem
	 * 
	 * Recibe el evento de problema de audio enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_audio_problem( result )
	{
		let rJSON = JSON.parse( result );

		this.emit("vc:audioProblem", rJSON );
	}

	/**
	 * @method	_detected
	 * 
	 * Recibe el evento de detección enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_detected( result )
	{
		let rJSON = JSON.parse( result );

		this.emit("vc:detected", rJSON.AudioPosition );
	}

	/**
	 * @method	_recognized
	 * 
	 * Recibe el evento de texto reconocido enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_recognized( result )
	{
		let response = this._construct_result( result );

		this.emit("vc:recognized", response );

		if( this.continuos && !this._stoped ) {
			this.listen();
		}
	}

	/**
	 * @method	_hypothesized
	 * 
	 * Recibe el evento de texto hipotético enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_hypothesized( result )
	{
		let response = this._construct_result( result );

		this.emit("vc:hypothesized", response );
	}

	/**
	 * @method	_rejected
	 * 
	 * Recibe el evento de rejected enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_rejected( result )
	{
		let response = this._construct_result( result );

		this.emit("vc:rejected", response );

		if( this.continuos && !this._stoped ) {
			this.listen();
		}
	}

	/**
	 * @method	_completed
	 * 
	 * Recibe el evento de completado enviado por el motor de reconocimiento
	 * 
	 * @param	{string}	result		Resultado del evento en JSON
	 * @returns	{void}
	 */
	_completed( result )
	{
		let response = this._construct_result( result );
		
		this.emit("vc:completed", response );
	}

	/**
	 * @method	_construct_result
	 * 
	 * Construye el resultado devuelto por C# para devolverlo en un orden adecuado.
	 * 
	 * @param 	{object} 	result 			Resltado de un reconocimiento de voz
	 * @returns	{object}					Objeto con el resultado ordenado debidamente.
	 */
	_construct_result( result ) 
	{
		let rJSON = JSON.parse( result );

		let constructed = {};
		constructed.Result = rJSON.Result;
		constructed.Result.Semantics = JSON.parse( rJSON.Semantics );

		return constructed.Result;
	}
}

module.exports.VoiceRecognizer = VoiceRecognizer;
const fs = require("fs");
const path = require("path");
const addon = require('bindings')('voice-recognition');
const { Worker } = require("worker_threads");
const events = require( "events" ).EventEmitter;

class VoiceRecognizer extends events {
	constructor( culture = "" ) 
	{
		super();

		this.sameThread = false;
		this.continuos = true;

		this._worker = null;
		this._stoped = true;
		this._setedFunction = false;
		this._isConstructed = true;

		let installeds = this.installed_cultures();

		if( !addon.constructorJS( culture )) {
			this._isConstructed = false;
			console.error( "[voice-recognition]: Culture [" + culture + "] is not installed on the device. Installed: " + JSON.stringify(installeds));
		}
	}

	/**
	 * @method	listen
	 *
	 * Listen to the recognition engine to hear a phrase.
	 *
	 * @returns	{void}
	 */
	listen() 
	{
		// We stop if the addon has not been built

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}

		// We listen to the engine

		if( this.sameThread ) {
			this._set_function_emit();

			this._stoped = false;

			setImmediate(() => {
				addon.listen();
			})
		} else {
			this._worker = new Worker( path.resolve( __dirname , "worker.js" ) );

			this._worker.postMessage({
				listen: true,
				continuos: this.continuos
			});

			this._worker.on( "message", response => {
				this._get_result( response.evName, response.result );
			});

			this._worker.on( "error", error => {
				console.error( "[voice-recognition]: ", error )
			});

			this._worker.on( "exit", () => {
				
			});
		}
	}

	/** TODO: Esta función está por implemtar en C#
	 * @method	set_input_from_wav
	 * 
	 * It tells the recognizer from which audio file it will recognize. If this is not assigned, 
	 * the recognizer will do so directly from the microphone.
	 * 
	 * @param 	{string} 	file 			Path to the audio file from which we want recognition.
	 * @returns	{void}
	 */
	set_input_from_wav( file = null )
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
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
	 * Assign a grammar to the recognizer from a file.
	 * 
	 * @param	{string}	file 			Path to the XML file containing the grammar.
	 * @returns	{void}
	 */
	add_grammar_from_xml( file = null, name = "mygrammar" ) 
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}

		if( !file || !fs.existsSync( file ) ) {
			console.error( "[voice-recognition]: Grammar file does not exists." )
			return false;
		}
		
		addon.add_grammar_XML( file, name );
	}

	/** TODO: This function is yet to be implemented.
	 * @method	add_grammar
	 * 
	 * Add programmatic grammar to the speech recognition program.
	 * 
	 * @param 	{object} 	grammar 		Object with the grammar we want to add.
	 * @returns	{void}
	 */
	add_grammar( grammar = null ) 
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}
		console.warn( "[voice-recognition]: Esta función aun no está implementada." )

		// TODO: Hacer lo que sea.

		//this.recognizer.add_grammar();
	}

	/** TODO: This function is yet to be implemented.
	 * @method	rem_grammar
	 * 
	 * Remove a grammar from the recognizer.
	 * 
	 * @param 	{???} 		grammar 		The grammar we want to remove from the object.
	 * @returns	{void}
	 */
	rem_grammar( grammar ) 
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}
		console.warn( "[voice-recognition]: Esta función aun no está implementada." )
		return;
	}

	/**
	 * @method	stop
	 *
	 * Stop speech recognition.
	 *
	 * @returns	{void}
	 */
	stop() 
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}

		if( this.sameThread ) {
			this._stoped = true;
		} else {
			if( this._worker ) {
				this._worker.terminate().
					then(() => {
						this._worker = null;
						
					});
			}
		}
	}

	/**
	 * @method	get_engine_culture
	 * 
	 * Get recognition engine culture.
	 * 
	 * @returns	{string}		Culture that the recognition engine is using.
	 */
	get_engine_culture() 
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}

		return addon.get_engine_culture();
	}

	/**
	 * @method	installed_cultures
	 * 
	 * Get the cultures installed on the device.
	 * 
	 * @returns	{array}			Array with installed cultures.
	 */
	installed_cultures()
	{
		// Detenemos si no se ha construido el addon

		if( !this._isConstructed ) {
			console.error( "[voice-recognition]: Addon is not instantiated" )
			return;
		}

		let cultures = addon.get_installed_cultures();
		
		if( cultures != "" ) {
			cultures = JSON.parse( cultures );
		}

		return cultures;
	}

	/**
	 * @method	_set_function_emit
	 * 
	 * Configure the function to which the addon will return the results.
	 * 
	 * @returns	{void}
	 */
	_set_function_emit()
	{
		if( this._setedFunction ) {
			return;
		}
		
		// Le enviamos al addon el emiter

		addon.result_function(this._get_result.bind(this));
		this._setedFunction = true;
	}

	/**
	 * @method	get_installed_cultures
	 * 
	 * Get the cultures installed on the device when building the addon.
	 * 
	 * @returns	{array}					Array with installed cultures.
	 */
	static get_installed_cultures()
	{
		let cultures = addon.get_installed_cultures();
		
		if( cultures != "" ) {
			cultures = JSON.parse( cultures );
		}

		return cultures;
	}

	/**
	 * @method	_get_result
	 * 
	 * Method that is called from the addon when the recognition engine returns some
	 * event.
	 * 
	 * @param	{string}	evName		Name of the event to call.
	 * @param	{string}	result		Recognition engine event result
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
		} else if( evName == "vcpr:error" ){
			this._error_addon(result);
		}
	}

	/**
	 * @method	_audio_state
	 * 
	 * Receive the audio status event sent by the recognition.
	 * 
	 * @param	{string}	result		Event result in JSON
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
	 * Receive the audio level event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
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
	 * You receive the audio problem event sent by the recognition engine
	 * 
	 * @param	{string}	result		Event result in JSON
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
	 * Receive the detection event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
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
	 * Receive the recognized text event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
	 * @returns	{void}
	 */
	_recognized( result )
	{
		let response = this._construct_result( result );

		this.emit("vc:recognized", response );

		if( this.sameThread && this.continuos && !this._stoped ) {
			this.listen();
		}
	}

	/**
	 * @method	_hypothesized
	 * 
	 * Receive the hypothetical text event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
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
	 * Receive the rejected event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
	 * @returns	{void}
	 */
	_rejected( result )
	{
		let response = this._construct_result( result );

		this.emit("vc:rejected", response );

		if( this.sameThread && this.continuos && !this._stoped ) {
			this.listen();
		}
	}

	/**
	 * @method	_completed
	 * 
	 * Receive the completion event sent by the recognition engine.
	 * 
	 * @param	{string}	result		Event result in JSON
	 * @returns	{void}
	 */
	_completed( result )
	{
		let response = this._construct_result( result );
		
		this.emit("vc:completed", response );
	}

	/**
	 * @method	_error_addon
	 * 
	 * Handles the errors returned by the addon.
	 * 
	 * @param 	{string} 	error 			Error returned by the addon.
	 * @returns	{void}
	 */
	_error_addon( error )
	{
		console.error( "[voice-recognition]: " + error );
		this.stop();
	}

	/**
	 * @method	_construct_result
	 * 
	 * Construct the result returned by C# to return it in a proper order.
	 * 
	 * @param 	{object} 	result 			Voice recognition result.
	 * @returns	{object}					Object with the result properly ordered.
	 */
	_construct_result( result ) 
	{
		let response = JSON.parse( result );
		response.Semantics = JSON.parse( response.Semantics );

		return response;
	}
}

module.exports.VoiceRecognizer = VoiceRecognizer;
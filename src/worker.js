const addon = require('bindings')('voice-recognition');
const { parentPort } = require("worker_threads");

class WorkerVc
{
	constructor()
	{
		this.continuos = true;

		// We send the addon the response function

		addon.result_function(this._get_result.bind(this));
	}

	/**
	 * @method	_listen
	 * 
	 * Listen to the recognition engine.
	 */
	_listen() 
	{
		setImmediate(() => {
			addon.listen();
		})
	}

	/**
	 * @method	_get_result
	 * 
	 * Method that is called from the addon when the recognition engine returns some 
	 * event.
	 * 
	 * @param	{string}	evName		Nombre del evento al que hay que llamar
	 * @param	{string}	result		Resultado del evento del motor de reconocimiento
	 * @returns	{void}
	 */
	_get_result( evName, result )
	{
		let response = {
			evName: evName,
			result: result
		}
		
		parentPort.postMessage( response );

		if( this.continuos && ( evName == "vc:recognized" || evName == "vc:rejected" )) {
			this._listen();
		}
	}
}

const worker = new WorkerVc();

parentPort.on( "message", response => {
	if( response.continuos != undefined ) {
		worker.continuos = response.continuos;
	}

	if( response.listen != undefined ) {
		worker._listen();
	}
});
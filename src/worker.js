const addon = require('bindings')('voice-recognition');
const { parentPort } = require("worker_threads");

class WorkerVc
{
	constructor()
	{
		this.continuos = true;

		// Le enviamos al addon el emiter

		addon._call_emit(this._get_result.bind(this));
	}

	/**
	 * @method	_listen
	 * 
	 * Pone a la escucha el motor de reconocimiento
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
	 * Metodo que es llamado desde el addon cuando el motor de reconociemiento devuelve algún
	 * evento
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Web.Script.Serialization;

namespace CsVoiceRecognition
{

    public class CsRecognizer
    {
        // Función que emite los eventos a CPP
        public Func<string, string, string> emitEventToCpp;

        // Creamos la instancia estática para poder acceder desde C++ a la clase instanciada
        private static CsRecognizer oInstance;

        // Extensiones
        public JavaScriptSerializer JSON = new JavaScriptSerializer();

        // Motor de reconocimiento
        public SpeechRecognitionEngine Engine;

        // Clase de gramáticas
        public CsGrammars Grammars = new CsGrammars();

        // Determina si el motor está escuchando o no
        public bool IsListen = false;
        
        // Audio input
        public string AudioInput;


        /**
         * @method  Constructor
         * 
         * Constructo de la clase CsRecognizer.
         */
        public CsRecognizer()
        {

        }

        /**
         * @method  Instance
         * 
         * Creamos el método que instancia la clase en un ámbito privado
         * 
         * @returns {CsRecognizer}      Clase instanciada.
         */
        public static CsRecognizer Instance()
        {

            if (oInstance == null)
            {
                oInstance = new CsRecognizer();
            }

            return oInstance;
        }

        /**
         * @method  AddGrammarXML
         * 
         * Añade una gramática a través de un archivo XML
         * 
         * @param   {string}    file            Ruta al archivo de gramática
         * @returns {void}
         */
        public void AddGrammarXML(string path, string name)
        {
            Grammars.AddXML(path, name);
        }

        /** TODO: Esta función está por implementar
         * @method SetAudioInput
         * 
         * Asigna el audio input que se va a usar por el motor de reconocimiento
         * 
         * @param   {sring}     audio       Cadena con el audio que se va a usar
         * @returns {void}
         */
        public void SetAudioInput(string audio = null)
        {

        }

        /**
        * @method  LoadGrammars
        * 
        * Carga las gramáticas correspondientes en el motor de reconocimiento de voz
        * 
        * @returns {void}
        */
        private void LoadGrammars()
        {
            if (Grammars.Length() == 0)
            {
                Engine.LoadGrammar(new DictationGrammar());
            }
            else
            {
                Grammars.LoadAll(Engine);
            }
        }

        /**
         * @method  pr_load_audio_input
         * 
         * Carga el input de audio que se va a usar para el reconocimiento.
         * 
         * @returns {void}
         */
        private void LoadAudioInput()
        {
            if (AudioInput == null)
            {
                Engine.SetInputToDefaultAudioDevice();
            }
            else
            {
                // TODO: Asignar archivo de audio desde una URL
            }
        }

        /**
         * @method  Listen
         * 
         * Pone a la escucha el motor de reconocimiento de voz
         * 
         * @returns {void}
         */
        public void Listen()
        {
            if (Engine == null)
            {
                // Cargamos el motor
                Engine = new SpeechRecognitionEngine();

                // Cargamos la gramática en el motor de reconocimiento
                LoadGrammars();

                // Carga el input de audio que se va a usar para el reconocimiento.
                LoadAudioInput();

                // Add a handler for the speech recognized event.  
                Engine.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(EventAudioStateChange);
                Engine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(EventAudioLevelUpdate);
                Engine.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(EventAudiProblem);
                Engine.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(EventSpeechDetected);
                Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(EventSpeechRecognized);
                Engine.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(EventSpeechHiposized);
                Engine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(EventSpeechRejected);
                //Engine.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(EventSpeechCompleted);
            }

            // Inciiamos el reconocimiento 
            if(!IsListen)
            {
                IsListen = true;
                Engine.Recognize();
            }
        }
        
        /**
         * @method  EventAudioStateChange
         * 
         * Función que recoge el cambio de estado de la entrada de audio.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventAudioStateChange(object sender, AudioStateChangedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioState");
        }

        /**
         * @method  EventAudioLevelUpdate
         * 
         * Función que recoge el nivel de audio que recoge el reconocedor
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventAudioLevelUpdate(object sender, AudioLevelUpdatedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioLevel");
        }

        /**
         * @method  EventAudiProblem
         * 
         * Función que cuando se detecta un problema de audio.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventAudiProblem(object sender, AudioSignalProblemOccurredEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioProblem");
        }

        /**
         * @method  EventSpeechDetected
         * 
         * Función que recoge el evento de deteción del motor de reconocimiento.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventSpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:detected");
        }

        /**
         * @method  EventSpeechRecognized
         * 
         * Función que recoge el evento de reconocido del motor de reconicimiento.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            IsListen = false;

            CsResult result = new CsResult();
            result.CreateRecognizer(e);
            string data = JSON.Serialize(result);

            EventDispatch(data, "vc:recognized");
        }

        /**
         * @method  EventSpeechHiposized
         * 
         * Función que recoge el evento de resultados hipotéticos del motor de reconicimiento.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventSpeechHiposized(object sender, SpeechHypothesizedEventArgs e)
        {
            IsListen = false;

            CsResult result = new CsResult();
            result.CreateHypothesized(e);
            string data = JSON.Serialize(result);
            EventDispatch(data, "vc:hypothesized");
        }

        /**
         * @method  EventSpeechRejected
         * 
         * Función que recoge el evento de no coincidentes con gramática del motor de reconicimiento.
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        public void EventSpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            IsListen = false;

            CsResult result = new CsResult();
            result.CreateRejected(e);
            string data = JSON.Serialize(result);
            EventDispatch(data, "vc:rejected");
        }

        /**
         * @method  EventSpeechCompleted
         * 
         * Función que recoge el evento de comletado con gramática del motor de reconicimiento. Este evento solo
         * se dispara cuando se hace uso de RecognizedAsync().
         * 
         * @param   {object}    sender          Objeto enviado por el evento de reconocimiento.
         * @paran   {object}    e               Evento con el resultado devuelto por el motor de reconocimiento.
         * @returns {void}
         */
        /*public void EventSpeechCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            CsResult result = new CsResult();
            result.CreateCompleted(e);
            string data = JSON.Serialize(result);
            EventDispatch(data, "vc:completed");
        }*/

        /**
         * @method  EventTrigger
         * 
         * Devuelve los valores de los eventos enviados por el motor de reconocimiento
         * 
         * @param   {string}    data            Datos en JSON creados de los eventos del motor de reconocimiento
         * @returns {string}                    Datos del evento serializados en JSON
         */
        public void EventDispatch(string data, string evName)
        {
            emitEventToCpp( data, evName );
        }
    }

}

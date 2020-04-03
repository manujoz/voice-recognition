using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Web.Script.Serialization;

namespace VoiceRecognizer
{

    public partial class Recognizer
    {
        // Recognizer engine
        public SpeechRecognitionEngine Engine;

        // Recognizer engine culture
        public string Culture = "";

        // Determine if the engine is listening or not
        public bool IsListen = false;

        // Audio input
        private string AudioInputFile;

        // Grammars class
        public CsGrammars Grammars = new CsGrammars();

        // Web extensions
        public JavaScriptSerializer JSON = new JavaScriptSerializer();

        /**
         * @method  Constructor
         * 
         * Constructor of the Recognizer class.
         */
        public Recognizer()
        {
            
        }

        /**
         * @method  Init
         * 
         * Initialize the recognition engine
         * 
         * @returns {void}
         */
        public void Init(string culture = "")
        {
            // Si el motor está instanciado
            if(Engine != null) {
                return;
            }

            // Si pasamos una cultura específica comprobamos que este instalad
            RecognizerInfo cultObj = null;

            if (culture != "") {
                foreach (RecognizerInfo installed in SpeechRecognitionEngine.InstalledRecognizers()) {
                    if (installed.Culture.Name.ToString() == culture) {
                        cultObj = installed;
                        break;
                    }
                }

                if (cultObj == null) {
                    emitEventToCpp("Culture " + culture + " is not installed on device. Installed cultures: " + InstalledCultures(), "vcpr:error");
                    return;
                }
            }

            // Instanciamos el motor de reconocimiento
            if (cultObj != null) {
                Engine = new SpeechRecognitionEngine(cultObj);
            } else {
                Engine = new SpeechRecognitionEngine();
            }

            // Carga el input de audio que se va a usar para el reconocimiento.
            LoadAudioInput();

            // Añade los eventos al motor de reconocimiento 
            Engine.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(EventAudioStateChange);
            Engine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(EventAudioLevelUpdate);
            Engine.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(EventAudiProblem);
            Engine.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(EventSpeechDetected);
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(EventSpeechRecognized);
            Engine.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(EventSpeechHypothesized);
            Engine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(EventSpeechRejected);
            //Engine.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(EventSpeechCompleted);

            // Asignamos la propiedad de cultura
            Culture = Engine.RecognizerInfo.Culture.ToString();
        }

        /**
         * @method  Destroy
         * 
         * Destroy the recognizer engine
         * 
         * @returns {void}
         */
        public void Destroy()
        {
            if(Engine != null) {
                Engine.Dispose();
                Engine = null;
                Culture = "";
            }
        }

        /**
         * @method  IsInstalledCulture
         * 
         * Return if a culture is installed on device
         * 
         * @params  {string}    culture         String with culture that we wnat to verify
         * @returns {boolean}
         */
        public bool IsInstalledCulture(string culture)
        {
            foreach (RecognizerInfo installed in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (installed.Culture.Name.ToString() == culture)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * @method  InstalledCultures
         * 
         * Returns a JSON string with the cultures installed on the device
         * 
         * @returns {string}      Installed cultures.
         */
        public string InstalledCultures()
        {
            List<string> cultures = new List<string>();

            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                cultures.Add(config.Culture.Name.ToString());
            }

            return JSON.Serialize(cultures);
        }

        /**
         * @method  AddGrammarXML
         * 
         * Add a grammar from XML file
         * 
         * @param   {string}    file            Path to grammar file
         * @returns {void}
         */
        public void AddGrammarXML(string path, string name)
        {
            if (!System.IO.File.Exists(path)) {
                emitEventToCpp("Grammar file not found: " + path, "vcpr:error");
                return;
            }

            Grammars.AddXML(path, name);
        }

        /** 
         * @method SetAudioInputFile
         * 
         * Assign the audio input to be used by the recognition engine from a file.
         * 
         * @param   {sring}     audio       Path to audio file
         * @returns {void}
         */
        public void SetAudioInputFile(string path)
        {
            if( !System.IO.File.Exists(path)) {
                emitEventToCpp("Audio file not found: " + path, "vcpr:error");
                return;
            }
            
            AudioInputFile = path;
        }

        /**
         * @method  Listen
         * 
         * Activate voice recognition
         * 
         * @returns {void}
         */
        public void Listen()
        {
            // Check if engine is instantiated
            if (Engine == null)
            {
                emitEventToCpp("Engine recognizer is not instantiated", "vcpr:error");
                return;
            }

            // Load grammars
            LoadGrammars();

            // Start recognition
            if (!IsListen)
            {
                IsListen = true;
                Engine.Recognize();
            }

        }

        /**
         * @method  LoadAudioInput
         * 
         * Loads the audio input to be used for recognition engine.
         * 
         * @returns {void}
         */
        private void LoadAudioInput()
        {
            if (AudioInputFile == null)
            {
                Engine.SetInputToDefaultAudioDevice();
            }
            else
            {
                // TODO: Assign audio file as input from a path
            }
        }

        /**
        * @method  LoadGrammars
        * 
        * Load the corresponding grammars into the speech recognition engine
        * 
        * @returns {void}
        */
        private void LoadGrammars()
        {
            // Assign the function of emitting events to CPP if it is not assigned
            if (Grammars.emitEventToCpp == null)
            {
                Grammars.emitEventToCpp = emitEventToCpp;
            }

            // Load grammars
            if (Grammars.Length() == 0)
            {
                Engine.LoadGrammar(new DictationGrammar());
            }
            else
            {
                Grammars.LoadAll(Engine);
            }
        }

    }

}

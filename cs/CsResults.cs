using System;
using System.Collections.Generic;
using System.Text;
using System.Speech.Recognition;

namespace CsVoiceRecognition
{
    class CsResult
    {
        public object Result;
        public string Semantics;

        /**
         * @method  CreateRecognizer
         * 
         * Crea un objeto de resultado a partir de un evento de reconocido
         * 
         * @param   {SpeechRecognizedEventArgs}     Evento devuelto por el reconocedor.
         * @returns {void}
         */
        public void CreateRecognizer(SpeechRecognizedEventArgs e)
        {
            Result = e.Result;
            Semantics = prConstructSemanticsJSON(e.Result.Semantics);
        }

        /**
         * @method  CreateHypothesized
         * 
         * Crea un objeto de resultado a partir de un evento de hypothesized
         * 
         * @param   {SpeechHypothesizedEventArgs}       Evento devuelto por el reconocedor.
         * @returns {void}
         */
        public void CreateHypothesized(SpeechHypothesizedEventArgs e)
        {
            Result = e.Result;
            Semantics = prConstructSemanticsJSON(e.Result.Semantics);
        }

        /**
         * @method  CreateRejected
         * 
         * Crea un objeto de resultado a partir de un evento de rejected
         * 
         * @param   {SpeechRecognitionRejectedEventArgs}    Evento devuelto por el reconocedor.
         * @returns {void}
         */
        public void CreateRejected(SpeechRecognitionRejectedEventArgs e)
        {
            Result = e.Result;
            Semantics = prConstructSemanticsJSON(e.Result.Semantics);
        }

        /**
         * @method  CreateCompleted
         * 
         * Crea un objeto de resultado a partir de un evento de completado
         * 
         * @param   {RecognizeCompletedEventArgs}       Evento devuelto por el reconocedor.
         * @returns {void}
         */
        public void CreateCompleted(RecognizeCompletedEventArgs e)
        {
            Result = e.Result;
            Semantics = prConstructSemanticsJSON(e.Result.Semantics);
        }

        /**
         * @method  prConstructSemanticsJSON
         * 
         * Construye un objeto de semántica en JSON manualmente para que pueda ser leido en javascript.
         * 
         * @param   {SemanticValue}         Objeto de semántica devuelto por el motor de reconocimiento.
         * @returns {string}                String en formato JSON con el objeto de semántica serializado.
         */
        private string prConstructSemanticsJSON(SemanticValue sem)
        {
            string semantics = null;
            foreach (KeyValuePair<String, SemanticValue> child in sem)
            {
                semantics += (semantics == null) ? "[" : ",";
                semantics += "{\"" + child.Key + "\":\"" + child.Value.Value + "\"}";
                //Console.WriteLine("Los litros a repostar son: {0} {1}", child.Key, child.Value.Value ?? "null");
            }

            if (semantics != null)
            {
                semantics += "]";
            }

            return semantics;
        }
    }
}

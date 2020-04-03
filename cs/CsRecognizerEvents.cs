using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;



namespace VoiceRecognizer
{
    public partial class Recognizer
    {
        // Function that issues events to CPP
        public Func<string, string, string> emitEventToCpp;

        /**
         * @method  EventAudioStateChange
         * 
         * Method that collects the change of state of the audio input.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventAudioStateChange(object sender, AudioStateChangedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioState");
        }

        /**
         * @method  EventAudioLevelUpdate
         * 
         * Method that collects the audio level that the recognizer collects
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventAudioLevelUpdate(object sender, AudioLevelUpdatedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioLevel");
        }

        /**
         * @method  EventAudiProblem
         * 
         * Method that when an audio problem is detected.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventAudiProblem(object sender, AudioSignalProblemOccurredEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:audioProblem");
        }

        /**
         * @method  EventSpeechDetected
         * 
         * Method that collects the detection event of the recognition engine.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventSpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            string data = JSON.Serialize(e);
            EventDispatch(data, "vc:detected");
        }

        /**
         * @method  EventSpeechRecognized
         * 
         * Method that collects the recognition event of the recognition engine.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            IsListen = false;

            CsResult result = new CsResult();
            result.CreateRecognizer(e, Grammars);
            string data = JSON.Serialize(result.Result);

            EventDispatch(data, "vc:recognized");
        }

        /**
         * @method  EventSpeechHypothesized
         * 
         * Method that collects the event of hypothetical results of the recognition engine.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            CsResult result = new CsResult();
            result.CreateHypothesized(e, Grammars);
            string data = JSON.Serialize(result.Result);
            EventDispatch(data, "vc:hypothesized");
        }

        /**
         * @method  EventSpeechRejected
         * 
         * Method that collects the non-coincident event with the recognition engine grammar.
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        private void EventSpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            IsListen = false;

            CsResult result = new CsResult();
            result.CreateRejected(e, Grammars);
            string data = JSON.Serialize(result.Result);
            EventDispatch(data, "vc:rejected");
        }

        /**
         * @method  EventSpeechCompleted
         * 
         * Method that collects the event with the grammar of the recognition engine. This event only
         * fired when RecognizedAsync() is used
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         * @returns {void}
         */
        /*private void EventSpeechCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            CsResult result = new CsResult();
            result.CreateCompleted(e, Grammars);
            string data = JSON.Serialize(result.Result);
            EventDispatch(data, "vc:completed");
        }*/

        /**
         * @method  EventTrigger
         * 
         * Returns the values of the events sent by the recognition engine
         * 
         * @param   {object}    sender          Object sent by the recognition event.
         * @paran   {object}    e               Event with the result returned by the recognition engine.
         */
        private void EventDispatch(string data, string evName)
        {
            emitEventToCpp(data, evName);
        }

    }
}

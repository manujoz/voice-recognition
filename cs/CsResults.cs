using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Speech.Recognition;
using System.Collections;

namespace VoiceRecognizer
{
    class CsResult
    {
        public CsResultObject Result = new CsResultObject();

        /**
         * @method  CreateRecognizer
         * 
         * Create a result object from a recognized event
         * 
         * @param   {SpeechRecognizedEventArgs}     Event returned by the recognizer.
         * @returns {void}
         */
        public void CreateRecognizer(SpeechRecognizedEventArgs e, CsGrammars Grammars)
        {
            if( Grammars.Length() > 0 )
            {
                Result.Alternates = e.Result.Alternates;
                Result.Homophones = e.Result.Homophones;
                Result.ReplacementWordUnits = e.Result.ReplacementWordUnits;
                Result.Words = e.Result.Words;
            }

            Result.Audio = e.Result.Audio;
            Result.Confidence = e.Result.Confidence;
            Result.Grammar = e.Result.Grammar;
            Result.HomophoneGroupId = e.Result.HomophoneGroupId;
            Result.Semantics = ConstructSemanticsJSON(e.Result.Semantics);
            Result.Text = e.Result.Text;
        }

        /**
         * @method  CreateHypothesized
         * 
         * Create a result object from a hypothesized event
         * 
         * @param   {SpeechHypothesizedEventArgs}       Event returned by the recognizer.
         * @returns {void}
         */
        public void CreateHypothesized(SpeechHypothesizedEventArgs e, CsGrammars Grammars)
        {
            if (Grammars.Length() > 0)
            {
                Result.Alternates = e.Result.Alternates;
                Result.Homophones = e.Result.Homophones;
                Result.ReplacementWordUnits = e.Result.ReplacementWordUnits;
                Result.Words = e.Result.Words;
            }

            Result.Audio = e.Result.Audio;
            Result.Confidence = e.Result.Confidence;
            Result.Grammar = e.Result.Grammar;
            Result.HomophoneGroupId = e.Result.HomophoneGroupId;
            Result.Semantics = ConstructSemanticsJSON(e.Result.Semantics);
            Result.Text = e.Result.Text;
        }

        /**
         * @method  CreateRejected
         * 
         * Create a result object from a rejected event
         * 
         * @param   {SpeechRecognitionRejectedEventArgs}    Event returned by the recognizer.
         * @returns {void}
         */
        public void CreateRejected(SpeechRecognitionRejectedEventArgs e, CsGrammars Grammars)
        {
            if (Grammars.Length() > 0)
            {
                Result.Alternates = e.Result.Alternates;
                Result.Homophones = e.Result.Homophones;
                Result.ReplacementWordUnits = e.Result.ReplacementWordUnits;
                Result.Words = e.Result.Words;
            }

            Result.Audio = e.Result.Audio;
            Result.Confidence = e.Result.Confidence;
            Result.Grammar = e.Result.Grammar;
            Result.HomophoneGroupId = e.Result.HomophoneGroupId;
            Result.Semantics = ConstructSemanticsJSON(e.Result.Semantics);
            Result.Text = e.Result.Text;
        }

        /**
         * @method  CreateCompleted
         * 
         * Create a result object from a completion event
         * 
         * @param   {RecognizeCompletedEventArgs}       Event returned by the recognizer.
         * @returns {void}
         */
        public void CreateCompleted(RecognizeCompletedEventArgs e, CsGrammars Grammars)
        {
            if (Grammars.Length() > 0)
            {
                Result.Alternates = e.Result.Alternates;
                Result.Homophones = e.Result.Homophones;
                Result.ReplacementWordUnits = e.Result.ReplacementWordUnits;
                Result.Words = e.Result.Words;
            }

            Result.Audio = e.Result.Audio;
            Result.Confidence = e.Result.Confidence;
            Result.Grammar = e.Result.Grammar;
            Result.HomophoneGroupId = e.Result.HomophoneGroupId;
            Result.Semantics = ConstructSemanticsJSON(e.Result.Semantics);
            Result.Text = e.Result.Text;
        }

        /**
         * @method  prConstructSemanticsJSON
         * 
         * Construct a JSON semantics object manually so that it can be read in javascript.
         * 
         * @param   {SemanticValue}         Semantics object returned by the recognition engine.
         * @returns {string}                String in JSON format with serialized semantics object.
         */
        private string ConstructSemanticsJSON(SemanticValue sem)
        {
            string semantics = null;
            foreach (KeyValuePair<String, SemanticValue> child in sem)
            {
                semantics += (semantics == null) ? "{" : ",";
                semantics += "\"" + child.Key + "\":\"" + child.Value.Value + "\"";
            }

            if (semantics != null)
            {
                semantics += "}";
            }

            return semantics;
        }

    }

    class CsResultObject
    {
        public object   Alternates;
        public object   Audio;
        public object   Confidence;
        public object   Grammar;
        public object   Homophones;
        public int      HomophoneGroupId;
        public object   ReplacementWordUnits;
        public string   Semantics;
        public string   Text;
        public object   Words;
    }
}

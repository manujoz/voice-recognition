using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace VoiceRecognizer
{
    public class CsGrammars
    {
        public Func<string, string, string> emitEventToCpp;

        // Array with the grammars that are added to the class.
        public List<Grammar> Items = new List<Grammar>();

        /**
         * @method  AddXML
         * 
         * Add a grammar from an XML file.
         * 
         * @param   {string}    file        Path to the file from which the grammar will be created.
         * @param   {string}    name        Name that will have the grammar.
         * @returns {Grammar}               Grammar object created from the file.
         */
        public Grammar AddXML(string file = null, string name = null)
        {
            foreach (Grammar gr in Items)
            {
                if (Exists(name))
                {
                    return gr;
                }
            }

            Grammar ng = new Grammar(file);
            ng.Name = name;
            Items.Add(ng);
            return ng;
        }

        /**
         * @method  Load
         * 
         * Load a grammar into the recognition engine.
         * 
         * @param   {Grammar}                    gr             Grammar to be loaded.
         * @param   {SpeechRecognitionEngine}    engine         Recognition engine in which the grammar will be loaded.
         * @returns {void}
         */
        public void Load(Grammar gr, SpeechRecognitionEngine engine)
        {
            if (!IsLoaded(gr))
            {
                try
                {
                    engine.LoadGrammar(gr);
                }
                catch (Exception e)
                {
                    emitEventToCpp(e.ToString(), "vcpr:error");
                }
            }
        }

        /**
         * @method  LoadAll
         * 
         * Load all the grammars into the recognition engine.
         * 
         * @param   {SpeechRecognitionEngine}    engine         Recognition engine in which the grammar will be loaded.
         * @returns {void}
         */
        public void LoadAll(SpeechRecognitionEngine engine)
        {
            foreach (Grammar gr in Items)
            {
                Load(gr, engine);
            }
        }

        /**
         * @method  Length
         * 
         * Returns the number of grammars we have stored in the class.
         * 
         * @returns {int}           Amount of grammars loaded.
         */
        public int Length()
        {
            int lengh = 0;
            foreach (Grammar gr in Items)
            {
                ++lengh;
            }

            return lengh;
        }

        /**
         * @method  Exists
         * 
         * Returns whether a grammar already exists in the class.
         * 
         * @returns {bool}           TRUE if the grammar exists, FALSE if it does not exist.
         */
        private bool Exists(string name)
        {
            foreach (Grammar gr in Items)
            {
                if (gr.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * @method  IsLoaded
         * 
         * Returns whether a grammar is loaded into any recognition engine.
         * 
         * @param   {Grammar}        Grammar that we want to check if it is loaded.
         * @returns {bool}           TRUE if the grammar is loaded, FALSE if it is not.
         */
        private bool IsLoaded(Grammar grLoad)
        {
            if (Items != null)
            {
                foreach (Grammar gr in Items)
                {
                    if (grLoad.Name == gr.Name && gr.Loaded)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    /**
     * @object  ClGrammarXMLFileData
     * 
     * Object to be able to deserialize the data sent from javascript when we want to load
     * a grammar file.
     * 
     * Javascript will send an object {"file": "path / to / files.xml", "name": "grammar name"}, if
     * we want to be able to deserialize it in C # this object is necessary.
     */
    class ClGrammarXMLFileData
    {
        public string File;
        public string Name;
    }

}

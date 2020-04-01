using System;
using System.Collections.Generic;
using System.Speech.Recognition;

namespace CsVoiceRecognition
{
    public class CsGrammars
    {
        public Func<string, string, string> emitEventToCpp;

        // Array con las gramáticas que se añaden a la clase.
        public List<Grammar> Items = new List<Grammar>();

        /**
         * @method  AddXML
         * 
         * Añade una gramátcia a partir de un archivo XML.
         * 
         * @param   {string}    file        Ruta al archivo a partir del cual se creará la gramátcia.
         * @param   {string}    name        Nombre que tendrá la gramática.
         * @returns {Grammar}               Objeto de gramática creado a partir del archivo.
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
         * Carga una gramática en el motor de reconocimiento.
         * 
         * @param   {Grammar}                    gr             Gramática que debe ser cargada.
         * @param   {SpeechRecognitionEngine}    engine         Motor de reconociemiento en el que se va a cargar la gramática.
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
         * Carga todas las gramáticas en el motor de reconocimiento.
         * 
         * @param   {SpeechRecognitionEngine}    engine         Motor de reconociemiento en el que se va a cargar la gramática.
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
         * Devuelve la cantidad de gramáticas que tenemos almacenadas en la clase.
         * 
         * @returns {int}           Cantidad de gramáticas cargadas.
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
         * Devuelve si una gramática existe ya en la clase.
         * 
         * @returns {bool}           TRUE si la gramática exise, FALSE si no existe.
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
         * Devuelve si una gramática está cargada en algún motor de reconocimiento.
         * 
         * @param   {Grammar}        Gramática que quermos comprobar si está cargada.
         * @returns {bool}           TRUE si la gramática está cargada, FALSE si no lo está.
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
     * Objeto para poder deserializar los datos enviados desde javascript cuando queremos cargar
     * una archivo de gramática.
     * 
     * Javascript enviará un objeto {"file":"path/to/files.xml","name":"nombre de la gramatica"}, si
     * queremos poder deserializarlo en C# es necesario este objeto.
     */
    class ClGrammarXMLFileData
    {
        public string File;
        public string Name;
    }

}

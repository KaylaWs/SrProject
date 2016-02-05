using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// This is for testing openNPL
/// </summary>
namespace NaturalLanguageProcessingCSharp
{
    public class EntityExtractor
    {

        private string sentenceModelPath = HttpContext.Current.Server.MapPath("~/Bin/da-sent.bin");
        private string nameFinderModelPath;
        private string tokenModelPath = HttpContext.Current.Server.MapPath("~/Bin/da-token.bin");
        public enum EntityType
        {
            Date = 0,
            Location,
            Money,
            Organization,
            Person,
            Time
        }
        public List<string> ExtractEntities(string inputData, EntityType targetType)
        {
            /*required steps to detect names are:
             * downloaded sentence, token, and name models from http://opennlp.sourceforge.net/models-1.5/
             * 1. Parse the input into sentences
             * 2. Parse the sentences into tokens
             * 3. Find the entity in the tokens
 
            */

            //------------------Preparation -- Set Name Finder model path based upon entity type-----------------
            switch (targetType)
            {
                case EntityType.Date:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-date.bin");
                    break;
                case EntityType.Location:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-location.bin");
                    break;
                case EntityType.Money:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-money.bin");
                    break;
                case EntityType.Organization:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-organization.bin");
                    break;
                case EntityType.Person:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-person.bin");
                    break;
                case EntityType.Time:
                    nameFinderModelPath = HttpContext.Current.Server.MapPath("~/Bin/en-ner-time.bin");
                    break;
                default:
                    break;
            }

            //----------------- Preparation -- load models into objects-----------------
            //initialize the sentence detector
            opennlp.tools.sentdetect.SentenceDetectorME sentenceParser = prepareSentenceDetector();

            //initialize person names model
            opennlp.tools.namefind.NameFinderME nameFinder = prepareNameFinder();

            //initialize the tokenizer--used to break our sentences into words (tokens)
            opennlp.tools.tokenize.TokenizerME tokenizer = prepareTokenizer();

            //------------------  Make sentences, then tokens, then get names--------------------------------

            String[] sentences = sentenceParser.sentDetect(inputData); //detect the sentences and load into sentence array of strings
            List<string> results = new List<string>();

            foreach (string sentence in sentences)
            {
                //now tokenize the input.
                //"Don Krapohl enjoys warm sunny weather" would tokenize as
                //"Don", "Krapohl", "enjoys", "warm", "sunny", "weather"
                string[] tokens = tokenizer.tokenize(sentence);

                //do the find
                opennlp.tools.util.Span[] foundNames = nameFinder.find(tokens);

                //important:  clear adaptive data in the feature generators or the detection rate will decrease over time.
                nameFinder.clearAdaptiveData();

                results.AddRange(opennlp.tools.util.Span.spansToStrings(foundNames, tokens).AsEnumerable());
            }

            return results;
        }

        #region private methods
        private opennlp.tools.tokenize.TokenizerME prepareTokenizer()
        {
            java.io.FileInputStream tokenInputStream = new java.io.FileInputStream(tokenModelPath);     //load the token model into a stream
            opennlp.tools.tokenize.TokenizerModel tokenModel = new opennlp.tools.tokenize.TokenizerModel(tokenInputStream); //load the token model
            return new opennlp.tools.tokenize.TokenizerME(tokenModel);  //create the tokenizer
        }
        private opennlp.tools.sentdetect.SentenceDetectorME prepareSentenceDetector()
        {
            java.io.FileInputStream sentModelStream = new java.io.FileInputStream(sentenceModelPath);       //load the sentence model into a stream
            opennlp.tools.sentdetect.SentenceModel sentModel = new opennlp.tools.sentdetect.SentenceModel(sentModelStream);// load the model
            return new opennlp.tools.sentdetect.SentenceDetectorME(sentModel); //create sentence detector
        }
        private opennlp.tools.namefind.NameFinderME prepareNameFinder()
        {
            java.io.FileInputStream modelInputStream = new java.io.FileInputStream(nameFinderModelPath); //load the name model into a stream
            opennlp.tools.namefind.TokenNameFinderModel model = new opennlp.tools.namefind.TokenNameFinderModel(modelInputStream); //load the model
            return new opennlp.tools.namefind.NameFinderME(model);                   //create the namefinder
        }
        #endregion


    }
}
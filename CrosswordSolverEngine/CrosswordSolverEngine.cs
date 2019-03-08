using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CrosswordSolverEngine
{
    public class CrosswordSolverEngine
    {
        // I think it will be faster in the reading to determine whether numbers are identical.
        Dictionary<int,string> _words = new Dictionary<int,string>();
        Dictionary<char,Dictionary<int,List<int>>> _indexes = new Dictionary<char, Dictionary<int, List<int>>>();
        Dictionary<int, int> _wordLengths = new Dictionary<int, int>();
        Dictionary<int, List<int>> _wordsByLength = new Dictionary<int, List<int>>();
        const string _ALL_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        int _maxLength = 0;
        public CrosswordSolverEngine(string sourceFilePath)
        {
            // read the csv.
            // add items to list
            // index
            var sourceFile = new System.IO.StreamReader(sourceFilePath);

            int indexer = 0;
            while(!sourceFile.EndOfStream)
            {
                var lineItem = sourceFile.ReadLine();
                _words.Add(indexer,lineItem);
                _wordLengths.Add(indexer, lineItem.Length);
                indexer++;
            }

            sourceFile.Close();

            _maxLength = _wordLengths.Values.Max(length => length);

            // set up our indexing for word length
            // there will be no zero length words
            for(int i=1; i< _maxLength+1; i++)
            {
                _wordsByLength.Add(i, new List<int>());
            }
            
            // set up our indexing crossreference for each letter.
            foreach(var character in _ALL_LETTERS)
            {
                var perCharacterIndex = new Dictionary<int, List<int>>();
                for (int i =0; i<_maxLength; i++)
                {
                    perCharacterIndex.Add(i, new List<int>());
                }
                _indexes.Add(character, perCharacterIndex);
            }

            // ok now index up all those words.
            // we can speed this up a bit.. because our indexes shouldn't conflict.
            foreach(var word in _words)
            {
                for(int i =0; i<word.Value.Length; i++)
                {
                    var internalDictionary = _indexes[word.Value[i]];
                    var indexedList = internalDictionary[i];
                    indexedList.Add(word.Key);
                }

                // and index its length.
                _wordsByLength[word.Value.Length].Add(word.Key);
            }           


        }

        public List<string> WordSeek(string wordSearchParam)
        {
            //foreach character, get the list of ids that match
            //word must exist in each set.
            List<int> matches = _wordsByLength[wordSearchParam.Length];
            List<int> comparisonMatches;
            for(int i=0;i<wordSearchParam.Length;i++)
            {
                if (_ALL_LETTERS.Contains(wordSearchParam[i]))
                {
                    comparisonMatches = matches;
                    matches = _indexes[wordSearchParam[i]][i].Where(index => comparisonMatches.Contains(index)).ToList();
                }
            }

            List<string> matchedWords = _words.Where(a => matches.Contains(a.Key)).Select(a => a.Value).ToList();
            return matchedWords;
        }
    }
}

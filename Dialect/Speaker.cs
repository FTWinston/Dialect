using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public class Speaker
    {
        public Dialect Dialect { get; set; }
        public PronounciationManager Pronounciation { get; private set; }

        public Speaker()
        {
            Dialect = new Dialect();
            Pronounciation = PronounciationManager.CreateDefault();
            Pronounciation.LookupComplete += WordLookupComplete;
        }

        private Queue<string> GenerationQueue = new Queue<string>();

        public void GenerateIPA(string text)
        {
            GenerationQueue.Enqueue(text);

            if (GenerationQueue.Count == 1)
                StartGeneration();
        }

        public event EventHandler<string> GenerationComplete;

        private void StartGeneration()
        {
            var text = GenerationQueue.Peek();
            var words = SeparateWords(text);
            currentJobWordOrder.Clear();
            currentJobWordsAwaitingPronounciation.Clear();
            currentJobPronounciations.Clear();

            foreach (var word in words)
                if (word != null)
                {
                    currentJobWordOrder.Add(word);

                    if (!currentJobWordsAwaitingPronounciation.Contains(word))
                        currentJobWordsAwaitingPronounciation.Add(word);
                }

            if (currentJobWordOrder.Count == 0)
            {
                if (GenerationComplete != null)
                    GenerationComplete(this, string.Empty);
                return;
            }

            foreach (var word in currentJobWordsAwaitingPronounciation)
                Pronounciation.Lookup(word);
        }

        List<string> currentJobWordOrder = new List<string>();
        SortedSet<string> currentJobWordsAwaitingPronounciation = new SortedSet<string>();
        SortedList<string, string> currentJobPronounciations = new SortedList<string, string>();

        static readonly char[] whitespace = { ' ' };
        protected string[] SeparateWords(string text)
        {
            var words = text.Split(whitespace, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];

                int startPos; // skip non-alphanumeric characters at start and end
                for (startPos = 0; startPos < word.Length; startPos++)
                    if (char.IsLetterOrDigit(word[startPos]))
                        break;

                if (startPos == word.Length)
                {// no alphanumeric characters in this "word" ... its not a word
                    words[i] = null;
                    continue;
                }

                int endPos;
                for (endPos = word.Length - 1; endPos >= startPos; endPos--)
                    if (char.IsLetterOrDigit(word[endPos]))
                        break;

                words[i] = word.Substring(startPos, endPos - startPos + 1).ToLower();
            }

            return words;
        }

        private void WordLookupComplete(object sender, PronounciationSource.LookupEventArgs e)
        {
            currentJobPronounciations[e.Spelling] = e.Pronounciation;
            currentJobWordsAwaitingPronounciation.Remove(e.Spelling);

            if (currentJobWordsAwaitingPronounciation.Count == 0)
                ContinueGeneration();
        }

        private void ContinueGeneration()
        {
            string text = CombineWordPronounciations();

            text = Dialect.PerformSubstitution(text);

            if (GenerationComplete != null)
                GenerationComplete(this, text);

            GenerationQueue.Dequeue();
            if (GenerationQueue.Count > 0)
                StartGeneration();
        }

        private string CombineWordPronounciations()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < currentJobWordOrder.Count; i++)
            {
                sb.Append(" ");
                sb.Append(currentJobPronounciations[currentJobWordOrder[i]]);
            }
            return sb.Remove(0, 1).ToString();
        }
    }
}

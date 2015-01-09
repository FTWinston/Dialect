using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public class Speaker
    {
        public Dialect Dialect { get; set; }
        public PronunciationManager Pronunciation { get; private set; }
        SpeechSynthesizer Voice { get; set; }

        public Speaker()
        {
            Dialect = new Dialect();
            Pronunciation = PronunciationManager.CreateDefault();
            Pronunciation.LookupComplete += WordLookupComplete;
            
            Voice = new SpeechSynthesizer();
            Voice.SetOutputToDefaultAudioDevice();
        }

        public void SayAutomatically()
        {
            if (GenerationComplete != null)
                return;

            GenerationComplete += (o, e) =>
            {
                PromptBuilder pb = new PromptBuilder();
                pb.StartParagraph();
                pb.StartSentence();
                pb.AppendTextWithPronunciation(e.Spelling, e.Pronunciation); // this doesn't like spaces. Looks like this needs called word-for-word?
                pb.EndSentence();
                pb.EndParagraph();
                Voice.SpeakAsync(pb);
            };
        }

        private Queue<string> GenerationQueue = new Queue<string>();

        public void GenerateIPA(string text)
        {
            GenerationQueue.Enqueue(text);

            if (GenerationQueue.Count == 1)
                StartGeneration();
        }

        public event EventHandler<PronunciationEventArgs> GenerationComplete;

        private void StartGeneration()
        {
            currentJobText = GenerationQueue.Peek();
            var words = SeparateWords(currentJobText);
            currentJobWordOrder.Clear();
            currentJobWordsAwaitingPronunciation.Clear();
            currentJobPronunciations.Clear();

            var wordsToLookup = new SortedSet<string>();

            foreach (var word in words)
                if (word != null)
                {
                    currentJobWordOrder.Add(word);

                    if (!currentJobWordsAwaitingPronunciation.Contains(word))
                    {
                        currentJobWordsAwaitingPronunciation.Add(word);
                        wordsToLookup.Add(word);
                    }
                }

            if (currentJobWordOrder.Count == 0)
            {
                if (GenerationComplete != null)
                    GenerationComplete(this, new PronunciationEventArgs(currentJobText, string.Empty));
                return;
            }

            foreach (var word in wordsToLookup) // use a separate set here to avoid iterating currentJobWordsAwaitingPronunciation while it (might) be modifed by a synchronous Lookup
                Pronunciation.Lookup(word);
        }

        string currentJobText;
        List<string> currentJobWordOrder = new List<string>();
        SortedSet<string> currentJobWordsAwaitingPronunciation = new SortedSet<string>();
        SortedList<string, string> currentJobPronunciations = new SortedList<string, string>();

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

        private void WordLookupComplete(object sender, PronunciationEventArgs e)
        {
            currentJobPronunciations[e.Spelling] = e.Pronunciation;
            currentJobWordsAwaitingPronunciation.Remove(e.Spelling);

            if (currentJobWordsAwaitingPronunciation.Count == 0)
                ContinueGeneration();
        }

        private void ContinueGeneration()
        {
            string pronunciation = CombineWordPronunciations();

            pronunciation = Dialect.PerformSubstitution(pronunciation);

            if (GenerationComplete != null)
                GenerationComplete(this, new PronunciationEventArgs(currentJobText, pronunciation));

            GenerationQueue.Dequeue();
            if (GenerationQueue.Count > 0)
                StartGeneration();
            else
            {
                currentJobText = null;
                currentJobWordOrder.Clear();
                currentJobPronunciations.Clear();
            }
        }

        private string CombineWordPronunciations()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < currentJobWordOrder.Count; i++)
            {
                sb.Append(" ");
                sb.Append(currentJobPronunciations[currentJobWordOrder[i]]);
            }
            return sb.Remove(0, 1).ToString();
        }
    }
}

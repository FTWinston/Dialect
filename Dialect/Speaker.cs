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
                pb.AppendTextWithPronunciation(e.Spelling, SubstituteCharacters(e.Pronunciation)); // this doesn't like spaces. Looks like this needs called word-for-word?
                pb.EndSentence();
                pb.EndParagraph();
                Voice.SpeakAsync(pb);
            };
        }

        private string SubstituteCharacters(string ipa)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in ipa)
                switch (c)
                {
                    case 'ɑ': sb.Append("&#x0251;"); break;
                    case 'ɐ': sb.Append("&#x0250;"); break;
                    case 'ɒ': sb.Append("&#x0252;"); break;
                    case 'æ': sb.Append("&#x00E6;"); break;
                    case 'ɓ': sb.Append("&#x0253;"); break;
                    case 'ʙ': sb.Append("&#x0299;"); break;
                    case 'β': sb.Append("&#x03B2;"); break;
                    case 'ɔ': sb.Append("&#x0254;"); break;
                    case 'ɕ': sb.Append("&#x0255;"); break;
                    case 'ç': sb.Append("&#x00E7;"); break;
                    case 'ɗ': sb.Append("&#x0257;"); break;
                    case 'ɖ': sb.Append("&#x0256;"); break;
                    case 'ð': sb.Append("&#x00F0;"); break;
                    case 'ʤ': sb.Append("&#x02A4;"); break;
                    case 'ə': sb.Append("&#x0259;"); break;
                    case 'ɘ': sb.Append("&#x0258;"); break;
                    case 'ɚ': sb.Append("&#x025A;"); break;
                    case 'ɛ': sb.Append("&#x025B;"); break;
                    case 'ɜ': sb.Append("&#x025C;"); break;
                    case 'ɝ': sb.Append("&#x025D;"); break;
                    case 'ɞ': sb.Append("&#x025E;"); break;
                    case 'ɟ': sb.Append("&#x025F;"); break;
                    case 'ʄ': sb.Append("&#x0284;"); break;
                    case 'g': // letter g
                    case 'ɡ': sb.Append("&#x0261;"); break;
                    case 'ɠ': sb.Append("&#x0260;"); break;
                    case 'ɢ': sb.Append("&#x0262;"); break;
                    case 'ʛ': sb.Append("&#x029B;"); break;
                    case 'ɦ': sb.Append("&#x0266;"); break;
                    case 'ɧ': sb.Append("&#x0267;"); break;
                    case 'ħ': sb.Append("&#x0127;"); break;
                    case 'ɥ': sb.Append("&#x0265;"); break;
                    case 'ʜ': sb.Append("&#x029C;"); break;
                    case 'ɨ': sb.Append("&#x0268;"); break;
                    case 'ɪ': sb.Append("&#x026A;"); break;
                    case 'ʝ': sb.Append("&#x029D;"); break;
                    case 'ɭ': sb.Append("&#x026D;"); break;
                    case 'ɬ': sb.Append("&#x026C;"); break;
                    case 'ɫ': sb.Append("&#x026B;"); break;
                    case 'ɮ': sb.Append("&#x026E;"); break;
                    case 'ʟ': sb.Append("&#x029F;"); break;
                    case 'ɱ': sb.Append("&#x0271;"); break;
                    case 'ɯ': sb.Append("&#x026F;"); break;
                    case 'ɰ': sb.Append("&#x0270;"); break;
                    case 'ŋ': sb.Append("&#x014B;"); break;
                    case 'ɳ': sb.Append("&#x0273;"); break;
                    case 'ɲ': sb.Append("&#x0272;"); break;
                    case 'ɴ': sb.Append("&#x0274;"); break;
                    case 'ø': sb.Append("&#x00F8;"); break;
                    case 'ɵ': sb.Append("&#x0275;"); break;
                    case 'ɸ': sb.Append("&#x0278;"); break;
                    case 'θ': sb.Append("&#x03B8;"); break;
                    case 'œ': sb.Append("&#x0153;"); break;
                    case 'ɶ': sb.Append("&#x0276;"); break;
                    case 'ʘ': sb.Append("&#x0298;"); break;
                    case 'ɹ': sb.Append("&#x0279;"); break;
                    case 'ɺ': sb.Append("&#x027A;"); break;
                    case 'ɾ': sb.Append("&#x027E;"); break;
                    case 'ɻ': sb.Append("&#x027B;"); break;
                    case 'ʀ': sb.Append("&#x0280;"); break;
                    case 'ʁ': sb.Append("&#x0281;"); break;
                    case 'ɽ': sb.Append("&#x027D;"); break;
                    case 'ʂ': sb.Append("&#x0282;"); break;
                    case 'ʃ': sb.Append("&#x0283;"); break;
                    case 'ʈ': sb.Append("&#x0288;"); break;
                    case 'ʧ': sb.Append("&#x02A7;"); break;
                    case 'ʉ': sb.Append("&#x0289;"); break;
                    case 'ʊ': sb.Append("&#x028A;"); break;
                    case 'ʋ': sb.Append("&#x028B;"); break;
                    case 'ⱱ': sb.Append("&#x2C71;"); break;
                    case 'ʌ': sb.Append("&#x028C;"); break;
                    case 'ɣ': sb.Append("&#x0263;"); break;
                    case 'ɤ': sb.Append("&#x0264;"); break;
                    case 'ʍ': sb.Append("&#x028D;"); break;
                    case 'χ': sb.Append("&#x03C7;"); break;
                    case 'ʎ': sb.Append("&#x028E;"); break;
                    case 'ʏ': sb.Append("&#x028F;"); break;
                    case 'ʑ': sb.Append("&#x0291;"); break;
                    case 'ʐ': sb.Append("&#x0290;"); break;
                    case 'ʒ': sb.Append("&#x0292;"); break;
                    case 'ʔ': sb.Append("&#x0294;"); break;
                    case 'ʡ': sb.Append("&#x02A1;"); break;
                    case 'ʕ': sb.Append("&#x0295;"); break;
                    case 'ʢ': sb.Append("&#x02A2;"); break;
                    case 'ǀ': sb.Append("&#x01C0;"); break;
                    case 'ǁ': sb.Append("&#x01C1;"); break;
                    case 'ǂ': sb.Append("&#x01C2;"); break;
                    case 'ǃ': sb.Append("&#x01C3;"); break;
                    case 'ˈ': sb.Append("&#x02C8;"); break;
                    case 'ˌ': sb.Append("&#x02CC;"); break;
                    case ':': // colon
                    case 'ː': sb.Append("&#x02D0;"); break;
                    case 'ˑ': sb.Append("&#x02D1;"); break;
                    case 'ʼ': sb.Append("&#x02BC;"); break;
                    case 'ʴ': sb.Append("&#x02B4;"); break;
                    case 'ʰ': sb.Append("&#x02B0;"); break;
                    case 'ʱ': sb.Append("&#x02B1;"); break;
                    case 'ʲ': sb.Append("&#x02B2;"); break;
                    case 'ʷ': sb.Append("&#x02B7;"); break;
                    case 'ˠ': sb.Append("&#x02E0;"); break;
                    case 'ˤ': sb.Append("&#x02E4;"); break;
                    case '˞': sb.Append("&#x02DE;"); break;
                    default: sb.Append(c); break;
                }
            return sb.ToString();
        }

        private Queue<string> GenerationQueue = new Queue<string>();

        public void GenerateIPA(string text)
        {
            GenerationQueue.Enqueue(text);

            if (GenerationQueue.Count == 1)
                StartGeneration();
        }

        public event EventHandler<Word> GenerationComplete;

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
                    GenerationComplete(this, new Word(currentJobText, string.Empty));
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

        private void WordLookupComplete(object sender, Word e)
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
                GenerationComplete(this, new Word(currentJobText, pronunciation));

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

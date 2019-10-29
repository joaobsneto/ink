using Ink.Parsed;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ink {
    internal partial class InkParser {
        private const int maxCharCount = 100;
        protected List<Parsed.Object> Speech() {
            Whitespace();
            string author = "";
            if (string.IsNullOrEmpty(ParseString("\""))) {
                author = Parse(UpperCaseIdentifier);
                if (author == null) return null;
                Whitespace();
                if (ParseString(":") == null) return null;
                Whitespace();
            }
            if (string.IsNullOrEmpty(ParseString("\""))) {
                return null;
            }
           // string content = Parse(ContentTextNoEscape);
            var parsedLine = Parse(MixedTextAndLogic);
            if (parsedLine == null) {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            Parsed.Object parsedLineElement;
            Text parsedLineText;
            int lastQuote;
            bool hasFoundQuote = false;
            int charCount = 0;
            for (int i = parsedLine.Count-1; i >= 0; i--) {
                parsedLineElement = parsedLine[i];
                if (!(parsedLineElement is Text))
                    continue;
                parsedLineText = (Text)parsedLineElement;
                charCount += parsedLineText.text.Length;
                //search last quote
                if (hasFoundQuote) continue;
                lastQuote = parsedLineText.text.LastIndexOf("\"");
                if (lastQuote < 0) {
                    sb.Insert(0, parsedLineText.text.Trim());
                } else {
                    lastQuote++;
                    if (lastQuote < parsedLineText.text.Length -1) {
                        sb.Insert(0, parsedLineText.text.Substring(lastQuote, parsedLineText.text.Length - lastQuote).Trim());

                    }
                    hasFoundQuote = true;
                }
            }
            if (!hasFoundQuote) {
                Warning("Is it a speech? Last quote was not found");
                return null;
            }
            if (sb.Length > 0) {
                Warning("Is it a speech? There is content after last quote");
                return null;
            }
            if (charCount > maxCharCount) {
                Warning($"Speech exceeds character limits of {maxCharCount} characters");
            }
            return null;
        }
        private CharacterSet _upperCaseIdentifierCharSet;
        protected string UpperCaseIdentifier() {
            if (_upperCaseIdentifierCharSet == null) {
                _upperCaseIdentifierCharSet = new CharacterSet();
                _upperCaseIdentifierCharSet.AddRange('A', 'Z');
                _upperCaseIdentifierCharSet.AddRange('0', '9');
                _upperCaseIdentifierCharSet.Add('_');
            }

            // Parse remaining characters (if any)
            var name = ParseCharactersFromCharSet(_upperCaseIdentifierCharSet);
            if (name == null)
                return null;

            // Reject if it's just a number
            bool isNumberCharsOnly = true;
            foreach (var c in name) {
                if (!(c >= '0' && c <= '9')) {
                    isNumberCharsOnly = false;
                    break;
                }
            }
            if (isNumberCharsOnly) {
                return null;
            }

            return name;
        }

    }
}

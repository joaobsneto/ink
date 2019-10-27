using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ink {
    internal partial class InkParser {

        protected List<Parsed.Object> Speech() {
            Whitespace();
            string author = "";
            if (string.IsNullOrEmpty(ParseString("\""))) {
                author = Parse(UpperCaseIdentifier);
                Whitespace();
                ParseString(":");
                Whitespace();
            }
            if (string.IsNullOrEmpty(ParseString("\""))) {
                return null;
            }
            string content = Parse(ContentTextNoEscape);
            if (content == null)
                return null;
            int lastQuote = content.LastIndexOf('"');
            if (lastQuote < 0) {
                Warning("Is it a speech? Quote is missing.");
                return null;
            }
            content = content.Substring(0, lastQuote);
            return new List<Parsed.Object>() { new Speech(author, content), new Text("\n") };
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

using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ink {
    internal partial class InkParser {

        protected Speech  Speech() {
            Whitespace();
            string author = "";
            if (string.IsNullOrEmpty(ParseString("\""))) {
                if (string.IsNullOrEmpty(ParseString("("))) {
                    return null;
                }
                author = Parse(Identifier);
                if (string.IsNullOrEmpty(ParseString(")"))) {
                    Warning("Was this supposed to be a speech? Missing ')' after who is the speecher");
                    return null;
                }
                Whitespace();
                ParseString(":");
                Whitespace();
                if (string.IsNullOrEmpty(ParseString("\""))) {
                    return null;
                }
            }
            string content = ParseUntilCharactersFromString("\n\r");
            if (content == null)
                return null;
            int lastQuote = content.LastIndexOf('"');
            if (lastQuote < 0) {
                Warning("Was this supposed to be a speech? Last quote is missing.");
                return null;
            }
            content = content.Substring(0, lastQuote);
            return new Speech(author, content);
        }

    }
}

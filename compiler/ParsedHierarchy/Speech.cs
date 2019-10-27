namespace Ink.Parsed {
    internal class Speech : Parsed.Object {
        public string author;
        public string speechContent;

        public const int maxCharCount = 100;

        public Speech(string author, string content) {
            this.author = author;
            speechContent = content;
        }

        public override Runtime.Object GenerateRuntimeObject() {
            if (speechContent.Length > maxCharCount) {
                Warning($"Speech content exceeds {maxCharCount} char count.");
            }
            return new Runtime.StringValue((string.IsNullOrEmpty(author)) ? $"\"{speechContent}\"" : 
                $"{author}:\"{speechContent}\"");
        }
    }
}

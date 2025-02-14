using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class Line
    {

        public enum EType
        {
            Conversation,
            Anonymous,

        }

        public static string AnonymousLine
        {
            get => anonymousLine;
            set => anonymousLine = value;
        }

        private static string anonymousLine = string.Empty;


        public EType Type
        {
            get => type;
            set => type = value;
        }

        public string Text
        {
            get => text;
            set => text = value;
        }

        public string Speaker
        {
            get => speaker;
            set => speaker = value;
        }

        [SerializeField] EType type;
        [SerializeField] [TextArea(5, 5)] string text;
        [SerializeField] public string speaker;

        public void SetStyle(EType style) => Type = style;
        public void SetLine(string line) => Text = line;
        public void SetSpeaker(string speaker) => Speaker = speaker;
        public void MakeAnonymous() => Speaker = AnonymousLine;

        public Line()
        {
            Type = EType.Anonymous;
            Text = String.Empty;
            Speaker = String.Empty;
        }

        public Line(EType eType, string text, string speaker)
        {
            Type = eType;
            Text = text;
            Speaker = speaker;
        }

        public static Line Create(EType style, string line, string speaker)
        {
            return new Line() { Type = style, Text = line, Speaker = speaker };
        }

        public static Line CreateEmpty()
        {
            return Create(EType.Anonymous, string.Empty, string.Empty);
        }

    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class Dialogue
    {
        public int LinesIndex { get ; set ;}
        [field: SerializeField] public List<Line> Lines { get; set; }

        public void AddLine(Line line) => Lines.Add(line);
        public void RemoveLine(Line line) => Lines.Remove(line);
        public void ResetIndex() => LinesIndex = 0;
        private void IncrementIndex() => LinesIndex++;
        public Line GetNextLine()
        {
            if (LinesIndex >= Lines.Count)
                ResetIndex();

            var nextLine = Lines[LinesIndex];

            IncrementIndex();

            return nextLine;
        }

        public Dialogue() => Lines = new List<Line>();
        public Dialogue(List<Line> lines, int linesIndex)
        {
            Lines = lines;
            LinesIndex = linesIndex;
        }

        public static Dialogue Create(List<Line> lines, int linesIndex) =>
            new Dialogue() { Lines = lines, LinesIndex = linesIndex };

        public static Dialogue CreateEmpty() => Create(new List<Line>(), 0);
        public static Dialogue CreateWithEmptyLines(int countOfEmptyLines)
        {
            List<Line> emptyLines = new List<Line>();

            while (countOfEmptyLines > 0)
            {
                emptyLines.Add(Line.CreateEmpty());
                countOfEmptyLines--;
            }

            return Create(emptyLines, 0);
        }

    }
}
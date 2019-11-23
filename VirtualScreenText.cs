using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VirtualScreenText
{
    private readonly Queue<string> lines = new Queue<string>();

    public string Text
    {
        get
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string line in lines)
                stringBuilder.Append(line).Append("\n");
            return stringBuilder.ToString();
        }
        set
        {
            lines.Clear();
            WriteLine(value);
        }
    }

    private Godot.Label label;
    public Godot.Label Label
    {
        get { return label; }
        set
        {
            label = value;
            if (label != null)
                label.Text = Text;
        }
    }

    public uint Height { get; set; } = 25;
    public uint Width { get; set; } = 80;

    public override string ToString()
    {
        return Text;
    }

    public VirtualScreenText CLS()
    {
        Text = string.Empty;
        return this;
    }

    public VirtualScreenText WriteLine(string value)
    {
        foreach (string line in Wrap(value).Split('\n'))
            lines.Enqueue(line);
        if (Height > 0)
            while (lines.Count() > Height)
                lines.Dequeue();
        if (Label != null)
            Label.Text = Text;
        return this;
    }

    public string Wrap(string value)
    {
        return Wrap(value, Width);
    }

    public static string Wrap(string value, uint width)
    {
        if (width <= 0)
            return value;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string a in value.Split('\n'))
            foreach (string b in ChunksUpto(a, width))
                stringBuilder.Append(b).Append("\n");
        return TrimLastCharacter(stringBuilder.ToString());
    }

    public static IEnumerable<string> ChunksUpto(string str, uint maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += (int)maxChunkSize)
            yield return str.Substring(i, Math.Min((int)maxChunkSize, str.Length - i));
    }

    public static string TrimLastCharacter(string str)
    {
        return string.IsNullOrEmpty(str) ? str : str.Substring(0, (str.Length - 1));
    }
}

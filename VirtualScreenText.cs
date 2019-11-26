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

    private Godot.ColorRect cursor;
    public Godot.ColorRect Cursor
    {
        get { return cursor; }
        set
        {
            cursor = value;
            if (cursor != null)
                SetCursor();
        }
    }

    public VirtualScreenText SetCursor()
    {
        uint x = (uint)(lines.Count == 0 ? 0 : lines.Last().Length > 79 ? 0 : lines.Last().Length);
        return SetCursor(x, (uint)(lines.Count - (lines.Count > 0 ? 1 : 0)));
    }

    public VirtualScreenText SetCursor(uint x, uint y)
    {
        if (Cursor != null)
            Cursor.RectGlobalPosition = new Godot.Vector2(x * 9, y * 16 + 14);
        return this;
    }

    public uint Height { get; set; } = 25;
    public uint Width { get; set; } = 80;

    public TimeSpan BlinkRate { get; set; } = TimeSpan.FromSeconds(0.25d);
    private DateTime LastBlink = DateTime.Now;

    public bool ShowCursor
    {
        get
        {
            return Cursor == null ? false : Cursor.Visible;
        }
        set
        {
            if (Cursor != null)
                Cursor.Visible = value;
        }
    }

    public VirtualScreenText UpdateCursor()
    {
        DateTime now = DateTime.Now;
        if (Cursor != null && now - LastBlink >= BlinkRate)
        {
            LastBlink = now;
            ShowCursor = !ShowCursor;
        }
        return this;
    }

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
        SetCursor();
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

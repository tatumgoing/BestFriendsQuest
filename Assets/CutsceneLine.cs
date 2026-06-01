public class CutsceneLine
{
    public CutsceneSpeaker Speaker;
    public string Line;

    public CutsceneLine(CutsceneSpeaker speaker, string line)
    {
        Speaker = speaker;
        Line = line;
    }
}

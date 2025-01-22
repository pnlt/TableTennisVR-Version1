using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    private static List<CoverLine> _coverLines =  new List<CoverLine>();
    public static List<CoverLine> CoverLines => _coverLines;

    public static void AddElement(CoverLine coverLine)
    {
        _coverLines.Add(coverLine);
    }
    
}

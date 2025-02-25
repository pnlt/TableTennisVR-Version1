using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    public Checkpoints checkpoints;
    private static List<CoverLine> _coverLines = new List<CoverLine>();
    public static List<CoverLine> CoverLines => _coverLines;

    public static void AddElement(CoverLine coverLine)
    {
        _coverLines.Add(coverLine);
    }

    public static void RemoveAllElements()
    {
        _coverLines.Clear();
    }

    private void Update()
    {
        //if (checkpoints.ListCheckpoints[0].IsInTurn)
        //{
            //Invoke(nameof(ResetCoverLines), 1);
        //}
    }

    private async void ResetCoverLines()
    {
        try
        {
            if (_coverLines.Count > 0)
            {
                for (int i = 0;
                     i < _coverLines.Count;
                     i++)
                {
                    _coverLines[i].EnablingObject(flag: true);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
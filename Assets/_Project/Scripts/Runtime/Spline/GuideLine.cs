using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    public Checkpoints checkpoints;
    private static List<CoverLine> _coverLines = new List<CoverLine>();
    public static List<CoverLine> CoverLines => _coverLines;
    
    private static bool _needsReset = false;
    private float _resetTimer = 0f;
    private float _resetDelay = 1.5f; // Wait time before resetting lines

    public static void AddElement(CoverLine coverLine)
    {
        if (!_coverLines.Contains(coverLine))
        {
            _coverLines.Add(coverLine);
        }
    }
    public static void NotifyLineHit(CoverLine line)
    {
        _needsReset = true;
    }

    public static void RemoveAllElements()
    {
        _coverLines.Clear();
        _needsReset = false;
    }

    private void Update()
    {
        // Check if we need to reset lines
        if (_needsReset)
        {
            _resetTimer += Time.deltaTime;
            
            if (_resetTimer >= _resetDelay)
            {
                _resetTimer = 0f;
                _needsReset = false;
                ResetCoverLines();
            }
        }
    }

    private async void ResetCoverLines()
    {
        try
        {
            Debug.Log($"Resetting {_coverLines.Count} cover lines");
            
            if (_coverLines.Count > 0)
            {
                // Make a copy of the list to avoid modification issues during iteration
                var linesToReset = new List<CoverLine>(_coverLines);
                
                foreach (var line in linesToReset)
                {
                    if (line != null)
                    {
                        line.EnablingObject(true);
                        await UniTask.Delay(TimeSpan.FromSeconds(0.1f)); // Shorter delay for smoother appearance
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
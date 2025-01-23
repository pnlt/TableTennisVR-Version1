using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GameManager : Singleton<GameManager>
{
    private bool _isLineCompletion;
    public bool IsLineCompletion { get { return _isLineCompletion; } set { _isLineCompletion = value; } }
}

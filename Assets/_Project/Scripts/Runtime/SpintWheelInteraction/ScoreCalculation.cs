using UnityEngine;

public abstract class ScoreCalculation : MonoBehaviour
{
    [SerializeField] protected float gainedScore;
    protected float multipliedComboNum;      // Multiplication of score is gained when make combo with spin wheel
    
    public abstract void CalculateScore();   
}

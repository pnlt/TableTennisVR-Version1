using UnityEngine;

public class TestCheckpoint : MonoBehaviour
{
    public GameObject imagePrefab;
    public int checkpoints;
    public static TestCheckUI[] listCheckUI;

    private void Start()
    {
        listCheckUI = new TestCheckUI[checkpoints];
        SpawnCheck();
    }

    private void SpawnCheck()
    {
        for (int i = 0; i < checkpoints; i++)
        {
            var check = Instantiate(imagePrefab);
            check.transform.SetParent(transform, false);
            listCheckUI[i] = check.GetComponent<TestCheckUI>();
        }
    }
}

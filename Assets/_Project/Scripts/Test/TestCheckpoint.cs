using UnityEngine;

public class TestCheckpoint : MonoBehaviour
{
    public GameObject imagePrefab;
    public Checkpoints checkpoints;
    public static TestCheckUI[] listCheckUI;

    private void Start()
    {
        listCheckUI = new TestCheckUI[checkpoints.ListCheckpoints.Count];
        SpawnCheck();
    }

    private void SpawnCheck()
    {
        for (int i = 0; i < checkpoints.ListCheckpoints.Count; i++)
        {
            var check = Instantiate(imagePrefab);
            check.transform.SetParent(transform, false);
            listCheckUI[i] = check.GetComponent<TestCheckUI>();
        }
    }
}

using UnityEngine;

public class BoltManager : MonoBehaviour
{

    public GameObject[] bolts;
    private int counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveBolt(int index)
    {
        if (bolts[index].activeSelf)
        {
            counter++;
            bolts[index].SetActive(false);

            if(counter >= 8)
            {
                GameObject.FindObjectOfType<PauseMenu>().WinGame();
            }
        }
    }
}

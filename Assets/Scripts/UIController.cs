using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Canvas worldText;
    public TextMeshProUGUI movement;
    public TextMeshProUGUI jump;

    void Awake()
    {
        jump.gameObject.SetActive(false);
    }

    public void SetActive(bool x, string y)
    {
        if(y == "MoveText")
        {
            movement.gameObject.SetActive(x);
        }
        else if(y == "JumpText")
        {
            jump.gameObject.SetActive(x);
        }
    }
}

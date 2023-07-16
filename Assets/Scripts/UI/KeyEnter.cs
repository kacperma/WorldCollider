using UnityEngine;
using UnityEngine.UI;

public class KeyEnter : MonoBehaviour
{
    [SerializeField]
    private KeyCode key;

    void Update()
    {
       if (Input.GetKeyDown(key))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
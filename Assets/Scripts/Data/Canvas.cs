using UnityEngine;

public class Canvas : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
   
}

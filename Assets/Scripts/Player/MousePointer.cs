using UnityEngine;

public class MousePointer : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }   
}
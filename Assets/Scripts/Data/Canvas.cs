using UnityEngine;

// 안씀 삭제해애함 
// HERE
public class Canvas : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
   
}

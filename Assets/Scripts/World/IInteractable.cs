using UnityEngine;

// 안씀 삭제해애함 
// HERE
namespace ST07.World
{
    public interface IInteractable
    {
        bool CanInteract(GameObject actor);
        void Interact(GameObject actor);
    }
}




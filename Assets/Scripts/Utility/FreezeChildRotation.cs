using UnityEngine;

// too lazy so I go with this solution :(
public class FreezeChildRotation : MonoBehaviour 
{
    private void Update()
    {
        if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}

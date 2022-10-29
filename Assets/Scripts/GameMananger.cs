using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
public class GameMananger : MonoBehaviour
{
    public List<SpriteLibraryAsset> CharacterSpriteLibraryAssets;
    public static GameMananger instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}

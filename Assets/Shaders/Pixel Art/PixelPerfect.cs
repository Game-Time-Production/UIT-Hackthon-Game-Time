using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect : MonoBehaviour
{
    SpriteRenderer render;
    void Start()
    {
        SetPixelSize();
    }
    void SetPixelSize()
    {
        render = GetComponent<SpriteRenderer>();
        float ratio_y = render.material.GetFloat("_RatioY");
        float ratio_x = render.material.GetFloat("_RatioX");

        Vector2 sprite_size = GetComponent<SpriteRenderer>().sprite.rect.size;
        float sprite_x = sprite_size.x / 200; // Pxiel Per Unit
        float sprite_y = sprite_size.y / 200;

        ratio_y /= sprite_y;
        ratio_x /= sprite_x;

        render.material.SetFloat("_RatioY", ratio_y);
        render.material.SetFloat("_RatioX", ratio_x);
    }

}

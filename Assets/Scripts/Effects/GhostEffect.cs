using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GhostEffect : MonoBehaviour
{
    [SerializeField] float ghostDelay;
    [SerializeField] float ghostDelayTimer;
    [SerializeField] GameObject ghostEffectPrefab;
    SpriteRenderer spriteRenderer;
    public bool makeGhost = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        ghostDelayTimer = ghostDelay;
    }
    void Update()
    {
        if (ghostDelayTimer > 0)
        {
            ghostDelayTimer -= Time.deltaTime;
        }
        else
        {
            GameObject currentGhost = Instantiate(ghostEffectPrefab, transform.position, transform.rotation);
            currentGhost.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            ghostDelayTimer = ghostDelay;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.4f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float defaultAplha = 0.8f;
    private float alphaMultiplayer = 0.9f;

    private Transform player; 

    [SerializeField]
    private SpriteRenderer playerSR;
    private SpriteRenderer spriteRenderer;

    private Color color;

    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        alpha = defaultAplha;
        spriteRenderer.sprite = playerSR.sprite;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;
        transform.position = player.position + new Vector3(0.4f, -0.1f);
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplayer;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;

        if(Time.time >= (activeTime + timeActivated) || alpha <= .01f)
        {
            PlayerAfterImagePool.Instace.AddToPool(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;

    float movePosX;
    float movePosY;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movePosX = Random.Range(-1f, 1f);
        movePosY = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        if(GameManager.Manager.level == 3)
        {
            MoveCard();
        }
    }
    void MoveCard()
    {
        transform.position += new Vector3(movePosX, movePosY, 0f) * Time.deltaTime;
        if(transform.position.x >= 2.3f || transform.position.x <= -2.3f)
        {
            movePosX *= -1;
        }
        if(transform.position.y >= 4.3f || transform.position.y <= -4.3f)
        {
            movePosY *= -1;
        }
    }
    public void OpenCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("isOpen", true);
        Transform back = transform.Find("back");
        transform.Find("Front").gameObject.SetActive(true);
        back.gameObject.SetActive(false);
        back.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        if(GameManager.Manager.firstCard == null)
        {
            GameManager.Manager.timer = Time.time;
            GameManager.Manager.firstCard = gameObject;
        }
        else
        {
            GameManager.Manager.secondCard = gameObject;
            GameManager.Manager.IsMatched();
        }
    }
    public void DestroyCard()
    {
        Destroy(gameObject, 1.0f);
    }

    public void CloseCard()
    {
        Invoke(nameof(CloseCardInvoke), 1.0f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("Front").gameObject.SetActive(false);
    }
}

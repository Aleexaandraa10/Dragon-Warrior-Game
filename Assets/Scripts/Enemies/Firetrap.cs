using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [Header("Firetrap Timers")]
    [SerializeField] private float damage;
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool triggered; //when the trap gets triggered
    private bool active; //when the trap is active and can hurt the player

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!triggered)
            {
                //trigger the firetrap
                StartCoroutine(ActivateFiretrap());

            }
            if (active)
            {
                collision.GetComponent<Heallth>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        //turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRenderer.color = Color.red; //turn the sprite red to notify the player

        //wait for delay, activate trap, turn on animation, return color back to normal 
        yield return new WaitForSeconds(activationDelay);
        spriteRenderer.color = Color.white; //turn the sprite red to its initial color
        active = true;
        anim.SetBool("activated", true);

        //wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activationDelay);
        active = false;
        triggered  = false;
        anim.SetBool("activated", false);
    }

}

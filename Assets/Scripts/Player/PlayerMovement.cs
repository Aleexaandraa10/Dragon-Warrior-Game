using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    private Rigidbody2D body; //body este o referinta a componentei Rigidbody2D a GameObject-ului pe care lucram (player aici)
    [SerializeField] private float speed; //ma ajuta sa vad var speed in Inspector in Unity, chiar daca e private
                                          // si pot modifica variabila speed fara sa o fac publica 

    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    

    private void Start() //ruleaza doar cand porneste jocul
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() //ruleaza la fiecare frame a jocului
    {
        horizontalInput = Input.GetAxis("Horizontal");

 
        //Flip player when moving left/right
        if (horizontalInput > 0.01f) //daca caracterul merge dreapta
        {
            transform.localScale = Vector3.one; //pozitia corpului este inspre dreapta (adica ia valoarea coord X cu acel Vector3.one)
        }
        else if (horizontalInput < -0.01f) //daca caracterul merge stanga
        {
            transform.localScale = new Vector3(-1,1,1); //tot corpul se va indrepta spre stanga
        }



        //set animator parameters
        anim.SetBool("run", horizontalInput!=0);
        anim.SetBool("grounded", isGrounded());

        if (wallJumpCooldown > 0.2f)
        {
            
            //angularVelocity = viteza de rotatie a ob pe axa Z(in 2D)
            //Input.GetAxis("Horizontal") = cat de mult jucatorul apasa stanga dreapta, horizontal = 1(dreapta), =-1(stanga), =0(stau pe loc)
            //velocity = viteza de deplasare a unui obiect pe X, Y, Z
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if(onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 7;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // ob nostru mentine ac viteza orizontal
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
            
        }
       
        
    }

  
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider!=null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

}

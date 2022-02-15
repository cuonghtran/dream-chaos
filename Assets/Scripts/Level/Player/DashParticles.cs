using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashParticles : MonoBehaviour
{
    bool facingRight = true;
    // Start is called before the first frame update
    void OnEnable()
    {
        var player = transform.parent.GetComponent<Player>();
        if (player.facingRight && !facingRight)
        {
            facingRight = true;
            Flip();
        }
        else if (!player.facingRight && facingRight)
        {
            facingRight = false;
            Flip();
        }

        //transform.localScale = new Vector2(transform.parent.localScale.x, gameObject.transform.localScale.y);

    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

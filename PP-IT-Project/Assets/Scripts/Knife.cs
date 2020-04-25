using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    

    public float speed;
    public SpriteRenderer MyRender;

    [HideInInspector]
    public bool Shot;

    private Vector3 lastPos;
    private Vector3 initialPos;

    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        PlayShowAnimation();
    }

    // Update is called once per frame
    void Update()
    {

        if (Shot)
        { 
            lastPos = transform.position;

            transform.position += Vector3.up * speed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Linecast(lastPos, transform.position);
            

            if(hit.collider != null)
            {
                Shot = false;
                if (hit.transform.tag == "Knife")
                {
                    //Lost
                    LevelManager.Instance.WorongHit();
                    myRigidbody.bodyType = RigidbodyType2D.Dynamic;

                    myRigidbody.AddTorque(10, ForceMode2D.Impulse);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                    transform.parent = hit.transform;
                    myCollider.enabled = true;
                    LevelManager.Instance.SuccessfulHit(myRigidbody);
                    Shot = false;
                }
            }
        }
    }

    public void Shoot()
    {
        Shot = true;
    }

    public void PlayShowAnimation()
    {
        StartCoroutine("ShowKnife");
        
    }

    IEnumerator ShowKnife()
    {
        float startTime = Time.time;
        float duration = 0.3f;
        Vector3 downPos = initialPos - new Vector3(0, 0.5f, 0);

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            MyRender.color = new Color(MyRender.color.a, MyRender.color.g, MyRender.color.b, Mathf.Lerp(0, 1, t));
            transform.position = Vector3.Lerp(downPos, initialPos, t);
            yield return null;
        }
        MyRender.color = new Color(MyRender.color.a, MyRender.color.g, MyRender.color.b, 1);
        transform.position = initialPos;
    }
}

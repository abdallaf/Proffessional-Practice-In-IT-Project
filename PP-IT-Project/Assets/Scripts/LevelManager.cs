using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int knifesCount;
    private Knife curKnife;
    private int curKnifesCount;
    private int hits;
    private bool canPlay = true;



    [SerializeField] private TargetController target;
    [SerializeField] private GameObject knifeGO;
    [SerializeField] private Transform knifeSpawnPoint;

    private List<Rigidbody2D> curHitKnifes = new List<Rigidbody2D>();
   
    

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        curKnifesCount = knifesCount;
        SpawnKnife();

    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay)
            return;
        if (Input.GetMouseButtonDown(0) && curKnife != null && curKnifesCount > -1)
        {
            ShootAKnife();
        }
    }

    void ShootAKnife()
    {
        curKnife.Shoot();
        curKnifesCount--;

        if(curKnifesCount > 0)
        {
            SpawnKnife();
        }
        else
        {
            curKnife = null;
        }
    }

    void SpawnKnife()
    {
        GameObject knife = Instantiate(knifeGO, knifeSpawnPoint.position, Quaternion.identity);
        curKnife = knife.GetComponent<Knife>();
    }

    public void SuccessfulHit(Rigidbody2D knife)
    {
        target.GotHit();
       hits++;

        curHitKnifes.Add(knife);
 

        if (hits == knifesCount)
        {
            Win();
        }
    }

    public void WorongHit()
    {
        Lose();
    }

    void Win()
    {
        for (int i = 0; i < curHitKnifes.Count; i++)
        {
            Rigidbody2D cKnife = curHitKnifes[i];
            cKnife.transform.parent = null;

            cKnife.bodyType = RigidbodyType2D.Dynamic;

            Vector3 forceDirection = (cKnife.transform.position - target.transform.position).normalized * 4;
            if (i == curHitKnifes.Count - 1)
                forceDirection.y = 10;
            cKnife.AddForceAtPosition(forceDirection, target.transform.position, ForceMode2D.Impulse);
            cKnife.AddTorque(4, ForceMode2D.Impulse);

            Destroy(cKnife, 3);
        }
        target.DestroyMe();
        GameManager.Instance.ReloadScene();
    }

    void Lose()
    {
        canPlay = false;
        GameManager.Instance.ReloadScene();
    }
    

}


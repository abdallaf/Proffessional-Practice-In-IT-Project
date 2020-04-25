using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private Vector3 initialPos;

    [SerializeField] private GameObject destroyedTarget;
    [SerializeField] private List<Rigidbody2D> destroyedTargetPieces;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -150) * Time.deltaTime);
    }
    public void GotHit()
    {
        StopCoroutine("Pushing");
        StartCoroutine("Pushing");
    }
    IEnumerator Pushing()
    {
        float startTime = Time.time;
        float duration = 0.025f;
        Vector3 upPos = initialPos + new Vector3(0, 0.1f, 0);

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(initialPos, upPos, t);
            yield return null;
        }

        startTime = Time.time;
        duration = 0.2f;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            t = 1 - Mathf.Abs(Mathf.Pow(t - 1, 2));
            transform.position = Vector3.Lerp(upPos, initialPos, t);
            yield return null;
        }
        transform.position = initialPos;
    }

    public void DestroyMe()
    {
        destroyedTarget.transform.parent = null;
        destroyedTarget.SetActive(true);

        for (int i = 0; i < destroyedTargetPieces.Count; i++)
        {
            Vector3 forceDirection = (destroyedTargetPieces[i].transform.position - transform.position).normalized * 4;
            forceDirection.y = forceDirection.y < 0 ? forceDirection.y * -1 : forceDirection.y;
            destroyedTargetPieces[i].AddForceAtPosition(forceDirection, transform.position, ForceMode2D.Impulse);
            destroyedTargetPieces[i].AddTorque(4, ForceMode2D.Impulse);
            Destroy(destroyedTargetPieces[i].gameObject, 3);
        }
        Destroy(gameObject);
    }

}

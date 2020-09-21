using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCreate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject destroyedCrate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyCrate()
    {
        GameObject d = Instantiate(destroyedCrate, transform.position, transform.rotation);
        d.SetActive(true);
        Destroy(this.gameObject);
    }
}

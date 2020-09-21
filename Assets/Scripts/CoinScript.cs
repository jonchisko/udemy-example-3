using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{


    [SerializeField]
    private int _amount = 1;
    [SerializeField]
    private AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            Player pScript = other.gameObject.GetComponent<Player>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                pScript.Coin = _amount;
                AudioSource.PlayClipAtPoint(_clip, transform.position, 1.0f);
                Destroy(this.gameObject);
            }
        }
    }

}

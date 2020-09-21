using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour
{
    [SerializeField]
    AudioClip _clip;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            Player playerScript = other.gameObject.GetComponent<Player>();
            if(playerScript.Coin > 0)
            {
                playerScript.Coin = -1;
                AudioSource.PlayClipAtPoint(_clip, transform.position, 1.0f);
                playerScript.EnableWeapon();
                //other.transform.Find("Weapon").gameObject.SetActive(true);
                Debug.Log("Here's ya gun!");
            }
            {
                Debug.Log("No Coin No Fun!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public Text currentAmmo;
    public Text maxAmmo;
    public Player playerScript;
    public Image coinImage;


    // Start is called before the first frame update
    void Start()
    {
        currentAmmo.text = playerScript.GetCurrentAmmo().ToString();
        maxAmmo.text = playerScript.GetMaxAmmo().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        playerScript.OnAmmoChange += UpdateCurrentAmmoText;
        playerScript.OnCoinPickUp += EnableCoin;
    }

    private void OnDisable()
    {
        playerScript.OnAmmoChange -= UpdateCurrentAmmoText;
        playerScript.OnCoinPickUp -= EnableCoin;
    }

    void UpdateCurrentAmmoText(int value)
    {
        currentAmmo.text = value.ToString();
    }

    void EnableCoin()
    {
        if (playerScript.Coin > 0) 
        { 
            coinImage.gameObject.SetActive(true); 
        }
        else
        {
            coinImage.gameObject.SetActive(false);
        }

    }
}

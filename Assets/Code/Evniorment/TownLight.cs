using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownLight : MonoBehaviour
{
    [Header("# Town Light")]
    [Space]
    [SerializeField] GameObject TownGlobarLight;
    [SerializeField] GameObject TownCharacter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TownCharacter.activeSelf == true && TownGlobarLight.activeSelf == false)
        {
            TownGlobarLight.gameObject.SetActive(true);
        }
        else if(TownCharacter.activeSelf == false && TownGlobarLight.activeSelf == true)
        {
            TownGlobarLight.gameObject.SetActive(false);
        }
        
    }
}

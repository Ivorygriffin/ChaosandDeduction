using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSlider : MonoBehaviour
{
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        //TODO: change this due to calling a singleton every frame?
        // idk if thats bad, probably fine
        if (PlayerManager.Instance.characterFire)
            slider.value = PlayerManager.Instance.characterFire.loadedSpell.GetAmmoCount();
    }
}

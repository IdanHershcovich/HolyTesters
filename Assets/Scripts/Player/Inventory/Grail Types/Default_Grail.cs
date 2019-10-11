using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Grail : Grail
{
        /// <summary> The reference to the prefab for the projectile </summary>
    [SerializeField] private GameObject projectile;

    [SerializeField]
    private GameObject hitBoxPrefab;

    public override void baseEffect() {
        
        StartCoroutine(baseEffectCoroutine());

    }

    public IEnumerator baseEffectCoroutine()
    {
        SoundController.Instance.PlaySoundEffect(SoundType.PUNCH);
        GameObject currentObject
            = Instantiate(hitBoxPrefab, Player.Instance.transform.position, Player.Instance.transform.rotation, Player.Instance.transform);
        //Debug.Log(currentObject);
        currentObject.GetComponent<DefaultBasic>().SetValues(baseDamage, baseKnockback, baseStunTime);

        for (float timer = 0; timer < baseAttackLength; timer += Time.deltaTime)
        {
            yield return null;
        }

        Destroy(currentObject);
    }

    public override void superEffect() {


    }

}

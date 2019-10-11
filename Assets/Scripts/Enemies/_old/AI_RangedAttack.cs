using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the attack that ranged enemies use
/// </summary>
public class AI_RangedAttack : MonoBehaviour
{

    /// <summary> The damage to the player that the attack has </summary>
    [SerializeField] private float attackPower;
    /// <summary> The end lag time after attacking </summary>
    [SerializeField] private float attackEnd;

    /// <summary> The timer for how long the player has been in range </summary>
    private float playerRangeTimer;

    /// <summary> The reference to the prefab for the projectile </summary>
    [SerializeField] private GameObject projectile;

    /// <summary> Internal reference to the main enemy class holding the status info </summary>
    Enemy_old status;
    /// <summary> Internal reference to the enemy nav agent script </summary>
    EnemyNavAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponentInParent<Enemy_old>();
        nav = GetComponentInParent<EnemyNavAgent>();
    }

    /// <summary>
    /// Sends out projectiles to attack the player
    /// Called when the player is in range
    /// </summary>
    /// <param name="player"> Reference to the player in range</param>
    private void fireRangedAttack(PlayerStatus player)
    {
        // Play sound effect
        SoundController.Instance.PlaySoundEffect(SoundType.BAT_SHOT);

        //spawns a projectile and sends it forward
        EnemyProjectileScript forward = Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        forward.transform.position = transform.position;
        //forward.player = player;
        //forward.attackPower = attackPower;
        forward.direction = Vector3.ProjectOnPlane((player.transform.position - transform.position).normalized, Vector3.up);

        //spawns a projectile and sends it off-center to the left
        EnemyProjectileScript skewedLeft = Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        skewedLeft.transform.position = transform.position;
        //skewedLeft.player = player;
        //skewedLeft.attackPower = attackPower;
        skewedLeft.direction = Quaternion.Euler(0, -15, 0) * forward.direction;

        //spawns a projectile and sends it off-center to the right
        EnemyProjectileScript skewedRight = Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        skewedRight.transform.position = transform.position;
        //skewedRight.player = player;
        //skewedRight.attackPower = attackPower;
        skewedRight.direction = Quaternion.Euler(0, 15, 0) * forward.direction;

    }

    void OnTriggerStay(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            // If timer just started/restarted, attack player
            if (playerRangeTimer == 0)
            {
                // Fire attack towards player
                PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
                fireRangedAttack(player);

                status.InitiateAttackCoolDown();
            }

            //increment timer
            playerRangeTimer += Time.fixedDeltaTime;

            // Reset timer after reaching end lag time
            if (playerRangeTimer >= attackEnd)
            {
                playerRangeTimer = 0;
            }
        }*/
    }

    private void OnTriggerExit(Collider other)
    {

        // reset timer after player leaves attack range
        if (other.CompareTag("Player"))
        {
            playerRangeTimer = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>, Buffable
{

    // Internal references
    private PlayerStatus status;
    private PlayerAttack attack;
    private PlayerMovement movement;
    public Inventory_Reworked inventory;
    private DebuffList debuffs = new DebuffList();

    // Start is called before the first frame update
    override 
    protected void Awake()
    {
        // Call awake method on Singleton
        base.Awake();

        // Initialize internal references
        status = GetComponent<PlayerStatus>();
        attack = GetComponent<PlayerAttack>();
        movement = GetComponent<PlayerMovement>();
        
    }

    void Update(){
        debuffs.ApplyDebuffEffects();
    }

    public void Die()
    {
        inactive = true;
    }

    /// <summary>
    /// Move the player in given direction
    /// <param name="direction">Direction to move the player in</param>
    /// </summary>
    public void Move(Vector2 direction)
    {
        movement.MovePlayer(direction);
    }

    /// <summary>
    /// Executes a player dash
    /// </summary>
    /// <param name="direction">Direction to have the player dash in</param>
    public void Dash(Vector2 direction)
    {
        movement.InitiateDash(direction);
    }

    public void BasicAttack()
    {
        attack.BasicAttack(inventory.getEquippedGrail());
    }

    public void SuperAttack()
    {
        attack.SuperAttack(inventory.getEquippedGrail());
    }

    public void FillSuper(float percentage){
        inventory.getEquippedGrail().addSuperBarPercent(percentage);
    }

    public void ReceiveAttack(HitAttr hitAttr)
    {
        CameraScript.Instance.StartCameraShake(0.5f, 1f, 2f);

        status.TakeDamage(hitAttr.damage, hitAttr.iFrameTimeMod);
        movement.TakeKnockBack(hitAttr.knockback.normalized, hitAttr.knockback.magnitude, hitAttr.stunTime);

    }

    public void Heal(float f){
        status.Heal(f);
    }

    public void ChangeFacingDirection(Vector3 newDirection)
    {
        movement.ChangeFacingDirection(newDirection);
    }

    // apply a debuff
    // If the debuff already exists on the enemy, remove it and apply the new one.
    public void RegisterDebuff(Debuff db){
        db.SetTarget(this);
        debuffs.RegisterDebuff(db);
    }
    
    // remove a debuff
    public void RemoveDebuff(Debuff db){
        debuffs.RemoveDebuff(db);
    }

    public bool HasDebuff(Debuff db){
        return debuffs.HasDebuff(db);
    }

    //used for slow debuff
    public void SetMoveSpeedMultiplier(float f){
        movement.moveSpeedMultiplier = f;
    }

    //used for bleed debuff
    public void TakeDebuffDamage(float f){
        status.TakeDamage(f, 0);
    }
    //used for vampire grail
    public float GetPercentHealth(){
        return(status.health / status.GetMaxHealth());
    }
    public float GetCurrentHealth(){
        return(status.health);
    }

    // TODO: Wills should probably implement these
    public void Tint(Color color, float amount){}
    public void UnTint(){}

}

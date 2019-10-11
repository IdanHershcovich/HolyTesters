using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Buffable 
{
    void RegisterDebuff(Debuff db);
    void RemoveDebuff(Debuff db);
    void SetMoveSpeedMultiplier(float f);
    bool HasDebuff(Debuff db);
    void TakeDebuffDamage(float f);
    void Tint(Color color, float amount);
    void UnTint();

}

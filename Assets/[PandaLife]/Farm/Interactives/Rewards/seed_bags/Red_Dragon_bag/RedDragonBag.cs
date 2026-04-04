using UnityEngine;

public class RedDragonBag : RewardBagElement
{

    public override bool CheckCondition()
    {
        // Verificamos que el GameManager exista para evitar errores de referencia nula
        if (GameManager.instance == null) return false;

        // Si es el día correcto y la bolsa aún está desactivada, devolvemos true
        return rewardDay <= GameManager.instance.numeroDia;
    }
}
using UnityEngine;

public class BlueberryBag : RewardBagElement
{
    public override void MostrarMensaje()
    {
        //Debug.Log($"¡Día {rewardDay}! Has desbloqueado el Saco de arandanos. ¡Ideal para construir!");
    }

    public override bool CheckCondition()
    {
        // Verificamos que el GameManager exista para evitar errores de referencia nula
        if (GameManager.instance == null) return false;

        // Si es el día correcto y la bolsa aún está desactivada, devolvemos true
        return rewardDay <= GameManager.instance.numeroDia;
    }
}
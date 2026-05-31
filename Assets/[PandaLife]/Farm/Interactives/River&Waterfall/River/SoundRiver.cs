using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SoundRiver : MonoBehaviour
{
    public SplineContainer riverSpline;
    public Transform player;

    void Update()
    {
        if (riverSpline == null || player == null) return;
        //  Buscamos el punto más cercano del Spline respecto a la posición del jugador
        // Retorna un valor 't' (entre 0 y 1) y la posición exacta en el mundo 3D
        SplineUtility.GetNearestPoint(
            riverSpline.Spline,
            riverSpline.transform.InverseTransformPoint(player.position),
            out float3 nearestPointLocal,
            out float t
        );

        //  Convertimos ese punto local a coordenadas del mundo real
        Vector3 nearestPointWorld = riverSpline.transform.TransformPoint(nearestPointLocal);

        // Movemos ESTE objeto (el que tiene el Audio Source) a esa posición del río
        transform.position = nearestPointWorld;
    }
}

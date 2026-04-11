using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    [Header("UI Pines")]
    [SerializeField] private TMP_Text pin1Text;
    [SerializeField] private TMP_Text pin2Text;
    [SerializeField] private TMP_Text pin3Text;

    private void Start()
    {
        PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();

        if (pandaReq != null)
        {
            // IMPORTANTE: Usamos GetCurrentRequests() en lugar de Generate
            List<string> pedidosDeHoy = pandaReq.GetCurrentRequests();

            if (pedidosDeHoy.Count >= 3)
            {
                if (pin1Text != null) pin1Text.text = pedidosDeHoy[0];
                if (pin2Text != null) pin2Text.text = pedidosDeHoy[1];
                if (pin3Text != null) pin3Text.text = pedidosDeHoy[2];
            }
        }
    }

    public void ActualizarTextosManual()
    {
        PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
        if (pandaReq != null)
        {
            List<string> pedidosDeHoy = pandaReq.GetCurrentRequests();
            if (pedidosDeHoy.Count >= 3)
            {
                if (pin1Text != null) pin1Text.text = pedidosDeHoy[0];
                if (pin2Text != null) pin2Text.text = pedidosDeHoy[1];
                if (pin3Text != null) pin3Text.text = pedidosDeHoy[2];
            }
        }
    }

}
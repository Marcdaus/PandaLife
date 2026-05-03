using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour
{
    [Header("UI Pines")]

    [SerializeField] private Image pin1Image;
    [SerializeField] private Image pin2Image;
    [SerializeField] private Image pin3Image;
    [SerializeField] private List<RequestSpritePair> requestSprites;

    [System.Serializable]
    public class RequestSpritePair
    {
        public string requestName;
        public Sprite sprite;
    }

    private Dictionary<string, Sprite> spriteDict;

    private void Awake()
    {
        spriteDict = new Dictionary<string, Sprite>();

        foreach (var pair in requestSprites)
        {
            spriteDict[pair.requestName] = pair.sprite;
        }
    }

    private Sprite GetSpriteFromRequest(string request)
    {

        if (spriteDict.ContainsKey(request))
            return spriteDict[request];

        Debug.LogWarning("No sprite found for request: " + request);
        return null;
    }

    private void Start()
    {
        PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();

        if (pandaReq != null)
        {
            List<string> pedidosDeHoy = pandaReq.GetCurrentRequests();

            if (pedidosDeHoy.Count >= 3)
            {
                if (pin1Image != null) pin1Image.sprite = GetSpriteFromRequest(pedidosDeHoy[0]);
                if (pin2Image != null) pin2Image.sprite = GetSpriteFromRequest(pedidosDeHoy[1]);
                if (pin3Image != null) pin3Image.sprite = GetSpriteFromRequest(pedidosDeHoy[2]);
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
                if (pin1Image != null) pin1Image.sprite = GetSpriteFromRequest(pedidosDeHoy[0]);
                if (pin2Image != null) pin2Image.sprite = GetSpriteFromRequest(pedidosDeHoy[1]);
                if (pin3Image != null) pin3Image.sprite = GetSpriteFromRequest(pedidosDeHoy[2]);
            }
        }
    }
    

}
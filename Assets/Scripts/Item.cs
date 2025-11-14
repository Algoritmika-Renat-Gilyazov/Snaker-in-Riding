using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;
    public string id = "snaker:item";

    [SerializeField]
    private Image texture;

    void Start()
    {
        if (texture != null && sprite != null)
        {
            texture.sprite = sprite;
        }
    }

    public void SetTexture(Sprite spr)
    {
        if (texture != null)
        {
            texture.sprite = spr;
        }
    }
    public Sprite GetTexture()
    {
        return texture.sprite;
    }

    public void ResetTexture()
    {
        if (texture != null && sprite != null)
        {
            texture.sprite = sprite;
        }
    }

    public virtual void Use()
    {

    }
}

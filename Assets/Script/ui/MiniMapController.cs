using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MiniMapController : MonoBehaviour, IPointerDownHandler
{
    public Camera miniMapCam;
    [SerializeField] float bgX;
    [SerializeField] GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 cursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform,
            eventData.pressPosition, eventData.pressEventCamera, out cursor))
        {
            cursor.x += 500f;
            cursor.y -= 250f;
            float textureposx = GetComponent<RawImage>().texture.width;
            float scale = textureposx / bgX;
            Vector2 mousepos = new Vector2(cursor.x / scale, cursor.y / scale);
            player.transform.up = (mousepos - (Vector2)player.transform.position).normalized;
            player.GetComponent<playerc>().asd(mousepos);
        }      
    }
}

using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private RectTransform dot;

    void Update()
    {
        dot.anchoredPosition = new Vector3(player.transform.position.x-25, player.transform.position.y-70)/1.48f;
    }
}

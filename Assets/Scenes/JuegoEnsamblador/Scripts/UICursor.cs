using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursorRect;
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite cursorSprite;
    [SerializeField] private Vector2 cursorOffset = Vector2.zero;
    [SerializeField] private bool hideSystemCursor = true;

    private void Awake()
    {
        if (cursorImage != null && cursorSprite != null)
        {
            cursorImage.sprite = cursorSprite;
        }
    }

    private void OnEnable()
    {
        if (hideSystemCursor)
        {
            Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        if (hideSystemCursor)
        {
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (cursorRect == null)
        {
            return;
        }

        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        cursorRect.position = mouse.position.ReadValue() + cursorOffset;
    }
}

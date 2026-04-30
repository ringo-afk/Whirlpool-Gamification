using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private Renderer bgRenderer;
    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset+= new Vector2(0, speed*Time.deltaTime);
    }
}

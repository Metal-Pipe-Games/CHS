using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletController : MonoBehaviour
{
    public float speed = 315;
    public LayerMask layers;
    VisualEffect vfx;
    public float Attack;
    public float MaxDistance = 100;
    public float DestroyDelay = 30;
    float distance = 0;
    MeshRenderer rend;

    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            float s = speed * Time.deltaTime;
            distance += s;
            Vector3 movement = s * transform.forward;
            //Debug.DrawLine(transform.position, transform.position + movement, Color.red, 60, false);
            if (Physics.Raycast(transform.position, movement, out RaycastHit hit, s, layers.value))
            {
                transform.position = hit.point;
                transform.up = -transform.forward;
                if (hit.collider.CompareTag("Enemy"))
                {
                    vfx.SendEvent("OnEnemyHit");
                    hit.collider.transform.root.GetComponent<EnemyHealth>().Damage(Attack);
                }else
                {
                    var cord = hit.textureCoord;
                    var u = cord.x;
                    var v = cord.y;
                    var rend = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    try
                    {
                        if (rend)
                        {
                            var texture = rend.material.mainTexture as Texture2D;
                            var min = Color.white;
                            var max = Color.black;
                            for (int x = -1; x < 3; x++)
                            {
                                for (int y = -1; y < 3; y++)
                                {
                                    var col = texture.GetPixelBilinear(x + u, y + v);
                                    if (min.grayscale > col.grayscale) min = col;
                                    if (max.grayscale < col.grayscale) max = col;
                                }
                            }
                            vfx.SetVector4("MinColor", min);
                            vfx.SetVector4("MaxColor", max);
                        }
                        else throw new System.Exception();
                    }
                    catch
                    {
                        vfx.ResetOverride("MinColor");
                        vfx.ResetOverride("MaxColor");
                    }
                    vfx.SendEvent("OnOtherHit");
                }
                /*else if (hit.collider.CompareTag("Level"))
                {
                
                }/**/
                rend.enabled = false;
                finished = true;
                speed = 0;
                Destroy(gameObject, DestroyDelay);
            }
            else
            {
                if (distance > MaxDistance) Destroy(gameObject, DestroyDelay);
                transform.position += movement;
            }
        }
    }
}

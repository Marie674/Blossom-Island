using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Behaviours/PhysicCharBehaviour", 10)]
    public class PhysicCharBehaviour : MonoBehaviour
    {

        [System.Flags]
        public enum eCollFlags
        {
            NONE = 0,
            DOWN = (1 << 0),
            LEFT = (1 << 1),
            RIGHT = (1 << 2),
            UP = (1 << 3)
        }

        public bool CanMove = true;

        public Vector3 Dir;
        public float MaxRunSpeed = 2f;
        public float MaxSpeed = 1f;
        public bool IsCollEnabled = true;

        private Vector3 m_vPrevPos;
        private float m_speed;


        public eCollFlags CollFlags = eCollFlags.NONE;

        public Rect CollRect = new Rect(-0.14f, -0.04f, 0.28f, 0.12f);

        public NeedBase energyNeed;

        public bool IsMoving
        {
            get { return Dir.sqrMagnitude > 0; }
        }

        void Start()
        {

            m_vPrevPos = transform.position;

            //	energyNeed = PlayerNeedManager.Instance.GetNeed ("Energy");
        }

        void FixedUpdate()
        {

            //RpgMapHelper.DebugDrawRect( transform.position, CollRect, Color.white );
            if (Dir.sqrMagnitude > 0f)
            {
                // divide by n per second ( n:2 )
                if (Input.GetButton("Run") && energyNeed.CurrentValue > 0)
                {
                    m_speed += (MaxRunSpeed - m_speed) / Mathf.Pow(2f, Time.deltaTime);

                    // runningBool.MetBool = true;

                }
                else
                {
                    //runningBool.MetBool = false;

                    m_speed += (MaxSpeed - m_speed) / Mathf.Pow(2f, Time.deltaTime);
                }

            }
            else
            {
                // runningBool.MetBool = false;
                m_speed /= Mathf.Pow(2f, Time.deltaTime);
            }
            Dir.z = 0f;
            if (CanMove)
            {
                transform.position += Dir * m_speed * Time.deltaTime;
                float zPos = ((transform.position.y / 100) - (transform.position.x / 1000)) - 5f;
                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, zPos);


                transform.position = newPos;
            }

        }

        public void TeleportTo(Vector3 vPos)
        {
            transform.position = vPos;
            m_vPrevPos = vPos;
        }

    }
}

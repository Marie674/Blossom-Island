using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [RequireComponent(typeof(DirectionalAnimation))]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [AddComponentMenu("RpgMapEditor/Controllers/CharBasicController", 10)]
    public class CharBasicController : MonoBehaviour
    {
        public DirectionalAnimation[] AnimCtrl { get { return m_animCtrl; } }
        public PhysicCharBehaviour PhyCtrl { get { return m_phyChar; } }

        public bool IsVisible
        {
            get
            {
				return m_animCtrl[0].TargetSpriteRenderer.enabled;

            }

            set
            {
                SetVisible( value );
            }
        }

        protected DirectionalAnimation[] m_animCtrl;
        protected PhysicCharBehaviour m_phyChar;

        protected float m_timerBlockDir = 0f;

        protected virtual void Start()
        {
            m_animCtrl = GetComponentsInChildren<DirectionalAnimation>();
         	m_phyChar = GetComponent<PhysicCharBehaviour>();
        }

        protected virtual void Update()
        {
            float fAxisX = Input.GetAxis("Horizontal");
            float fAxisY = Input.GetAxis("Vertical");
            UpdateMovement(fAxisX, fAxisY);
        }


        protected void UpdateMovement( float fAxisX, float fAxisY )
        {
			m_timerBlockDir -= Time.deltaTime;
            m_phyChar.Dir = new Vector3(fAxisX, fAxisY, 0);

			foreach (DirectionalAnimation animCtrl in m_animCtrl) {
				animCtrl.IsPlaying = m_phyChar.IsMoving;
				animCtrl.SetAnimDirection(m_phyChar.Dir);
			}

        }

        public virtual void SetVisible( bool value )
        {
			foreach (DirectionalAnimation animCtrl in m_animCtrl) {
				animCtrl.TargetSpriteRenderer.enabled = value;
			}
        }
    }
}
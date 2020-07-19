using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Controllers/PlayerController", 10)]
	public class PlayerController : CharBasicController {

		public Camera2DController Camera2D;

		private FollowObjectBehaviour m_camera2DFollowBehaviour;

        #region Singleton and Persistence
        static PlayerController s_instance;
        void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

			if(s_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                s_instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }      
        }
        #endregion

        public void UndoDontDestroyOnLoad()
        {
            s_instance = null;
        }

        void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (mode != UnityEngine.SceneManagement.LoadSceneMode.Single) return;

            if (s_instance != this) // this happens if UndoDontDestroyOnLoad was called
            {
                DestroyImmediate(gameObject);
            }
        }

        public override void SetVisible(bool value)
        {
            base.SetVisible(value);
        }

		protected override void Start () 
		{
            base.Start();
			if( Camera2D == null )
			{
				Camera2D = GameObject.FindObjectOfType<Camera2DController>();
			}
			
			m_camera2DFollowBehaviour = Camera2D.transform.GetComponent<FollowObjectBehaviour>();
			m_camera2DFollowBehaviour.Target = transform;
		}

        protected override void Update()
		{
            eAnimDir savedAnimDir = m_animCtrl[0].AnimDirection;
            base.Update();

                bool isMoving = (m_phyChar.Dir.sqrMagnitude >= 0.01);
                if (isMoving)
                {
                    //m_phyChar.Dir.Normalize();
                    m_camera2DFollowBehaviour.Target = transform;
                }
                else
                {
                    m_phyChar.Dir = Vector3.zero;
                }
		}

	}
}
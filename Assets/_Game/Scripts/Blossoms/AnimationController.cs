using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blossoms
{
    public class AnimationController : MonoBehaviour
    {
        public enum CharacterDirection
        {
            Down,
            Right,
            Up,
            Left
        }

        public List<Animator> Animators = new List<Animator>();
        public CharacterDirection CurrentDirection;
        public Vector3 Dir;
        public bool IsRunning;
        public float WalkSpeed = 4f;

        public float RunSpeed = 8f;

        public float SpeedMultiplier = 1f;
        float CurrentSpeed = 0f;
        Rigidbody2D RB;

        bool IsActing = false;

        Vector2 PreviousPos;
        Vector2 Velocity = Vector2.zero;
        void Start()
        {
            PreviousPos = transform.position;
            RB = GetComponent<Rigidbody2D>();
        }
        public CharacterDirection GetAnimDir(Vector2 vDir)
        {
            //            print(vDir);
            if (Mathf.Abs(vDir.x - vDir.y) <= 0.1f)
            {
                return CurrentDirection;
            }
            float angle360 = Vector3.Cross(-Vector2.up, vDir).z >= 0f ? Vector2.Angle(-Vector2.up, vDir) : 360f - Vector2.Angle(-Vector2.up, vDir);
            int dirIdx = Mathf.RoundToInt((angle360 * 4) / 360f) % 4;
            // if (IsRunning == false && CurrentSpeed < WalkSpeed * SpeedMultiplier)
            // {
            //     return CurrentDirection;
            // }
            // else if (IsRunning == true && CurrentSpeed < RunSpeed * SpeedMultiplier)
            // {
            //     return CurrentDirection;

            // }
            return (CharacterDirection)(dirIdx);
        }

        void FixedUpdate()
        {

            Velocity.x = -(PreviousPos.x - transform.position.x) / Time.deltaTime;
            Velocity.y = -(PreviousPos.y - transform.position.y) / Time.deltaTime;

            PreviousPos = transform.position;

            //Set move direction
            Dir = new Vector3(Mathf.Round(Velocity.x / 0.15f) * 0.15f, Mathf.Round(Velocity.y / 0.15f) * 0.15f, 0);
            Dir.z = 0f;

            // Set z value to reflect X and Y position (walk in front and behinf objects)
            float zPos = ((transform.position.y / 100) - (transform.position.x / 1000)) - 5f;
            CurrentDirection = GetAnimDir(Dir);

            UpdateAnimation();

        }

        void UpdateAnimation()
        {

            bool left = false;
            bool right = false;
            bool down = false;
            bool up = false;

            float speed = (Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.y)) / 2;

            switch (CurrentDirection)
            {
                case CharacterDirection.Down:
                    down = true;
                    left = false;
                    right = false;
                    up = false;
                    break;
                case CharacterDirection.Left:
                    left = true;
                    down = false;
                    right = false;
                    up = false;
                    break;
                case CharacterDirection.Right:
                    right = true;
                    left = false;
                    down = false;
                    up = false;
                    break;
                case CharacterDirection.Up:
                    up = true;
                    left = false;
                    right = false;
                    down = false;
                    break;
                default:
                    return;

            }
            foreach (Animator anim in Animators)
            {
                anim.SetBool("Down", down);
                anim.SetBool("Left", left);
                anim.SetBool("Right", right);
                anim.SetBool("Up", up);

                anim.SetFloat("Speed", speed);
                if (IsActing == false)
                {
                    if (speed < 1)
                    {
                        speed = 1f;
                    }
                    anim.speed = speed;
                }
                else
                {
                    anim.speed = 1f;
                }
            }
        }


    }
}


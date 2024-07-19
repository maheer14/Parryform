using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a Player attempts to parry during a jump.
    /// </summary>
    public class PlayerParry : Simulation.Event<PlayerParry>
    {
        public PlayerController player;
        public EnemyController enemy;

        // Parry strength values for different collision types
        public float parryStrength = 7.5f;

        public float parryWindowDuration = 2.5f;

        private float parryWindowEndTime;

        public int parryNo = 2;
        public override void Execute()
        {
            StartParryWindow();
            // Check if the player is in the correct state to parry
            if (IsWithinParryWindow())
            {
                // Play the parry audio
                if (player.audioSource && player.parryAudio)
                {
                    player.audioSource.PlayOneShot(player.parryAudio);
                }
                Debug.Log("inside parry...");
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down, 1f);
/*                RaycastHit2D hitRight = Physics2D.Raycast(player.transform.position, Vector2.right, 1f);
                RaycastHit2D hitLeft = Physics2D.Raycast(player.transform.position, Vector2.left, 1f);*/
/*                Debug.Log(hit.collider);
                Debug.Log(hitRight.collider);
                Debug.Log(hitLeft.collider);*/
                if (hit.collider != null && parryNo>0)
                {
                    if (hit.collider.CompareTag("Parryable"))
                    {
                        Debug.Log(hit.collider);
                        Debug.Log("Down Parry...");
                        player.animator.SetTrigger(parryNo--);
                        downParry();
                    }
                }
                else
                {
                    parryNo = 2;
                }
            }
        }

        private void downParry()
        {
            player.Bounce(parryStrength);

        }
        private bool IsWithinParryWindow()
        {
            return Time.time <= parryWindowEndTime;
        }

        private void StartParryWindow()
        {
            parryWindowEndTime = Time.time + parryWindowDuration;
        }
    }
}


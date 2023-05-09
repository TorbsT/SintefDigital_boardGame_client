using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Network
{
    internal class WatchdogPinger : MonoBehaviour
    {
        private enum State
        {
            INACTIVE,
            WAITING,
            PINGING,
        }
        [SerializeField] private Vector2 failureNoticeSize = new(500f, 500f);
        [SerializeField] private int textSize = 30;
        [SerializeField, Range(0f, 45f)] private float successCooldown = 15f;
        [SerializeField, Range(0f, 5f)] private float failureCooldown = 1f;
        [SerializeField, Range(0, 100f)] private float timeoutLimit = 61f;
        [SerializeField] private string disconnectToScene = "IntroScene";
        private float cooldown;
        private State state;
        private int consecutiveFailures;
        private float timeSinceContact;

        private void Start()
        {
            NetworkData.Instance.MeChanged += MeChanged;
        }
        private void Update()
        {
            timeSinceContact += Time.deltaTime;
            if (state != State.WAITING) return;
            cooldown -= Time.deltaTime;
            if (cooldown > 0f) return;
            // Time to ping
            state = State.PINGING;
            RestAPI.Instance.CheckIn(
                (success) =>
                {
                    cooldown = successCooldown;
                    state = State.WAITING;
                    consecutiveFailures = 0;
                    timeSinceContact = 0f;
                },
                (failure) =>
                {
                    cooldown = failureCooldown;
                    state = State.WAITING;
                    consecutiveFailures++;
                    if (timeSinceContact > timeoutLimit)
                    {
                        Debug.LogWarning("Lost connection to server - watchdog pings not received");
                        Destroy(gameObject);
                        SceneManager.LoadSceneAsync(disconnectToScene);
                    }
                },
                NetworkData.Instance.UniqueID
                );
        }
        private void OnGUI()
        {
            if (consecutiveFailures == 0) return;
            float width = failureNoticeSize.x;
            float height = failureNoticeSize.y;
            float fromX = (Screen.width-width)/2f;
            float fromY = (Screen.height-height)/2f;
            GUI.Box(new Rect(fromX, fromY, width, height),
                $"<size={textSize}><color=red>" +
                $"Lost connection to server." +
                $"\nRetrying... (attempt {consecutiveFailures})" +
                $"\nTimeout: {Mathf.FloorToInt(timeSinceContact)}" +
                $"</color></size>");
        }
        private void MeChanged(NetworkData.Player? me)
        {
            if (me == null) state = State.INACTIVE;
            else state = State.WAITING;
        }
    }
}

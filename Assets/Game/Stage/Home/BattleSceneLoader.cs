using Lion.Player;
using Lion.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.Home
{
    public class BattleSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName = "";

        private bool _isFadeOutInProgress = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // トリガーに触れた相手がプレイヤーでない場合は処理を終了。
            if (collision.gameObject != PlayerController.Instance.gameObject)
            {
                return;
            }

            // すでにフェードアウト処理が進行中の場合は処理を終了。
            if (_isFadeOutInProgress)
            {
                return;
            }

            // プレイヤーが戦闘に参加可能かどうかを確認し、不可能な場合は処理を終了。
            if (!CombatEligibilityChecker.IsEligibleForCombat)
            {
                // var message = GameMessagePool.Instance.Get();
                var playerPosition = PlayerController.Instance.transform.position;
                var offset = new Vector3(0, 1, 0);
                var message = ActorMessagePool.Instance.Get(playerPosition + offset);
                message.Text = "Hello";
                message.Color = Color.red;
                return;
            }

            _isFadeOutInProgress = true;
            ScreenFader.Instance.FadeIn(onComplete: LoadScene);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
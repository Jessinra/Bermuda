using System;
using System.Collections;
using UnityEngine;

enum AIBattleState {
    ENGAGE,
    AVOID,
    IDLE,
}

public class AIBattle : MonoBehaviour {

    [SerializeField] private Vector2 aimDelay = new Vector2(0.1F, 0.3F);
    [SerializeField] private AISkillConfig skillConfig = null;

    private Player player;
    private GameObject targetPlayer;

    private AIBattleState state = AIBattleState.IDLE;

    private Vector2 direction;
    private Vector2 offset;
    private Vector2 targetPosition;

    private bool shotReady = true;
    private bool skillReady = true;
    private bool shieldReady = true;
    private bool boostReady = true;

    void Start() {
        this.player = this.transform.parent.transform.parent.gameObject.GetComponent<Player>();
        this.targetPosition = (Vector2) this.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = other.gameObject;

            StartCoroutine(CheckSelfCondition());
            StartCoroutine(ExecuteScript());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = null;
            this.state = AIBattleState.IDLE;

            StopCoroutine(CheckSelfCondition());
            StopCoroutine(ExecuteScript());
        }
    }

    IEnumerator ExecuteScript() {
        while (this.state != AIBattleState.IDLE && targetPlayer != null) {

            if (this.state == AIBattleState.ENGAGE) {

                this.EngageTarget();
                float reAimDelay = UnityEngine.Random.Range(aimDelay.x, aimDelay.y);
                yield return new WaitForSeconds(reAimDelay);
            
            } else if (this.state == AIBattleState.AVOID) {

                this.AvoidTarget();
                yield return new WaitForSeconds(0.2F);
            }
        }

        if (targetPlayer == null){
            this.state = AIBattleState.IDLE;
        }

        yield break;
    }

    void EngageTarget() {

        if (shotReady) {

            // Aim
            this.targetPosition = (Vector2) this.targetPlayer.transform.position;
            this.offset = this.targetPosition - (Vector2) this.transform.position;
            this.direction = Vector2.ClampMagnitude(offset, 0.15f);

            if (Math.Abs(this.direction.y) < 0.2F) {
                player.CreateDefaultShot();
                StartCoroutine(ShotCooldown());
            }
        }
    }

    void AvoidTarget() {

        if (shieldReady) {
            StartCoroutine(ActivateShield());
            StartCoroutine(ShieldCooldown());
        }

        if (boostReady) {
            StartCoroutine(ActivateBoost());
            StartCoroutine(BoostCooldown());
        }
    }

    IEnumerator ActivateShield() {
        this.player.ActivateShield();
        yield return new WaitForSeconds(this.skillConfig.shieldDuration);
        this.player.DeactivateShield();
        yield break;
    }

    IEnumerator ActivateBoost() {
        this.player.ActivateBoost(this.skillConfig.boostMultiplier);
        yield return new WaitForSeconds(this.skillConfig.boostDuration);
        this.player.DeactivateBoost(this.skillConfig.boostMultiplier);
        yield break;
    }

    IEnumerator CheckSelfCondition() {
        while (this.player != null) {
            if (this.player.GetHP() < 25) {
                this.state = AIBattleState.AVOID;
            } else {
                this.state = AIBattleState.ENGAGE;
            }
            yield return new WaitForSeconds(0.4F);
        }
        yield break;
    }

    IEnumerator ShotCooldown() {
        this.shotReady = false;
        yield return new WaitForSeconds(this.skillConfig.shotCooldown);
        this.shotReady = true;
    }

    IEnumerator SkillCooldown() {
        this.skillReady = false;
        yield return new WaitForSeconds(this.skillConfig.skillCooldown);
        this.skillReady = true;
    }

    IEnumerator ShieldCooldown() {
        this.shieldReady = false;
        yield return new WaitForSeconds(this.skillConfig.shieldCooldown);
        this.shieldReady = true;
    }

    IEnumerator BoostCooldown() {
        this.boostReady = false;
        yield return new WaitForSeconds(this.skillConfig.boostCooldown);
        this.boostReady = true;
    }
}

[System.Serializable]
public class AISkillConfig {
    public float shotCooldown = 0.25F;
    public float skillCooldown = 3.0F;
    public float shieldCooldown = 8.0F;
    public float boostCooldown = 8.0F;

    public float shieldDuration = 3.0F;
    public float boostDuration = 3.0F;
    public float boostMultiplier = 2F;
}
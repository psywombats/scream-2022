using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MonsterController : MonoBehaviour {

    private Rigidbody body;
    public Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    public const int MinDist = 6;
    public const int MaxDist = 30;
    public const int IdealDist = 18;
    public const float MinV = 1f;
    public const float MaxV = 2.5f;
    public const float CatchupV = 4f;
    public const float a = 0.25f;
    public const float RampTime = 8f;
    public const float DeathTime = 3f;

    private float deathMult;
    private float soundMult;
    private float v;

    public bool Unbound { get; set; }
    public bool Dying { get; set; }

    [SerializeField] private StudioEventEmitter triangle;
    [SerializeField] private StudioEventEmitter monster;

    private Bus monBus;
    private Bus bgmBus;
    private CorridorController corridor;

    private HashSet<PanelLightComponent> killedLights = new HashSet<PanelLightComponent>();

    public void Start() {
        corridor = FindObjectOfType<CorridorController>();
        v = MinV;

        monBus = RuntimeManager.GetBus("bus:/MON");
        bgmBus = RuntimeManager.GetBus("bus:/BGM");

        monster.Play();
        triangle.Play();
    }

    public void Update() {
       foreach (var light in corridor.allLights) {
            if (!light.IsShutDown && light.transform.position.x > transform.position.x && !killedLights.Contains(light)) {
                StartCoroutine(KillLightRoutine(light));
                killedLights.Add(light);
            }
        }
        var r = 1f - ((DistToAvatar() - MinDist) / (MaxDist - MinDist));
        if (Dying) {
            soundMult += -1 * Time.deltaTime / DeathTime;
            deathMult += Time.deltaTime;
            deathMult += Time.deltaTime / DeathTime;
            if (deathMult > 1f) {
                deathMult = 1f;
            }
        } else {
            soundMult += Time.deltaTime / RampTime;
        }
        if (soundMult > 1f) soundMult = 1f;
        AvatarEvent.Instance.PreventApproach = Unbound ? r *r : r;
        AvatarEvent.Instance.LookAway = 1f - ((DistToAvatar() - MinDist) / (MaxDist - MinDist));
        AvatarEvent.Instance.CantLook = Unbound ? r : soundMult;
        
        if (!Dying) {
            if (DistToAvatar() > IdealDist) {
                v += a;
            }
            else {
                v -= a;
            }
            v = Mathf.Clamp(v, MinV, DistToAvatar() < MaxDist ? CatchupV : MaxV);
            Body.velocity = new Vector3(Dying ? 0f : (-1f * v), 0f, 0f);
        }

        var ugliness = AvatarEvent.Instance.TowardsBeastLook() * soundMult * AvatarEvent.Instance.PreventApproach;
        bgmBus.setVolume((1f - ugliness) * (1f - deathMult));
        monBus.setVolume(deathMult < 1f ? Mathf.Min(1f, ugliness + deathMult) : 0f);
    }

    public void OnDisable() {
        AvatarEvent.Instance.PreventApproach = 0f;
        AvatarEvent.Instance.LookAway = 0f;
        AvatarEvent.Instance.CantLook = 0;
        bgmBus.setVolume(1f);
        monBus.setVolume(0f);
        triangle.Stop();
        monster.Stop();
        monBus.stopAllEvents(STOP_MODE.IMMEDIATE);
        AudioManager.Instance.StopSFX();
    }

    public float DistToAvatar() {
        return Mathf.Abs(AvatarEvent.Instance.transform.position.x - transform.position.x);
    }

    private IEnumerator KillLightRoutine(PanelLightComponent light) {
        if (!Dying) {
            yield return CoUtils.Wait(Random.Range(0f, .3f));
            light.IsShutDown = true;
            light.PlayBootupSFX();
        }
    }
}

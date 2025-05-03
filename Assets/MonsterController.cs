using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class MonsterController : MonoBehaviour
{
    #region enum
    public enum MovementMode
    {
        Default,
        PlayerChase
    }

    public enum MonsterType
    {
        Normal,
        Screamer,
        Stalker
    }
    #endregion

    [Header("에이전트")]
    public NavMeshAgent agent;
    public LayerMask targetLayer;
    public PlayerController[] detectedPlayers;
    public GameObject target;
    Collider detectionCollider;
    public MovementMode movementMode;
    public float reconRange;
    public Vector3 Destination;
    public float waitTime;

    //Sound
    public AudioSource IdleSoundSource;
    public AudioClip[] IdleSoundClips;

    [Space(10)]
    public MonsterType type;

    [Header("Normal")]
    float timer = 0;

    [Space(10)]
    [Header("Screamer")]
    public AudioSource AS_Source;
    public AudioClip screamClip;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Destination = GetRandomPosition();

        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.GetComponent<SphereCollider>().radius = reconRange / 2;

        IdleSoundSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {


        detectedPlayers = GetOnSightPlayer(GameManager.AllPlayers);

        if (detectedPlayers.Length > 0)
        {
            target = GetNearestPlayer(detectedPlayers).gameObject;
            movementMode = (target != null) ? MovementMode.PlayerChase : MovementMode.Default;
        }
        else
        {
            movementMode = MovementMode.Default;
            target = null;
        }

        switch (type)
        {
            case MonsterType.Normal:
                NormalTypeBehavior();
                break;

            case MonsterType.Screamer:
                ScreamerTypeBehavior();
                // 스크리머 타입 행동 추가 예정
                break;
        }
    }

    private void NormalTypeBehavior()
    {
        #region 기본상태 소리
        if (IdleSoundSource.isPlaying == false)
        {
            IdleSoundSource.clip = IdleSoundClips[Random.Range(0, IdleSoundClips.Length)];
            IdleSoundSource.Play();
        }
        #endregion

        switch (movementMode)
        {
            case MovementMode.Default:
                if (agent.remainingDistance < 0.2f)
                {
                    if (timer >= waitTime)
                    {
                        Destination = GetRandomPosition();
                        timer = 0;
                        agent.SetDestination(Destination);
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
                else
                {
                    agent.SetDestination(Destination);
                }
                break;

            case MovementMode.PlayerChase:
                if (target != null && NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, 50f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                break;
        }
    }

    private void ScreamerTypeBehavior()
    {
        #region 기본상태 소리
        if (IdleSoundSource.isPlaying == false && !AS_Source.isPlaying)
        {
            IdleSoundSource.clip = IdleSoundClips[Random.Range(0, IdleSoundClips.Length)];
            IdleSoundSource.Play();
        }

        else
        {
            IdleSoundSource.Stop();
        }
        #endregion

        switch (movementMode)
        {
            case MovementMode.Default:
                if (AS_Source.isPlaying) AS_Source.Stop();

                if (agent.remainingDistance < 0.2f)
                {
                    if (timer >= waitTime)
                    {
                        Destination = GetRandomPosition();
                        timer = 0;
                        agent.SetDestination(Destination);
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
                else
                {
                    agent.SetDestination(Destination);
                }
                break;

            case MovementMode.PlayerChase:
                if (target != null && NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, 50f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    if(AS_Source == null)
                    {
                        AS_Source = gameObject.AddComponent<AudioSource>();
                    }

                    if (AS_Source.isPlaying == false)
                    {
                        AS_Source.clip = screamClip;
                        AS_Source.Play();
                    }

                }
                break;
        }
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * reconRange;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 20f, NavMesh.AllAreas))
        {
            randomPos = hit.position;
        }

        return randomPos;
    }

    public PlayerController[] GetOnSightPlayer(PlayerController[] players)
    {
        List<PlayerController> result = new List<PlayerController>();
        Collider[] cols = Physics.OverlapSphere(transform.position, reconRange, targetLayer);

        for (int i = 0; i < cols.Length; ++i)
        {
            Vector3 dir = (cols[i].transform.position - transform.position).normalized;
            float distance = Vector3.Distance(cols[i].transform.position, transform.position);

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance) == false)
            {

                result.Add(cols[i].transform.GetComponent<PlayerController>());
                Debug.DrawRay(transform.position, dir * distance, Color.red);

            }
        }

        return result.ToArray();
    }

    public PlayerController GetNearestPlayer(PlayerController[] players)
    {
        PlayerController closestPc = null;
        float minDist = Mathf.Infinity;

        foreach (PlayerController player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                closestPc = player;
                minDist = dist;
            }
        }

        return closestPc;
    }
}

#if UNITY_EDITOR


[CustomEditor(typeof(MonsterController))]
public class MonsterControllerEditor : Editor
{
    SerializedProperty typeProp;
    SerializedProperty reconRangeProp;
    SerializedProperty targetLayerProp;
    SerializedProperty movementModeProp;
    SerializedProperty waitTimeProp;
    SerializedProperty idleSoundClipsProp;
    SerializedProperty screamClipProp;
    SerializedProperty agentProp;
    SerializedProperty targetProp;

    void OnEnable()
    {
        typeProp = serializedObject.FindProperty("type");
        reconRangeProp = serializedObject.FindProperty("reconRange");
        targetLayerProp = serializedObject.FindProperty("targetLayer");
        movementModeProp = serializedObject.FindProperty("movementMode");
        waitTimeProp = serializedObject.FindProperty("waitTime");
        idleSoundClipsProp = serializedObject.FindProperty("IdleSoundClips");
        screamClipProp = serializedObject.FindProperty("screamClip");
        agentProp = serializedObject.FindProperty("agent");
        targetProp = serializedObject.FindProperty("target");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("몬스터 설정", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.PropertyField(reconRangeProp);
        DrawLayerMaskField(targetLayerProp, "Target Layer");
        EditorGUILayout.PropertyField(movementModeProp);
        EditorGUILayout.PropertyField(waitTimeProp);

        EditorGUILayout.PropertyField(idleSoundClipsProp, new GUIContent("Idle Sound Clips"), true);

        EditorGUILayout.Space(10);

        // 타입별 세부 설정
        var type = (MonsterController.MonsterType)typeProp.enumValueIndex;
        switch (type)
        {
            case MonsterController.MonsterType.Normal:
                EditorGUILayout.LabelField("Normal 타입 설정", EditorStyles.boldLabel);
                break;

            case MonsterController.MonsterType.Screamer:
                EditorGUILayout.LabelField("Screamer 타입 설정", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(screamClipProp, new GUIContent("Scream Clip"));
                break;

            case MonsterController.MonsterType.Stalker:
                EditorGUILayout.LabelField("Stalker 타입 설정", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Stalker 설정을 여기에 추가하세요.", MessageType.Info);
                break;
        }

        EditorGUILayout.Space(10);

        // 디버그용 필드
        EditorGUILayout.PropertyField(agentProp, new GUIContent("Agent (Auto-Assigned)"));
        EditorGUILayout.PropertyField(targetProp, new GUIContent("Current Target"));

        serializedObject.ApplyModifiedProperties();
    }

    void DrawLayerMaskField(SerializedProperty property, string label)
    {
        LayerMask tempMask = EditorGUILayout.MaskField(label, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(property.intValue), InternalEditorUtility.layers);
        property.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
    }
}
#endif



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy")]

public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float EnemyChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }

    [field: SerializeField] public int Damage;

    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }

    [field: SerializeField] public int ExperiencePoints = 10; // 기본값을 10으로 설정
}

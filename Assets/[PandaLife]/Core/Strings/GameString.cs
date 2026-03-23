using UnityEngine;

[CreateAssetMenu(fileName = "GameString", menuName = "Strings/GameString")]
public class GameString : ScriptableObject
{
    [SerializeField] string m_value;

    public string Value { get => m_value; }
}

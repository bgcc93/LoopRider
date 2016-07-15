using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour {
    public bool bounceOff;
    public float colRecoveryTime = 0.25f;
    public float nextCol = 0;
    public int bounceOffSide = 0;
	public abstract void TakeDamage(float dam, bool hardHit);
    public abstract void EmergencyTurn(int side);
    public abstract void Turn(int side);
    public abstract void TurnWithFactor(int side, float factor);
}

using UnityEngine;

[RequireComponent(typeof(Animator))]

public class IK : MonoBehaviour
{
    protected Animator avatar;

    public Transform bodyObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform lookAtObj = null;

    public float leftFootWeightPosition = 1.0f;
    public float leftFootWeightRotation = 1.0f;

    public float rightFootWeightPosition = 1.0f;
    public float rightFootWeightRotation = 1.0f;

    public float leftHandWeightPosition = 1.0f;
    public float leftHandWeightRotation = 1.0f;

    public float rightHandWeightPosition = 1.0f;
    public float rightHandWeightRotation = 1.0f;

    public float lookAtWeight = 1.0f;

    void Start()
    {
        avatar = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!avatar) return;

        avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeightPosition);
        avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeightRotation);

        avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeightPosition);
        avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeightRotation);

        avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeightPosition);
        avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeightRotation);

        avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeightPosition);
        avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeightRotation);

        avatar.SetLookAtWeight(lookAtWeight, 0.3f, 0.6f, 1.0f, 0.5f);

        if (bodyObj != null)
        {
            avatar.bodyPosition = bodyObj.position;
            avatar.bodyRotation = bodyObj.rotation;
        }

        if (leftFootObj != null)
        {
            avatar.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
            avatar.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
        }

        if (rightFootObj != null)
        {
            avatar.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
            avatar.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
        }

        if (leftHandObj != null)
        {
            avatar.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
            avatar.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
        }

        if (rightHandObj != null)
        {
            avatar.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
            avatar.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
        }

        if (lookAtObj != null)
        {
            avatar.SetLookAtPosition(lookAtObj.position);
        }
    }
}

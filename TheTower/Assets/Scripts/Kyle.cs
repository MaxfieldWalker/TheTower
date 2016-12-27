using UnityEngine;

namespace Assets.Scripts
{
    public class Kyle
    {
        private GameObject wristBone_L;
        private GameObject wristBone_R;

        public PlayerControlMode currentMode { get; set; }

        public Kyle(GameObject wristBone_L, GameObject wristBone_R)
        {
            this.wristBone_L = wristBone_L;
            this.wristBone_R = wristBone_R;
            this.currentMode = PlayerControlMode.MoveLeftHand;
        }

        public void moveHandUp()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.wristBone_L.transform.Rotate(new Vector3(0, 0, -1f));
            if (currentMode == PlayerControlMode.MoveRightHand) this.wristBone_R.transform.Rotate(new Vector3(0, 0, -1f));
        }

        public void moveHandBottom()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.wristBone_L.transform.Rotate(new Vector3(0, 0, 1f));
            if (currentMode == PlayerControlMode.MoveRightHand) this.wristBone_R.transform.Rotate(new Vector3(0, 0, 1f));
        }

        public void moveHandRight()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.wristBone_L.transform.Rotate(new Vector3(0, 1f, 0f));
            if (currentMode == PlayerControlMode.MoveRightHand) this.wristBone_R.transform.Rotate(new Vector3(0, 1f, 0f));
        }

        public void moveHandLeft()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.wristBone_L.transform.Rotate(new Vector3(0, -1f, 0f));
            if (currentMode == PlayerControlMode.MoveRightHand) this.wristBone_R.transform.Rotate(new Vector3(0, -1f, 0f));
        }
    }
}

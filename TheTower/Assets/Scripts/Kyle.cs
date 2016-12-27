using UnityEngine;

namespace Assets.Scripts
{
    public class Kyle
    {
        private GameObject wristBone_L;
        private GameObject wristBone_R;
        private GameObject forearmBone_L;
        private GameObject forearmBone_R;
        private GameObject kyle;

        public PlayerControlMode currentMode { get; set; }

        public Kyle(
            GameObject wristBone_L,
            GameObject wristBone_R,
            GameObject forearmBone_L,
            GameObject forearmBone_R,
            GameObject kyle)
        {
            this.wristBone_L = wristBone_L;
            this.wristBone_R = wristBone_R;
            this.forearmBone_L = forearmBone_L;
            this.forearmBone_R = forearmBone_R;
            this.kyle = kyle;
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

        /// <summary>
        /// 脇を締める
        /// </summary>
        public void tightenSide()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.forearmBone_L.transform.Rotate(new Vector3(0, 1.0f, 0));
            if (currentMode == PlayerControlMode.MoveRightHand) this.forearmBone_R.transform.Rotate(new Vector3(0, -1f, 0f));
        }

        /// <summary>
        /// 脇を緩める
        /// </summary>
        public void looseSide()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand) this.forearmBone_L.transform.Rotate(new Vector3(0, 1.0f, 0));
            if (currentMode == PlayerControlMode.MoveRightHand) this.forearmBone_R.transform.Rotate(new Vector3(0, -1f, 0f));
        }

        public void moveBodyUp()
        {
            this.kyle.transform.position += new Vector3(0, 0.1f, 0);
        }

        public void moveBodyDown()
        {
            this.kyle.transform.position += new Vector3(0, -0.1f, 0);
        }
    }
}

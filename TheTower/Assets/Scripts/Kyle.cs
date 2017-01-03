using UnityEngine;

namespace Assets.Scripts
{
    public class Kyle : MonoBehaviour
    {
        public Transform bodyObj = null;
        public Transform leftFootObj = null;
        public Transform rightFootObj = null;
        public Transform leftHandObj = null;
        public Transform rightHandObj = null;
        public Transform lookAtObj = null;

        public PlayerControlMode currentMode { get; set; }

        private const float MoveAmount = 0.1f;
        private bool leftFootLocked = false;
        private bool rightFootLocked = false;
        private bool leftHandLocked = false;
        private bool rightHandLocked = false;

        private void Start()
        {
            this.currentMode = PlayerControlMode.MoveLeftHand;
        }

        public void moveLimbUp()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand && canMoveLeftHandTo(MoveDirection.Up))
            {
                this.leftHandObj.transform.Translate(new Vector3(0, MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightHand && canMoveRightHandTo(MoveDirection.Up))
            {
                this.rightHandObj.transform.Translate(new Vector3(0, MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveLeftFoot && canMoveLeftFootTo(MoveDirection.Up))
            {
                this.leftFootObj.transform.Translate(new Vector3(0, MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightFoot && canMoveRightFootTo(MoveDirection.Up))
            {
                this.rightFootObj.transform.Translate(new Vector3(0, MoveAmount, 0f));
            }
        }

        public void moveLimbDown()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand && canMoveLeftHandTo(MoveDirection.Down))
            {
                this.leftHandObj.transform.Translate(new Vector3(0, -MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightHand && canMoveRightHandTo(MoveDirection.Down))
            {
                this.rightHandObj.transform.Translate(new Vector3(0, -MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveLeftFoot && canMoveLeftFootTo(MoveDirection.Down))
            {
                this.leftFootObj.transform.Translate(new Vector3(0, -MoveAmount, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightFoot && canMoveRightFootTo(MoveDirection.Down))
            {
                this.rightFootObj.transform.Translate(new Vector3(0, -MoveAmount, 0f));
            }
        }

        public void moveLimbRight()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand && canMoveLeftHandTo(MoveDirection.Right))
            {
                this.leftHandObj.transform.Translate(new Vector3(MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightHand && canMoveRightHandTo(MoveDirection.Right))
            {
                this.rightHandObj.transform.Translate(new Vector3(MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveLeftFoot && canMoveLeftFootTo(MoveDirection.Right))
            {
                this.leftFootObj.transform.Translate(new Vector3(MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightFoot && canMoveRightFootTo(MoveDirection.Right))
            {
                this.rightFootObj.transform.Translate(new Vector3(MoveAmount, 0f, 0f));
            }
        }

        public void moveLimbLeft()
        {
            if (currentMode == PlayerControlMode.MoveLeftHand && canMoveLeftHandTo(MoveDirection.Left))
            {
                this.leftHandObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightHand && canMoveRightHandTo(MoveDirection.Left))
            {
                this.rightHandObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveLeftFoot && canMoveLeftFootTo(MoveDirection.Left))
            {
                this.leftFootObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0f));
            }
            if (currentMode == PlayerControlMode.MoveRightFoot && canMoveRightFootTo(MoveDirection.Left))
            {
                this.rightFootObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0f));
            }
        }

        public void moveBodyUp()
        {
            if (canMoveBodyTo(MoveDirection.Up))
            {
                this.bodyObj.transform.Translate(new Vector3(0, MoveAmount, 0));
                this.lookAtObj.transform.Translate(new Vector3(0, MoveAmount, 0));
            }
        }

        public void moveBodyDown()
        {
            if (canMoveBodyTo(MoveDirection.Down))
            {
                this.bodyObj.transform.Translate(new Vector3(0, -MoveAmount, 0));
                this.lookAtObj.transform.Translate(new Vector3(0, -MoveAmount, 0));
            }
        }

        public void moveBodyLeft()
        {
            if (canMoveBodyTo(MoveDirection.Left))
            {
                this.bodyObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0));
                this.lookAtObj.transform.Translate(new Vector3(-MoveAmount, 0f, 0));
            }
        }

        public void moveBodyRight()
        {
            if (canMoveBodyTo(MoveDirection.Right))
            {
                this.bodyObj.transform.Translate(new Vector3(MoveAmount, 0f, 0));
                this.lookAtObj.transform.Translate(new Vector3(MoveAmount, 0f, 0));
            }
        }

        /// <summary>
        /// 左手を動かせる条件
        /// 右手と少なくとも1つの足がロック状態である
        /// </summary>
        /// <returns></returns>
        private bool canMoveLeftHandTo(MoveDirection direction)
        {
            if (overDistance(this.leftHandObj, this.bodyObj, direction, 2.5f)) return false;

            return rightHandLocked && (leftFootLocked || rightFootLocked);
        }

        /// <summary>
        /// 右手を動かせる条件
        /// 左手と少なくとも1つの足がロック状態である
        /// </summary>
        /// <returns></returns>
        private bool canMoveRightHandTo(MoveDirection direction)
        {
            if (overDistance(this.rightHandObj, this.bodyObj, direction, 2.5f)) return false;

            return leftHandLocked && (leftFootLocked || rightFootLocked);
        }

        private bool overDistance(Transform goal, Transform root, MoveDirection direction, float threshold)
        {
            Vector3 addition = Vector3.zero;
            switch (direction)
            {
                case MoveDirection.Up:
                    addition = new Vector3(0f, MoveAmount, 0f);
                    break;
                case MoveDirection.Right:
                    addition = new Vector3(MoveAmount, 0f);
                    break;
                case MoveDirection.Down:
                    addition = new Vector3(0f, -MoveAmount, 0f);
                    break;
                case MoveDirection.Left:
                    addition = new Vector3(-MoveAmount, 0f, 0f);
                    break;
                default:
                    break;
            }

            // オブジェクト間の距離を求める
            float dist = Vector3.Distance(goal.transform.position + addition, root.transform.position);
            if (dist > threshold) return true;

            return false;
        }

        /// <summary>
        /// 左足を動かせる条件
        /// 両手または右足と少なくとも1つの手がロック状態である
        /// </summary>
        /// <returns></returns>
        private bool canMoveLeftFootTo(MoveDirection direction)
        {
            if (overDistance(this.leftFootObj, this.bodyObj, direction, 3.0f)) return false;

            return (leftHandLocked && rightHandLocked)
                || (rightFootLocked && (leftHandLocked || rightHandLocked));
        }

        /// <summary>
        /// 右足を動かせる条件
        /// 両手または左足と少なくとも1つの手がロック状態である
        /// </summary>
        /// <returns></returns>
        private bool canMoveRightFootTo(MoveDirection direction)
        {
            if (overDistance(this.rightFootObj, this.bodyObj, direction, 3.0f)) return false;

            return (leftHandLocked && rightHandLocked)
                || (leftFootLocked && (leftHandLocked || rightHandLocked));
        }


        /// <summary>
        /// ボディを動かせる条件
        /// 両手または手足それぞれ少なくとも1つがロック状態である
        /// </summary>
        /// <returns></returns>
        private bool canMoveBodyTo(MoveDirection direction)
        {
            if (overDistance(this.bodyObj, this.leftFootObj, direction, 3.0f)) return false;

            return (leftHandLocked && rightHandLocked)
                || ((leftHandLocked || rightHandLocked) && (leftFootLocked || rightFootLocked));
        }

        public void delegateOnTriggerEnter(GameObject self, Collider other)
        {
            Debug.Log("trigger enter: " + self.name);

            if (self.name == "Left_Foot_Collider")
            {
                leftFootLocked = true;
            }
            if (self.name == "Right_Foot_Collider")
            {
                rightFootLocked = true;
            }
            if (self.name == "Left_Wrist_Collider")
            {
                leftHandLocked = true;
            }
            if (self.name == "Right_Wrist_Collider")
            {
                rightHandLocked = true;
            }
        }


        public void delegateOnTriggerExit(GameObject self, Collider other)
        {
            Debug.Log("trigger exit: " + self.name);

            if (self.name == "Left_Foot_Collider")
            {
                leftFootLocked = false;
            }
            if (self.name == "Right_Foot_Collider")
            {
                rightFootLocked = false;
            }
            if (self.name == "Left_Wrist_Collider")
            {
                leftHandLocked = false;
            }
            if (self.name == "Right_Wrist_Collider")
            {
                rightHandLocked = false;
            }
        }
    }

    enum MoveDirection
    {
        Up,
        Right,
        Down,
        Left
    }
}

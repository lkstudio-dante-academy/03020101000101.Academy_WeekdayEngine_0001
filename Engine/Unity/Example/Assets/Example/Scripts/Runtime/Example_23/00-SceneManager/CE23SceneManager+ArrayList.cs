using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if E23_ARRAY_LIST
/** Example 23 - 배열 리스트 */
public partial class CE23SceneManager : CSceneManager {
	#region 변수
	private CArrayList<int> m_oValList = new CArrayList<int>();
	#endregion // 변수

	#region 함수
	/** 추가 버튼을 눌렀을 경우 */
	public void OnTouchAddBtn() {
		m_oValList.Add(this.GetVal());
		this.SetIsDirtyUpdateState(true);
	}

	/** 추가 버튼을 눌렀을 경우 */
	public void OnTouchInsertBtn() {
		m_oValList.Insert(this.GetIdx(), this.GetVal());
		this.SetIsDirtyUpdateState(true);
	}

	/** 제거 버튼을 눌렀을 경우 */
	public void OnTouchRemoveBtn() {
		m_oValList.Remove(this.GetVal());
		this.SetIsDirtyUpdateState(true);
	}

	/** 제거 버튼을 눌렀을 경우 */
	public void OnTouchRemoveAtBtn() {
		m_oValList.RemoveAt(this.GetIdx());
		this.SetIsDirtyUpdateState(true);
	}

	/** 상태를 갱신한다 */
	protected override void UpdateState() {
		base.UpdateState();

		int nNumVals = Mathf.Max(m_oValList.Count, 
			m_oArrayListTargetRoot.transform.childCount);

		for(int i = 0; i < nNumVals; ++i) {
			CE23Target oTarget = null;

			// 타겟이 존재 할 경우
			if(i < m_oArrayListTargetRoot.transform.childCount) {
				oTarget = m_oArrayListTargetRoot.transform.GetChild(i).GetComponent<CE23Target>();
			} else {
				oTarget = CFactory.CreateCloneGameObj<CE23Target>("Target",
					m_oArrayListOriginTarget, m_oArrayListTargetRoot, Vector3.zero, Vector3.one, Vector3.zero);
			}

			oTarget?.gameObject.SetActive(i < m_oValList.Count);
		}
	}
	#endregion // 함수
}
#endif // #if E23_ARRAY_LIST

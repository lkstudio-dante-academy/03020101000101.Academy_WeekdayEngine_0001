using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/** 네트워트 관리자 - 서버 */
public partial class CE20NetworkManager : CSingleton<CE20NetworkManager> {
	/** 매칭 정보 */
	private record STMatchingInfo {
		public Socket m_oPlayer01;
		public Socket m_oPlayer02;
	}

	#region 변수
	private Socket m_oServerSocket = null;
	private List<STMatchingInfo> m_oMatchingInfoList = new List<STMatchingInfo>();
	#endregion // 변수

	#region 함수
	/** 서버 소켓을 구동시킨다 */
	public void RunServerSocket() {
#if UNITY_EDITOR
		StartCoroutine(this.CoRunServerSocket());
#endif // #if UNITY_EDITOR
	}

	/** 매칭 응답을 전송한다 */
	private void SendMatchingResponse(STMatchingInfo a_stMatchingInfo) {
		var stPacketInfo = new STPacketInfo() {
			m_eType = E20PacketType.MATCHING_RESPONSE
		};

		this.SendPacketInfo(a_stMatchingInfo.m_oPlayer01, stPacketInfo);
		this.SendPacketInfo(a_stMatchingInfo.m_oPlayer02, stPacketInfo);
	}

	/** 매칭 정보를 탐색한다 */
	private STMatchingInfo FindMatchingInfo() {
		STMatchingInfo stMatchingInfo = null;

		// 매칭 정보가 없을 경우
		if(m_oMatchingInfoList.Count <= 0) {
			stMatchingInfo = new STMatchingInfo();
			m_oMatchingInfoList.Add(stMatchingInfo);
		} else {
			stMatchingInfo = m_oMatchingInfoList.Last();

			// 플레이 중인 매칭 정보 일 경우
			if(stMatchingInfo.m_oPlayer01 != null && stMatchingInfo.m_oPlayer02 != null) {
				stMatchingInfo = new STMatchingInfo();
				m_oMatchingInfoList.Add(stMatchingInfo);
			}
		}

		return stMatchingInfo;
	}

	/** 서버 소켓을 구동시킨다 */
	private IEnumerator CoRunServerSocket() {
		m_oServerSocket = new Socket(AddressFamily.InterNetwork,
			SocketType.Stream, ProtocolType.Tcp);

		m_oServerSocket.Bind(new IPEndPoint(IPAddress.Any, 9080));
		m_oServerSocket.Listen(byte.MaxValue);

		do {
			yield return null;

			// 연결 요청이 존재 할 경우
			if(m_oServerSocket.Poll(0, SelectMode.SelectRead)) {
				var oSocket = m_oServerSocket.Accept();
				var stMatchingInfo = this.FindMatchingInfo();

				// 매칭 대기 플레이어가 없을 경우
				if(stMatchingInfo.m_oPlayer01 == null) {
					stMatchingInfo.m_oPlayer01 = oSocket;
				} else {
					stMatchingInfo.m_oPlayer02 = oSocket;
				}

				// 매칭이 완료 되었을 경우
				if(stMatchingInfo.m_oPlayer01 != null && stMatchingInfo.m_oPlayer02 != null) {
					this.SendMatchingResponse(stMatchingInfo);
				}
			}
		} while(true);
	}
	#endregion // 함수
}

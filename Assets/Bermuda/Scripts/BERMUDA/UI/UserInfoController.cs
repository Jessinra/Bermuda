using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoController : MonoBehaviour {

	[SerializeField] private Slider sliderHP = null;
	[SerializeField] private Slider sliderFuel = null;

	[SerializeField] private List<Sprite> characterThumbnail = new List<Sprite>();

	[SerializeField] private Text uiPlayerName = null;
	[SerializeField] private Image uiPlayerThumbnail = null;

	private Player playerScript = null;

	void Start() {
		StartCoroutine(SetupUserProfile());
	}

	IEnumerator SetupUserProfile() {

		while (playerScript == null) {
			yield return new WaitForSeconds(0.1F);
			playerScript = this.transform.parent.gameObject.GetComponent<Player>();
		}

		uiPlayerName.text = playerScript.GetUsername();
		uiPlayerThumbnail.sprite = characterThumbnail[playerScript.GetPlayerType() - 1];
		
		StartCoroutine(GetUserStatus());
		yield break;
	}

	IEnumerator GetUserStatus() {

		while (playerScript != null) {
			sliderHP.value = playerScript.GetHP();
			sliderFuel.value = playerScript.GetFuel();

			yield return new WaitForSeconds(0.2F);
		}

		yield break;
	}
}
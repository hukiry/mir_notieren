using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PromptBox : UIViewMono<PromptBox>
{
	private GameObject okBtn;
	private GameObject cancelBtn;
	private Text contentTxt;
	private Text titleTxt;

	private Action okAactionCall = null, cancelAactionCall = null;
	private void Awake()
	{
		okBtn = transform.Find("horizontal/ok").gameObject;
		cancelBtn = transform.Find("horizontal/cancel").gameObject;
		contentTxt = transform.Find("back/content").GetComponent<Text>();
	}

	private void Start()
	{
		UIEventListener.Get(okBtn).onClick = o =>
		{
			this.gameObject.SetActive(false);
			okAactionCall?.Invoke();
		};

		UIEventListener.Get(cancelBtn).onClick = o =>
		{
			this.gameObject.SetActive(false);
			cancelAactionCall?.Invoke();
		};
	}

	public void Show(int id, Action okAactionCall, Action cancelAactionCall)
    {
        this.gameObject.SetActive(true);
     
		contentTxt.text = this.GetMultiText(id);
		okBtn.SetActive(okAactionCall != null);
		cancelBtn.SetActive(cancelAactionCall != null);
		this.okAactionCall = okAactionCall;
		this.cancelAactionCall = cancelAactionCall;

		this.transform.SetAsFirstSibling();
	}

	Dictionary<string, Dictionary<int, string>> lanDic = new Dictionary<string, Dictionary<int, string>>() {
		//简体
		{"cn",new Dictionary<int, string> (){
				{ 1, "下载失败,请检查网络后重试！" },
				{ 2, "网络不稳定,请检查网络后重试！" },
				{ 3, "请更新新包，获取更多的精彩！" }
			}
		},
		//繁体
		{"hk",new Dictionary<int, string>()
			{
				{ 1, "下載失敗,請檢查網絡後重試！" },
				{ 2, "網絡不穩定,請檢查網絡後重試！" },
				{ 3, "請更新新包，獲取更多的精彩！" },
			}
		},
		//英语
		{"en",new Dictionary<int, string>()
			{
				{ 1, "Download failed, please check the network and try again!" },
				{ 2, "The network is unstable, please check the network and try again!" },
				{ 3, "Please update the new package to get more wonderful things!" }
			}
		},
		//西班牙语
		{"es",new Dictionary<int, string> ()
			{
				{ 1, "Error en la descarga, verifique la red y vuelva a intentarlo !" },
				{ 2, "La red es inestable, verifique la red e inténtelo nuevamente !" },
				{ 3, "¡Actualice el nuevo paquete para obtener más emoción!" }
			}
		},
		//葡萄牙语
		{"pt",new Dictionary<int, string> ()
			{
				{ 1, "Falha no download, verifique a rede e tente novamente !" },
				{ 2, "A rede está instável, verifique a rede e tente novamente !" },
				{ 3, "Atualize o novo pacote para obter mais entusiasmo!" }
			}
		},
		//法语
		{ "fr", new Dictionary<int, string>()
			{
				{ 1, "Le téléchargement a échoué, veuillez vérifier le réseau et réessayer !" },
				{ 2, "Le réseau est instable, veuillez vérifier le réseau et réessayer !" },
				{ 3, "Veuillez mettre à jour le nouveau package pour obtenir plus de sensations !" }
			}
		},
		//德语
		{ "de", new Dictionary<int, string>()
			{
				{ 1, "Download fehlgeschlagen, bitte überprüfen Sie das Netzwerk und versuchen Sie es erneut!" },
				{ 2, "Das Netzwerk ist instabil, bitte überprüfen Sie das Netzwerk und versuchen Sie es erneut!"},
				{ 3, "Bitte aktualisieren Sie das neue Paket, um mehr Spannung zu erhalten!"}
			}
		},
		//俄语
		{ "ru",new Dictionary<int, string> ()
			{
				{ 1, "Не удалось загрузить, проверьте сеть и повторите попытку!"},
				{ 2, "Сеть нестабильна, проверьте сеть и повторите попытку!" },
				{ 3, "Пожалуйста, обновите новый пакет, чтобы получить больше удовольствия!" }
			}
		},
		//意大利语
		{ "it",new Dictionary<int, string> ()
			{
				{ 1, "Download fallito, controlla la rete e riprova!" },
				{ 2, "La rete è instabile, controlla la rete e riprova!" },
				{ 3, "Aggiorna il nuovo pacchetto per avere più entusiasmo!" }
			}
		},
		//韩语
		{"ko",new Dictionary<int, string> (){
				{ 1, "다운로드 실패, 네트워크를 확인하고 다시 시도하십시오!" },
				{ 2, "네트워크가 불안정합니다. 네트워크를 확인하고 다시 시도하십시오!" },
				{ 3, "더 멋진 것을 얻으려면 새 패키지를 업데이트하세요!" },
			}
		},
		//日语
		{"ja",new Dictionary<int, string> (){
				{ 1, "ダウンロードに失敗しました。ネットワークを確認して、もう一度お試しください!" },
				{ 2, "ネットワークが不安定です。ネットワークを確認して、もう一度お試しください!" },
				{ 3, "新しいパッケージを更新して、もっとすばらしいものを手に入れてください!" },
			}
		},
		//土耳其语
		{"tr",new Dictionary<int, string>()
			{
				{ 1, "İndirme başarısız, lütfen ağı kontrol edin ve tekrar deneyin!" },
				{ 2, "Ağ kararsız, lütfen ağı kontrol edin ve tekrar deneyin!" },
				{ 3, "Daha harika şeyler elde etmek için lütfen yeni paketi güncelleyin!" },
			}
		},
	};

	public string GetMultiText(int id)
	{
		var code = Hukiry.SDK.SdkManager.ins.getLanguageCode();
		if (!lanDic.ContainsKey(code)) code = "en";
		return lanDic[code][id];
	}

	public override void HideView()
	{
		this.gameObject.SetActive(false);
		Destroy(this.gameObject);
	}
}

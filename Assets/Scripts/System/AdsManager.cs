using UnityEngine.Advertisements;
using UnityEngine;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static AdsManager SharedInstance;

    private readonly string appStoreID = "3994942";
    private readonly string playStoreID = "3994943";

    private readonly string videoAd = "video";
    private readonly string rewardedVideoAd = "rewardedVideo";

    public bool isTestMode;

    private void Awake()
    {
        if (SharedInstance == null)
            SharedInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        InitializeAds();
    }

    void InitializeAds()
    {
        string gameID = appStoreID;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            gameID = appStoreID;
        else if (Application.platform == RuntimePlatform.Android)
            gameID = playStoreID;

        Advertisement.Initialize(gameID, isTestMode);
    }

    public void PlayVideoAd()
    {
        if (!Advertisement.IsReady(videoAd))
            return;
        Advertisement.Show(videoAd);
    }

    public void PlayRewardedVideoAd()
    {
        if (!Advertisement.IsReady(rewardedVideoAd))
            return;
        Advertisement.Show(rewardedVideoAd);
    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //throw new System.NotImplementedException();
    }
}

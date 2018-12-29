using UnityEngine;

public class Preloader : MonoBehaviour {
    public static Preloader Inst;
    [SerializeField] Transform loader;

    bool isON;
	private void Awake()
	{
		Inst = this;
        transform.localScale = Vector3.zero;
	}
	
    internal void Show(){
        isON = true;
        transform.SetAsLastSibling();
        transform.localScale = Vector3.one;
    }

    internal void Hide(){
        isON = false;
        transform.localScale = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(isON){
            loader.Rotate(Vector3.forward, 5 * Time.deltaTime);
        }
	}
}

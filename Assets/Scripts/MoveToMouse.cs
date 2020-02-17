using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToMouse : MonoBehaviour
{

  public Button taskButton;
  public GameObject treeButton;
  public float speed = 5;
  bool action = false;
  Collider2D colli;

  private GameObject trackedObject;
  private Vector2 target;

  bool uiActive = false;
  bool buttonWasPressed = false;
  
  
  void Start () {
		taskButton.onClick.AddListener(onTaskClick);
    treeButton.SetActive (uiActive);
  }

  void onTaskClick() {
    Debug.Log("ya clicked me!");
    this.buttonWasPressed = true;
    this.uiActive = false;
    treeButton.SetActive(this.uiActive);
  }


  void HandleLeftClick() {
    if(!Input.GetMouseButtonDown(0)) { return; }
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    var t = SelectTarget();
    if(t != null) {
      trackedObject = t;
      this.uiActive = true;
      treeButton.SetActive(uiActive);
    }

    if(this.buttonWasPressed) {
      trackedObject.GetComponent<Tree>().Target = mousePos;
      this.buttonWasPressed = false;
    }
  }
  GameObject SelectTarget() {
    GameObject result = null;
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
    if (hit.collider != null && hit.collider.gameObject != null) {
      var hitObject = hit.collider.gameObject;
      if(hitObject.name == "Tree(Clone)") {
        result = hitObject;
      }
    }
    return result;
  }
  
  void Update () {

    HandleLeftClick();



    // treeButton.SetActive (true);
    // if (this.trackedObject != null && Input.GetMouseButtonDown(0)) {
    //   var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //   this.target = new Vector2(mouse.x, mouse.y);
      
    // }
      
    // if (Input.GetMouseButtonDown(1)) {
    //   this.trackedObject = null;
    //   treeButton.SetActive (false);
    // }

    // if (Input.GetMouseButtonDown(0)) {
    //   Debug.Log("click");
    
     
    // }
  }    
 
}

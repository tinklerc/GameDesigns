using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
  public GameObject treeButton;
  public float speed = 5;
  bool action = false;
  Collider2D colli;

  private GameObject trackedObject;
  private Vector2 target;
  
  void Start () {
    treeButton.SetActive (false);

  }

  void MoveObjects() {
    if(this.trackedObject == null || this.target == null) { return; }
    var currentPos = this.trackedObject.transform.position;
    this.trackedObject.transform.position = Vector2.MoveTowards(currentPos, target, speed * Time.deltaTime);
  }
  
  void Update () {

    MoveObjects();

    if (this.trackedObject != null && Input.GetMouseButtonDown(0)) {
      var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      this.target = new Vector2(mouse.x, mouse.y);
      
    }
      
    if (Input.GetMouseButtonDown(1)) {
      this.trackedObject = null;
      treeButton.SetActive (false);
    }

    if (Input.GetMouseButtonDown(0)) {
      Debug.Log("click");
      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
      
      RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
      if (hit.collider != null && hit.collider.gameObject != null) {
        var hitObject = hit.collider.gameObject;
        if(hitObject.name == "Tree(Clone)") {
          this.trackedObject = hitObject;
          treeButton.SetActive (true);
          Debug.Log("tracking - " + this.trackedObject.name);
        } else {
          Debug.Log("Expected tree but did not get");
          Debug.Log("Got " + hitObject.name);
        }
      }
    }
  }    
 
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum state
{
    move = 0,
    rotation = 1,
    setpoint = 2,
    inside = 3,
    scale = 4
}

public class TouchScript : MonoBehaviour
{
    public LayerMask touchInputMask;
    public Transform THE_WORLD_IS_MINE;
    private Transform target = null;
    private objectScript targetScript;
    private Vector3 trim;
    private float dist;
    private Vector3 oldScale; // скейл объекта при начале зума
    public state Stat;
    private Vector3 startPos;
    private float screenSize; // для зума с учетом разрешения экрана

    // для корутины движения
    private bool isMoving = false;

    public float SensevityScale = 75f; // скорость скейла
    public float rotationSpeed = 360f * 2f; // скорость вращения

    // ------------------------------------------
    private bool _inside;

    public Text text;

    // ====== лог на канвасе ====
    public Text LOG;
    private int logCount = 0;
    void AddLog(string _str)
    {
        LOG.text += _str + '\n';
        logCount++;
        if (logCount > 20)
        {
            LOG.text = LOG.text.Remove(0, LOG.text.IndexOf('\n')+1);
            logCount--;
        }
    }
    // ==========================
    public bool setPos;

    //----------------------------------------------
    //moveInside
    [Header("Слой/слои пола")]
    public LayerMask layer_floor;
    public float camHeight = 1;

    private void Start()
    {
        Stat = state.move;
        screenSize = Screen.width > Screen.height ? Screen.width : Screen.height;
    }

    public void SetState(state s)
    {
        Stat = s;
    }

    public void statesSwich(string str)
    {
        setPos = false;
        if (str == "move")
        {
            Stat = state.move;
        }
        if (str == "rotate")
        {
            Stat = state.rotation;
        }
        if (str == "setpoint")
        {
            Stat = state.setpoint;
            setPos = true;
        }
        if (str == "inside")
        {
            Stat = state.inside;
            _inside = true;
            setPos = false;
        }
        if (str == "scale")
        {
            Stat = state.scale;
        }
        if (str == "inside")
        {
            Stat = state.inside;
            setPos = false;
        }
        text.text = Stat.ToString();
    }

    public void changeSens(float sens)
    {
        rotationSpeed = sens;
        SensevityScale = sens;
        //text.text = "" + rotationSpeed;
    }

    void OnMoveStart(Vector3 _pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, touchInputMask))
        {
            target = hit.transform; // запоминаем объект
            targetScript = target.GetComponent<objectScript>(); // запоминаем скрипт на объекте
            dist = hit.distance; // запоминем расстояние до объекта
            trim = hit.collider.transform.position - hit.point;// запоминем смещение от рейкаста до центра объекта
        }
    }

    void OnMoveStay(Vector3 _pos)
    {
        if (!target) return;
        Ray ray = Camera.main.ScreenPointToRay(_pos); // делаем луч
        Vector3 pos = ray.origin + ray.direction * dist; // из луча находим точку на расстоянии dist
        pos += trim; // смещаем от точки по запомненному смещению
        target.position = pos; // задаём позицию
    }

    void OnMoveEnd(Vector3 _pos)
    {
        target = null;
    }

    void OnRotateStart(Vector3 _pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, touchInputMask))
        {
            target = hit.transform; // запоминаем объект
            targetScript = target.GetComponent<objectScript>(); // запоминаем скрипт на объекте
            startPos = _pos; // запоминаем начальное положение
        }
    }

    void OnRotateStay(Vector3 _pos)
    {
        if (!target) return;
        float dX = (_pos.x - startPos.x) / Screen.width; // приводим смещение к размеру экрана
        float dY = (_pos.y - startPos.y) / Screen.height; // приводим смещение к размеру экрана
        target.Rotate(Vector3.up, -dX * rotationSpeed, Space.World); // вращаем
        target.Rotate(Vector3.up, -dY * rotationSpeed, Space.World); // вращаем
        startPos = _pos; // запомним новое положение тача, чтобы постоянно не крутилось
    }

    void OnRotateEnd(Vector3 _pos)
    {
        target = null;
    }

    void OnZoomStart(Vector3 _pos1, Vector3 _pos2)
    {
        Ray ray = Camera.main.ScreenPointToRay(_pos1);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, touchInputMask))
        {
            target = hit.transform; // запоминаем объект
            targetScript = target.GetComponent<objectScript>(); // запоминаем скрипт на объекте
            oldScale = target.localScale; // запомнить скейл
        }
        dist = (_pos1 - _pos2).magnitude; // запоминаем расстояние между тачами в начале
    }

    void OnZoomStay(Vector3 _pos1, Vector3 _pos2)
    {
		if (!targetScript) return;
        float delta = (_pos1 - _pos2).magnitude-dist; // считаем разницу между тачами между кадрами
		delta = delta / screenSize * SensevityScale; // приводим в размерам экрана и чувствительности
		targetScript.Scale(delta); // задаём скейл
		dist = (_pos1 - _pos2).magnitude;

	}

    void OnZoomEnd(Vector3 _pos1, Vector3 _pos2)
    {
        target = null;
    }

    // перемещает КАМЕРУ
    // isInverseMove - двигает наоборот
    void FloorChange(Transform _tr, bool _toUp, float _time)
    {
        Ray ray = new Ray(Vector3.zero, _toUp ? Vector3.up : Vector3.down); // делаем луч ВВЕРХ или ВНИЗ
        Vector3 point = GetTeleportPoint(ray); // ищем точку
        if (point.y != ray.origin.y)
            SmoothMoveBy(_tr, -(point + Vector3.up * camHeight), _time); // двигаем
    }

    public void ButtonUpperFloor()
    {
        
        FloorChange(MoveFloorToTap._stage.transform, true, 1.0f);
    }

    public void ButtonLowerFloor()
    {
        FloorChange(MoveFloorToTap._stage.transform, false, 1.0f);
    }

    void Update()
    {
#if UNITY_EDITOR
        switch (Stat)
        {
            case state.move:
                {
                    if (Input.GetMouseButtonDown(0))
                        OnMoveStart(Input.mousePosition);
                    if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
                        OnMoveStay(Input.mousePosition);
                    if (!Input.GetMouseButton(0))
                        OnMoveEnd(Vector3.zero);
                    break;
                }

            //поворот
            case state.rotation:
                {
                    if (Input.GetMouseButtonDown(0))
                        OnRotateStart(Input.mousePosition);
                    if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
                        OnRotateStay(Input.mousePosition);
                    if (!Input.GetMouseButton(0))
                        OnRotateEnd(Vector3.zero);
                    break;
                }

            case state.scale:
                {
                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                        OnZoomStart(Input.mousePosition, Input.mousePosition + Vector3.right);
                        OnZoomStay(Input.mousePosition, Input.mousePosition + Vector3.right * (1 + Input.GetAxis("Mouse ScrollWheel"))); // типа двигаем правый тач
                    }
                    else
                    {
                        OnZoomEnd(Vector3.zero, Vector3.zero);
                    }
                    break;
                }

            case state.inside:
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                        Vector3 new_pos = GetTeleportPoint(ray); // метод поиска точки
                        new_pos.y += cam_height; // поднимаем точку на высоту камеры
                        world_tr.transform.position += (Camera.main.transform.position - new_pos);

                       
                    }
                    break;
                }
        }
#endif
        switch (Stat)
        {
            case state.move:
                {
                    if (Input.touchCount == 1)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                            OnMoveStart(touch.position);
                        if (touch.phase == TouchPhase.Moved)
                            OnMoveStay(touch.position);
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            OnMoveEnd(touch.position);
                    }
                    break;
                }
            case state.rotation:
                {
                    if (Input.touchCount == 1)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                            OnRotateStart(touch.position);
                        if (touch.phase == TouchPhase.Moved)
                            OnRotateStay(touch.position);
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            OnRotateEnd(touch.position);
                    }
                    break;
                }
            case state.scale:
                {
                    if (Input.touchCount >= 2)
                    {
                        Touch touch1 = Input.GetTouch(0);
                        Touch touch2 = Input.GetTouch(1);
                        if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                            OnZoomStart(touch1.position, touch2.position);
                        if ((touch1.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Stationary) && // тач1 стоит или двигается
                           (touch2.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Stationary) && // тач2  стоит или двигается
                           (touch1.phase != TouchPhase.Stationary && touch2.phase != TouchPhase.Stationary)) // тач1 не стоит && тач2 не стоит
                            OnZoomStay(touch1.position, touch2.position);
                        if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended ||
                           touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Canceled)
                            OnZoomEnd(touch1.position, touch2.position);
                    }
                    break;
                }
        }
    }
    public void Inside()
    {
        objectScript objTemp = (objectScript)FindObjectOfType(typeof(objectScript));
        if (objTemp != null)
        {
            objTemp.Inside();
        }
    }

    #region move_control_Inside

    private Camera cam_tr;
    private float cam_height = 0;
    public Transform world_tr;

    // Поиск точки 
    private Vector3 GetTeleportPoint(Ray _ray)
    {
        bool toUp = _ray.direction.y > 0f ? true : false; // вверх или вниз?
        RaycastHit[] rhs = Physics.RaycastAll(_ray, Mathf.Infinity, layer_floor); // рейкастим насквозь
        if (rhs.Length == 0)
        {
            print("No floor!");
            return _ray.origin; // если ни одной точки - возвращаем исходную точку
        }

        int min1 = 0; // индекс ближайшего
        int min2 = -1; // индекс следующего после ближайшего
        if (rhs.Length > 1) // если больше одного попадания - ищем очередность...
        {
            for (int i = 1; i < rhs.Length; i++)
            {
                if (rhs[i].distance < rhs[min1].distance)
                {
                    min1 = i;
                }
                else
                {
                    if (min2 != -1)
                    {
                        if (rhs[i].distance < rhs[min2].distance)
                            min2 = i;
                    }
                    else min2 = i;
                }
            }
        }
        else // ...иначе очередность не нужна
        {
            if (!toUp)
            {
                print("At lower floor!");
                return _ray.origin; // если вниз и всего одна точка - ниже нет этажей - возвращаем исходную точку
            }
            min1 = 0;
            min2 = 0;
        }

        switch (toUp)
        {
            case (true): // если вверх, то надо поднять точку над полом
                {
                    Collider col = rhs[min1].collider; // коллайдер
                    Renderer rend = col.gameObject.GetComponent<Renderer>(); // рендер коллайдера
                    float height = rend.bounds.size.y + 0.01f; // высота пола + чуть-чуть
                    Vector3 v = rhs[min1].point; // точка коллизии
                    v.y += height; // поднимаем на высоту
                    Ray ray = new Ray(v, Vector3.down); // луч вниз для поиска верхней грани
                    RaycastHit rh; // ага-ага
                    Physics.Raycast(ray, out rh, 1f, layer_floor); // рейкастим, проверять не надо, ведь мы ТОЧНО над коллайдером
                    print("To up:" + rh.collider.name); // дебаг ))
                    return rh.point; // отдаём точку
                }
            case (false): // если вниз, то просто отдаём вторую точку
                {
                    print("To down:" + rhs[min2].collider.name); // дебаг ))
                    return rhs[min2].point; // отдаём точку
                }
        }
        return _ray.origin;
    }

    public void jumpToClik(){
        
    }

    /// <summary>
    /// Заглушка корутины плавного движения - просто запускает корутину
    /// </summary>
    /// <param name="_target">Объект движения</param>
    /// <param name="_diff">Вектор смещения</param>
    /// <param name="_time">Длительность</param>
    /// <returns></returns>
    private void SmoothMoveBy(Transform _target, Vector3 _diff, float _time)
    {
        StartCoroutine(C_SmoothMove(_target, _target.position, _target.position + _diff, _time));
    }
    /// <summary>
    /// Корутина плавного движения
    /// </summary>
    /// <param name="_target">Объект движения</param>
    /// <param name="_from">Координата начала</param>
    /// <param name="_to">Координата конца</param>
    /// <param name="_time">Длительность</param>
    /// <returns></returns>
    private IEnumerator C_SmoothMove(Transform _target, Vector3 _from, Vector3 _to, float _time)
    {
        if (isMoving) yield break; // если запустили при работающей корутине - выходим
        isMoving = true; // флаг движения включим

        float timer = 0f; // внутренний таймер
        //_to.y += camHeight;
        while (timer <= _time)
        {
            float coeff = timer / _time; // для лерпа
            _target.position = Vector3.Lerp(_from, _to, coeff); // перемещаем лерпом
            timer += Time.deltaTime; // таймер увеличиваем
            yield return null; // ждём следующий кадр
        }
        isMoving = false;  // флаг движения выключим
    }

    #endregion
   
   
       
}
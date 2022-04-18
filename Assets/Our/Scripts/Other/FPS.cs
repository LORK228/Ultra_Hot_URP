using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    float _updateInterval = 1f;// Устанавливаем временной интервал для обновления частоты кадров до 1 секунды  
    float _accum = .0f;// Суммарное время  
    int _frames = 0;// Сколько кадров было запущено за время _updateInterval  
    float _timeLeft;
    string fpsFormat;

    void Start()
    {
        _timeLeft = _updateInterval;
        Application.targetFrameRate = 60;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 200, 200), fpsFormat);
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        //Time.timeScale может контролировать скорость выполнения Update и LateUpdate,  
        //Time.deltaTime рассчитывается в секундах, время до завершения последнего кадра  
        // Делим, чтобы получить время, использованное в соответствующем кадре  
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;// Номер кадра  

        if (_timeLeft <= 0)
        {
            float fps = _accum / _frames;
            //Debug.Log(_accum + "__" + _frames);  
            fpsFormat = System.String.Format("{0:F2}FPS", fps);// Сохраняем два десятичных знака  
            

            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TitleScene_Ctrl : MonoBehaviour
{
    //FIn_goT -> Title -> FOut_goM -> FIn_goM -> Movie -> FOut_goT
    //↑_____________________________________________________|
  
    public enum _mode_ { FIn_goT, FOut_goT, FIn_goM, FOut_goM, Movie, Title };
     
    public _mode_ mode;
    public Image BlackPlane;
    public float cntT;
    public VideoPlayer title, movie;  
    public bool SETIME;
    public AudioSource se;

    void Start()
    {
        mode = _mode_.FIn_goT;
        movie.isLooping = false;
        title.isLooping = true;
        BlackPlane.color = Color.black;
    }

    void Next()
    { 
        SETIME = false;
    }

    void Update()
    {
        if (SETIME)
        {
            return;
        }  
        else
        {
            if (mode == _mode_.FOut_goM || mode == _mode_.FOut_goT)
            {
                BlackPlane.color += new Color(0, 0, 0, Time.deltaTime);
                if (BlackPlane.color.a >= 1)
                {
                    if (mode == _mode_.FOut_goM)
                    {
                        title.Stop();
                        mode = _mode_.FIn_goM;
                    }
                    if (mode == _mode_.FOut_goT)
                    {
                        movie.Stop();
                        mode = _mode_.FIn_goT;
                    }
                }
            }
            else if (mode == _mode_.FIn_goM || mode == _mode_.FIn_goT)
            {
                BlackPlane.color -= new Color(0, 0, 0, Time.deltaTime);

                if (BlackPlane.color.a <= 0)
                {
                    if (mode == _mode_.FIn_goM)
                    {
                        movie.Play();
                        mode = _mode_.Movie;
                    }
                    if (mode == _mode_.FIn_goT)
                    {
                        title.Play();
                        mode = _mode_.Title;
                    }
                }
            }
            else if (mode == _mode_.Movie)
            {
                cntT += Time.deltaTime;
                if (cntT > 1)
                    if (!movie.isPlaying || Key.A.Down || Key.B.Down || Key.X.Down || Key.Y.Down)
                    {
                        cntT = 0;
                        mode = _mode_.FOut_goT;
                    }

            }
            else
            {
                cntT += Time.deltaTime;

                if (cntT > 20)
                {
                    cntT = 0;
                    mode = _mode_.FOut_goM;
                }
                if (Key.A.Down || Key.B.Down || Key.X.Down || Key.Y.Down)
                {
                    SETIME = true;
                    se.Play();
                    SceneLoader.Load("SelectScene");
                    Invoke("Next", 2); 
                    cntT = 0;
                    BlackPlane.color = Color.clear; 
                }

            }
        }
    }


}

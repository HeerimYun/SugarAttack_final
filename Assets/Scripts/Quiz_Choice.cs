﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * 4지선다형 퀴즈 스크립트
 */
public class Quiz_Choice : MonoBehaviour {

    /*객관식 개수*/
    const int CHOICE_NUM = 4;
    /*문제*/
    Text question;
    /*선택지 - 버튼*/
    Toggle[] choice_btn = new Toggle[CHOICE_NUM];
    /*선택지  - 텍스트*/
    Text[] choice_text = new Text[CHOICE_NUM];
    /*정답확인 버튼*/
    Button checkAnswerBtn;
    /*퀴즈*/
    ChoiceQuiz quiz;
    /*사용자 선택 정답*/
    string choosedAnswer;
    
    /*결과 팝업*/
    GameObject quizResult;
    /*팝업 상태*/
    Vector3 open = new Vector3(1, 1, 1);
    Vector3 close = new Vector3(0, 1, 1);
    /*팝업 이미지*/
    SpriteRenderer popUpImg;

    /*correct_particle*/
    ParticleSystem correctParticle;
    /*Popup pop anim*/
    Animator popAnim;
    /*result sound*/
    AudioSource resultSound;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    public static int index = 0;

    // Use this for initialization
    void Start () {
        RandomQuiz();
        GetUI();
        SetUI();
	}
	
    /**
     * 화면의 UI 객체들 로드하기
     */
    public void GetUI()
    {
        question = GameObject.Find("QuizText").GetComponent<Text>();

        for (int i=0; i<CHOICE_NUM; i++)
        {
            choice_btn[i] = GameObject.Find("Btn_" + (i + 1)).GetComponent<Toggle>();
            choice_btn[i].isOn = false;
            choice_text[i] = GameObject.Find("Text"+(i + 1)).GetComponent<Text>();
        }

        checkAnswerBtn = GameObject.Find("CheckAnswerBtn").GetComponent<Button>();
        checkAnswerBtn.interactable = false;

        quizResult = GameObject.Find("QuizResult");
        quizResult.transform.localScale = close;

        popUpImg = GameObject.Find("QuizResult/Image").GetComponent<SpriteRenderer>();

        correctParticle = GameObject.Find("QuizResult/correct_panpare").GetComponent<ParticleSystem>();
        correctParticle.Pause();

        popAnim = GameObject.Find("QuizResult").GetComponent<Animator>();

        resultSound = GameObject.Find("QuizResult").GetComponent<AudioSource>();
    }

    /**
     * 문제와 선택지 세팅
     */
    public void SetUI()
    {
        //question = 현재 문제의 string
        question.text = quiz.question;

        //for문으로 선택지 하나씩 넣어줄 것
        for (int i=0; i < choice_btn.Length; i++)
        {
            choice_text[i].text = quiz.choice[i];
        }
    }
    
    /**
     * 정답확인 버튼 누를 시 동작
     */
    public void OnClickCheckAnswer()
    {
        bool isAnswer = false;
        //맞았는지 틀렸는지 판단
        //if (선택한 버튼의 선택지 text.Equals(실제 정답 text string)
        if (choosedAnswer.Equals(quiz.answer))
        {
            //맞았어요!
            isAnswer = true;
        }
        else
        {
            //틀렸어요!
            isAnswer = false;
        }

        DisplayResult(isAnswer);
    }

    /**
     * 퀴즈 맞춤 여부에 대한 결과 팝업 띄우기
     */
    public void DisplayResult(bool isAnswer)
    {
        string correctness = "";
        if (isAnswer)
        {
            correctness = "correct";
            /*particle view*/
            correctParticle.Play();
            /*resultSound*/
            resultSound.PlayOneShot(correctSound);
        }
        else
        {
            correctness = "incorrect";
            /*particle view*/
            correctParticle.Stop();
            /*resultSound*/
            resultSound.PlayOneShot(incorrectSound);
        }
        /*popup pop anim*/
        popAnim.SetTrigger("ResultPop");
        popUpImg.sprite = Resources.Load<Sprite>("quiz/" + correctness + "_" + GameData.GetCharByOrder(GameData.currentOrder).name);
        quizResult.transform.localScale = open;
    }

    public void RandomQuiz()
    {
        index = Random.Range(0, GameData.choiceQuizzes.Length);
        quiz = GameData.choiceQuizzes[index];
        quiz.appeard++;
    }

    /**
     * 선택지 누를 시 호출
     */
    public void OnClickChoiceToggle()
    {
        checkAnswerBtn.interactable = true;

        for (int i=0; i<4; i++)
        {
            if (choice_btn[i].isOn)
            {
                choosedAnswer = choice_text[i].text;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * 퀴즈 보상 화면 스크립트
 */
public class QuizReward : MonoBehaviour {

    /*3개의 쪽지*/
    GameObject rewards;
    /*결과 객체*/
    GameObject result;
    /*결과텍스트 영역*/
    GameObject resultArea;
    /*결과 텍스트*/
    Text resultText;
    /*결과 종이 이미지*/
    Image resultPaper;
    /*캔디 이미지*/
    Image candyImg;
    /*get Text*/
    Image getText;
    /*캔디 개수*/
    int num;
    /*현재 캐릭터*/
    //Image charImg;
    /*현재 캐릭터 객체*/
    Character currentChar;

    /*Animation*/
    Animator rewardShow;

    /*등장시간*/
    double durationShow = 1.24;

    /*실시간과 화면 머물 시간*/
    double currentTime = 0;
    double duraition = 4;

    // Use this for initialization
    void Start () {
        currentChar = GameData.GetCharByOrder(GameData.currentOrder);
        
        GetUI();
        SetResult();
    }

    public void GetUI()
    {
        rewards = GameObject.Find("Rewards");
        result = GameObject.Find("Result");
        result.transform.localScale = GameData.close;
        resultText = GameObject.Find("paper/TextArea/Text").GetComponent<Text>();
        resultPaper = GameObject.Find("paper").GetComponent<Image>();
        candyImg = GameObject.Find("candyImage").GetComponent<Image>();
        getText = GameObject.Find("getText").GetComponent<Image>();
        resultArea = GameObject.Find("paper/TextArea");
        rewardShow = GameObject.Find("paper").GetComponent<Animator>();
        appearSource = GameObject.Find("Rewards").GetComponent<AudioSource>();
        //charImg = GameObject.Find("CharacterImage").GetComponent<Image>();
        //charImg.sprite = Resources.Load<Sprite>("Characters/" + currentChar.name + "/idle/" + currentChar.name.ToLower() + "_idle_01");
    }

    /**
     * 세 개 쪽지 버튼 누를 때 동작하는 함수
     */
    public void OnClickRewards()
    {
        ////종이 색깔 결정
        //resultPaper.sprite = Resources.Load<Sprite>("quiz_reward/" + EventSystem.current.currentSelectedGameObject.name + "paper");

        if(EventSystem.current.currentSelectedGameObject.name.Equals("reward1")) {
            rewardShow.SetTrigger("SelectReward1");
        } else if(EventSystem.current.currentSelectedGameObject.name.Equals("reward2")) {
            rewardShow.SetTrigger("SelectReward2");
        } else if (EventSystem.current.currentSelectedGameObject.name.Equals("reward3")) {
            rewardShow.SetTrigger("SelectReward3");
        }

        result.transform.localScale = GameData.open;

        //쪽지 3개는 안보이게 한다.
        rewards.transform.localScale = GameData.close;

        //reward 해당 사탕을 플레이어에게 준다.
        currentChar.candy += num;
    }


    public void SetResult()
    {
        num = UnityEngine.Random.Range(1, 4);
        candyImg.sprite = Resources.Load<Sprite>("quiz_reward/candy" + num);
        resultText.text = "포도당사탕 " + num + "개 획득!";
    }


    // Update is called once per frame
    void Update () {       
        //결과가 펼쳐지면,
        if (result.transform.localScale == GameData.open)
        {
            currentTime += Time.deltaTime; //시간을 흐르게 하고,
        }

        if (currentTime > durationShow) {
            candyImg.transform.localScale = GameData.open;
            getText.transform.localScale = GameData.open;
            resultArea.transform.localScale = GameData.open;
        } 

        //화면에 머무는 시간이 지나면
        if (currentTime > duraition)
        {
            GameData.TurnChange(); //턴을 넘기고,
            PageMove.MoveToRoulette(); //룰렛 화면으로 이동
        }
	}
}

using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	private const int WILL_PAUSE_AUDIO_COUNT = 2;

	public static AudioController instance;

	public AudioSource musicMenu;

	public AudioSource musicCity;

	public AudioSource musicDriving;

	public AudioSource musicFighting;

	public AudioSource audioButton1;

	public AudioSource audioButton2;

	public AudioSource audioGetItem;

	public AudioSource audioUpgrade;

	public AudioSource audioNumRoll;

	public AudioSource audioPickItem;

	public AudioSource audioPickCash;

	public AudioSource audioHandGun;

	public AudioSource audioMachineGun;

	public AudioSource audioEngine;

	public AudioSource audioThrotte;

	public AudioSource audioBrake;

	public AudioSource audioHitHuman;

	public AudioSource audioHitCar;

	public AudioSource audioHitWall;

	public AudioSource audioSlotStick;

	public AudioSource audioSlotRoll;

	public AudioSource audioSlotStop;

	public AudioSource audioCountDown;

	public AudioSource audioStar;

	public AudioSource audioExplode;

	public AudioSource audioReload;

	public AudioSource audioCarLaunch;

	public AudioSource audioPause;

	public AudioSource audioCarHitHuman;

	public AudioSource audioCarHitCar;

	public AudioSource audioCarHitWall;

	public AudioSource audioAIMachineGun;

	public AudioSource audioPunchPerson;

	public AudioSource audioStarLabel;

	public AudioSource audioManPunched;

	public AudioSource audioWomenPunched;

	public AudioSource audioManScream;

	public AudioSource audioWomenScream;

	public AudioSource audioCarAlarm;

	public AudioSource audioManScream2;

	public AudioSource audioWomenScream2;

	public List<AudioSource> currentLoopAudioList_Music = new List<AudioSource>();

	public List<AudioSource> currentLoopAudioList_Sound = new List<AudioSource>();

	public AudioSource[] willPauseAudioList = new AudioSource[2];

	public bool[] isAudioPlayingList = new bool[2];

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		initAudioList();
	}

	public void reset()
	{
		currentLoopAudioList_Music.Clear();
		currentLoopAudioList_Sound.Clear();
	}

	private void initAudioList()
	{
		willPauseAudioList[1] = audioMachineGun;
		willPauseAudioList[0] = audioEngine;
		for (int i = 0; i < isAudioPlayingList.Length; i++)
		{
			isAudioPlayingList[i] = false;
		}
	}

	public void play(AudioType audioType)
	{
		bool flag = false;
		AudioSource audioSource = null;
		bool flag2 = false;
		switch (audioType)
		{
		case AudioType.MENU:
			if (!musicMenu.isPlaying)
			{
				flag = true;
				audioSource = musicMenu;
				flag2 = true;
			}
			break;
		case AudioType.CITY:
			if (!musicCity.isPlaying)
			{
				flag = true;
				audioSource = musicCity;
				flag2 = true;
			}
			break;
		case AudioType.DRIVING:
			if (!musicDriving.isPlaying)
			{
				flag = true;
				audioSource = musicDriving;
				flag2 = true;
			}
			break;
		case AudioType.FIGHTING:
			if (!musicFighting.isPlaying)
			{
				flag = true;
				audioSource = musicFighting;
				flag2 = true;
			}
			break;
		case AudioType.BUTTON1:
			flag = true;
			audioSource = audioButton1;
			break;
		case AudioType.BUTTON2:
			flag = true;
			audioSource = audioButton2;
			break;
		case AudioType.GET_ITEM:
			flag = true;
			audioSource = audioGetItem;
			break;
		case AudioType.UPGRADE:
			flag = true;
			audioSource = audioUpgrade;
			break;
		case AudioType.NUM_ROLL:
			if (!audioNumRoll.isPlaying)
			{
				flag = true;
				audioSource = audioNumRoll;
			}
			break;
		case AudioType.PICK_ITEM:
			flag = true;
			audioSource = audioPickItem;
			break;
		case AudioType.PICK_CASH:
			flag = true;
			audioSource = audioPickCash;
			break;
		case AudioType.HANDGUN:
			flag = true;
			audioSource = audioHandGun;
			break;
		case AudioType.MACHINE_GUN:
			if (!audioMachineGun.isPlaying)
			{
				flag = true;
				audioSource = audioMachineGun;
			}
			break;
		case AudioType.BRAKE:
			flag = true;
			audioSource = audioBrake;
			break;
		case AudioType.EXPLODE:
			flag = true;
			audioSource = audioExplode;
			break;
		case AudioType.ENGINE:
			if (!audioEngine.isPlaying)
			{
				flag = true;
				audioSource = audioEngine;
			}
			break;
		case AudioType.THROTTE:
			flag = true;
			audioSource = audioThrotte;
			break;
		case AudioType.HIT_HUMAN:
			flag = true;
			audioSource = audioHitHuman;
			break;
		case AudioType.HIT_CAR:
			flag = true;
			audioSource = audioHitCar;
			break;
		case AudioType.PUNCH_PERSON:
			flag = true;
			audioSource = audioPunchPerson;
			break;
		case AudioType.HIT_WALL:
			flag = true;
			audioSource = audioHitWall;
			break;
		case AudioType.CAR_HIT_HUMAN:
			flag = true;
			audioSource = audioCarHitHuman;
			break;
		case AudioType.CAR_HIT_CAR:
			flag = true;
			audioSource = audioCarHitCar;
			break;
		case AudioType.CAR_HIT_WALL:
			flag = true;
			audioSource = audioCarHitWall;
			break;
		case AudioType.SLOT_STICK:
			flag = true;
			audioSource = audioSlotStick;
			break;
		case AudioType.SLOT_STOP:
			flag = true;
			audioSource = audioSlotStop;
			break;
		case AudioType.SLOT_ROLL:
			if (!audioSlotRoll.isPlaying)
			{
				flag = true;
				audioSource = audioSlotRoll;
			}
			break;
		case AudioType.COUNT_DOWN:
			flag = true;
			audioSource = audioCountDown;
			break;
		case AudioType.STAR:
			flag = true;
			audioSource = audioStar;
			break;
		case AudioType.STAR_LABEL:
			flag = true;
			audioSource = audioStarLabel;
			break;
		case AudioType.MANPUNCHED:
			flag = true;
			audioSource = audioManPunched;
			break;
		case AudioType.RELOAD:
			flag = true;
			audioSource = audioReload;
			break;
		case AudioType.CAR_LAUNCH:
			flag = true;
			audioSource = audioCarLaunch;
			break;
		case AudioType.PAUSE:
			flag = true;
			audioSource = audioPause;
			break;
		case AudioType.AIMACHINEGUN:
			flag = true;
			audioSource = audioAIMachineGun;
			break;
		case AudioType.WOMENPUNCHED:
			flag = true;
			audioSource = audioWomenPunched;
			break;
		case AudioType.MANSCREAM:
			flag = true;
			audioSource = ((!audioManScream.isPlaying) ? ((!audioManScream2.isPlaying) ? ((Random.Range(0, 2) <= 0) ? audioManScream2 : audioManScream) : audioManScream2) : audioManScream);
			break;
		case AudioType.WOMENSCREAM:
			flag = true;
			audioSource = ((!audioWomenScream.isPlaying) ? ((!audioWomenScream2.isPlaying) ? ((Random.Range(0, 2) <= 0) ? audioWomenScream2 : audioWomenScream) : audioWomenScream2) : audioWomenScream);
			break;
		case AudioType.CARALARM:
			flag = true;
			audioSource = audioCarAlarm;
			break;
		}
		if (!flag || !(audioSource != null))
		{
			return;
		}
		if (audioSource.loop)
		{
			addToLooplist(audioSource, flag2);
		}
		if (flag2)
		{
			if (!audioSource.isPlaying && GlobalInf.musicFlag)
			{
				audioSource.volume = 1f;
				audioSource.Play();
			}
		}
		else if (GlobalInf.soundFlag)
		{
			audioSource.volume = 0.8f;
			audioSource.Play();
		}
	}

	public void updateMusicVolume(float volum)
	{
		for (int i = 0; i < currentLoopAudioList_Music.Count; i++)
		{
			currentLoopAudioList_Music[i].volume = volum;
		}
	}

	public void updateSoundVolume(float volum)
	{
		for (int i = 0; i < currentLoopAudioList_Sound.Count; i++)
		{
			if (currentLoopAudioList_Sound[i] == audioEngine)
			{
				currentLoopAudioList_Sound[i].volume = volum;
			}
			else
			{
				currentLoopAudioList_Sound[i].volume = volum;
			}
		}
	}

	public void stop(AudioType soundType)
	{
		AudioSource audioSource = null;
		bool isMusic = false;
		switch (soundType)
		{
		case AudioType.MENU:
			musicMenu.Stop();
			audioSource = musicMenu;
			isMusic = true;
			break;
		case AudioType.CITY:
			musicCity.Stop();
			audioSource = musicCity;
			isMusic = true;
			break;
		case AudioType.DRIVING:
			musicDriving.Stop();
			audioSource = musicDriving;
			isMusic = true;
			break;
		case AudioType.FIGHTING:
			musicFighting.Stop();
			audioSource = musicFighting;
			isMusic = true;
			break;
		case AudioType.ENGINE:
			audioEngine.Stop();
			audioSource = audioEngine;
			isAudioPlayingList[0] = false;
			break;
		case AudioType.NUM_ROLL:
			audioNumRoll.Stop();
			audioSource = audioNumRoll;
			break;
		case AudioType.SLOT_ROLL:
			audioSlotRoll.Stop();
			audioSource = audioSlotRoll;
			break;
		case AudioType.MACHINE_GUN:
			audioMachineGun.Stop();
			audioSource = audioMachineGun;
			isAudioPlayingList[1] = false;
			break;
		}
		if (audioSource != null)
		{
			removeFromLoopList(audioSource, isMusic);
		}
	}

	public void stopAll()
	{
		if (musicMenu != null)
		{
			musicMenu.Stop();
		}
		if (musicCity != null)
		{
			musicCity.Stop();
		}
		if (musicFighting != null)
		{
			musicFighting.Stop();
		}
		if (musicDriving != null)
		{
			musicDriving.Stop();
		}
		currentLoopAudioList_Sound.Clear();
		currentLoopAudioList_Music.Clear();
		for (int i = 0; i < 2; i++)
		{
			isAudioPlayingList[i] = false;
		}
	}

	public void turnOffMusic()
	{
		for (int i = 0; i < currentLoopAudioList_Music.Count; i++)
		{
			currentLoopAudioList_Music[i].Stop();
		}
	}

	public void turnOnMusic()
	{
		for (int i = 0; i < currentLoopAudioList_Music.Count; i++)
		{
			currentLoopAudioList_Music[i].Play();
		}
	}

	public void turnOffSound()
	{
		for (int i = 0; i < currentLoopAudioList_Sound.Count; i++)
		{
			if (!isAudioPause(currentLoopAudioList_Sound[i]))
			{
				currentLoopAudioList_Sound[i].Stop();
			}
		}
	}

	public void turnOnSound()
	{
		for (int i = 0; i < currentLoopAudioList_Sound.Count; i++)
		{
			if (!isAudioPause(currentLoopAudioList_Sound[i]))
			{
				currentLoopAudioList_Sound[i].Play();
			}
		}
	}

	public void pauseSounds()
	{
		for (int i = 0; i < 2; i++)
		{
			isAudioPlayingList[i] = false;
			if (willPauseAudioList[i] != null)
			{
				isAudioPlayingList[i] = willPauseAudioList[i].isPlaying || willPauseAudioList[i].time > 0.01f;
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (isAudioPlayingList[j])
			{
				willPauseAudioList[j].Pause();
			}
		}
	}

	public void resumeSounds()
	{
		for (int i = 0; i < 2; i++)
		{
			if (isAudioPlayingList[i] && !willPauseAudioList[i].loop)
			{
				willPauseAudioList[i].Stop();
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (isAudioPlayingList[j])
			{
				willPauseAudioList[j].Play();
			}
		}
	}

	private bool isAudioPause(AudioSource audio)
	{
		for (int i = 0; i < 2; i++)
		{
			if (isAudioPlayingList[i] == (bool)audio)
			{
				if (isAudioPlayingList[i])
				{
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private void addToLooplist(AudioSource audio, bool isMusic)
	{
		if (isMusic)
		{
			if (!currentLoopAudioList_Music.Contains(audio))
			{
				currentLoopAudioList_Music.Add(audio);
			}
		}
		else if (!currentLoopAudioList_Sound.Contains(audio))
		{
			currentLoopAudioList_Sound.Add(audio);
		}
	}

	private void removeFromLoopList(AudioSource audio, bool isMusic)
	{
		if (isMusic)
		{
			currentLoopAudioList_Music.Remove(audio);
		}
		else
		{
			currentLoopAudioList_Sound.Remove(audio);
		}
	}
}

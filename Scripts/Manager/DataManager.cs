using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KNJ
{
    /// <summary>
    /// 저장할 데이터를 모은 클래스
    /// 저장하는 자료 -> 플레이어 위치&회전, 퍼즐 클리어 여부
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public float[] playerPosition;
        public float[] playerRotation;
        public List<GameClearData> puzzleClearData;
    }

    /// <summary>
    /// 퍼즐 클리어 정보를 담는 구조체
    /// id = 퍼즐의 고유 아이디, isClear =  퍼즐 클리어 여부
    /// </summary>
    [System.Serializable]
    public struct GameClearData
    {
        public string id;
        public bool isClear;

        public GameClearData(string id, bool isClear)
        {
            this.id = id;
            this.isClear = isClear;
        }
    }

    /// <summary>
    /// 게임 데이터 저장 및 불러오기를 담당하는 클래스
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        /// <summary>
        /// 현재 저장된 데이터를 담는 필드
        /// 이거 null이면 Get함수들 작동 안함
        /// </summary>
        private SaveData curSaveData;

        /// <summary>
        /// 현재 정보를 바탕으로 데이터를 저장하는 함수
        /// </summary>
        public void SaveData()
        {
            curSaveData = new SaveData();

            // 게임 매니저로 플레이어 위치랑 회전 정보 가져옴
            try
            {
                Vector3 playerPos = CharacterManager.Instance.Player.transform.position;
                curSaveData.playerPosition = new float[3] { playerPos.x, playerPos.y, playerPos.z };
                Quaternion playerRot = CharacterManager.Instance.Player.transform.rotation;
                curSaveData.playerRotation = new float[4] { playerRot.x, playerRot.y, playerRot.z, playerRot.w };
            }
            catch
            {
                Debug.LogError("플레이어 정보 없음");
            }

            // 게임 매니저에서 퍼즐 클리어 정보 가져옴 (퍼즐 고유 아이디랑 클리어 여부)
            try
            {
                curSaveData.puzzleClearData = new List<GameClearData>();
                Dictionary<string, bool> puzzleClearData = PuzzleDataManager.Instance.puzzleClearData;
                foreach (KeyValuePair<string, bool> kvp in puzzleClearData)
                {
                    curSaveData.puzzleClearData.Add(new GameClearData(kvp.Key, kvp.Value));
                }
            }
            catch
            {
                Debug.LogError("퍼즐 클리어 정보 없음");
            }

            string json = JsonUtility.ToJson(curSaveData, true);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);

            Debug.Log($"데이터 저장 성공!\n{Application.persistentDataPath}");
        }

        /// <summary>
        /// 저장된 데이터를 바탕으로 SaveData 필드를 불러오는 함수
        /// </summary>
        /// <returns>불러오기 성공 or 실패</returns>
        public bool LoadData()
        {
            SaveData data = null;

            try
            {
                string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
                data = JsonUtility.FromJson<SaveData>(json);
            }
            catch
            {
                Debug.Log("데이터 불러오기 실패");
                return false;
            }

            // 불러올 데이터가 없는 경우
            if(data == null)
            {
                Debug.Log("데이터 불러오기 실패");
                return false;

            }

            curSaveData = data;

            Debug.Log("데이터 불러오기 성공");
            return true;
        }

        /// <summary>
        /// 세이브데이터에서 플레이어 위치 정보를 가져오는 함수
        /// </summary>
        /// <returns>플레이어 위치 Vector3</returns>
        public Vector3 GetPlayerPositionData()
        {
            if(curSaveData == null)
            {
                Debug.Log("불러올 데이터가 없습니다.");
            }

            return new Vector3(curSaveData.playerPosition[0],  curSaveData.playerPosition[1], curSaveData.playerPosition[2]);
        }

        /// <summary>
        /// 세이브데이터에서 플레이어 회전 정보를 가져오는 함수
        /// 아마 쓸 일은 없을 듯?
        /// </summary>
        /// <returns>플레이어 회전 정보 Quaternion</returns>
        public Quaternion GetPlayerRotationData()
        {
            if (curSaveData == null)
            {
                Debug.Log("불러올 데이터가 없습니다.");
            }

            return new Quaternion(curSaveData.playerRotation[0], curSaveData.playerRotation[1], curSaveData.playerRotation[2], curSaveData.playerRotation[3]);
        }

        /// <summary>
        /// 세이브데이터에서 퍼즐 클리어 여부를 가져오는 함수
        /// </summary>
        /// <returns>퍼즐 클리어 상황 딕셔너리</returns>
        public Dictionary<string, bool> GetGameClearData()
        {
            if (curSaveData == null)
            {
                Debug.Log("불러올 데이터가 없습니다.");
            }

            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach(GameClearData data in curSaveData.puzzleClearData)
            {
                dic[data.id] = data.isClear;
            }

            return dic;
        }
    }

}


# PuzzleCat

## 📌 Overview
3D 퍼즐 어드벤쳐 게임.  
레이저 퍼즐 미니게임의 로직과 데이터 저장 기능을 담당하여 진행한 짧은 팀 프로젝트입니다.

- **개발 기간:** 2025.08.14 - 2025.08.22
- **개발 인원:** 총 5명 (개발 5)  
- **플랫폼:** PC
- **엔진:** Unity 2022.3.17f1 

---

## 🧑‍💻 담당 업무 (My Role)
### 레이저 퍼즐
- 레이저 퍼즐 로직 구현
- 퍼즐에 사용되는 각종 오브젝트 구현

### 데이터 저장 및 불러오기
- 플레이어 및 퍼즐 클리어 정보를 json으로 변환하여 저장

---

## 🛠 Tech Stack
**Language:** C#  
**Engine:** Unity  
**Tools:** Git, GitHub

---

## 🔍 주요 구현 기능 (Key Features)

### 1) 레이저 퍼즐 로직 및 기능
- 레이저 상호작용을 위한 정보를 담은 구조체 구현([LaserHitInfo](), [LaserRaycastInfo](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Puzzle/Laser_Puzzle/Common/Constants.cs))
- 레이저 및 플레이어 입력에 의한 상호작용을 구현하기 위한 인터페이스([ILaserInteractable](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Puzzle/Laser_Puzzle/Interface/ILaserInteractable.cs), [IInteractable](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Puzzle/Laser_Puzzle/Interface/IInteractable.cs))
- [LaserPuzzleManager](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Puzzle/Laser_Puzzle/Managers/LaserPuzzleManager.cs)를 통해 해당 퍼즐 스테이지의 진행을 관리
- 레이저 발사 기능을 Raycast와 LineRenderer를 통해서 구현([LaserRaycaster](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Puzzle/Laser_Puzzle/Objects/LaserRaycaster.cs))

### 2) 데이터 저장 및 불러오기
- [DataManger](https://github.com/ONEJEUNGWOO/Puzzle_Cat/blob/main/Assets/02.%20Script/Manager/DataManager.cs)를 통해 플레이어의 위치, 회전, 각 퍼즐의 클리어 정보를 저장

---

## 📸 스크린샷



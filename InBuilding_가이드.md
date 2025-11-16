# InBuilding 씬 개발 가이드

이 문서는 InBuilding 씬의 구체적인 개발 방법과 JSON 데이터 구조를 설명합니다.

## 📋 목차

1. [개요](#개요)
2. [JSON 데이터 구조](#json-데이터-구조)
3. [Enter Building 오브젝트 설정](#enter-building-오브젝트-설정)
4. [InBuilding 씬 설정](#inbuilding-씬-설정)
5. [JSON 파일 생성 및 관리](#json-파일-생성-및-관리)
6. [테스트 방법](#테스트-방법)

---

## 🎯 개요

InBuilding 씬은 건물 내부를 표현하는 씬으로, 다음과 같은 기능을 제공합니다:

- **다중 건물 지원**: 여러 개의 Enter Building 오브젝트가 각각 다른 건물로 연결
- **JSON 기반 데이터**: 건물 내부 아이템 정보를 JSON 파일로 관리
- **동적 아이템 배치**: JSON 파일의 데이터를 읽어서 아이템을 자동으로 배치

### 작동 흐름

```
City 씬
  ↓ (Enter Building 오브젝트 상호작용)
PlayerPrefs에 건물 정보 저장 (BuildingType, BuildingIndex)
  ↓
InBuilding 씬 로드
  ↓
BuildingDataLoader가 JSON 파일 로드
  ↓
해당 건물의 아이템들을 씬에 배치
```

---

## 📄 JSON 데이터 구조

### 전체 구조

```json
{
    "home": {
        "buildings": [
            {
                "houseIndex": "Index",
                "houseItem": [
                    {
                        "ItemName": "name",
                        "ItemCount": count,
                        "ItemPosition": {
                            "x": posX,
                            "y": posY
                        }
                    }
                ]
            }
        ]
    },
    "CVS": {
        "buildings": [...]
    },
    "Mart": {
        "buildings": [...]
    }
}
```

### 데이터 클래스 구조

- **HouseData**: 전체 건물 데이터 (home, CVS, Mart 포함)
- **BuildingTypeData**: 건물 타입별 데이터 (buildings 리스트 포함)
- **HouseIndexData**: 특정 건물의 데이터 (houseIndex, houseItem 리스트)
- **HouseItemData**: 개별 아이템 데이터 (ItemName, ItemCount, ItemPosition)

### 예시 JSON 파일

```json
{
    "home": {
        "buildings": [
            {
                "houseIndex": "1",
                "houseItem": [
                    {
                        "ItemName": "Apple",
                        "ItemCount": 3,
                        "ItemPosition": {
                            "x": 2.5,
                            "y": 1.0
                        }
                    },
                    {
                        "ItemName": "Scrap",
                        "ItemCount": 5,
                        "ItemPosition": {
                            "x": -1.0,
                            "y": 0.5
                        }
                    }
                ]
            },
            {
                "houseIndex": "2",
                "houseItem": [
                    {
                        "ItemName": "Meat",
                        "ItemCount": 2,
                        "ItemPosition": {
                            "x": 0.0,
                            "y": 0.0
                        }
                    }
                ]
            }
        ]
    },
    "CVS": {
        "buildings": [
            {
                "houseIndex": "1",
                "houseItem": [
                    {
                        "ItemName": "Water",
                        "ItemCount": 10,
                        "ItemPosition": {
                            "x": 1.0,
                            "y": 1.0
                        }
                    }
                ]
            }
        ]
    },
    "Mart": {
        "buildings": []
    }
}
```

---

## 🏢 Enter Building 오브젝트 설정

City 씬에서 건물 입구에 배치하는 Enter Building 오브젝트 설정 방법입니다.

### Step 1: Enter Building 오브젝트 생성

1. City 씬에서 빈 GameObject 생성 → 이름: "Enter Building 1" (또는 적절한 이름)
2. 위치: 건물 입구 위치로 설정

### Step 2: Collider 설정

1. BoxCollider2D 컴포넌트 추가
2. **Is Trigger**: 체크
3. Size: 적절한 크기 (예: 2 x 2)

### Step 3: SceneTransitionTrigger 설정

1. `SceneTransitionTrigger` 스크립트 추가
2. 다음 필드 설정:
   - **Target Scene Name**: "InBuilding"
   - **Use Transition Scene**: 체크
   - **Requires Interaction**: 체크
   - **Interaction Key**: E (기본값)
   - **Interaction Prompt**: "E키를 눌러 건물 입장"
   - **Building Type**: "home" (또는 "CVS", "Mart")
   - **Building Index**: "1" (해당 건물의 인덱스)

### Step 4: 여러 건물 설정

각 건물마다 별도의 Enter Building 오브젝트를 생성하고, 각각 다른 Building Type과 Building Index를 설정합니다.

**예시:**
- Enter Building 1: Building Type="home", Building Index="1"
- Enter Building 2: Building Type="home", Building Index="2"
- Enter Building 3: Building Type="CVS", Building Index="1"

---

## 🏠 InBuilding 씬 설정

### Step 1: 기본 씬 설정

1. Unity에서 `InBuilding` 씬 열기
2. 필수 시스템 오브젝트 확인:
   - **Game Manager**
   - **Time System**
   - **Ending Manager**
3. 빈 GameObject 생성 → 이름: "Scene Initializer"
   - `SceneInitializer` 스크립트 추가
   - Auto Initialize: 체크

### Step 2: BuildingDataLoader 설정

1. 빈 GameObject 생성 → 이름: "Building Data Loader"
2. `BuildingDataLoader` 스크립트 추가
3. 다음 필드 설정:
   - **JSON File Name**: "BuildingData.json"
   - **Load From Resources**: false (persistentDataPath에서 로드)
   - **Item Parent**: 빈 GameObject 생성 후 할당 (선택사항, 아이템들을 정리하기 위함)
   - **Item Prefab**: Lootable 컴포넌트가 있는 프리팹 할당
   - **Debug Log**: 체크 (개발 중에는 유용)

### Step 3: Item Prefab 생성

1. 빈 GameObject 생성 → 이름: "Lootable Item"
2. Sprite Renderer 추가 (아이템 스프라이트 할당)
3. BoxCollider2D 추가
   - **Is Trigger**: 체크
4. `Lootable` 스크립트 추가
5. 프리팹으로 저장 (Assets/Prefabs/LootableItem.prefab)

### Step 4: 플레이어 배치

1. Player 프리팹 배치
2. 시작 위치: 건물 입구 (예: (0, 0, 0))

### Step 5: 카메라 설정

1. Main Camera에 `CameraFollow` 스크립트 추가
2. Target은 자동으로 찾거나 Player를 직접 할당

### Step 6: 출구 설정

1. GameObject 생성 → 이름: "Exit to City"
2. 위치: 건물 출구
3. BoxCollider2D 추가 (Is Trigger)
4. `SceneTransitionTrigger` 추가
   - Target Scene Name: "City"
   - Use Transition Scene: 체크
   - Requires Interaction: 체크
   - Building Type, Building Index는 비워둠 (출구이므로 불필요)

---

## 📁 JSON 파일 생성 및 관리

### JSON 파일 위치

JSON 파일은 두 가지 위치에서 로드할 수 있습니다:

1. **Resources 폴더** (Load From Resources = true)
   - 경로: `Assets/Resources/BuildingData.json`
   - 빌드에 포함됨
   - 수정하려면 프로젝트를 다시 빌드해야 함

2. **persistentDataPath** (Load From Resources = false, 권장)
   - 경로: `%USERPROFILE%\AppData\LocalLow\<CompanyName>\<ProductName>\BuildingData.json`
   - 런타임에 수정 가능
   - 게임 실행 중에도 변경 가능

### JSON 파일 생성 방법

#### 방법 1: 수동 생성

1. 위의 예시 JSON 구조를 참고하여 파일 생성
2. 파일을 persistentDataPath에 저장
3. 파일 이름: `BuildingData.json`

#### 방법 2: Unity 에디터에서 생성 (추후 구현 가능)

에디터 스크립트를 만들어서 Unity 내에서 JSON 파일을 생성/편집할 수 있습니다.

### JSON 파일 검증

JSON 파일을 생성한 후 다음을 확인하세요:

- [ ] JSON 문법이 올바른가? (온라인 JSON 검증기 사용 가능)
- [ ] 모든 필드명이 정확한가? (대소문자 구분)
- [ ] ItemName이 Resources/Items 폴더의 아이템 이름과 일치하는가?
- [ ] ItemPosition의 x, y 값이 적절한가?

---

## 🧪 테스트 방법

### Step 1: JSON 파일 준비

1. 예시 JSON 파일을 persistentDataPath에 저장
2. 파일 이름: `BuildingData.json`

### Step 2: Enter Building 오브젝트 확인

1. City 씬에서 Enter Building 오브젝트 확인
2. Building Type과 Building Index가 올바르게 설정되었는지 확인

### Step 3: 게임 실행

1. City 씬에서 게임 시작
2. Enter Building 오브젝트에 접근
3. E키를 눌러 건물 입장

### Step 4: 아이템 배치 확인

1. InBuilding 씬이 로드되면
2. BuildingDataLoader가 JSON 파일을 읽고
3. 해당 건물의 아이템들이 지정된 위치에 배치됨
4. 콘솔에 로그 메시지 확인 (Debug Log가 켜져있는 경우)

### Step 5: 아이템 수집 테스트

1. 배치된 아이템에 접근
2. 상호작용하여 아이템 수집
3. 인벤토리에 아이템이 추가되는지 확인

---

## 🔧 문제 해결

### 아이템이 배치되지 않을 때

1. **JSON 파일 경로 확인**
   - persistentDataPath 위치 확인: `Debug.Log(Application.persistentDataPath);`
   - 파일이 실제로 존재하는지 확인

2. **건물 정보 확인**
   - PlayerPrefs에 BuildingType과 BuildingIndex가 저장되었는지 확인
   - 콘솔에서 확인: `Debug.Log($"Type: {PlayerPrefs.GetString("BuildingType")}, Index: {PlayerPrefs.GetString("BuildingIndex")}");`

3. **아이템 이름 확인**
   - JSON의 ItemName이 Resources/Items 폴더의 아이템 itemName과 정확히 일치하는지 확인
   - 대소문자 구분됨

4. **Item Prefab 확인**
   - Item Prefab에 Lootable 컴포넌트가 있는지 확인
   - Collider2D가 Is Trigger로 설정되어 있는지 확인

### JSON 파싱 오류

1. JSON 문법 확인 (온라인 검증기 사용)
2. 필드명이 정확한지 확인 (ItemName, ItemCount, ItemPosition)
3. Vector2 구조 확인 (x, y 필드)

### 아이템을 찾을 수 없을 때

1. Resources/Items 폴더에 해당 아이템이 있는지 확인
2. 아이템의 itemName 필드가 JSON의 ItemName과 일치하는지 확인
3. Resources 폴더 구조 확인:
   ```
   Assets/
     Resources/
       Items/
         Apple.asset
         Scrap.asset
         ...
   ```

---

## 📝 다음 단계

1. **에디터 도구 개발**: Unity 에디터에서 JSON 파일을 생성/편집할 수 있는 도구
2. **아이템 시각화**: 씬 뷰에서 아이템 위치를 시각적으로 표시
3. **저장 시스템 연동**: 아이템 수집 후 JSON 파일 업데이트
4. **다양한 건물 타입**: 추가 건물 타입 지원

---

## ✅ 체크리스트

InBuilding 씬 개발 완료 후 확인사항:

- [ ] BuildingDataLoader 오브젝트가 씬에 배치되어 있는가?
- [ ] Item Prefab이 올바르게 할당되어 있는가?
- [ ] JSON 파일이 올바른 위치에 있는가?
- [ ] Enter Building 오브젝트의 Building Type과 Index가 설정되어 있는가?
- [ ] 아이템이 올바른 위치에 배치되는가?
- [ ] 아이템 수집이 정상적으로 작동하는가?
- [ ] 출구로 나가기 기능이 작동하는가?


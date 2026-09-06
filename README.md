# 🎮 ViveClicker

<img width="358" height="643" alt="image" src="https://github.com/user-attachments/assets/0e9a213f-9d13-4c1b-999c-25d8550c38e1" />

> **Unity CLI & Claude Code (MCP)** 기반의 바이브 코딩(Vibe Coding)으로 에이전트와 협업하여 구현한 8비트 모바일 클리커 게임

---

## 🖥️ 장르 : 2D 8-Bit Retro Clicker (모바일 캐주얼)
화면의 코인을 탭하여 골드를 모으고, 10단계의 티어별 업그레이드를 해금하여 초당/클릭당 골드 획득량을 극대화하는 클리커 게임입니다.

## ⏱️ 개발 기간
- 2026.04 (핵심 루프 구현, MCP 연동 및 에이전트 코드 오딧 완료)

## 🧑 제작 인원
- **장현교** : 1인 개발 (기획, 아키텍처 설계, Unity CLI-MCP 파이프라인 구축 및 바이브 코딩 오케스트레이션)

## ⚙️ 개발 환경
- **Engine** : Unity (Unity 6.3 LTS)
- **Language** : C# (.NET Standard)
- **Agentic Tools** : Claude Code (CLI), Model Context Protocol (MCP) Server for Unity
- **VCS & OS** : Git, GitHub Desktop / Windows

---

## 🤖 핵심 차별점 : Unity CLI + MCP 기반 바이브 코딩 (Vibe Coding)

본 프로젝트는 단순 질의응답 형태의 AI 보조 방식을 탈피하여, **터미널 환경의 Claude Code CLI가 MCP를 통해 유니티 씬을 직접 제어하고 C# 코드를 감사하는 차세대 개발 파이프라인**을 검증하고 적용했습니다.

### 1. 개발 파이프라인 아키텍처

```
[Developer / Architect]
       │  (의도 전달, 라이프사이클 설계 및 아키텍처 가이드)
       ▼
[Claude Code (CLI Agent)] ──(MCP Server)──▶ [Unity Editor & C# Solution]
       │                                              │
       ├─ 씬 내 GameObject 동적 생성 및 트랜스폼 정렬   │
       ├─ UI RectTransform 앵커링 및 인스펙터 바인딩    │
       ├─ 이벤트 리스너 및 AudioSource 컴포넌트 연결    │
       └─ C# 스크립트 수정, 인메모리 버그 & GC 최적화 ──┘
```

### 2. Unity CLI & MCP 연동 씬 오케스트레이션
- **GUI 조작 자동화**: 복잡한 인스펙터 수작업 클릭 없이 터미널 CLI 프롬프트 지시를 통해 상단 세이브/초기화 패널, 10티어 업그레이드 슬롯 등의 UI 계층 구조와 RectTransform 앵커링을 자동화했습니다.
- **컴포넌트 리퍼런스 및 이벤트 바인딩**: `SoundManager`, `UpgradeListManager`, `SaveManager` 등의 싱글톤 매니저와 버튼 `onClick` 이벤트, 오디오 클립(`Coin.wav`) 참조를 CLI 명령으로 무결하게 연결했습니다.

### 3. 에이전트 기반 정적 코드 감사 (Code Audit) 및 최적화
- **생명주기 버그 추적 및 교정**: `DontDestroyOnLoad` 처리된 `CurrencyManager`가 씬 리로드 시 인메모리 상태를 유지하여 "초기화 후에도 이전 골드 수치가 유지되는 UI-상태 불일치 버그"를 정적 분석을 통해 조기에 탐지하고 `ResetState()` 패턴을 설계해 해결했습니다.
- **GC Allocation 스로틀링**: 초당 골드(GPS)가 동작할 때 `Update()` 루프 내 매 프레임 문자열 보간 및 TextMeshPro 버텍스 리빌드로 인한 60fps GC 스파이크를 방지하기 위해, 정수 단위 캐싱 기반 이벤트 스로틀링을 적용했습니다.
- **엔진 권장 최적화 API 준수**: 정렬 연산 비용이 발생하는 구형 `FindObjectsOfType` 호출을 전면 교체하여 가벼운 `FindObjectsByType(FindObjectsSortMode.None)`로 마이그레이션했습니다.

---

## 📌 주요 게임 기능

### 코인 클릭 및 성장 루프
- 코인 클릭 시 기본 재화(GPC) 획득 및 시간 경과에 따른 자동 재화 생성(GPS) 메커니즘을 구현했습니다.
- 지수적 비용 증가 공식을 적용한 10단계 업그레이드 데이터(`ScriptableObject`) 파이프라인을 구축했습니다.

### 타격감 연출 및 8비트 사운드
- 외부 플러그인(DOTween 등) 의존성을 배제하고 유니티 순수 코루틴만으로 스케일 펀치 애니메이션을 구현했습니다.
- 획득 골드를 직관적으로 보여주는 플로팅 텍스트 팝업 및 페이드아웃 효과를 적용했습니다.
- 연타 시에도 음이 씹히지 않고 자연스럽게 겹쳐 재생되도록 `AudioSource.PlayOneShot` 기반의 저작권 프리(CC0) 8비트 코인 SFX를 연결했습니다.

### 세이브/로드 및 초기화 시스템
- 모바일 OS 샌드박스 표준 경로(`Application.persistentDataPath`)에 JSON 포맷으로 데이터를 직렬화/역직렬화하여 앱 삭제 시 찌꺼기 파일이 남지 않도록 설계했습니다.
- 상단 전용 UI 버튼을 통해 플레이어가 원하는 시점에 수동 저장하거나 인게임 상태를 완전 리셋할 수 있는 기능을 제공합니다.

---

## 🎯 배운 점 및 회고

1. **개발자의 아키텍처 이해도와 통제력의 중요성**
   - AI 에이전트에게 구현을 위임하더라도, 유니티의 실행 순서(`Awake` vs `Start`), 스크립트 실행 순서(Execution Order), 싱글톤의 생명주기(`DontDestroyOnLoad`), GC 메커니즘에 대한 명확한 지식이 없으면 에이전트가 만든 미묘한 상태 불일치 버그를 발견할 수 없음을 체감했습니다.
2. **Unity CLI + MCP 워크플로우의 생산성 혁신**
   - 단순 코드 복사-붙여넣기 작업을 완전히 걷어내고, CLI 에이전트가 씬 배치와 스크립트 리팩토링을 직접 수행하게 함으로써 반복적인 UI 바인딩 시간과 프로토타이핑 비용을 획기적으로 단축할 수 있었습니다.

# Simple 2D Medieval Platformer (Unity 6.3 LTS / Android)

Open this project with **Unity 6.3 LTS (6000.3.4f1)**.

Important setup inside Unity:
1. Install **Android Build Support** from Unity Hub.
2. Open **File > Build Profiles** and switch to **Android**.
3. Open **Edit > Project Settings > Player > Other Settings > Configuration** and set **Active Input Handling = Input Manager (Old)**.
4. Set orientation to **Landscape Left** and **Landscape Right**.
5. Set scripting backend to **IL2CPP** and target architecture to **ARM64** for release builds.
6. Make sure the scene list contains:
   - MainMenu
   - Level1_Forest
   - Level2_Castle

The game uses:
- runtime-generated placeholder sprites
- runtime-built UI for HUD, menus, pause, and touch controls
- PlayerPrefs for save data
- two ScriptableObject weapon assets
- two ScriptableObject enemy stat assets

The project can run in the Editor with keyboard:
- A / D or Left / Right = Move
- Space = Jump
- J = Attack
- K = Switch weapon
- Esc = Pause
